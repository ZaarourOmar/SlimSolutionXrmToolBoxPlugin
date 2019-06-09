using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution_Quality_Checker.Models
{
    public class ValidationSettings
    {
        Dictionary<string, bool> _settingsKVPs;
        public Dictionary<string, bool> SettingsKVPs { get => _settingsKVPs; set => _settingsKVPs = value; }
        public static ValidationSettings CurrentValidationSettings { get; set; } = ValidationSettings.Default;

        public ValidationSettings(bool checkAll)
        {
            CheckAll = checkAll;
            SettingsKVPs = new Dictionary<string, bool>();
            if(CheckAll)
            {
                SettingsKVPs.Add("CheckProcesses", true);
                SettingsKVPs.Add("CheckEntityComponents", true);
            }
            else
            {
                SettingsKVPs.Add("CheckProcesses", true);
                SettingsKVPs.Add("CheckEntityComponents", true);
            }
        }

      
        public static ValidationSettings Default
        {
            get
            {
                return new ValidationSettings(true);
            }
        }
        public bool CheckAll { get; set; }
      
    }
}
