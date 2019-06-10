using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using SlimSolution.Models;

namespace SlimSolution.Validators
{
    public class ComponentsValidator : IValidator
    {

        public event EventHandler<ErrorEventArgs> OnValidatorError;
        public event EventHandler<ProgressEventArgs> OnValidatorProgress;

        public IOrganizationService CRMService { get; set; }
        public string Message { get { return "Checking Forms, Views and Attributes"; } }


        public ComponentsValidator(IOrganizationService service)
        {
            CRMService = service;
        }

        public ValidationResults Validate(CRMSolution solution)
        {
            ValidationResults validatorResults = new ValidationResults();
            List<RetrieveEntityResponse> solutionEntities = GetAllEntitiesInTheSolution(solution);
            ValidationResults results = ValidateManagedSolutionComponents(solution, solutionEntities);
            validatorResults.AddResultSet(results);
            return validatorResults;

        }

        private List<RetrieveEntityResponse> GetAllEntitiesInTheSolution(CRMSolution solution)
        {
            // get all solution entities 
            QueryExpression processQuery = new QueryExpression("solutioncomponent");
            processQuery.ColumnSet = new ColumnSet(true);
            processQuery.Criteria.AddCondition("componenttype", ConditionOperator.Equal, 1); // find entities
            processQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, solution.Id);
            var components = CRMService.RetrieveMultiple(processQuery);


            // request each entity data separately and add it to a list
            List<RetrieveEntityResponse> allEntities = new List<RetrieveEntityResponse>();
            foreach (var componentEntity in components.Entities)
            {
                RetrieveEntityRequest entityRequest = new RetrieveEntityRequest
                {
                    EntityFilters = EntityFilters.All,
                    MetadataId = componentEntity.GetAttributeValue<Guid>("objectid"),
                    RetrieveAsIfPublished = true,

                };
                RetrieveEntityResponse entityResponse = (RetrieveEntityResponse)CRMService.Execute(entityRequest);
                allEntities.Add(entityResponse);
            }

            return allEntities;

        }

        private ValidationResults ValidateManagedSolutionComponents(CRMSolution solution, List<RetrieveEntityResponse> managedEntities)
        {

            ValidationResults results = new ValidationResults();

            // export the solution as managed and extract it to get the customizations xml
            ExportSolutionRequest exportRequest = new ExportSolutionRequest();
            exportRequest.Managed = true;
            exportRequest.SolutionName = solution.UniqueName;
            OnValidatorProgress?.Invoke(this, new ProgressEventArgs("Exporting solution as managed"));
            ExportSolutionResponse managedResponse = CRMService.Execute(exportRequest) as ExportSolutionResponse;
            if (managedResponse != null)
            {
                try
                {
                    string zipFileName = solution.UniqueName + ".zip";
                    string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    string targetDirectory = appDataFolder + "\\HealthCheckerSolutions\\";
                    string customiationXmlPath = targetDirectory + "\\customizations.xml";
                    string zipPath = targetDirectory + zipFileName;

                    // cleanup an existing directory files
                    if (Directory.Exists(targetDirectory))
                    {
                        Directory.Delete(targetDirectory, true);
                    }

                    // recreate the directory
                    if (!Directory.Exists(targetDirectory))
                    {
                        Directory.CreateDirectory(targetDirectory);
                    }

                    // write the managed solution as a zip file in the directory
                    OnValidatorProgress?.Invoke(this, new ProgressEventArgs("Saving Managed Solution"));
                    File.WriteAllBytes(zipPath, managedResponse.ExportSolutionFile);

                    // extract the zip file to get the customizations.xml file content
                    OnValidatorProgress?.Invoke(this, new ProgressEventArgs("Extracting Managed Solution"));
                    ZipFile.ExtractToDirectory(zipPath, targetDirectory);


                    //at this point customization.xml file should be ready, load it into an xdocument object
                    if (File.Exists(customiationXmlPath))
                    {
                        OnValidatorProgress?.Invoke(this, new ProgressEventArgs("Checking the Customization File"));

                        XDocument customizationsXml = XDocument.Load(customiationXmlPath);
                        results.AddResultSet(CheckAttributes(customizationsXml, managedEntities));
                        results.AddResultSet(CheckViews(customizationsXml, managedEntities));
                        results.AddResultSet(CheckForms(customizationsXml, managedEntities));
                    }

                }

                catch (IOException ex)
                {
                    // fire an error to be catched by whoever is listening to the OnValidationError Event
                    OnValidatorError?.Invoke(this, new ErrorEventArgs(ex));
                }

            }
            return results;
        }

        private ValidationResults CheckForms(XDocument customizationsXml, List<RetrieveEntityResponse> managedEntities)
        {
            ValidationResults results = new ValidationResults();
            var entities = (from c in customizationsXml.Descendants("Entity") select c).Distinct();
            Dictionary<string, List<XElement>> formCollections = new Dictionary<string, List<XElement>>();
            foreach (var entity in entities)
            {
                if (!entity.Element("Name").Value.Contains("_") && entity.Descendants("systemform") != null) // we need only system entities tht have no publishers
                    formCollections[entity.Element("Name").Value] = (from x in entity.Descendants("systemform") select x).ToList<XElement>();
            }

            foreach (var formCollection in formCollections)
            {
                if (formCollection.Value.Count > 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    Models.ValidationResult result = new Models.ValidationResult();
                    stringBuilder.Append("Only add these forms to the solution:\n\n");
                    foreach (XElement formElement in formCollection.Value)
                    {
                        if (formElement.Descendants("LocalizedName") != null)
                            stringBuilder.Append(formElement.Descendants("LocalizedName").ElementAt(0).Attribute("description").Value + ",   \n");
                    }

                    if (formCollections.Count > 0)
                    {
                        result.Description = stringBuilder.ToString();
                        result.Suggestions = "Try to have only the needed forms in the solution. Any custom form or a modified managed form are good to be in the solution but nothing else.";
                        result.Type = formCollection.Key + " " + "Entity Forms";
                        result.PriorityLevel = ValidationResultLevel.Medium;
                        results.AddResult(result);
                    }
                }
            }
            return results;
        }

        private ValidationResults CheckViews(XDocument customizationsXml, List<RetrieveEntityResponse> managedEntities)
        {
            ValidationResults results = new ValidationResults();
            var entities = (from c in customizationsXml.Descendants("Entity") select c).Distinct();
            Dictionary<string, List<XElement>> viewsCollections = new Dictionary<string, List<XElement>>();
            foreach (var entity in entities)
            {
                if (!entity.Element("Name").Value.Contains("_")) // we need only system entities tht have no publishers
                    viewsCollections[entity.Element("Name").Value] = (from x in entity.Descendants("savedquery") select x).ToList<XElement>();
            }

            foreach (var viewCollection in viewsCollections)
            {
                if (viewCollection.Value.Count > 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    Models.ValidationResult result = new Models.ValidationResult();
                    stringBuilder.Append("Only add these views to the solution:\n\n");
                    foreach (XElement viewElement in viewCollection.Value)
                    {
                        if (viewElement.Descendants("LocalizedName") != null)
                            stringBuilder.Append(viewElement.Descendants("LocalizedName").ElementAt(0).Attribute("description").Value + ",   \n");
                    }

                    if (viewsCollections.Count > 0)
                    {
                        result.Description = stringBuilder.ToString();
                        result.Suggestions = "Try to have only the needed views in the solution. Any custom views or a modified managed views are good to be in the solution but nothing else.";
                        result.Type = viewCollection.Key + " " + "Entity Views";
                        result.PriorityLevel = ValidationResultLevel.Medium;
                        results.AddResult(result);
                    }
                }
            }
            return results;
        }

        private ValidationResults CheckAttributes(XDocument customizationsXml, List<RetrieveEntityResponse> managedEntities)
        {
            ValidationResults results = new ValidationResults();
            var entities = (from c in customizationsXml.Descendants("Entity") select c).Distinct();
            Dictionary<string, List<XElement>> attributesCollections = new Dictionary<string, List<XElement>>();
            foreach (var entity in entities)
            {
                if (!entity.Element("Name").Value.Contains("_")) // we need only system entities tht have no publishers
                    attributesCollections[entity.Element("Name").Value] = (from x in entity.Descendants("attribute") select x).ToList<XElement>();
            }

            foreach (var attCollection in attributesCollections)
            {
                if (attCollection.Value.Count > 0)
                {
                    StringBuilder s = new StringBuilder();
                    Models.ValidationResult result = new Models.ValidationResult();
                    s.Append("Only add these fields to the solution:\n\n");
                    foreach (XElement element in attCollection.Value)
                    {
                        if (element.Attribute("PhysicalName") != null && element.Descendants("displayname") != null)
                            s.Append($"{element.Descendants("displayname").ElementAt(0).Attribute("description").Value} ({ element.Attribute("PhysicalName").Value}),   \n");
                    }
                    if (attributesCollections.Count > 0)
                    {
                        result.Description = s.ToString();
                        result.Suggestions = "Try to have only the needed fields in the solution. Any custom field or a modified managed field are good to be in the solution but nothing else.";
                        result.Type = attCollection.Key + " " + "Entity Fields";
                        result.PriorityLevel = ValidationResultLevel.Medium;
                        results.AddResult(result);
                    }
                }
            }
            return results;
        }
    }
}
