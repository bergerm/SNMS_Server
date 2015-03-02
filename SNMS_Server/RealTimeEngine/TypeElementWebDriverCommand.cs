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
        string m_sDestinationElementName;
        string m_sString;
        bool m_bStringIsVariable;

        public TypeElementWebDriverCommand( string destinationElementName,
                                            string str,
                                            bool isVariable ) : base("TypeElement")
        {
            m_sDestinationElementName = destinationElementName;
            m_sString = str;
            m_bStringIsVariable = isVariable;
        }

        override protected bool CommandLogic()
        {
            if (m_webElementsDictionary.GetElement(m_sDestinationElementName) == null)
            {
                return false;
            }

            string sTempString;

            if (m_bStringIsVariable)
            {
                sTempString = (m_variableDictionary.GetVariable(m_sString)).GetString();
            }
            else
            {
                sTempString = m_sString;
            }

            sTempString = sTempString.Replace("\\n", "\n");

            m_webElementsDictionary.GetElement(m_sDestinationElementName).Type(sTempString);
            return true;
        }

        override public Command Clone()
        {
            return new TypeElementWebDriverCommand(m_sDestinationElementName, m_sString, m_bStringIsVariable);
        }
    }
}
