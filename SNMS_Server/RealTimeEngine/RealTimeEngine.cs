using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using SNMS_Server.Configurations;
using SNMS_Server.Connection;
using System.Threading;

namespace SNMS_Server.RealTimeEngines
{
    class ConfigurationScheduler
    {
        Configuration m_configuration;
        List<Timer> m_timerList;
        Mutex m_mutex;
        NetworkStream m_stream;

        string m_sErrorString;

        public ConfigurationScheduler(Configuration configuration, NetworkStream stream)
        {
            m_configuration = configuration;
            m_sErrorString = "";
            m_timerList = new List<Timer>();
            m_mutex = new Mutex();
            m_stream = stream;
        }

        void RunSequence(string sSequenceName, Mutex mutex)
        {
            m_mutex.WaitOne();
            m_sErrorString = "";
            m_configuration.RunSequence(sSequenceName, ref m_sErrorString, mutex);
            m_mutex.ReleaseMutex();
        }

        void RunScheduledSequence(string sSequenceName, Mutex mutex)
        {
            ProtocolMessage configurationStatusMessage = new ProtocolMessage();
            configurationStatusMessage.SetMessageType(ProtocolMessageType.PROTOCOL_MESSAGE_SAVE_CONFIGURATION_STATUS_MESSAGE);
            configurationStatusMessage.AddParameter(m_configuration.GetID());

            RunSequence(sSequenceName, mutex);

            if( m_sErrorString == "" )
            {
                configurationStatusMessage.AddParameter("Running");
            }
            else
            {
                configurationStatusMessage.AddParameter("ERROR");
            }

            ConnectionHandler.SendMessage(m_stream, configurationStatusMessage);
        }

        public void ScheduleSequence(string sSequenceName, int dwInterval, Mutex mutex)
        {
            Timer timer = new Timer(e => RunScheduledSequence(sSequenceName, mutex),
                                        null,
                                        TimeSpan.Zero,
                                        TimeSpan.FromMinutes(dwInterval));

            m_timerList.Add(timer);    
        }

        public void StopTimers()
        {
            m_mutex.WaitOne();
            foreach (Timer timer in m_timerList)
            {
                timer.Dispose();  
            }
            m_mutex.ReleaseMutex();
        }

        public void CloseWebDrivers()
        {
            m_configuration.CloseWebDriver();
        }
    }

    class RealTimeEngine
    {
        Dictionary<string, ConfigurationScheduler> m_configurationSchedulerDictionary;

        public RealTimeEngine()
        {
            m_configurationSchedulerDictionary = new Dictionary<string, ConfigurationScheduler>();
        }

        public void AddConfiguration(NetworkStream stream, string sConfigurationName, Configuration configuration)
        {
            ConfigurationScheduler scheduler = new ConfigurationScheduler(configuration, stream);
            m_configurationSchedulerDictionary[sConfigurationName] = scheduler;
        }

        public bool SetSchedule(string sConfigurationName, string sSequenceName, int dwInterval, Mutex mutex)
        {
            if (!m_configurationSchedulerDictionary.Keys.Contains(sConfigurationName))
            {
                return false;
            }

            m_configurationSchedulerDictionary[sConfigurationName].ScheduleSequence(sSequenceName, dwInterval, mutex);
            return true;
        }

        public void StopSequences()
        {
            foreach (KeyValuePair<string, ConfigurationScheduler> pair in m_configurationSchedulerDictionary)
            {
                pair.Value.StopTimers();
            }
        }

        public void CloseWebDrivers()
        {
            foreach (KeyValuePair<string, ConfigurationScheduler> pair in m_configurationSchedulerDictionary)
            {
                pair.Value.CloseWebDrivers();
            }
        }
    }
}
