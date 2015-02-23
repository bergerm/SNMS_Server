using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SNMS_Server.RealTimeEngine
{
    class CheckTriggerCommand : GeneralCommand
    {
        public CheckTriggerCommand()
            : base("Sleep")
        {
        }

        override protected bool CommandLogic()
        {
            return false;
        }
    }
}
