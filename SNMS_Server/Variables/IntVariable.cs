using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.Variables
{
    class IntVariable : Variable
    {
        int m_dwValue;

        public IntVariable(int value) : base("integer")
        {
            m_dwValue = value;
        }

        public IntVariable(int value, bool isConstant)
            : base("integer", isConstant)
        {
            m_dwValue = value;
        }

        public void Increase(int dwIncreaseSize)
        {
            m_dwValue += dwIncreaseSize;
        }

        override public string GetString()
        {
            return m_dwValue.ToString();
        }

        override public void SetVariable(string sValue)
        {
            m_dwValue = Int32.Parse(sValue);
        }
    }
}
