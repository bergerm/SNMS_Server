using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMS_Server.RealTimeEngine.Sequences
{
    class SequenceNode
    {
        bool m_bIsConditionalNode;
        Command m_command;
        SequenceNode m_nextNode;
        SequenceNode m_nextNodeIfConditionFailed;

        public SequenceNode(Command command, bool bIsConditional = false)
        {
            m_bIsConditionalNode = bIsConditional;
            m_command = command;
            m_nextNode = null;
            m_nextNodeIfConditionFailed = null;
        }

        public void SetNextNode(SequenceNode nextNode, bool bCondition = true)
        {
            if (bCondition)
            {
                m_nextNode = nextNode;
            }
            else
            {
                m_nextNodeIfConditionFailed = nextNode;
            }
        }

        public SequenceNode GetNextNode(bool bConditionSucces)
        {
            if (m_bIsConditionalNode && !bConditionSucces)
            {
                return null;
            }

            if (bConditionSucces)
            {
                return m_nextNode;
            }
            return m_nextNodeIfConditionFailed;
        }

        public Command GetCommand()
        {
            return m_command;
        }

        public bool IsNodeConditional()
        {
            return m_bIsConditionalNode;
        }
    }
}
