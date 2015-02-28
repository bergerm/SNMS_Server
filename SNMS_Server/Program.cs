using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.RealTimeEngine;
using SNMS_Server.Connectivity;
using SNMS_Server.Variables;
using SNMS_Server.RealTimeEngine.Sequences;
using SNMS_Server.Plugins;

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

            Sequence login = plugin.GetSequence("login");

            plugin.SetVariable("userName", "marmaulucas@gmail.com");
            plugin.SetVariable("password", "chabon1975");

            login.Run(ref sErrorString);


            plugin.SetVariable("checkWall_tempWallItemMinutesAgoMax", "59");
            Sequence checkWall = plugin.GetSequence("checkWall");

            checkWall.Run(ref sErrorString);


            System.Console.WriteLine("All sequences are finished!");

            if (sErrorString != "")
            {
                System.Console.WriteLine(sErrorString);
            }

            System.Console.ReadLine();
        }
    }
}
