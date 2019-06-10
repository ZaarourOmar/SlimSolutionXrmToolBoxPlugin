using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimSolution.Models
{
    public class CRMSolutionComponent
    {
        public CRMSolutionComponent(Guid id)
        {
            ID = id;
        }

        public Guid ID { get; set; }
    }
}
