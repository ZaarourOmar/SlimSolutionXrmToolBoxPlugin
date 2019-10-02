using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using SlimSolution.Models;
using SlimSolution.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimSolution
{
    public class ValidatorsManager
    {
        public event EventHandler<ErrorEventArgs> OnError;
        public event EventHandler<ProgressEventArgs> OnProgressChanged;
        public IOrganizationService CRMService { get; internal set; }
        public ValidationResults Results { get; set; }
        public List<IValidator> Validators { get; set; }
        public Settings MySettings { get; set; }

        public CRMSolution Solution { get; set; }
        public ValidatorsManager(IOrganizationService service, CRMSolution solution, Settings mySettings)
        {
            Solution = solution;
            CRMService = service;
            MySettings = mySettings;
            Results = new ValidationResults();
            Validators = GetAvailableValidators();
            
        }

        public ValidationResults Validate(CRMSolution solution)
        {
            ValidationResults allValidatorsResult = new ValidationResults();

            if (Validators == null || Validators.Count == 0)
            {
                throw new InvalidOperationException("No Validators exist, please change the validation settings first");
            }

            //publish all customizations first if the settings allow it
            if (MySettings.AlwaysPublish)
            {
                OnProgressChanged?.Invoke(this, new ProgressEventArgs("Publishing customizations"));
                PublishAllXmlRequest publishRequest = new PublishAllXmlRequest();
                CRMService.Execute(publishRequest);
            }

            // start the validators
            foreach (IValidator validator in Validators)
            {

                validator.OnValidatorError += (s, e) =>
                {
                    OnError?.Invoke(s, e);
                };
                validator.OnValidatorProgress += (s, e) =>
                {
                    OnProgressChanged?.Invoke(s, new ProgressEventArgs(e.Message));
                };

                ValidationResults validatorResult = validator.Validate();
                allValidatorsResult.AddResultSet(validatorResult);
            }

            return allValidatorsResult;
        }
        public List<IValidator> GetAvailableValidators()
        {
            List<IValidator> validators = new List<IValidator>();
            if (MySettings.CheckComponents)
            {
                validators.Add(new ComponentsValidator(CRMService, Solution));
            }
            if (MySettings.CheckProcesses)
            {
                validators.Add(new ProcessValidator(CRMService, Solution));
            }

            return validators;
        }


    }

}
