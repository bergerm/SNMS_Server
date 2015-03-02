using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SNMS_Server.RealTimeEngine
{
    class SleepCommand : GeneralCommand
    {
        int m_dwMilliSecs;

        public SleepCommand(int dwMilliSecs) : base("Sleep")
        {
            m_dwMilliSecs = dwMilliSecs;
        }

        override protected bool CommandLogic()
        {
            Thread.Sleep(m_dwMilliSecs);
            return true;
        }

        override public Command Clone()
        {
            return new SleepCommand(m_dwMilliSecs);
        }
    }
}
