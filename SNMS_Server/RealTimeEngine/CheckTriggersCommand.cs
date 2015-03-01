using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using SNMS_Server.Plugins;
using SNMS_Server.RealTimeEngine.Sequences;
using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngine
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

            Sequence reactionSequence = m_sequence.GetPlugin().GetSequence("reactToWallPost");

            string sErrorString = "";

            reactionSequence.Run(ref sErrorString);

            System.Console.WriteLine(sErrorString);

            return true;
        }

        public virtual Command Clone()
        {
            return new CheckTriggersCommand(m_sTriggersType, m_sReaction);
        }
    }
}
