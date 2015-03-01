using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Connectivity;

namespace SNMS_Server.RealTimeEngine
{
    class ClickElementWebDriverCommand : WebDriverCommand
    {
        string m_sDestinationElementName;

        public ClickElementWebDriverCommand( string destinationElementName ) : base("ClickElement")
        {
            m_sDestinationElementName = destinationElementName;
        }

        override protected bool CommandLogic()
        {
            if (m_webElementsDictionary.GetElement(m_sDestinationElementName.ToLower()) == null)
            {
                return false;
            }

            m_webElementsDictionary.GetElement(m_sDestinationElementName.ToLower()).Click();
            return true;
        }

        public virtual Command Clone()
        {
            return new ClickElementWebDriverCommand(m_sDestinationElementName);
        }
    }
}
