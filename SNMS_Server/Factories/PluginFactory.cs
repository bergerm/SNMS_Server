using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using SNMS_Server.Plugins;
using SNMS_Server.Logging;
using System.Net.Sockets;

using SNMS_Server.Connection;

namespace SNMS_Server.Factories
{
    class PluginFactory
    {
        static void DeleteFolderContents(string sFolderPath)
        {
            if (Directory.Exists(sFolderPath))
            {
                Directory.Delete(sFolderPath, true);
            }
            Directory.CreateDirectory(sFolderPath);
        }

        public static bool DownloadPlugins(NetworkStream stream, string sPluginFilesFolderPath, ref List<int> pluginIdsList, ref List<string> pluginFilePathsList)
        {
            pluginIdsList = new List<int>();
            pluginFilePathsList = new List<string>();

            DeleteFolderContents(sPluginFilesFolderPath);
            
            ProtocolMessage getPluginsMessage = new ProtocolMessage();
            getPluginsMessage.SetMessageType(ProtocolMessageType.PROTOCOL_MESSAGE_GET_PLUGINS_WITH_BLOBS);

            ConnectionHandler.SendMessage(stream, getPluginsMessage);

            ProtocolMessage response = ConnectionHandler.GetMessage(stream);

            int numOfPlugins = response.GetParameterAsInt(0);

            int dwCurrentPlugin = 0;

            for (dwCurrentPlugin = 0; dwCurrentPlugin < numOfPlugins; dwCurrentPlugin++)
            {
                int dwPluginParameterOffset = 1 + dwCurrentPlugin * 5;

                int pluginID = response.GetParameterAsInt(dwPluginParameterOffset);

                string pluginName = response.GetParameterAsString(dwPluginParameterOffset + 1);

                string pluginDescription = response.GetParameterAsString(dwPluginParameterOffset + 2);

                bool pluginEnabled = response.GetParameterAsBool(dwPluginParameterOffset + 3);

                byte[] pluginFile = null;
                response.GetParameter(ref pluginFile, dwPluginParameterOffset + 4);

                string path = sPluginFilesFolderPath + pluginName + DateTime.Now.Ticks.ToString() + ".xml";

                File.WriteAllBytes(path, pluginFile);

                pluginIdsList.Add(pluginID);
                pluginFilePathsList.Add(path);
            }

            return true;
        }

        public static List<Plugin> Build(string sPluginFilesFolderPath, List<int> pluginIdsList, List<string> pluginFilePathsList)
        {
            // TODO: Should download all plugins from DB here and load them instead of this.
            //List<int> pluginIDs = new List<int>();
            //pluginIDs.Add(6);
            //List<string> pluginPaths = new List<string>();
            //pluginPaths.Add("..\\..\\WorkingPlugins\\FacebookPlugin.xml");
            
            List<Plugin> pluginList = new List<Plugin>();

            Logger logger = Logger.Instance();

            int index = 0;
            foreach (string filePath in pluginFilePathsList)
            {
                string sErrorString = "";
                Plugin plugin = PluginBuilder.Build(pluginIdsList[index], filePath, ref sErrorString);

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
