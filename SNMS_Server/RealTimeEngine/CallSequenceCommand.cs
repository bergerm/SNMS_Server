using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.RealTimeEngine
{
    class CallSequenceCommand : GeneralCommand
    {
        string m_sSequenceName;

        public CallSequenceCommand(string sSequenceName)
            : base("Call")
        {
            m_sSequenceName = sSequenceName;
        }

        override protected bool CommandLogic()
        {
            //Not available yet
            return false;
        }
    }
}
