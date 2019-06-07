using Microsoft.Xrm.Sdk;
using Solution_Quality_Checker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Quality_Checker.Validators
{
    public class WebResourceValidator : Validator
    {
        public WebResourceValidator(IOrganizationService service) : base(service)
        {
        }

        public override ValidationResults Validate(CRMSolution solution)
        {
            throw new NotImplementedException();
        }
    }
}
