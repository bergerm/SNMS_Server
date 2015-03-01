using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Variables;
using SNMS_Server.Plugins;
using SNMS_Server.RealTimeEngine.Sequences;

namespace SNMS_Server.RealTimeEngine
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

        public void SetSequence(Sequence sequence)
        {
            m_sequence = sequence;
        }

        public Sequence GetSequence()
        {
            return m_sequence;
        }

        public Plugin GetPlugin(Plugin plugin)
        {
           return m_sequence.GetPlugin();
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

        public virtual Command Clone();
    }
}
