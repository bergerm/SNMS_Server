using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.RealTimeEngines;
using SNMS_Server.Connectivity;
using SNMS_Server.Variables;
using SNMS_Server.RealTimeEngines.Sequences;
using SNMS_Server.Plugins;
using SNMS_Server.Configurations;

namespace SNMS_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string sErrorString = "";

            PluginParser parser = new PluginParser();
            Plugin plugin = parser.ParsePlugin("..\\..\\FacebookPlugin.xml", ref sErrorString);

            if (sErrorString != "")
            {
                System.Console.WriteLine(sErrorString);
                System.Console.ReadLine();
                return;
            }

            sErrorString = "";
            Configuration configuration = new Configuration(1, "testConfigurationForFacebook", plugin);
            configuration.SetVariable("userName", "marmaulucas@gmail.com");
            configuration.SetVariable("password", "chabon1975");
            configuration.RunSequence("login", ref sErrorString);

            configuration.SetVariable("checkWall_tempWallItemMinutesAgoMax", "59");
            
            RealTimeEngine rtEngine = new RealTimeEngine();
            rtEngine.AddConfiguration("testConfigurationForFacebook", configuration);
            rtEngine.SetSchedule("testConfigurationForFacebook", "checkWall", 1);
            //configuration.RunSequence("checkWall", ref sErrorString);

            System.Console.ReadLine();
        }
    }
}
