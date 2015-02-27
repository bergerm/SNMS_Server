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

        public CheckTriggersCommand(string sTriggersType)
            : base("Call")
        {
            m_sTriggersType = sTriggersType;
        }

        override protected bool CommandLogic()
        {
            return false;
        }
    }
}
