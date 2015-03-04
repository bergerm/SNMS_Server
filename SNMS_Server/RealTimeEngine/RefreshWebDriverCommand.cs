using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.RealTimeEngines
{
    class RefreshWebDriverCommand : WebDriverCommand
    {
        public RefreshWebDriverCommand() : base("Refresh")
        {
        }

        override protected bool CommandLogic()
        {
            m_webDriver.Refresh();
            return true;
        }

        override public Command Clone()
        {
            return new RefreshWebDriverCommand();
        }
    }
}
