using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimSolution.Models
{

    public enum SolutionComponentType
    {
        Entity = 1,
        Attribute = 2,
        Relationship = 3,
        Form = 24,
        View = 26,
        Process = 29
    }
    public class CRMSolutionComponent
    {

        public CRMSolutionComponent(Guid id, string logicalName, SolutionComponentType type)
        {
            ID = id;
            Type = type;
            LogicalName = logicalName; // doesn't exist for all component types
        }

        public Guid ID { get; set; }
        public string LogicalName { get; set; }
        public SolutionComponentType Type { get; set; }
    }
}
