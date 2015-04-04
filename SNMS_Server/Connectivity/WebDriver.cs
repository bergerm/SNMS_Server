using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using System.Runtime.CompilerServices;

namespace SNMS_Server.Connectivity
{
    class WebDriver : ConnectivityModule
    {
        IWebDriver m_webDriver;
        string m_windowHandle;

        public WebDriver()
        {
            m_webDriver = new FirefoxDriver();
            //m_webDriver.Navigate().GoToUrl("http://www.yahoo.com");
            //GoTo("www.gmail.com");
            m_windowHandle = m_webDriver.WindowHandles[0];
        }

        ~WebDriver()
        {
            try
            {
                m_webDriver.Close();
            }
            catch (Exception e)
            {

            }
        }

        public void BringToFront()
        {
            m_webDriver.SwitchTo().Window(m_windowHandle);
        }

        public void GoTo(string url)
        {
            m_webDriver.Navigate().GoToUrl(url);
        }

        public void GoBack()
        {
            m_webDriver.Navigate().Back();
        }

        public void Refresh()
        {
            m_webDriver.Navigate().Refresh();
        }

        public class WebDriverElement
        {
            IWebElement m_webElement;

            public WebDriverElement(IWebElement element)
            {
                m_webElement = element;
            }

            public void Click()
            {
                m_webElement.Click();
            }

            public void Type(string str)
            {
                m_webElement.SendKeys(str);
            }

            public IWebElement GetIWebElement()
            {
                return m_webElement;
            }

            public WebDriverElement Clone()
            {
                return new WebDriverElement(m_webElement);
            }
        }

        WebDriverElement GetElement(WebDriverElement parentElement, By by)
        {
            if (parentElement == null)
            {
                IWebElement tempElement = m_webDriver.FindElement(by);
                if (null == tempElement)
                {
                    return null;
                }
                return new WebDriverElement(tempElement);
            }

            else
            {
                IWebElement tempElement = parentElement.GetIWebElement().FindElement(by);
                if (null == tempElement)
                {
                    return null;
                }
                return new WebDriverElement(tempElement);
            }
        }

        public WebDriverElement GetElementByCssSelector(WebDriverElement parentElement, string cssSelector)
        {
            return GetElement(parentElement, By.CssSelector(cssSelector));
        }

        public WebDriverElement GetElementByCssSelector(string cssSelector)
        {
            return GetElement(null, By.CssSelector(cssSelector));
        }

        public WebDriverElement GetElementByClassName(WebDriverElement parentElement, string className)
        {
            return GetElement(parentElement, By.ClassName(className));
        }

        public WebDriverElement GetElementByClassName(string className)
        {
            return GetElement(null, By.ClassName(className));
        }

        public WebDriverElement GetElementByTagName(WebDriverElement parentElement, string tagName)
        {
            return GetElement(parentElement, By.TagName(tagName));
        }

        public WebDriverElement GetElementByTagName(string tagName)
        {
            return GetElement(null, By.TagName(tagName));
        }

        public WebDriverElement GetElementByXpath(WebDriverElement parentElement, string xPath)
        {
            return GetElement(parentElement, By.XPath(xPath));
        }

        public WebDriverElement GetElementByXpath(string xPath)
        {
            return GetElement(null, By.XPath(xPath));
        }

        public WebDriverElement GetElementById(WebDriverElement parentElement, string id)
        {
            return GetElement(parentElement, By.Id(id));
        }

        public WebDriverElement GetElementById(string id)
        {
            return GetElement(null, By.Id(id));
        }

        public WebDriverElement GetActiveElement()
        {
            BringToFront();
            return new WebDriverElement(m_webDriver.SwitchTo().ActiveElement());
        }

        public void Close()
        {
            if (m_webDriver == null)
            {
                return;
            }

            m_webDriver.Close();
            m_webDriver = null;
        }
    }
}
