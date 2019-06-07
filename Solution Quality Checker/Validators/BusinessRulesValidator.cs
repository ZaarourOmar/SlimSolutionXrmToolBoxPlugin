using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Solution_Quality_Checker.Models;

namespace Solution_Quality_Checker.Validators
{
    class BusinessRulesValidator : Validator
    {
        public BusinessRulesValidator(IOrganizationService service):base(service)
        {
        }

        public override ValidationResults Validate(CRMSolution solution)
        {
            throw new NotImplementedException();
        }
    }
}
