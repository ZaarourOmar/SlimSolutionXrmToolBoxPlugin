using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Quality_Checker.Models
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Id = Guid.NewGuid();
            PriorityLevel = ValidationResultLevel.None;
        }
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Suggestions { get; set; }
        public ValidationResultLevel PriorityLevel { get; set; }
        public string Type { get;  set; }
    }
}

namespace Solution_Quality_Checker
{
    public enum ValidationResultLevel
    {
        None,Low,Medium,High
    }
}