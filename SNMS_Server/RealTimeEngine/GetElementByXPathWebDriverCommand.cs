﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Connectivity;

namespace SNMS_Server.RealTimeEngine
{
    class GetElementByXPathWebDriverCommand : WebDriverCommand
    {
        string m_parentElementName;
        string m_destinationElementName;
        string m_sId;

        public GetElementByXPathWebDriverCommand(string parentElementName,
                                                string destinationElementName,
                                                String Id) : base("GetElementByXPath")
        {
            m_parentElementName = parentElementName;
            m_destinationElementName = destinationElementName;
            m_sId = Id;
        }

        override protected bool CommandLogic()
        {
            WebDriver.WebDriverElement tempElement = null;

            if ("" != m_parentElementName)
            {
                tempElement = m_webDriver.GetElementByXpath(m_webElementsDictionary.GetElement(m_parentElementName), m_sId);
            }
            else
            {
                tempElement = m_webDriver.GetElementByXpath(null, m_sId);
            }

            m_webElementsDictionary.SetElement(m_destinationElementName,tempElement);
            return true;
        }
    }
}
