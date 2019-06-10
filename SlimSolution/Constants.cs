using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimSolution
{
    public class Constants
    {
        public const int ENTITY_COMPONENT_TYPE = 1;
        public const int PROCESS_COMPONENT_TYPE = 29;
        public static readonly string APP_DATA_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string APP_DATA_DIRECTOY_NAME = "\\HealthCheckerSolutions\\";
        public static readonly string TARGET_APPDATA_DIRECTORY = APP_DATA_FOLDER + APP_DATA_DIRECTOY_NAME;

    }
}
