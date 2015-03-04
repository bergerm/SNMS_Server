using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngines
{
    class IncreaseVariableCommand : GeneralCommand
    {
        string m_sVarName;
        int m_dwIncreaseValue;

        public IncreaseVariableCommand(string sVarName, int dwIncreaseValue)
            : base("IncreaseVariable")
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

            ((IntVariable)tempVar).Increase(m_dwIncreaseValue);
            m_variableDictionary.SetVariable(m_sVarName, tempVar);
            return true;
        }

        override public Command Clone()
        {
            return new IncreaseVariableCommand(m_sVarName, m_dwIncreaseValue);
        }

    }
}
