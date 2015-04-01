using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Plugins;

namespace SNMS_Server.Factories
{
    class PluginBuilder
    {
        public static Plugin Build(int dwPluginID, string sFilePath, ref string sErrorString)
        {
            PluginParser parser = new PluginParser();

            Plugin plugin = parser.ParsePlugin(sFilePath, ref sErrorString);
            plugin.SetID(dwPluginID);

            return plugin;
        }
    }
}
