using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngine
{
    class DecreaseVariableCommand : GeneralCommand
    {
        string m_sVarName;
        int m_dwIncreaseValue;

        public DecreaseVariableCommand(string sVarName, int dwIncreaseValue)
            : base("DecreaseVariable")
        {
            m_sVarName = sVarName;
            m_dwIncreaseValue = dwIncreaseValue;
        }

        protected override bool CommandLogic()
        {
            Variable tempVar = m_variableDictionary.GetVariable(m_sVarName);
            if (tempVar == null || tempVar.GetVarType()!="integer")
            {
                return false;
            }

            ((IntVariable)tempVar).Decrease(m_dwIncreaseValue);
            m_variableDictionary.SetVariable(m_sVarName, tempVar);
            return true;
        }
    }
}
