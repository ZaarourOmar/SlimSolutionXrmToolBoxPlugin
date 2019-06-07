using Microsoft.Xrm.Sdk;
using Solution_Quality_Checker.Models;
using System.Threading.Tasks;

namespace Solution_Quality_Checker.Validators
{
    public interface IValidator
    {
        IOrganizationService CRMService { get; set; }
        Task<ValidationResults> Validate(CRMSolution solution);
    }
}