using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
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
        public Entity BaseSolutionRecord { get; set; }
        public ExportSolutionResponse UnManagedSolutionData { get; set; }
        public ExportSolutionResponse ManagedSolutionData { get; set; }
        public string Name { get { return BaseSolutionRecord.GetAttributeValue<string>("uniquename"); } }

        public Guid Id { get { return BaseSolutionRecord.Id; } }

        public CRMSolution(Entity solutionRecord)
        {
            this.BaseSolutionRecord = solutionRecord;
        }
    }
}
