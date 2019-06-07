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
            Level = ValidationResultLevel.None;
        }
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Suggestions { get; set; }
        public ValidationResultLevel Level { get; set; }
    }
}

namespace Solution_Quality_Checker
{
    public enum ValidationResultLevel
    {
        None,Information,Warning,Error
    }
}