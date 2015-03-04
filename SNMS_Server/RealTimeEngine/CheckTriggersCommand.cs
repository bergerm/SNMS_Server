using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using SNMS_Server.Plugins;
using SNMS_Server.RealTimeEngines.Sequences;
using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngines
{
    class CheckTriggersCommand : GeneralCommand
    {
        string m_sTriggersType;
        string m_sReaction;

        public CheckTriggersCommand(string sTriggersType, string sReaction)
            : base("CheckTriggers")
        {
            m_sTriggersType = sTriggersType;
            m_sReaction = sReaction;
        }

        override protected bool CommandLogic()
        {
            StringVariable responseVariable = new StringVariable("I am responding!");
            m_variableDictionary.SetVariable("respondToPost_responseString".ToLower(), responseVariable);

            string sErrorString = "";
            m_sequence.GetConfiguration().RunSequence("reactToWallPost", ref sErrorString);

            System.Console.WriteLine(sErrorString);

            return true;
        }

        override public Command Clone()
        {
            return new CheckTriggersCommand(m_sTriggersType, m_sReaction);
        }
    }
}
