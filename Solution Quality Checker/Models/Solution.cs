using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Quality_Checker.Models
{
    public class Solution
    {
        public Guid Id { get; set; }
        public int Name { get; set; }
        public int FriendlyName { get; set; }
        public bool IsManaged { get; set; } = false;
    }
}
