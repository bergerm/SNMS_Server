using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.Variables
{
    abstract class Variable
    {
        string m_sVariableType;
        bool m_bIsConstant;
        bool m_bIsEnabled;

        public Variable(string type, bool bIsConstant)
        {
            m_sVariableType = type;
            m_bIsConstant = bIsConstant;
        }

        public Variable(string type)
        {
            m_sVariableType = type;
            m_bIsConstant = false;
        }

        public bool IsConstant()
        {
            return m_bIsConstant;
        }

        abstract public string GetString();
        abstract public void SetVariable(String sValue);

        public string GetVarType()
        {
            return m_sVariableType;
        }

        public abstract Variable Clone();
    }
}
