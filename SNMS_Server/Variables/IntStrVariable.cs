using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.Variables
{
    class IntStrVariable : Variable
    {
        string m_value;

        public IntStrVariable(int value) : base("integer or string")
        {
            m_value = value.ToString();
        }

        public IntStrVariable(string value) : base("integer or string")
        {
            m_value = value;
        }

        public IntStrVariable(int value, bool isConstant)
            : base("integer or string", isConstant)
        {
            m_value = value.ToString();
        }

        public IntStrVariable(string value, bool isConstant)
            : base("integer or string", isConstant)
        {
            m_value = value;
        }

        public bool Increase(int dwIncreaseSize)
        {
            int tempValue = 0;
            bool result = false;
            try
            {
                result = Int32.TryParse(m_value, out tempValue);
            }
            catch(Exception e)
            {

            }

            if (result)
            {
                tempValue += dwIncreaseSize;
                m_value = tempValue.ToString();
                return true;
            }

            return false;
        }

        public void Decrease(int dwDecreaseSize)
        {
            Increase(-dwDecreaseSize);
        }

        override public string GetString()
        {
            return m_value;
        }

        override public void SetVariable(string sValue)
        {
            m_value = sValue;
        }

        override public Variable Clone()
        {
            return new IntStrVariable(m_value);
        }
    }
}
