using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using SNMS_Server.Plugins;

namespace SNMS_Server.Factories
{
    class PluginFactory
    {
        static List<Plugin> Build(string sPluginFilesFolderPath)
        {
            // TODO: Should download all plugins from DB here

            string[] filePaths = Directory.GetFiles(sPluginFilesFolderPath);

            string sErrorString = "";

            List<Plugin> pluginList = new List<Plugin>();

            foreach (string filePath in filePaths)
            {
                Plugin plugin = PluginBuilder.Build(filePath, ref sErrorString);

                if (plugin != null)
                {
                    pluginList.Add(plugin);
                }
            }

            return pluginList;
        }
    }
}
