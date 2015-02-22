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
        string m_destinationElementName;

        public ClickElementWebDriverCommand( string destinationElementName ) : base("ClickElement")
        {
            m_destinationElementName = destinationElementName;
        }

        override protected bool CommandLogic()
        {
            if (m_webElementsDictionary.GetElement(m_destinationElementName.ToLower()) == null)
            {
                return false;
            }

            m_webElementsDictionary.GetElement(m_destinationElementName.ToLower()).Click();
            return true;
        }
    }
}
