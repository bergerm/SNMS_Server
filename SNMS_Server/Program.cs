using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

using SNMS_Server.RealTimeEngines;
using SNMS_Server.Connectivity;
using SNMS_Server.Variables;
using SNMS_Server.RealTimeEngines.Sequences;
using SNMS_Server.Plugins;
using SNMS_Server.Configurations;
using SNMS_Server.Connection;

namespace SNMS_Server
{
    class Program
    {
        const int TCP_PORT = 56824;

        static void Main(string[] args)
        {
            string sErrorString = "";

            ProtocolMessage connectionMessage = new ProtocolMessage();
            connectionMessage.SetMessageType(ProtocolMessageType.PROTOCOL_MESSAGE_CONNECTION);
            connectionMessage.AddParameter("server");

            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", TCP_PORT);

            NetworkStream stream = client.GetStream();

            ConnectionHandler.SendMessage(stream, connectionMessage);
            ProtocolMessage responseMessage = ConnectionHandler.GetMessage(stream);

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
