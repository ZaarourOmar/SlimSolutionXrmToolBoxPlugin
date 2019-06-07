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

        public IOrganizationService CRMService { get; internal set; }
        public ValidationResults HealthIssues { get; set; }
        public ValidationSettings CurrentValidationSettings { get { return ValidationSettings.CurrentValidationSettings; } }
        public IValidator ComponentValidator { get; set; }
        public IValidator ProcessValidator { get; set; }
        public IValidator CodeValidator { get; set; }

        public SolutionHealthManager(IOrganizationService service)
        {
            CRMService = service;
            HealthIssues = new ValidationResults();
            ComponentValidator = new ComponentsValidator(service);
            ProcessValidator = new ProcessValidator(service);
            CodeValidator = new CodeValidator(service);
        }

        public ValidationResults Validate(CRMSolution solution)
        {
            return null;
        }

    }
}
