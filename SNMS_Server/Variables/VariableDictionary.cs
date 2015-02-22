using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.Variables
{
    class VariableDictionary
    {
        Dictionary<string, Variable> m_dictionary;

        public VariableDictionary()
        {
            m_dictionary = new Dictionary<string, Variable>();
        }

        public void Clear()
        {
            m_dictionary = new Dictionary<string, Variable>();
        }

        public bool VariableExists(string varName)
        {
            return m_dictionary.ContainsKey(varName);
        }

        public void SetVariable(string varName, Variable var)
        {
            Variable tempVar = GetVariable(varName);
            if (tempVar != null && tempVar.IsConstant())
            {
                return;
            }

            m_dictionary[varName] = var;
        }

        public Variable GetVariable(string varName)
        {
            if (!VariableExists(varName))
            {
                return null;
            }

            return m_dictionary[varName];
        }
    }
}
