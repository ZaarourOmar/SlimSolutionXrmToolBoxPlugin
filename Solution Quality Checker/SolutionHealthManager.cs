using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Solution_Quality_Checker.Models;
using Solution_Quality_Checker.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Quality_Checker
{
    public class SolutionHealthManager
    {

        public event EventHandler<ErrorEventArgs> OnError;
        public event EventHandler<ProgressEventArgs> OnProgressChanged;

        public IOrganizationService CRMService { get; internal set; }
        public ValidationResults HealthIssues { get; set; }
        public ValidationSettings CurrentValidationSettings { get { return ValidationSettings.CurrentValidationSettings; } }
        public List<IValidator> Validators { get; set; }

        public SolutionHealthManager(IOrganizationService service)
        {
            CRMService = service;
            HealthIssues = new ValidationResults();
            Validators = GetValidators();
        }

        public ValidationResults Validate(CRMSolution solution)
        {
            ValidationResults finalResults = new ValidationResults();

            if (Validators == null || Validators.Count == 0)
            {
                throw new InvalidOperationException("No Validators exist, please change the validation settings first");
            }

            // publish all customizations first
            OnProgressChanged?.Invoke(this, new ProgressEventArgs("Publishing customizations"));
            PublishAllXmlRequest publishRequest = new PublishAllXmlRequest();
            CRMService.Execute(publishRequest);

            // start the validators
            foreach (IValidator validator in Validators)
            {

                validator.OnValidatorError += (s, e) =>
                {
                    OnError?.Invoke(s, e);
                };
                validator.OnValidatorProgress += (s, e) =>
                {
                    OnProgressChanged?.Invoke(s, new ProgressEventArgs(validator.Message + ":" + e.Message));
                };

                ValidationResults results = validator.Validate(solution);
                finalResults.AddResultSet(results);
            }

            return finalResults;
        }
        public List<IValidator> GetValidators()
        {
            List<IValidator> validators = new List<IValidator>();
            if (CurrentValidationSettings.SettingsKVPs["CheckEntityComponents"])
            {
                validators.Add(new ComponentsValidator(CRMService));
            }
            if (CurrentValidationSettings.SettingsKVPs["CheckProcesses"])
            {
                validators.Add(new ProcessValidator(CRMService));
            }

            return validators;
        }
    }

    public class PartialResultsEventArgs
    {
        public string Message { get; set; }
        public PartialResultsEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
