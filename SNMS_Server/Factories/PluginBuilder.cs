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
        public static Plugin Build(string sFilePath, ref string sErrorString)
        {
            PluginParser parser = new PluginParser();

            return parser.ParsePlugin(sFilePath, ref sErrorString);
        }
    }
}
