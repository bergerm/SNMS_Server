using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Connectivity;

namespace SNMS_Server.RealTimeEngine
{
    class GetElementByTagNameWebDriverCommand : WebDriverCommand
    {
        string m_parentElementName;
        string m_destinationElementName;
        string m_sId;
        bool m_bIdIsVariable;

        public GetElementByTagNameWebDriverCommand(string parentElementName,
                                                        string destinationElementName,
                                                        String Id,
                                                        bool bIdIsVariable) : base("GetElementByTagName")
        {
            m_parentElementName = parentElementName;
            m_destinationElementName = destinationElementName;
            m_sId = Id;
            m_bIdIsVariable = bIdIsVariable;
        }

        override protected bool CommandLogic()
        {
            WebDriver.WebDriverElement tempElement = null;

            string sElementId = "";
            if (m_bIdIsVariable)
            {
                sElementId = m_variableDictionary.GetVariable(m_sId).GetString();
            }
            else
            {
                sElementId = m_sId;
            }

            if ("" != m_parentElementName)
            {
                tempElement = m_webDriver.GetElementByTagName(m_webElementsDictionary.GetElement(m_parentElementName), sElementId);
            }
            else
            {
                tempElement = m_webDriver.GetElementByTagName(null, sElementId);
            }

            m_webElementsDictionary.SetElement(m_destinationElementName,tempElement);
            return true;
        }
    }
}
