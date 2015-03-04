using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;

using SNMS_Server.Connectivity;

namespace SNMS_Server.RealTimeEngines
{
    abstract class WebDriverCommand : Command
    {
        protected WebDriver m_webDriver;
        protected WebElementsDictionary m_webElementsDictionary;

        public WebDriverCommand(string subType) : base("WebDriverCommand", subType)
        {
            m_webDriver = null;
        }

        public void SetWebElementsDictionary(WebElementsDictionary dictionary)
        {
            m_webElementsDictionary = dictionary;
        }

        public void SetWebDriver(WebDriver driver)
        {
            m_webDriver = driver;
        }

        override public bool Execute()
        {
            if (m_webDriver == null)
            {
                return false;
            }

            return CommandLogic();
        }
    }
}
