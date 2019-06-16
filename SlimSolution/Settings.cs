using SlimSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimSolution
{
    /// <summary>
    /// This class can help you to store settings for your plugin
    /// </summary>
    /// <remarks>
    /// This class must be XML serializable
    /// </remarks>
    public class Settings
    {
        public Settings()
        {
        }
        public string LastUsedOrganizationWebappUrl { get; set; }
        public bool CheckComponents { get; set; } = true;
        public bool CheckProcesses { get; set; } = true;
        public bool AlwaysPublish { get; set; } = false;

    }

}