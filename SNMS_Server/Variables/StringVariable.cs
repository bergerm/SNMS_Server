using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.Variables
{
    class StringVariable : Variable
    {
        string m_sValue;

        public StringVariable(string value) : base("string")
        {
            m_sValue = value;
        }

        public StringVariable(string value, bool isConstant)
            : base("string", isConstant)
        {
            m_sValue = value;
        }

        override public string GetString()
        {
            return m_sValue;
        }

        override public void SetVariable(string sValue)
        {
            m_sValue = sValue;
        }

        public virtual Variable Clone()
        {
            return new StringVariable(m_sValue);
        }
    }
}
