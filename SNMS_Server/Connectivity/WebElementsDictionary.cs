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

        public WebElementsDictionary Clone()
        {
            WebElementsDictionary newDict = new WebElementsDictionary();
            foreach (KeyValuePair<string, WebDriver.WebDriverElement> entry in m_dictionary)
            {
                WebDriver.WebDriverElement element = (WebDriver.WebDriverElement)entry.Value;

                if (element == null)
                {
                    newDict.SetElement(entry.Key, null);
                }
                else
                {
                    newDict.SetElement(entry.Key, element.Clone());
                }
            }
            return newDict;
        }
    }
}
