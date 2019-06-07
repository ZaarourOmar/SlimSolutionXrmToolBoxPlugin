using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Solution_Quality_Checker.Models;

namespace Solution_Quality_Checker.Validators
{
    public class ComponentsValidator : Validator
    {
        public ComponentsValidator(IOrganizationService service) : base(service)
        {
        }

        public override string Message => "Checking Components";

        public override ValidationResults Validate(CRMSolution solution)
        {
            ValidationResults results = new ValidationResults();
            Thread.Sleep(2000);
            return results;

        }
    }
}
