using Microsoft.Xrm.Sdk;
using Solution_Quality_Checker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Quality_Checker.Validators
{
    public class CodeValidator : Validator
    {
        public CodeValidator(IOrganizationService service) : base(service)
        {
        }

        public override string Message => "Checking Code";

        public override ValidationResults Validate(CRMSolution solution)
        {
            ValidationResults results = new ValidationResults();
            return results;
        }
    }
}
