using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Connectivity;

using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngines
{
    class CheckElementVisibleWebDriverCommand : WebDriverCommand
    {
        string m_sElementName;

        public CheckElementVisibleWebDriverCommand(string elementName) : base("CheckElementVisible")
        {
            m_sElementName = elementName;
        }

        override protected bool CommandLogic()
        {
            try
            {
                
                WebDriver.WebDriverElement element = m_webElementsDictionary.GetElement(m_sElementName.ToLower());
                if (element == null || element.GetIWebElement().Displayed == false)
                {
                    m_variableDictionary.SetVariable("systemResultString", new StringVariable("false"));
                    return true;
                }
                m_variableDictionary.SetVariable("systemResultString", new StringVariable("true"));
            }
            catch(Exception e)
            {
                m_variableDictionary.SetVariable("systemResultString", new StringVariable("false"));
                return false;
            }

            return true;
        }

        override public Command Clone()
        {
            return new CheckElementVisibleWebDriverCommand(m_sElementName);
        }
    }
}
