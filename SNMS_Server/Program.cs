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
using SNMS_Server.Configuations;

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

            Configuration badConfiguration = new Configuration(1, "testConfigurationForFacebook", plugin);
            badConfiguration.SetVariable("userName", "marmaulu@gmail.com");
            badConfiguration.SetVariable("password", "ch975");
            badConfiguration.RunSequence("login", ref sErrorString);

            sErrorString = "";
            Configuration configuration = new Configuration(1, "testConfigurationForFacebook", plugin);
            configuration.SetVariable("userName", "marmaulucas@gmail.com");
            configuration.SetVariable("password", "chabon1975");
            configuration.RunSequence("login", ref sErrorString);

            configuration.SetVariable("checkWall_tempWallItemMinutesAgoMax", "59");
            configuration.RunSequence("checkWall", ref sErrorString);

            sErrorString = "";
            Configuration configuration2 = new Configuration(1, "testConfigurationForFacebook", plugin);
            configuration2.SetVariable("userName", "marmaulucas@gmail.com");
            configuration2.SetVariable("password", "chabon1975");
            badConfiguration.SetVariable("userName", "marmaulu@gmail.com");
            configuration2.RunSequence("login", ref sErrorString);

            configuration2.SetVariable("checkWall_tempWallItemMinutesAgoMax", "59");
            configuration2.RunSequence("checkWall", ref sErrorString);

            System.Console.WriteLine("All sequences are finished!");

            if (sErrorString != "")
            {
                System.Console.WriteLine(sErrorString);
            }

            System.Console.ReadLine();
            
            
            //Sequence login = plugin.GetSequence("login");

            //plugin.SetVariable("userName", "marmaulucas@gmail.com");
            //plugin.SetVariable("password", "chabon1975");

            //login.Run(ref sErrorString);


            //plugin.SetVariable("checkWall_tempWallItemMinutesAgoMax", "59");
            //Sequence checkWall = plugin.GetSequence("checkWall");

            //checkWall.Run(ref sErrorString);


            //System.Console.WriteLine("All sequences are finished!");

            //if (sErrorString != "")
            //{
            //    System.Console.WriteLine(sErrorString);
            //}

            //System.Console.ReadLine();
        }
    }
}
