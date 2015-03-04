﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Plugins;
using SNMS_Server.Variables;
using SNMS_Server.Connectivity;
using SNMS_Server.RealTimeEngines.Sequences;

namespace SNMS_Server.Configurations
{
    class Configuration
    {
        int m_dwConfigurationId;
        string m_sConfigurationName;

        Plugin m_plugin;

        VariableDictionary m_variableDictionary;
        WebElementsDictionary m_webElementDictionary;
        WebDriver m_webDriver;

        public Configuration(   int dwConfigurationId,
                                string sConfigurationName,
                                Plugin plugin )
        {
            m_dwConfigurationId = dwConfigurationId;
            m_sConfigurationName = sConfigurationName;
            m_plugin = plugin;

            //m_variableDictionary = new VariableDictionary();
            m_variableDictionary = plugin.CloneVariableDictionary();

            m_webElementDictionary = plugin.CloneWebElementsDictionary();
            m_webDriver = new WebDriver();
        }

        public void RunSequence(string sSequenceName, ref string sErrorString)
        {
            Sequence sequence = m_plugin.CloneSequence(sSequenceName);
            sequence.SetConfiguration(this);
            sequence.UpdateCommandsVariableDictionary(m_variableDictionary);
            sequence.UpdateCommandsWebDriver(m_webDriver,m_webElementDictionary);

            sequence.Run(ref sErrorString);

            if (sErrorString != "")
            {
                System.Console.WriteLine("Error on Sequence " + sSequenceName + " on plugin " + m_plugin.GetPluginName() + ":");
                System.Console.WriteLine(sErrorString);
            }
        }

        public Variable GetVariable(string sVarName)
        {
            return m_variableDictionary.GetVariable(sVarName);
        }

        public bool SetVariable(string sVarName, string sVarValue)
        {
            Variable tempVar = m_variableDictionary.GetVariable(sVarName.ToLower());
            if (tempVar == null)
            {
                return false;
            }

            tempVar.SetVariable(sVarValue);
            m_variableDictionary.SetVariable(sVarName, tempVar);
            return true;
        }
    }
}
