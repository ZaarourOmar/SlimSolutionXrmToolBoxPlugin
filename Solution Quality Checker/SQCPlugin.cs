using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Solution_Quality_Checker
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Solution Quality Checker"),
        ExportMetadata("Description", "Gives you a detailed quality report on your unmanaged solution"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "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"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class SQCPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new SQCControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public SQCPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.SQCPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}