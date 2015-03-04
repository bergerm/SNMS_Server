using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.RealTimeEngines
{
    class GoToWebDriverCommand : WebDriverCommand
    {
        string m_sDestination;
        bool m_bIsVariable;

        public GoToWebDriverCommand(string destination, bool bIsVariable) : base("GoTo")
        {
            m_sDestination = destination;
            m_bIsVariable = bIsVariable;
        }

        override protected bool CommandLogic()
        {
            string sDest;
            if (m_bIsVariable)
            {
                sDest = m_variableDictionary.GetVariable(m_sDestination).GetString();
            }
            else
            {
                sDest = m_sDestination;
            }

            m_webDriver.GoTo(sDest);
            return true;
        }

        override public Command Clone()
        {
            return new GoToWebDriverCommand(m_sDestination, m_bIsVariable);
        }
    }
}
