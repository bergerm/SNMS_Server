﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Connectivity;

using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngines
{
    class GetElementByCssSelectorWebDriverCommand : WebDriverCommand
    {
        string m_sParentElementName;
        string m_sDestinationElementName;
        string m_sId;
        bool m_bIdIsVariable;

        public GetElementByCssSelectorWebDriverCommand( string parentElementName,
                                                        string destinationElementName,
                                                        String Id,
                                                        bool bIdIsVariable) : base("GetElementByCssSelector")
        {
            m_sParentElementName = parentElementName;
            m_sDestinationElementName = destinationElementName;
            m_sId = Id;
            m_bIdIsVariable = bIdIsVariable;
        }

        override protected bool CommandLogic()
        {
            WebDriver.WebDriverElement tempElement = null;

            try
            {
                string sElementId = "";
                if (m_bIdIsVariable)
                {
                    sElementId = m_variableDictionary.GetVariable(m_sId).GetString();
                }
                else
                {
                    sElementId = m_sId;
                }

                if ("" != m_sParentElementName)
                {
                    tempElement = m_webDriver.GetElementByCssSelector(m_webElementsDictionary.GetElement(m_sParentElementName), sElementId);
                }
                else
                {
                    tempElement = m_webDriver.GetElementByCssSelector(null, sElementId);
                }

                m_webElementsDictionary.SetElement(m_sDestinationElementName, tempElement);
                m_variableDictionary.SetVariable("systemResultString", new StringVariable("true"));
            }
            catch (Exception e)
            {
                m_webElementsDictionary.SetElement(m_sDestinationElementName, null);
                m_variableDictionary.SetVariable("systemResultString", new StringVariable("false"));
            }

            return true;
        }

        override public Command Clone()
        {
            return new GetElementByCssSelectorWebDriverCommand(m_sParentElementName, m_sDestinationElementName, m_sId, m_bIdIsVariable);
        }
    }
}
