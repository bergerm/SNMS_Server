using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Connectivity;
using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngine
{
    class TypeElementWebDriverCommand : WebDriverCommand
    {
        string m_destinationElementName;
        string m_string;
        bool m_bStringIsVariable;

        public TypeElementWebDriverCommand( string destinationElementName,
                                            string str,
                                            bool isVariable ) : base("TypeElement")
        {
            m_destinationElementName = destinationElementName;
            m_string = str;
            m_bStringIsVariable = isVariable;
        }

        override protected bool CommandLogic()
        {
            if (m_webElementsDictionary.GetElement(m_destinationElementName) == null)
            {
                return false;
            }

            string sTempString;

            if (m_bStringIsVariable)
            {
                sTempString = (m_variableDictionary.GetVariable(m_string)).GetString();
            }
            else
            {
                sTempString = m_string;
            }

            sTempString = sTempString.Replace("\\n", "\n");

            m_webElementsDictionary.GetElement(m_destinationElementName).Type(sTempString);
            return true;
        }
    }
}
