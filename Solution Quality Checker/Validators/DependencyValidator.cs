using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Solution_Quality_Checker.Models;

namespace Solution_Quality_Checker.Validators
{
    public class DependencyValidator : Validator
    {
        public DependencyValidator(IOrganizationService service) : base(service)
        {
        }
        public override ValidationResults Validate(Solution solution)
        {
            throw new NotImplementedException();
        }
    }
}
