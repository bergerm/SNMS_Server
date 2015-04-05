using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using SNMS_Server.Plugins;
using SNMS_Server.Logging;

namespace SNMS_Server.Factories
{
    class PluginFactory
    {
        public static List<Plugin> Build(string sPluginFilesFolderPath)
        {
            // TODO: Should download all plugins from DB here and load them instead of this.
            List<int> pluginIDs = new List<int>();
            pluginIDs.Add(6);
            List<string> pluginPaths = new List<string>();
            pluginPaths.Add("..\\..\\WorkingPlugins\\FacebookPlugin.xml");

            //string[] filePaths = Directory.GetFiles(sPluginFilesFolderPath

            List<Plugin> pluginList = new List<Plugin>();

            Logger logger = Logger.Instance();

            int index = 0;
            foreach (string filePath in pluginPaths)
            {
                string sErrorString = "";
                Plugin plugin = PluginBuilder.Build(pluginIDs[index], filePath, ref sErrorString);

                if (plugin != null)
                {
                    pluginList.Add(plugin);
                }
                else
                {
                    string fileName = Path.GetFileName(filePath);
                    sErrorString = "Error while building PLugin '" + fileName + "' : " + sErrorString;
                    logger.Log(Logger.LOG_TYPE_ERROR, sErrorString);
                }

                index++;
            }

            return pluginList;
        }
    }
}
