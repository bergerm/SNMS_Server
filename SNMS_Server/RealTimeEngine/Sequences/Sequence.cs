﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Variables;
using SNMS_Server.RealTimeEngine;
using SNMS_Server.Connectivity;
using SNMS_Server.Plugins;
using SNMS_Server.Configuations;

namespace SNMS_Server.RealTimeEngine.Sequences
{
    class Sequence
    {
        string m_sSequenceName;
        Configuration m_configuration;
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

        SequenceNode Add(Command command, bool isConditional = false)
        {
            int index = m_sequenceNodesList.Count;
            command.SetSequence(this);
            SequenceNode newNode = new SequenceNode(index, command, isConditional);
            m_sequenceNodesList.Add(newNode);

            return newNode;
        }

        public SequenceNode AddCommand(StringCommand command, bool isConditional = false)
        {
            command.SetVariableDictionary(m_variableDictionary);
            return Add(command, isConditional);
        }

        public SequenceNode AddCommand(WebDriverCommand command, bool isConditional = false)
        {
            command.SetWebDriver(m_webDriver);
            command.SetWebElementsDictionary(m_webDriverElementDictionary);
            command.SetVariableDictionary(m_variableDictionary);
            return Add(command, isConditional);
        }

        public SequenceNode AddCommand(GeneralCommand command, bool isConditional = false)
        {
            command.SetVariableDictionary(m_variableDictionary);
            return Add(command, isConditional);
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
                //if (cmd.GetCommandType() == "WebDriverCommand")
                if (cmd is WebDriverCommand)
                {
                    ((WebDriverCommand)cmd).SetWebDriver(newWebDriver);
                    ((WebDriverCommand)cmd).SetWebElementsDictionary(newDict);
                }
            }
        }

        public void SetConfiguration(Configuration configuration)
        {
            m_configuration = configuration;
        }

        public Configuration GetConfiguration()
        {
            return m_configuration;
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
                    string sResultString = currentNode.GetCommand().GetVariableDictionary().GetVariable("systemResultString").GetString().ToLower();
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

                SequenceNode newNode = null;

                if (newCommand is StringCommand)
                {
                    newNode = newSequence.AddCommand((StringCommand)newCommand, isConditional);
                }
                else if (newCommand is WebDriverCommand)
                {
                    newNode = newSequence.AddCommand((WebDriverCommand)newCommand, isConditional);
                }
                else if (newCommand is GeneralCommand)
                {
                    newNode = newSequence.AddCommand((GeneralCommand)newCommand, isConditional);
                }
                newNode.SetNextNode(node.GetNextNode(true), true);
                newNode.SetNextNode(node.GetNextNode(false), false);
            }

            return newSequence;
        }
    }
}
