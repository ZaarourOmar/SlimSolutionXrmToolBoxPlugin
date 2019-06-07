using Microsoft.Xrm.Sdk;
using Solution_Quality_Checker.Models;
using System.Threading.Tasks;

namespace Solution_Quality_Checker.Validators
{
    public interface IValidator
    {
        IOrganizationService CRMService { get; set; }
        string Message { get; }
        ValidationResults Validate(CRMSolution solution);
    }
}