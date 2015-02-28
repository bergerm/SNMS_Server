﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Connectivity;

namespace SNMS_Server.RealTimeEngine
{
    class GetElementByCssSelectorWebDriverCommand : WebDriverCommand
    {
        string m_parentElementName;
        string m_destinationElementName;
        string m_sId;
        bool m_bIdIsVariable;

        public GetElementByCssSelectorWebDriverCommand( string parentElementName,
                                                        string destinationElementName,
                                                        String Id,
                                                        bool bIdIsVariable) : base("GetElementByCssSelector")
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
                tempElement = m_webDriver.GetElementByCssSelector(m_webElementsDictionary.GetElement(m_parentElementName), sElementId);
            }
            else
            {
                tempElement = m_webDriver.GetElementByCssSelector(null, sElementId);
            }

            m_webElementsDictionary.SetElement(m_destinationElementName,tempElement);
            return true;
        }
    }
}