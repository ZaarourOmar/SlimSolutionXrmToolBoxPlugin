using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Quality_Checker.Models
{
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
}
