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
            Plugin plugin = m_sequence.GetPlugin();
            Sequence sequence = plugin.GetSequence(m_sSequenceName);
            if (sequence == null)
            {
                return false;
            }

            string sErrorString = "";

            sequence.Run(ref sErrorString);
            if (sErrorString == "")
            {
                return true;
            }

            System.Console.WriteLine(sErrorString);
            return false;
        }
    }
}
