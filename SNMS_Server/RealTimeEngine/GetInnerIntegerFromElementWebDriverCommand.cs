using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Connectivity;
using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngine
{
    class GetInnerIntegerFromElementWebDriverCommand : WebDriverCommand
    {
        string m_sSourceElementName;
        string m_sDestinationVariableName;

        public GetInnerIntegerFromElementWebDriverCommand(  string sSourceElementName,
                                                            string sDestVariableName ) : base("GetInnerIntegerFromElement")
        {
            m_sSourceElementName = sSourceElementName;
            m_sDestinationVariableName = sDestVariableName;
        }

        override protected bool CommandLogic()
        {
            if (m_webElementsDictionary.GetElement(m_sSourceElementName) == null)
            {
                return false;
            }

            WebDriver.WebDriverElement element = m_webElementsDictionary.GetElement(m_sSourceElementName);

            if (element == null)
            {
                return false;
            }

            Variable oldVariable = m_variableDictionary.GetVariable(m_sDestinationVariableName);
            if (oldVariable.GetVarType() != "integer")
            {
                return false;
            }

            int sInnerInteger = Int32.Parse(element.GetIWebElement().Text);
            IntVariable tempVariable = new IntVariable(sInnerInteger);

            m_variableDictionary.SetVariable(m_sDestinationVariableName, tempVariable);

            return true;
        }

        override public Command Clone()
        {
            return new GetInnerIntegerFromElementWebDriverCommand(m_sSourceElementName, m_sDestinationVariableName);
        }
    }
}
