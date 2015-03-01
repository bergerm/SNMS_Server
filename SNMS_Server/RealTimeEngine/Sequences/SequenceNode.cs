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
        int m_dwIndex;
        int m_dwNextNode;
        int m_dwNextNodeIfConditionFailed;

        public SequenceNode(int dwIndex, Command command, bool bIsConditional = false)
        {
            m_dwIndex = dwIndex;
            m_bIsConditionalNode = bIsConditional;
            m_command = command;
            m_dwNextNode = -1;
            m_dwNextNodeIfConditionFailed = -1;
        }

        public bool SetNextNode(int dwNextNode, bool bCondition = true)
        {
            if (!m_bIsConditionalNode && !bCondition)
            {
                return false;
            }

            if (bCondition)
            {
                m_dwNextNode = dwNextNode;
            }
            else
            {
                m_dwNextNodeIfConditionFailed = dwNextNode;
            }

            return true;
        }

        public int GetNextNode(bool bConditionSucces)
        {
            if (!m_bIsConditionalNode && !bConditionSucces)
            {
                return -1;
            }

            if (bConditionSucces)
            {
                return m_dwNextNode;
            }
            return m_dwNextNodeIfConditionFailed;
        }

        public Command GetCommand()
        {
            return m_command;
        }

        public bool IsNodeConditional()
        {
            return m_bIsConditionalNode;
        }

        public int GetIndex()
        {
            return m_dwIndex;
        }

        public SequenceNode Clone()
        {
            SequenceNode newNode = new SequenceNode(m_dwIndex, m_command.Clone(), m_bIsConditionalNode);
            newNode.SetNextNode(m_dwNextNode, true);
            newNode.SetNextNode(m_dwNextNodeIfConditionFailed, false);
            return newNode;
        }
    }
}
