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
using Microsoft.Xrm.Sdk.Query;
using Solution_Quality_Checker.Models;

namespace Solution_Quality_Checker.Validators
{
    public class ComponentsValidator : Validator
    {
        public ComponentsValidator(IOrganizationService service) : base(service)
        {
        }

        public override string Message => "Checking Components";

        public override event EventHandler<ErrorEventArgs> OnValidatorError;
        public override event EventHandler<ProgressEventArgs> OnValidatorProgress;

        public override ValidationResults Validate(CRMSolution solution)
        {
            ValidationResults results = new ValidationResults();

            // get all solution components
            QueryExpression processQuery = new QueryExpression("solutioncomponent");
            processQuery.ColumnSet = new ColumnSet(true);
            processQuery.Criteria.AddCondition("componenttype", ConditionOperator.Equal, 29); // find entities
            processQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, solution.Id);


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
                        results = ProcessCustomizationsXml(customizationsXml);
                    }

                }

                catch (IOException ex)
                {
                    // fire an error
                    OnValidatorError?.Invoke(this, new ErrorEventArgs(ex));
                }
            }


            // find all managed components of the solution


            //find if any managed components contains no unmanaged changes and flag it.
            return results;

        }

        private ValidationResults ProcessCustomizationsXml(XDocument doc)
        {
            return new ValidationResults();
        }
    }
}
