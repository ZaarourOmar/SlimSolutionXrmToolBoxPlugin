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
using Solution_Quality_Checker.Models;

namespace Solution_Quality_Checker.Validators
{
    public class ComponentsValidator : IValidator
    {

        public event EventHandler<ErrorEventArgs> OnValidatorError;
        public event EventHandler<ProgressEventArgs> OnValidatorProgress;

        public IOrganizationService CRMService { get; set; }
        public string Message { get { return "Checking Components"; } }


        public ComponentsValidator(IOrganizationService service)
        {
            CRMService = service;
        }

        public ValidationResults Validate(CRMSolution solution)
        {
            ValidationResults results = new ValidationResults();

            List<RetrieveEntityResponse> managedEntities = GetAllManagedEntities(solution);

            ValidationResults results2 = ValidateManagedSolution(solution, managedEntities);
            results.AddResultSet(results2);
            // find all managed components of the solution


            //find if any managed components contains no unmanaged changes and flag it.
            return results;

        }

        private List<RetrieveEntityResponse> GetAllManagedEntities(CRMSolution solution)
        {
            // get all solution entities 
            QueryExpression processQuery = new QueryExpression("solutioncomponent");
            processQuery.ColumnSet = new ColumnSet(true);
            processQuery.Criteria.AddCondition("componenttype", ConditionOperator.Equal, 1); // find entities
            processQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, solution.Id);
            var components = CRMService.RetrieveMultiple(processQuery);

            List<RetrieveEntityResponse> allEntities = new List<RetrieveEntityResponse>();
            foreach (var componentEntity in components.Entities)
            {
                RetrieveEntityRequest entityRequest = new RetrieveEntityRequest
                {
                    EntityFilters = EntityFilters.Attributes,
                    MetadataId = componentEntity.GetAttributeValue<Guid>("objectid"),
                    RetrieveAsIfPublished = true,

                };
                RetrieveEntityResponse entityResponse = (RetrieveEntityResponse)CRMService.Execute(entityRequest);
                allEntities.Add(entityResponse);
            }

            return allEntities;

        }

        private ValidationResults ValidateManagedSolution(CRMSolution solution, List<RetrieveEntityResponse> managedEntities)
        {

            ValidationResults results = new ValidationResults();
            // export the solution as managed
            ExportSolutionRequest exportRequest = new ExportSolutionRequest();
            exportRequest.Managed = true;
            exportRequest.SolutionName = solution.UniqueName;
            OnValidatorProgress?.Invoke(this, new ProgressEventArgs("Exorting solution as managed"));
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


                    if (!Directory.Exists(targetDirectory))
                    {
                        Directory.CreateDirectory(targetDirectory);
                    }

                    OnValidatorProgress?.Invoke(this, new ProgressEventArgs("Saving Managed Solution"));

                    File.WriteAllBytes(zipPath, managedResponse.ExportSolutionFile);

                    OnValidatorProgress?.Invoke(this, new ProgressEventArgs("Extracting Managed Solution"));
                    ZipFile.ExtractToDirectory(zipPath, targetDirectory);


                    //at this point customization.xml file should be ready
                    if (File.Exists(customiationXmlPath))
                    {
                        OnValidatorProgress?.Invoke(this, new ProgressEventArgs("Checking the Customization File"));

                        XDocument customizationsXml = XDocument.Load(customiationXmlPath);
                        results = CheckAttributes(customizationsXml, managedEntities);
                    }

                }

                catch (IOException ex)
                {
                    // fire an error
                    OnValidatorError?.Invoke(this, new ErrorEventArgs(ex));
                }

            }
            return results;
        }

        private ValidationResults CheckAttributes(XDocument doc, List<RetrieveEntityResponse> managedEntities)
        {
            ValidationResults results = new ValidationResults();
            var entities = (from c in doc.Descendants("Entity") select c).Distinct();
            Dictionary<string, List<XElement>> attributesCollection = new Dictionary<string, List<XElement>>();
            foreach (var entity in entities)
            {
                if (!entity.Element("Name").Value.Contains("_")) // we need only system entities tht have no publishers
                    attributesCollection[entity.Element("Name").Value] = (from x in entity.Descendants("attribute") select x).ToList<XElement>();
            }

            foreach (var attCollection in attributesCollection)
            {
                StringBuilder s = new StringBuilder();
                Models.ValidationResult result = new Models.ValidationResult();
                s.Append("Only add these fields to the solution:\n");
                foreach (XElement element in attCollection.Value)
                {
                    if (element.Attribute("PhysicalName") != null)
                        s.Append(element.Attribute("PhysicalName").Value + ",   \n");
                }
                result.Description = s.ToString();
                result.Suggestions = "Try to have only the needed fields in the solution. Any custom field or a modified managed field are good to be in the solution but nothing else.";
                result.Type = attCollection.Key + " " + "Entity Fields";
                result.PriorityLevel = ValidationResultLevel.Medium;
                results.AddResult(result);
            }
            return results;
        }
    }
}
