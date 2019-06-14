using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimSolution.Models
{
 
    /// <summary>
    /// This class holds the list of issues that are found in a solution. 
    /// </summary>
    public class ValidationResults
    {
        public ValidationResults()
        {
            Id = Guid.NewGuid();
            ResultRecords = new List<ValidationResult>();
        }

        public ValidationResult this[int index]    // Indexer declaration  
        {
            get
            {
                return ResultRecords[index];
            }
        }

        public Guid Id { get; set; }
        public List<ValidationResult> ResultRecords { get; set; }

        public void AddResultSet(ValidationResults partialResults)
        {
            ResultRecords.AddRange(partialResults.ResultRecords);
        }

        internal void AddResult(ValidationResult singleResult)
        {
            ResultRecords.Add(singleResult);
        }
    }

    public class ValidationResult
    {
        public ValidationResult()
        {
            IDs = new List<Guid>();
            LogicalNames = new List<string>();
        }
        public List<Guid> IDs { get; set; }
        public string Description { get; set; }
        public string Suggestions { get; set; }
        public string Regarding { get; set; }
        public string EntityLogicalName { get; set; }
        public SolutionComponentType SolutionComponentType { get; set; }
        public List<string> LogicalNames { get; set; }
    }
}
