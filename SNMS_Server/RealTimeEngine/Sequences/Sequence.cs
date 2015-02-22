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
        List<Command> m_commandList;

        public Sequence(    string sSequenceName,
                            VariableDictionary varDict,
                            WebElementsDictionary webElementDict,
                            WebDriver webDriver )
        {
            m_sSequenceName = sSequenceName;
            m_variableDictionary = varDict;
            m_webDriverElementDictionary = webElementDict;
            m_webDriver = webDriver;
            m_commandList = new List<Command>();
        }

        void Add(Command command)
        {
            m_commandList.Add(command);
        }

        public void AddCommand(StringCommand command)
        {
            command.SetVariableDictionary(m_variableDictionary);
            Add(command);
        }

        public void AddCommand(WebDriverCommand command)
        {
            command.SetWebDriver(m_webDriver);
            command.SetWebElementsDictionary(m_webDriverElementDictionary);
            command.SetVariableDictionary(m_variableDictionary);
            Add(command);
        }

        public void AddCommand(GeneralCommand command)
        {
            command.SetVariableDictionary(m_variableDictionary);
            Add(command);
        }

        public void Run(ref string sErrorString)
        {
            foreach (Command cmd in m_commandList)
            {
                if (!cmd.Execute())
                {
                    sErrorString = sErrorString + "error on command " + cmd.GetSubType() + " on sequence " + m_sSequenceName + " ; ";
                }
            }
        }
    }
}
