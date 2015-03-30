﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Connection;

namespace SNMS_Server.Objects
{
    class Plugin
    {
        int m_dwPluginID;
        string m_sName;
        string m_sDescription;
        bool m_bEnabled;

        public Plugin()
        {
            m_dwPluginID = 0;
            m_sName = "";
            m_sDescription = "";
            m_bEnabled = false;
        }

        public void SetID(int dwID) { m_dwPluginID = dwID; }
        public int GetID() { return m_dwPluginID; }

        public void SetName(string sName) {m_sName = sName;}
        public string GetName() { return m_sName; }

        public void SetDescription(string sDescription) { m_sDescription = sDescription; }
        public string GetDescription() { return m_sDescription; }

        public void SetEnabled(bool bEnabled) { m_bEnabled = bEnabled; }
        public bool GetEnabled() { return m_bEnabled; }

        public static List<Plugin> ParseMessage(ProtocolMessage message)
        {
            List<Plugin> pluginList = new List<Plugin>();

            byte[] arr = null;
            message.GetParameter(ref arr, 0);
            int numOfPlugins = BitConverter.ToInt32(arr, 0);

            int dwCurrentPlugin = 0;

            for (dwCurrentPlugin = 0; dwCurrentPlugin < numOfPlugins; dwCurrentPlugin++)
            {
                Plugin plugin = new Plugin();
                int dwPluginParameterOffset = 1 + dwCurrentPlugin * 5;

                message.GetParameter(ref arr, dwPluginParameterOffset);
                plugin.SetID(BitConverter.ToInt32(arr, 0));

                plugin.SetName(message.GetParameterAsString(dwPluginParameterOffset + 1));

                plugin.SetDescription(message.GetParameterAsString(dwPluginParameterOffset + 2));

                message.GetParameter(ref arr, dwPluginParameterOffset + 3);
                int isEnabled = BitConverter.ToInt32(arr, 0);
                plugin.SetEnabled((isEnabled != 0)?true:false);

                pluginList.Add(plugin);
            }

            return pluginList;
        }
    }
}
