using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using SNMS_Server.Plugins;
using SNMS_Server.RealTimeEngines.Sequences;
using SNMS_Server.Configurations;
using SNMS_Server.Variables;
using SNMS_Server.Triggers;

namespace SNMS_Server.RealTimeEngines
{
    class CheckTriggersCommand : GeneralCommand
    {
        string m_sTriggersType;
        string m_sVariableName;
        string m_sReaction;

        public CheckTriggersCommand(string sTriggersType, string sVariableName, string sReaction)
            : base("CheckTriggers")
        {
            m_sTriggersType = sTriggersType;
            m_sVariableName = sVariableName;
            m_sReaction = sReaction;
        }

        override protected bool CommandLogic()
        {
            string sErrorString = "";
            Configuration configuration = m_sequence.GetConfiguration();
            List<Trigger> triggers = configuration.GetTriggers(m_sTriggersType);
            if (triggers == null || triggers.Count == 0)
            {
                return false;
            }

            Variable sourceVariable = configuration.GetVariable(m_sVariableName.ToLower());
            if (sourceVariable == null)
            {
                return false;
            }

            foreach (Trigger trigger in triggers)
            {
                bool result = trigger.Validate(sourceVariable.GetString());
                if (result)
                {
                    Sequence reaction = trigger.GetReaction();
                    string sReactionValue = trigger.GetReactionValue();

                    m_variableDictionary.SetVariable("SystemReactionValue".ToLower(), new StringVariable(sReactionValue));
                    configuration.RunSequence(reaction, ref sErrorString);

                    if (sErrorString != "")
                    {
                        System.Console.WriteLine("The above is an error on CheckTriggerCommands for triggerType " + m_sTriggersType + " in configuration " + configuration.GetName() + ".");
                    }
                }
            }
            /*StringVariable responseVariable = new StringVariable("I am responding!");
            m_variableDictionary.SetVariable("respondToPost_responseString".ToLower(), responseVariable);

            string sErrorString = "";
            m_sequence.GetConfiguration().RunSequence("reactToWallPost", ref sErrorString);

            System.Console.WriteLine(sErrorString);*/

            return true;
        }

        override public Command Clone()
        {
            return new CheckTriggersCommand(m_sTriggersType, m_sVariableName, m_sReaction);
        }
    }
}
