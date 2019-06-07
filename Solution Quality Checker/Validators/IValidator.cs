using Microsoft.Xrm.Sdk;
using Solution_Quality_Checker.Models;

namespace Solution_Quality_Checker.Validators
{
    public interface IValidator
    {
         IOrganizationService CRMService { get; set; }
         ValidationResults Validate(CRMSolution solution);
    }
}