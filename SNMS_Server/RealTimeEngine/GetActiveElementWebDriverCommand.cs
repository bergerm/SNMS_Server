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
        string m_destinationElementName;

        public GetActiveElementWebDriverCommand(string destinationElementName) : base("GetActiveElement")
        {
            m_destinationElementName = destinationElementName;
        }

        override protected bool CommandLogic()
        {
            WebDriver.WebDriverElement tempElement = null;

            tempElement = m_webDriver.GetActiveElement();

            if (tempElement == null)
            {
                return false;
            }

            m_webElementsDictionary.SetElement(m_destinationElementName,tempElement);
            return true;
        }
    }
}
