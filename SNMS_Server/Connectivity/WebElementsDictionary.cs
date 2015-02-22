using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.Connectivity
{
    class WebElementsDictionary
    {
        Dictionary<string, WebDriver.WebDriverElement> m_dictionary;

        public WebElementsDictionary()
        {
            m_dictionary = new Dictionary<string,WebDriver.WebDriverElement>();
        }

        public void Clear()
        {
            m_dictionary = new Dictionary<string, WebDriver.WebDriverElement>();
        }

        public bool ElementExists(string elementName)
        {
            return m_dictionary.ContainsKey(elementName);
        }

        public void SetElement(string elementName, WebDriver.WebDriverElement element)
        {
            m_dictionary[elementName] = element;
        }

        public WebDriver.WebDriverElement GetElement(string elementName)
        {
            if (!ElementExists(elementName))
            {
                return null;
            }

            return m_dictionary[elementName];
        }
    }
}
