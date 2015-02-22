using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Variables;

namespace SNMS_Server.RealTimeEngine
{
    abstract class StringCommand : Command
    {
        protected string m_sDestination;
        protected string m_sSource1;
        protected bool m_bSource1IsVariable;
        protected string m_sSource2;
        protected bool m_bSource2IsVariable;

        public StringCommand(   string subType,
                                string destination,
                                string source1,
                                bool s1Variable,
                                string source2,
                                bool s2Variable ) : base("StringCommand", subType)
        {
            m_sDestination = destination;
            m_sSource1 = source1;
            m_bSource1IsVariable = s1Variable;
            m_sSource2 = source2;
            m_bSource2IsVariable = s2Variable;
        }

        protected string GetSource1()
        {
            if (m_bSource1IsVariable)
            {
                Variable var = m_variableDictionary.GetVariable(m_sSource1);
                if (var == null)
                {
                    return "";
                }
                return var.GetString();
            }
            return m_sSource1;
        }

        protected string GetSource2()
        {
            if (m_bSource2IsVariable)
            {
                Variable var = m_variableDictionary.GetVariable(m_sSource2);
                if (var == null)
                {
                    return "";
                }
                return var.GetString();
            }
            return m_sSource2;
        }

        abstract override protected bool CommandLogic();
    }
}
