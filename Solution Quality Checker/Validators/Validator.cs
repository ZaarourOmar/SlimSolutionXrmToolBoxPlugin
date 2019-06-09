using Microsoft.Xrm.Sdk;
using Solution_Quality_Checker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Quality_Checker.Validators
{
    public abstract class Validator : IValidator
    {
        public IOrganizationService CRMService { get; set; }
        public abstract string Message { get; }

        public Validator(IOrganizationService service)
        {
            CRMService = service;
        }

        public abstract event EventHandler<ErrorEventArgs> OnValidatorError;
        public abstract event EventHandler<ProgressEventArgs> OnValidatorProgress;

        public abstract ValidationResults Validate(CRMSolution solution);

        public static List<IValidator> GetValidators(IOrganizationService service, ValidationSettings currentValidationSettings)
        {
            List<IValidator> validators = new List<IValidator>();
            if (currentValidationSettings.SettingsKVPs["CheckEntityComponents"])
            {
                validators.Add(new ComponentsValidator(service));
            }
            if (currentValidationSettings.SettingsKVPs["CheckProcesses"])
            {
                validators.Add(new ProcessValidator(service));
            }
            
            return validators;
        }
    }
}
