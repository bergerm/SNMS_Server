using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using SNMS_Server.Plugins;
using SNMS_Server.RealTimeEngine.Sequences;

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
            return false;
        }
    }
}
