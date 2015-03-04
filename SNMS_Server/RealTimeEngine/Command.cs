using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Variables;
using SNMS_Server.Plugins;
using SNMS_Server.RealTimeEngines.Sequences;

namespace SNMS_Server.RealTimeEngines
{
    abstract class Command
    {
        string m_sCommandType;
        string m_sCommandSubType;
        protected Sequence m_sequence;

        protected VariableDictionary m_variableDictionary;

        public Command( string type,
                        string subType)
        {
            m_sCommandType = type;
            m_sCommandSubType = subType;
        }

        public void SetVariableDictionary(VariableDictionary dict)
        {
            m_variableDictionary = dict;
        }

        public VariableDictionary GetVariableDictionary()
        {
            return m_variableDictionary;
        }

        public void SetSequence(Sequence sequence)
        {
            m_sequence = sequence;
        }

        public Sequence GetSequence()
        {
            return m_sequence;
        }

        abstract protected bool CommandLogic();

        virtual public bool Execute()
        {
            return CommandLogic();
        }

        public string GetCommandType()
        {
            return m_sCommandType;
        }

        public string GetSubType()
        {
            return m_sCommandSubType;
        }

        public abstract Command Clone();
    }
}
