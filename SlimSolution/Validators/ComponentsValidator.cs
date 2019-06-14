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

        public CRMSolution Solution { get; set; }
        public ValidationResults AllResults { get; set; }
        public ComponentsValidator(IOrganizationService service, CRMSolution solution)
        {
            CRMService = service;
            Solution = solution;
            AllResults = new ValidationResults();
        }

        public ValidationResults Validate()
        {
            ValidationResults results = ValidateManagedSolutionComponents();
            return results;

        }

        private ValidationResults ValidateManagedSolutionComponents()
        {

            ValidationResults results = new ValidationResults();

            // export the solution as managed and extract it to get the customizations xml
            ExportSolutionRequest exportRequest = new ExportSolutionRequest();
            exportRequest.Managed = true;
            exportRequest.SolutionName = Solution.UniqueName;
            OnValidatorProgress?.Invoke(this, new ProgressEventArgs("Exporting solution as managed"));
            ExportSolutionResponse managedResponse = CRMService.Execute(exportRequest) as ExportSolutionResponse;
            if (managedResponse != null)
            {
                try
                {
                    string zipFileName = Solution.UniqueName + ".zip";
                    string customiationXmlPath = Constants.APP_DATA_TEMP_DIRECTOY_PATH + "\\customizations.xml";
                    string zipPath = Constants.APP_DATA_TEMP_DIRECTOY_PATH + zipFileName;

                    // cleanup an existing directory files
                    if (Directory.Exists(Constants.APP_DATA_TEMP_DIRECTOY_PATH))
                    {
                        Directory.Delete(Constants.APP_DATA_TEMP_DIRECTOY_PATH, true);
                    }

                    // recreate the directory
                    if (!Directory.Exists(Constants.APP_DATA_TEMP_DIRECTOY_PATH))
                    {
                        Directory.CreateDirectory(Constants.APP_DATA_TEMP_DIRECTOY_PATH);
                    }

                    // write the managed solution as a zip file in the directory
                    OnValidatorProgress?.Invoke(this, new ProgressEventArgs("Saving Managed Solution"));
                    File.WriteAllBytes(zipPath, managedResponse.ExportSolutionFile);

                    // extract the zip file to get the customizations.xml file content
                    OnValidatorProgress?.Invoke(this, new ProgressEventArgs("Extracting Managed Solution"));
                    ZipFile.ExtractToDirectory(zipPath, Constants.APP_DATA_TEMP_DIRECTOY_PATH);


                    //at this point customization.xml file should be ready, load it into an xdocument object
                    if (File.Exists(customiationXmlPath))
                    {
                        OnValidatorProgress?.Invoke(this, new ProgressEventArgs("Checking the Customization File"));

                        XDocument customizationsXml = XDocument.Load(customiationXmlPath);
                        results.AddResultSet(CheckAttributes(customizationsXml));
                        results.AddResultSet(CheckViews(customizationsXml));
                        results.AddResultSet(CheckForms(customizationsXml));
                    }

                }

                catch (IOException ioEx)
                {
                    // fire an error to be catched by whoever is listening to the OnValidationError Event
                    OnValidatorError?.Invoke(this, new ErrorEventArgs(ioEx));
                    throw ioEx;
                }
                catch (Exception ex)
                {
                    OnValidatorError?.Invoke(this, new ErrorEventArgs(ex));
                    throw ex;
                }
            }
            return results;
        }

        private ValidationResults CheckForms(XDocument customizationsXml)
        {
            ValidationResults results = new ValidationResults();
            var entitiesXml = (from c in customizationsXml.Descendants("Entity") select c).Distinct();
            Dictionary<string, List<XElement>> formCollections = new Dictionary<string, List<XElement>>();
            foreach (var entityXml in entitiesXml)
            {
                if (entityXml.Element("EntityInfo") != null && entityXml.Element("EntityInfo").Element("entity") != null)
                    if (!entityXml.Element("EntityInfo").Element("entity").Element("EntitySetName").Value.Contains("_") && entityXml.Descendants("systemform") != null)
                        formCollections[entityXml.Descendants("EntitySetName").ElementAt(0).Value] = (from x in entityXml.Descendants("systemform") select x).ToList<XElement>();
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
                        {
                            stringBuilder.Append(formElement.Descendants("LocalizedName").ElementAt(0).Attribute("description").Value + ",   \n");
                            result.IDs.Add(new Guid(formElement.Element("formid").Value));
                        }
                    }

                    if (formCollections.Count > 0)
                    {
                        result.SolutionComponentType = SolutionComponentType.Form;
                        result.EntityLogicalName = formCollection.Key;
                        result.Description = stringBuilder.ToString();
                        result.Suggestions = "Try to have only the needed forms in the solution. Any custom form or a modified managed form are good to be in the solution but nothing else.";
                        result.Regarding = formCollection.Key + " " + "Entity Forms";
                        results.AddResult(result);
                    }
                }
            }
            AllResults.AddResultSet(results);
            return results;
        }

        private ValidationResults CheckViews(XDocument customizationsXml)
        {
            ValidationResults results = new ValidationResults();
            var entitiesXml = (from c in customizationsXml.Descendants("Entity") select c).Distinct();
            Dictionary<string, List<XElement>> viewsCollections = new Dictionary<string, List<XElement>>();
            foreach (var entityXml in entitiesXml)
            {
                if (entityXml.Element("EntityInfo") != null && entityXml.Element("EntityInfo").Element("entity") != null)
                    if (!entityXml.Element("EntityInfo").Element("entity").Element("EntitySetName").Value.Contains("_") && entityXml.Descendants("savedquery") != null)
                        viewsCollections[entityXml.Descendants("EntitySetName").ElementAt(0).Value] = (from x in entityXml.Descendants("savedquery") select x).ToList<XElement>();
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
                        {
                            stringBuilder.Append(viewElement.Descendants("LocalizedName").ElementAt(0).Attribute("description").Value + ",   \n");
                            result.IDs.Add(new Guid(viewElement.Element("savedqueryid").Value));
                        }
                    }

                    if (viewsCollections.Count > 0)
                    {
                        result.EntityLogicalName = viewCollection.Key;
                        result.SolutionComponentType = SolutionComponentType.View;
                        result.Description = stringBuilder.ToString();
                        result.Suggestions = "Try to have only the needed views in the solution. Any custom views or a modified managed views are good to be in the solution but nothing else.";
                        result.Regarding = viewCollection.Key + " " + "Entity Views";
                        results.AddResult(result);
                    }
                }
            }

            AllResults.AddResultSet(results);

            return results;
        }

        private ValidationResults CheckAttributes(XDocument customizationsXml)
        {
            ValidationResults results = new ValidationResults();
            var entitiesXml = (from c in customizationsXml.Descendants("Entity") select c).Distinct();
            Dictionary<string, List<XElement>> attributesCollections = new Dictionary<string, List<XElement>>();
            foreach (var entityXml in entitiesXml)
            {
                if (entityXml.Element("EntityInfo") != null && entityXml.Element("EntityInfo").Element("entity") != null)
                    if (!entityXml.Element("EntityInfo").Element("entity").Element("EntitySetName").Value.Contains("_") && entityXml.Descendants("attribute") != null)
                        attributesCollections[entityXml.Element("EntityInfo").Element("entity").Element("EntitySetName").Value] = (from x in entityXml.Descendants("attribute") select x).ToList<XElement>();
            }

            foreach (var attCollection in attributesCollections)
            {
                if (attCollection.Value.Count > 0)
                {
                    StringBuilder s = new StringBuilder();
                    Models.ValidationResult result = new Models.ValidationResult();
                    s.Append("Only add these fields to the solution:\n\n");
                    foreach (XElement attElement in attCollection.Value)
                    {
                        if (attElement.Attribute("PhysicalName") != null && attElement.Descendants("displayname") != null)
                        {
                            s.Append($"{attElement.Descendants("displayname").ElementAt(0).Attribute("description").Value} ({ attElement.Attribute("PhysicalName").Value}),   \n");
                            result.LogicalNames.Add(attElement.Element("LogicalName").Value);
                        }
                    }
                    if (attributesCollections.Count > 0)
                    {
                        result.EntityLogicalName = attCollection.Key;
                        result.SolutionComponentType = SolutionComponentType.Attribute;
                        result.Description = s.ToString();
                        result.Suggestions = "Try to have only the needed fields in the solution. Any custom field or a modified managed field are good to be in the solution but nothing else.";
                        result.Regarding = attCollection.Key + " " + "Entity Fields";
                        results.AddResult(result);
                    }
                }
            }

            AllResults.AddResultSet(results);
            return results;
        }

    }
}
