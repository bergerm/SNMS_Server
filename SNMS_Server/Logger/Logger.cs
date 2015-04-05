using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

using SNMS_Server.Connection;

namespace SNMS_Server.Logging
{
    class Logger
    {
        public const string LOG_TYPE_START = "System Start";
        public const string LOG_TYPE_SYSTEM_UPDATE = "System Updated";
        public const string LOG_TYPE_TRIGGER_VALIDATION = "Trigger Validation";
        public const string LOG_TYPE_REACTION = "Reaction";
        public const string LOG_TYPE_ERROR_ON_SEQUENCE = "Error on Sequence";
        public const string LOG_TYPE_ERROR = "Error";
        public const string LOG_TYPE_MESSAGE_FROM_SEQUENCE = "Message from Sequence";

        private static Logger instance;
        Mutex m_mutex;
        NetworkStream m_stream;

        private Logger(NetworkStream stream)
        {
            m_mutex = new Mutex();
            m_stream = stream;
        }

        public static Logger Instance(NetworkStream stream)
        {
            if (instance == null)
            {
                if (stream == null)
                {
                    return null;
                }
                instance = new Logger(stream);
            }
            return instance;
        }

        public static Logger Instance()
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }

        public void Log(string sLogType, string sLogMessage, string sLogLink = "")
        {
            m_mutex.WaitOne();

            ProtocolMessage logMessage = new ProtocolMessage();
            logMessage.SetMessageType(ProtocolMessageType.PROTOCOL_MESSAGE_SAVE_LOG_MESSAGE);

            logMessage.AddParameter(sLogType);
            logMessage.AddParameter(sLogMessage);
            logMessage.AddParameter(sLogLink);
            logMessage.AddParameter("Server");
            logMessage.AddParameter("");

            ConnectionHandler.SendMessage(m_stream, logMessage);

            m_mutex.ReleaseMutex();
        }
    }


}
