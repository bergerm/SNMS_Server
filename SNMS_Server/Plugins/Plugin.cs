using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using SNMS_Server.Variables;
using SNMS_Server.Connectivity;
using SNMS_Server.RealTimeEngines.Sequences;

namespace SNMS_Server.Plugins
{
    class Plugin
    {
        int m_dwPluginId;
        string m_sPluginName;
        string m_sPluginVersion;

        VariableDictionary m_startingVariableDictionary;
        WebElementsDictionary m_startingWebElementDictionary;
        WebDriver m_webDriver;

        SequenceDictionary m_sequenceDictionary;

        List<string> m_StartupSequences;
        List<SequenceTimer> m_timerList;

        XmlDocument m_xmlDoc;

        public Plugin()
        {
            m_startingVariableDictionary = new VariableDictionary();
            m_startingWebElementDictionary = new WebElementsDictionary();
            // m_webDriver = new WebDriver();
            // plugin by itself SHOULD NOT have a web driver
            m_webDriver = null;

            m_sequenceDictionary = new SequenceDictionary();
        }

        public void SetPluginName(string sName)
        {
            m_sPluginName = sName;
        }

        public string GetPluginName()
        {
            return m_sPluginName;
        }

        public void SetPluginVersion(string sVersion)
        {
            m_sPluginVersion = sVersion;
        }

        public string GetPluginVersion()
        {
            return m_sPluginVersion;
        }

        public void AddVariable(string sVarName, Variable var)
        {
            m_startingVariableDictionary.SetVariable(sVarName, var);
        }

        public Variable GetVariable(string sVarName)
        {
           return m_startingVariableDictionary.GetVariable(sVarName);
        }

        public bool SetVariable(string sVarName, string sVarValue)
        {
            Variable tempVar = m_startingVariableDictionary.GetVariable(sVarName.ToLower());
            if(tempVar == null)
            {
                return false;
            }

            tempVar.SetVariable(sVarValue);
            m_startingVariableDictionary.SetVariable(sVarName, tempVar);
            return true;
        }

        public VariableDictionary GetVariableDictionary()
        {
            return m_startingVariableDictionary;
        }

        public VariableDictionary CloneVariableDictionary()
        {
            return m_startingVariableDictionary.Clone();
        }

        public WebDriver GetWebDriver()
        {
            return m_webDriver;
        }

        public WebElementsDictionary GetWebElementsDictionary()
        {
            return m_startingWebElementDictionary;
        }

        public WebElementsDictionary CloneWebElementsDictionary()
        {
            return m_startingWebElementDictionary.Clone();
        }

        public void SetWebElement(string sElementName, WebDriver.WebDriverElement element)
        {
            m_startingWebElementDictionary.SetElement(sElementName, element);
        }

        public WebDriver.WebDriverElement GetWebElement(string sElementName)
        {
            return m_startingWebElementDictionary.GetElement(sElementName);
        }

        public bool WebElementExists(string sElementName)
        {
            return m_startingWebElementDictionary.ElementExists(sElementName);
        }

        public void AddSequence(string sName, Sequence sequence)
        {
            m_sequenceDictionary.SetSequence(sName, sequence);
        }

        public Sequence GetSequence(string sSequenceName)
        {
            return m_sequenceDictionary.GetSequence(sSequenceName);
        }

        public Sequence CloneSequence(string sSequenceName)
        {
            return m_sequenceDictionary.GetSequence(sSequenceName).Clone();
        }

        public void AddTimer(SequenceTimer timer)
        {
            m_timerList.Add(timer);
        }

        public List<SequenceTimer> GetTimers()
        {
            return m_timerList;
        }

        public void AddStartupSequence(string sSequenceName)
        {
            m_StartupSequences.Add(sSequenceName);
        }

        public List<string> GetStartupSequences()
        {
            return m_StartupSequences;
        }
    }
}
