﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SNMS_Server.Configurations;
using System.Threading;

namespace SNMS_Server.RealTimeEngines
{
    class ConfigurationScheduler
    {
        Configuration m_configuration;
        List<Timer> m_timerList;
        Mutex m_mutex;

        string m_sErrorString;

        public ConfigurationScheduler(Configuration configuration)
        {
            m_configuration = configuration;
            m_sErrorString = "";
            m_timerList = new List<Timer>();
            m_mutex = new Mutex();
        }

        void RunSequence(string sSequenceName, Mutex mutex)
        {
            m_mutex.WaitOne();
            m_sErrorString = "";
            m_configuration.RunSequence(sSequenceName, ref m_sErrorString, mutex);
            m_mutex.ReleaseMutex();
        }

        public void ScheduleSequence(string sSequenceName, int dwInterval, Mutex mutex)
        {
            Timer timer = new Timer(e => RunSequence(sSequenceName, mutex),
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

        public void AddConfiguration(string sConfigurationName, Configuration configuration)
        {
            ConfigurationScheduler scheduler = new ConfigurationScheduler(configuration);
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
