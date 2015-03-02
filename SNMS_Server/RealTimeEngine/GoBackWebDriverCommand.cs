using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.RealTimeEngine
{
    class GoBackWebDriverCommand : WebDriverCommand
    {
        public GoBackWebDriverCommand() : base("GoBack")
        {
        }

        override protected bool CommandLogic()
        {
            m_webDriver.GoBack();
            return true;
        }

        override public Command Clone()
        {
            return new GoBackWebDriverCommand();
        }
    }
}
