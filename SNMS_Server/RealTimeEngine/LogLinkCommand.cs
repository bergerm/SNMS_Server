using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using SNMS_Server.Plugins;
using SNMS_Server.RealTimeEngines.Sequences;
using SNMS_Server.Configurations;
using SNMS_Server.Variables;
using SNMS_Server.Triggers;
using SNMS_Server.Logging;

namespace SNMS_Server.RealTimeEngines
{
    class LogLinkCommand : GeneralCommand
    {
        string m_sLogMessageSource;
        bool m_bIsMessageSourceVariable;
        string m_sLogLinkSource;
        bool m_bIsLinkSourceVariable;

        public LogLinkCommand(string sLogMessageSource, bool bIsMessageSourceVariable, string sLogLinkSource, bool bIsLinkSourceVariable)
            : base("LogLink")
        {
            m_sLogMessageSource = sLogMessageSource;
            m_bIsMessageSourceVariable = bIsMessageSourceVariable;
            m_sLogLinkSource = sLogLinkSource;
            m_bIsLinkSourceVariable = bIsLinkSourceVariable;
        }

        override protected bool CommandLogic()
        {
            Logger logger = Logger.Instance();

            string sMessage;
            string sLink;

            if (m_bIsMessageSourceVariable)
            {
                Variable var = m_variableDictionary.GetVariable(m_sLogMessageSource);
                if (var == null)
                {
                    return false;
                }
                sMessage = var.GetString();
            }
            else
            {
                sMessage = m_sLogMessageSource;
            }

            if (m_bIsLinkSourceVariable)
            {
                Variable var = m_variableDictionary.GetVariable(m_sLogLinkSource);
                if (var == null)
                {
                    return false;
                }
                sLink = var.GetString();
            }
            else
            {
                sLink = m_sLogLinkSource;
            }

            logger.Log(Logger.LOG_TYPE_MESSAGE_FROM_SEQUENCE, sMessage, sLink);

            return true;
        }

        override public Command Clone()
        {
            return new LogLinkCommand(m_sLogMessageSource, m_bIsMessageSourceVariable, m_sLogLinkSource, m_bIsLinkSourceVariable);
        }
    }
}
