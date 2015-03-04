using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngines
{
    class VariableContainsCommand : GeneralCommand
    {
        string m_sVarName;
        string m_sSource;
        bool m_bIsSourceVariable;

        public VariableContainsCommand(string sVarName, string sSource, bool bIsSourceVariable)
            : base("VariableContains")
        {
            m_sVarName = sVarName;
            m_sSource = sSource;
            m_bIsSourceVariable = bIsSourceVariable;
        }

        protected override bool CommandLogic()
        {
            string sString;

            Variable variable = m_variableDictionary.GetVariable(m_sVarName);
            if (variable == null)
            {
                return false;
            }

            if (m_bIsSourceVariable)
            {
                Variable tempVar = m_variableDictionary.GetVariable(m_sSource);
                if (tempVar == null)
                {
                    return false;
                }

                sString = tempVar.ToString();
            }
            else
            {
                sString = m_sSource;
            }

            string sResult;
            if (variable.GetString().Contains(sString))
            {
                sResult = "true";
            }
            else
            {
                sResult = "false";
            }

            m_variableDictionary.SetVariable("systemResultString", new StringVariable(sResult));
            return true;
        }

        override public Command Clone()
        {
            return new VariableContainsCommand(m_sVarName, m_sSource, m_bIsSourceVariable);
        }

    }
}
