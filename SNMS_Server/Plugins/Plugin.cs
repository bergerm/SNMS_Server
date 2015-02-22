using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using SNMS_Server.Variables;
using SNMS_Server.Connectivity;
using SNMS_Server.RealTimeEngine.Sequences;

namespace SNMS_Server.Plugins
{
    class Plugin
    {
        int m_dwPluginId;
        string m_sPluginName;
        string m_sPluginVersion;

        VariableDictionary m_variableDictionary;
        WebElementsDictionary m_webElementDictionary;
        WebDriver m_webDriver;

        SequenceDictionary m_sequenceDictionary;

        XmlDocument m_xmlDoc;

        public Plugin()
        {
            m_variableDictionary = new VariableDictionary();
            m_webElementDictionary = new WebElementsDictionary();
            m_webDriver = new WebDriver();

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
            m_variableDictionary.SetVariable(sVarName, var);
        }

        public Variable GetVariable(string sVarName)
        {
           return m_variableDictionary.GetVariable(sVarName);
        }

        public bool SetVariable(string sVarName, string sVarValue)
        {
            Variable tempVar = m_variableDictionary.GetVariable(sVarName.ToLower());
            if(tempVar == null)
            {
                return false;
            }

            tempVar.SetVariable(sVarValue);
            m_variableDictionary.SetVariable(sVarName, tempVar);
            return true;
        }

        public VariableDictionary GetVariableDictionary()
        {
            return m_variableDictionary;
        }

        public WebDriver GetWebDriver()
        {
            return m_webDriver;
        }

        public WebElementsDictionary GetWebElementsDictionary()
        {
            return m_webElementDictionary;
        }

        public WebDriver.WebDriverElement GetWebElement(string sElementName)
        {
            return m_webElementDictionary.GetElement(sElementName);
        }

        public void AddSequence(string sName, Sequence sequence)
        {
            m_sequenceDictionary.SetSequence(sName, sequence);
        }

        public Sequence GetSequence(string sSequenceName)
        {
            return m_sequenceDictionary.GetSequence(sSequenceName);
        }
    }
}
