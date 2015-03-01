using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Variables;
using SNMS_Server.RealTimeEngine;
using SNMS_Server.Connectivity;
using SNMS_Server.Plugins;

namespace SNMS_Server.RealTimeEngine.Sequences
{
    class Sequence
    {
        string m_sSequenceName;
        Plugin m_plugin;
        VariableDictionary m_variableDictionary;
        WebElementsDictionary m_webDriverElementDictionary;
        WebDriver m_webDriver;
        List<SequenceNode> m_sequenceNodesList;

        public Sequence(    string sSequenceName,
                            VariableDictionary varDict,
                            WebElementsDictionary webElementDict,
                            WebDriver webDriver )
        {
            m_sSequenceName = sSequenceName;
            m_variableDictionary = varDict;
            m_webDriverElementDictionary = webElementDict;
            m_webDriver = webDriver;
            m_sequenceNodesList = new List<SequenceNode>();
        }

        void Add(Command command, bool isConditional = false)
        {
            int index = m_sequenceNodesList.Count;
            command.SetSequence(this);
            m_sequenceNodesList.Add(new SequenceNode(index, command, isConditional));
        }

        public void AddCommand(StringCommand command, bool isConditional = false)
        {
            command.SetVariableDictionary(m_variableDictionary);
            Add(command, isConditional);
        }

        public void AddCommand(WebDriverCommand command, bool isConditional = false)
        {
            command.SetWebDriver(m_webDriver);
            command.SetWebElementsDictionary(m_webDriverElementDictionary);
            command.SetVariableDictionary(m_variableDictionary);
            Add(command, isConditional);
        }

        public void AddCommand(GeneralCommand command, bool isConditional = false)
        {
            command.SetVariableDictionary(m_variableDictionary);
            Add(command, isConditional);
        }

        public void UpdateSequenceNodeNextNodeValue(int dwNodeIndex, bool bCondition, int dwNewNextNode)
        {
            m_sequenceNodesList[dwNodeIndex].SetNextNode(dwNewNextNode, bCondition);
        }

        public void UpdateCommandsVariableDictionary(VariableDictionary newDict)
        {
            foreach (SequenceNode node in m_sequenceNodesList)
            {
                node.GetCommand().SetVariableDictionary(newDict);
            }
        }

        public void UpdateCommandsWebDriver(WebDriver newWebDriver, WebElementsDictionary newDict)
        {
            foreach (SequenceNode node in m_sequenceNodesList)
            {
                Command cmd = node.GetCommand();
                if (cmd.GetCommandType() == "WebDriver")
                {
                    ((WebDriverCommand)cmd).SetWebDriver(newWebDriver);
                    ((WebDriverCommand)cmd).SetWebElementsDictionary(newDict);
                }
            }
        }

        public void SetPlugin(Plugin plugin)
        {
            m_plugin = plugin;
        }

        public Plugin GetPlugin()
        {
            return m_plugin;
        }

        public void Run(ref string sErrorString)
        {
            if (m_sequenceNodesList.Count <= 0)
            {
                sErrorString = sErrorString + "Sequence " + m_sSequenceName + " is empty! - ABORTED; ";
                return;
            }
            SequenceNode currentNode = m_sequenceNodesList[0];

            while (currentNode != null)
            {
                Command cmd = currentNode.GetCommand();
                bool bConditionResult = true;

                if (!cmd.Execute())
                {
                    sErrorString = sErrorString + "error on command " + cmd.GetSubType() + " (index " + currentNode.GetIndex() + ") on sequence " + m_sSequenceName + " ; ";
                    return;
                }

                if (currentNode.IsNodeConditional())
                {
                    string sResultString = m_variableDictionary.GetVariable("systemResultString").GetString().ToLower();
                    if (sResultString == "false")
                    {
                        bConditionResult = false;
                    }
                }

                int dwNextNodeIndex = currentNode.GetNextNode(bConditionResult);

                if (dwNextNodeIndex < 0 || dwNextNodeIndex >= m_sequenceNodesList.Count)
                {
                    currentNode = null;
                }
                else
                {
                    currentNode = m_sequenceNodesList[dwNextNodeIndex];
                }
            }
        }

        public Sequence Clone()
        {
            Sequence newSequence = new Sequence(m_sSequenceName, m_variableDictionary.Clone(), m_webDriverElementDictionary.Clone(), null);
            foreach (SequenceNode node in m_sequenceNodesList)
            {
                Command newCommand = node.GetCommand().Clone();
                bool isConditional = node.IsNodeConditional();

                if (newCommand is StringCommand)
                {
                    newSequence.AddCommand((StringCommand)newCommand, isConditional);
                }
                else if (newCommand is WebDriverCommand)
                {
                    newSequence.AddCommand((WebDriverCommand)newCommand, isConditional);
                }
                else if (newCommand is GeneralCommand)
                {
                    newSequence.AddCommand((GeneralCommand)newCommand, isConditional);
                }
            }

            return newSequence;
        }
    }
}
