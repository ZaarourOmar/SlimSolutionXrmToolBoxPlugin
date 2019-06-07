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

        public event EventHandler<PartialResultsEventArgs> OnPartialResultsDone;

        public IOrganizationService CRMService { get; internal set; }
        public ValidationResults HealthIssues { get; set; }
        public ValidationSettings CurrentValidationSettings { get { return ValidationSettings.CurrentValidationSettings; } }
        public List<IValidator> Validators { get; set; }
       
        public SolutionHealthManager(IOrganizationService service)
        {
            CRMService = service;
            HealthIssues = new ValidationResults();
            Validators = Validator.GetValidators(CRMService, CurrentValidationSettings);
        }

        public  ValidationResults Validate(CRMSolution solution)
        {
            ValidationResults finalResults = new ValidationResults();

            if(Validators==null ||Validators.Count==0)
            {
                throw new InvalidOperationException("No Validators exist, please change the validation settings first");
            }
            foreach(IValidator validator in Validators)
            {
                OnPartialResultsDone?.Invoke(this, new PartialResultsEventArgs(validator.Message));
                ValidationResults results =  validator.Validate(solution);
                finalResults.AddResultSet(results);
            }

            return finalResults;
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
