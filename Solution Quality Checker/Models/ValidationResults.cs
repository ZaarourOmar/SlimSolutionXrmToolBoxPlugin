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
            Results = new List<ValidationResult>();
        }

        public string Label { get; set; }
        public Guid Id { get; set; }
        public List<ValidationResult> Results { get; set; }

        public void AddResultSet(ValidationResults partialResults)
        {
            Results.AddRange(partialResults.Results);
        }
    }
}
