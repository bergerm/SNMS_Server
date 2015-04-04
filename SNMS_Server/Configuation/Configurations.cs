using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

using SNMS_Server.Plugins;
using SNMS_Server.Variables;
using SNMS_Server.Connectivity;
using SNMS_Server.RealTimeEngines.Sequences;
using SNMS_Server.Connection;
using SNMS_Server.Triggers;

namespace SNMS_Server.Configurations
{
    class Configuration
    {
        int m_dwID;
        Account m_account;
        string m_sName;
        bool m_bIsEnabled;

        //Plugin m_plugin;

        SequenceDictionary m_sequenceDictionary;
        VariableDictionary m_variableDictionary;
        TriggerGroupDictionary m_triggerGroupDictionary;
        WebElementsDictionary m_webElementDictionary;
        WebDriver m_webDriver;

        public void SetID(int id) { m_dwID = id; }
        public int GetID() { return m_dwID; }

        public void SetAccount(Account account) { m_account = account; }
        public Account GetAccount() { return m_account; }

        public void SetName(string sName) { m_sName = sName; }
        public string GetName() { return m_sName; }

        public void SetEnabled(bool isEnabled) { m_bIsEnabled = isEnabled; }
        public bool GetEnabled() { return m_bIsEnabled; }

        public Configuration(   int dwConfigurationId,
                                string sConfigurationName,
                                Account account,
                                bool isEnabled)
                                //Plugin plugin )
        {
            m_dwID = dwConfigurationId;
            m_sName = sConfigurationName;
            //m_plugin = plugin;
            m_account = account;
            m_bIsEnabled = isEnabled;

            m_sequenceDictionary = account.GetPlugin().CloneSequenceDictionary();

            //m_variableDictionary = plugin.CloneVariableDictionary();
            m_variableDictionary = account.GetPlugin().CloneVariableDictionary();

            //m_webElementDictionary = plugin.CloneWebElementsDictionary();
            m_webElementDictionary = account.GetPlugin().CloneWebElementsDictionary();

            m_triggerGroupDictionary = new TriggerGroupDictionary();

            m_webDriver = new WebDriver();
        }

        ~Configuration()
        {
            m_sequenceDictionary.Clear();
            m_variableDictionary.Clear();
            m_webElementDictionary.Clear();
        }

        public void RunSequence(Sequence sequence, ref string sErrorString, Mutex mutex)
        {
            if (!sequence.GetEnabled())
            {
                return;
            }

            if (mutex != null)
            {
                mutex.WaitOne();
            }

            sequence.SetConfiguration(this);
            sequence.UpdateCommandsVariableDictionary(m_variableDictionary);
            sequence.UpdateCommandsWebDriver(m_webDriver, m_webElementDictionary);

            sequence.Run(ref sErrorString);

            if (sErrorString != "")
            {
                System.Console.WriteLine("Error on Sequence " + sequence.GetName() + " on plugin " + m_account.GetPlugin().GetPluginName() + ":");
                System.Console.WriteLine(sErrorString);
            }

            if (mutex != null)
            {
                mutex.ReleaseMutex();
            }
        }

        public void RunSequence(string sSequenceName, ref string sErrorString, Mutex mutex)
        {
            if (!m_bIsEnabled)
            {
                return;
            }

            //Sequence sequence = m_plugin.CloneSequence(sSequenceName);
            //Sequence sequence = m_account.GetPlugin().CloneSequence(sSequenceName);
            Sequence sequence = m_sequenceDictionary.GetSequence(sSequenceName);
            if (sequence == null)
            {
                return;
            }

            RunSequence(sequence, ref sErrorString, mutex);
        }

        public Sequence GetSequence(string sSequenceName)
        {
            return m_sequenceDictionary.GetSequence(sSequenceName);
        }

        public Sequence GetSequence(int dwID)
        {
            return m_sequenceDictionary.GetSequence(dwID);
        }

        public void SetSequenceStatus(string sSequenceName, bool isEnabled)
        {
            m_sequenceDictionary.GetSequence(sSequenceName).SetEnabled(isEnabled);
        }

        public bool GetSequenceStatus(string sSequenceName)
        {
            return m_sequenceDictionary.GetSequence(sSequenceName).GetEnabled();
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

        public void AddTriggerGroup(TriggerGroup group)
        {
            m_triggerGroupDictionary.AddGroup(group.GetName(), group);
        }

        public List<Trigger> GetTriggers(string sTriggerGroupName)
        {
            TriggerGroup group = m_triggerGroupDictionary.GetTriggerGroup(sTriggerGroupName);
            if (group == null)
            {
                return null;
            }

            return group.GetTriggers();
        }

        public static List<Configuration> ParseMessage(ProtocolMessage message, Account account)
        {
            List<Configuration> configurationList = new List<Configuration>();

            int numOfConfigurations = message.GetParameterAsInt(0);

            int dwCurrentConfiguration = 0;

            for (dwCurrentConfiguration = 0; dwCurrentConfiguration < numOfConfigurations; dwCurrentConfiguration++)
            {
                int dwAccountParameterOffset = 1 + dwCurrentConfiguration * 4;

                int dwID = message.GetParameterAsInt(dwAccountParameterOffset);

                string sName = message.GetParameterAsString(dwAccountParameterOffset + 1);

                bool bIsEnabled = message.GetParameterAsBool(dwAccountParameterOffset + 3);

                Configuration configuration = new Configuration(dwID, sName, account, bIsEnabled);

                configurationList.Add(configuration);
            }

            return configurationList;
        }

        public void CloseWebDriver()
        {
            if (m_webDriver == null)
            {
                return;
            }

            m_webDriver.Close();
        }
    }
}
