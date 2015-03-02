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
        int m_dwDecreaseValue;

        public DecreaseVariableCommand(string sVarName, int dwIncreaseValue)
            : base("DecreaseVariable")
        {
            m_sVarName = sVarName;
            m_dwDecreaseValue = dwIncreaseValue;
        }

        protected override bool CommandLogic()
        {
            Variable tempVar = m_variableDictionary.GetVariable(m_sVarName);
            if (tempVar == null || tempVar.GetVarType()!="integer")
            {
                return false;
            }

            ((IntVariable)tempVar).Decrease(m_dwDecreaseValue);
            m_variableDictionary.SetVariable(m_sVarName, tempVar);
            return true;
        }

        override public Command Clone()
        {
            return new DecreaseVariableCommand(m_sVarName, m_dwDecreaseValue);
        }

    }
}
