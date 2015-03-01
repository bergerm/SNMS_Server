using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Connectivity;

namespace SNMS_Server.RealTimeEngine
{
    class GetActiveElementWebDriverCommand : WebDriverCommand
    {
        string m_sDestinationElementName;

        public GetActiveElementWebDriverCommand(string destinationElementName) : base("GetActiveElement")
        {
            m_sDestinationElementName = destinationElementName;
        }

        override protected bool CommandLogic()
        {
            WebDriver.WebDriverElement tempElement = null;

            tempElement = m_webDriver.GetActiveElement();

            if (tempElement == null)
            {
                return false;
            }

            m_webElementsDictionary.SetElement(m_sDestinationElementName,tempElement);
            return true;
        }

        public virtual Command Clone()
        {
            return new GetActiveElementWebDriverCommand(m_sDestinationElementName);
        }
    }
}
