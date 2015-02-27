using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngine
{
    class GreaterThanCommand : GeneralCommand
    {
        string m_sSource1;
        bool m_bIsSource1Variable;
        string m_sSource2;
        bool m_bIsSource2Variable;

        public GreaterThanCommand(string sSource1, bool bIsSource1Variable, string sSource2, bool bIsSource2Variable)
            : base("GreaterThan")
        {
            m_sSource1 = sSource1;
            m_bIsSource1Variable = bIsSource1Variable;
            m_sSource2 = sSource2;
            m_bIsSource2Variable = bIsSource2Variable;
        }

        protected override bool CommandLogic()
        {
            int num1, num2;

            if (m_bIsSource1Variable)
            {
                Variable tempVar = m_variableDictionary.GetVariable(m_sSource1);
                if (tempVar == null)
                {
                    return false;
                }

                num1 = Int32.Parse(tempVar.ToString());
            }
            else
            {
                num1 = Int32.Parse(m_sSource1);
            }


            if (m_bIsSource2Variable)
            {
                Variable tempVar = m_variableDictionary.GetVariable(m_sSource2);
                if (tempVar == null)
                {
                    return false;
                }

                num2 = Int32.Parse(tempVar.ToString());
            }
            else
            {
                num2 = Int32.Parse(m_sSource2);
            }

            string sResult;
            if (num1 > num2)
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
