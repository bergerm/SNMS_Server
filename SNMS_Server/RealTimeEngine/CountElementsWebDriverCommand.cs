using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Connectivity;
using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngines
{
    class CountElementsWebDriverCommand : WebDriverCommand
    {
        string m_sParentElementName;
        string m_sDestinationVariable;
        string m_sXpath;

        public CountElementsWebDriverCommand(string parentElementName,
                                                string destinationVariable,
                                                String Xpath) : base("CountElements")
        {
            m_sParentElementName = parentElementName;
            m_sDestinationVariable = destinationVariable;
            m_sXpath = Xpath;
        }

        override protected bool CommandLogic()
        {
            WebDriver.WebDriverElement parentElement = null;

            try
            {
                if ("" != m_sParentElementName)
                {
                    parentElement = m_webElementsDictionary.GetElement(m_sParentElementName);
                }
                else
                {
                    parentElement = null;
                }

                int count = m_webDriver.CountElements(parentElement, m_sXpath);

                if (count > 0)
                {
                    m_variableDictionary.SetVariable("systemResultString", new StringVariable("true"));
                }
                else
                {
                    m_variableDictionary.SetVariable("systemResultString", new StringVariable("false"));
                }

                m_variableDictionary.SetVariable(m_sDestinationVariable, new IntVariable(count));
            }
            catch (Exception e)
            {
                m_variableDictionary.SetVariable("systemResultString", new StringVariable("false"));
                m_variableDictionary.SetVariable(m_sDestinationVariable, new IntVariable(0));
            }

            return true;
        }

        override public Command Clone()
        {
            return new CountElementsWebDriverCommand(m_sParentElementName, m_sDestinationVariable, m_sXpath);
        }
    }
}
