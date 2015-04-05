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
using SNMS_Server.Factories;
using SNMS_Server.Engine;
using SNMS_Server.Logging;

namespace SNMS_Server
{
    class Program
    {
        const string TCP_HOST = "127.0.0.1";
        const int TCP_PORT = 56824;

        static void Main(string[] args)
        {
            ProtocolMessage connectionMessage = new ProtocolMessage();
            connectionMessage.SetMessageType(ProtocolMessageType.PROTOCOL_MESSAGE_CONNECTION);
            connectionMessage.AddParameter("server");

            TcpClient client = null;
            NetworkStream stream = null;

            try
            {
                client = new TcpClient();
                client.Connect(TCP_HOST, TCP_PORT);

                stream = client.GetStream();
            }
            catch (Exception e)
            {
                Environment.Exit(1);
            }

            ConnectionHandler.SendMessage(stream, connectionMessage);
            ProtocolMessage responseMessage = ConnectionHandler.GetMessage(stream);

            Logger logger = Logger.Instance(stream);
            logger.Log(Logger.LOG_TYPE_START, "Server Started");

            MainEngine mainEngine = new MainEngine();
            mainEngine.Start(stream);

            System.Console.ReadLine();

            mainEngine.Stop();
        }
    }
}
