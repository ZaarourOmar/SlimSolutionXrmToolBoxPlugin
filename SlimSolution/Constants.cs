using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimSolution
{
    public class Constants
    {
        public static readonly string APP_DATA_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
        public static readonly string APP_DATA_TEMP_DIRECTOY_PATH = APP_DATA_FOLDER + "SlimSolutionPluginTempFiles\\";

        public static string WHY_THIS_TOOL_TEXT = "Managed components can be easily added to unmanaged solution even though they are not really needed.This tool goes over some types of the solution components and checks which components need " +
                "to be in the solution. This helps in cleaning out the solution, reduce its size and reduce any " +
                "future layering / dependency problems \n\n Note: For now, this tool checks for " +
                "unneeded  inactive Processes, unneeded managed forms, managed Views and managed Attributes.";
    }
}
