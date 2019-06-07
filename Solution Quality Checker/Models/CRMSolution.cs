using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Quality_Checker.Models
{
    public class CRMSolution
    {
        public Solution BaseSolution { get; set; }
        public ExportSolutionResponse UnManagedSolutionData { get; set; }
        public ExportSolutionResponse ManagedSolutionData { get; set; }
        public CRMSolution(Solution solution)
        {
            this.BaseSolution = solution;
        }
    }
}
