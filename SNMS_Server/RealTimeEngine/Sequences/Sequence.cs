using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Variables;
using SNMS_Server.RealTimeEngine;
using SNMS_Server.Connectivity;

namespace SNMS_Server.RealTimeEngine.Sequences
{
    class Sequence
    {
        string m_sSequenceName;
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
            m_sequenceNodesList.Add(new SequenceNode(command, isConditional));
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

        public void Run(ref string sErrorString)
        {
            foreach (SequenceNode node in m_sequenceNodesList)
            {
                Command cmd = node.GetCommand();
                if (!cmd.Execute())
                {
                    sErrorString = sErrorString + "error on command " + cmd.GetSubType() + " on sequence " + m_sSequenceName + " ; ";
                }
            }
        }
    }
}
