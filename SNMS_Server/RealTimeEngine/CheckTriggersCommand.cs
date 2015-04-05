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
using SNMS_Server.Logging;

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
            Logger logger = Logger.Instance();

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
                    string sValidationLog = "Trigger " + trigger.GetID() + " was validated";
                    logger.Log(Logger.LOG_TYPE_TRIGGER_VALIDATION, sValidationLog);
                    
                    Sequence reaction = trigger.GetReaction();
                    if (reaction == null)
                    {
                        return true;
                    }
                    string sReactionValue = trigger.GetReactionValue();

                    string sReactionLog = "Reaction " + reaction.GetName() + " will be executed as a result of the trigger validation";
                    logger.Log(Logger.LOG_TYPE_REACTION, sReactionLog);

                    m_variableDictionary.SetVariable("SystemReactionValue".ToLower(), new StringVariable(sReactionValue));
                    configuration.RunSequence(reaction, ref sErrorString, null);

                    if (sErrorString != "")
                    {
                        string sErrorMessage = "The previous is an error on CheckTriggerCommands for triggerType " + m_sTriggersType + " in configuration " + configuration.GetName() + ".";
                        logger.Log(Logger.LOG_TYPE_ERROR_ON_SEQUENCE, sErrorMessage);
                        System.Console.WriteLine(sErrorMessage);
                    }
                }
            }

            return true;
        }

        override public Command Clone()
        {
            return new CheckTriggersCommand(m_sTriggersType, m_sVariableName, m_sReaction);
        }
    }
}
