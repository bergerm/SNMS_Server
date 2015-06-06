using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngines
{
    class SetVariableStringCommand : StringCommand
    {
        public SetVariableStringCommand(string destination,
                                        string source,
                                        bool isVariable) :
            base("SetVariable", destination, source, isVariable, "", false)
        {
        }

        protected override bool CommandLogic()
        {
            string temp = GetSource1();
            IntStrVariable newVar = new IntStrVariable(temp);
            m_variableDictionary.SetVariable(m_sDestination, newVar);

            return true;
        }

        override public Command Clone()
        {
            return new SetVariableStringCommand(m_sDestination, m_sSource1, m_bSource1IsVariable);
        }
    }
}
