using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using SNMS_Server.Configuations;
using SNMS_Server.RealTimeEngine.Sequences;

namespace SNMS_Server.RealTimeEngine
{
    class CallCommand : GeneralCommand
    {
        string m_sSequenceName;

        public CallCommand(string sequenceName)
            : base("Call")
        {
            m_sSequenceName = sequenceName;
        }

        override protected bool CommandLogic()
        {
            Configuration configuration = m_sequence.GetConfiguration();

            string sErrorString = "";

            configuration.RunSequence(m_sSequenceName, ref sErrorString); 

            if (sErrorString == "")
            {
                return true;
            }

            return false;
        }

        override public Command Clone()
        {
            return new CallCommand(m_sSequenceName);
        }
    }
}
