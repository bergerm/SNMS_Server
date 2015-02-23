using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngine
{
    class CompareVariableCommand : GeneralCommand
    {
        string m_sVar1Name;
        string m_sVar2Name;

        public CompareVariableCommand(string sVar1Name, string sVar2Name)
            : base("CompareVariable")
        {
            m_sVar1Name = sVar1Name;
            m_sVar2Name = sVar2Name;
        }

        protected override bool CommandLogic()
        {
            Variable tempVar1 = m_variableDictionary.GetVariable(m_sVar1Name);
            if (tempVar1 == null)
            {
                return false;
            }

            Variable tempVar2 = m_variableDictionary.GetVariable(m_sVar2Name);
            if (tempVar2 == null)
            {
                return false;
            }

            string sResult;
            if (tempVar1.GetString() == tempVar2.GetString())
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
    }
}
