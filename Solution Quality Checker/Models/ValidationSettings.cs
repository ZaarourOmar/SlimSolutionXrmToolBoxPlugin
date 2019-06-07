using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Quality_Checker.Models
{
    public class ValidationSettings
    {
        public ValidationSettings(bool checkAll)
        {
            CheckAll = checkAll;
        }

        public static ValidationSettings Default
        {
            get
            {
                return new ValidationSettings(true);
            }
        }

        public bool CheckAll
        {
            get { return CheckProcesses && CheckBusinessRules && CheckExtraComponents && CheckModularity && CheckBPF && CheckWebResources && CheckDependencies && CheckPublishers; }
            internal set { CheckAll = value; }
        }

        public bool CheckProcesses { get; set; }
        public bool CheckBusinessRules { get; set; }

        public void Clear()
        {
            CheckBPF = false;
        }

        public bool CheckModularity { get; set; }
        public bool CheckBPF { get; set; }
        public bool CheckWebResources { get; set; }
        public bool CheckPublishers { get; set; }
        public bool CheckDependencies { get; set; }
        public bool CheckExtraComponents { get; internal set; }
    }
}
