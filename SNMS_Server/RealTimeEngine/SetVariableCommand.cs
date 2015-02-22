using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngine
{
    class SetVariableCommand : GeneralCommand
    {
        string m_sVarName;
        string m_sVarValue;

        public SetVariableCommand(string sVarName, string sVarValue) : base ("SetVariable")
        {
            m_sVarName = sVarName;
            m_sVarValue = sVarValue;
        }

        protected override bool CommandLogic()
        {
            Variable tempVar = m_variableDictionary.GetVariable(m_sVarName);
            if (tempVar == null)
            {
                return false;
            }

            tempVar.SetVariable(m_sVarValue);
            m_variableDictionary.SetVariable(m_sVarName, tempVar);
            return true;
        }
    }
}
