using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;

using SNMS_Server.RealTimeEngines;
using SNMS_Server.RealTimeEngines.Sequences;
using SNMS_Server.Plugins;
using SNMS_Server.Configurations;
using SNMS_Server.Factories;
using SNMS_Server.Connection;
using SNMS_Server.Logging;


namespace SNMS_Server.Engine
{
    class MainEngine
    {
        //RealTimeEngine m_realTimeEngine;
        Thread m_runningThread;
        Mutex m_mutex;

        // 180000 ms equals 3 minutes.
        static int UPDATING_INTERVAL = 180000;

        const string PLUGIN_FOLDER_PATH = "..\\..\\WorkingPlugins\\";

        public MainEngine()
        {
            //m_realTimeEngine = null;
            m_runningThread = null;
            m_mutex = new Mutex();
        }

        RealTimeEngine CreateNewRealTimeEngine(NetworkStream stream)
        {
            string sErrorString = "";
            
            RealTimeEngine rtEngine = new RealTimeEngine();

            Logger logger = Logger.Instance();

            List<Plugin> listOfPlugins = PluginFactory.Build(PLUGIN_FOLDER_PATH);
            foreach (Plugin plugin in listOfPlugins)
            {
                List<Account> listOfAccounts = AccountFactory.Build(plugin, stream);
                foreach (Account account in listOfAccounts)
                {
                    List<Configuration> listOfConfigurations = ConfigurationFactory.Build(account, stream);
                    foreach (Configuration configuration in listOfConfigurations)
                    {
                        List<string> sStartupSequences = plugin.GetStartupSequences();
                        foreach (string seqName in sStartupSequences)
                        {
                            configuration.RunSequence(seqName, ref sErrorString, null);
                            if (sErrorString != "")
                            {
                                logger.Log(Logger.LOG_TYPE_ERROR_ON_SEQUENCE, sErrorString);
                                return null;
                            }
                        }

                        rtEngine.AddConfiguration(configuration.GetName(), configuration);
                        List<SequenceTimer> timers = plugin.GetTimers();
                        foreach (SequenceTimer timer in timers)
                        {
                            rtEngine.SetSchedule(configuration.GetName(), timer.GetSequenceName(), timer.GetPeriod(), m_mutex);
                        }
                    }
                }
            }

            return rtEngine;
        }

        void RunningThread(object netstream)
        {
            NetworkStream stream = (NetworkStream)netstream;

            Logger logger = Logger.Instance();

            RealTimeEngine rtEngine = null;
            bool bUpdateRequired = false;

            System.Console.WriteLine("Launching Main Engine");

            while (true)
            {
                m_mutex.WaitOne();

                if (rtEngine != null)
                {
                    rtEngine.StopSequences();
                    rtEngine.CloseWebDrivers();
                    rtEngine = null;
                }

                System.Console.WriteLine("Updating Main Engine");
                rtEngine = CreateNewRealTimeEngine(stream);
                m_mutex.ReleaseMutex();

                bUpdateRequired = false;

                if (rtEngine == null)
                {
                    System.Console.WriteLine("Main Engine errror: Cannot create RealTime Engine!");
                    logger.Log(Logger.LOG_TYPE_ERROR, "Main Engine errror: Cannot create RealTime Engine!");
                    Thread.Sleep(1000);
                    continue;
                }

                System.Console.WriteLine("Main Engine Updated");

                ProtocolMessage serverUpdatedMessage = new ProtocolMessage();
                serverUpdatedMessage.SetMessageType(ProtocolMessageType.PROTOCOL_MESSAGE_SERVER_UPDATED);
                ConnectionHandler.SendMessage(stream, serverUpdatedMessage);
                logger.Log(Logger.LOG_TYPE_SYSTEM_UPDATE, "System Updated");

                Stopwatch stopWatch = Stopwatch.StartNew();

                while (!bUpdateRequired)
                {
                    Thread.Sleep(1000);

                    if (stopWatch.ElapsedMilliseconds > UPDATING_INTERVAL)
                    {
                        ProtocolMessage serverUpdateStatusMessage = new ProtocolMessage();
                        serverUpdateStatusMessage.SetMessageType(ProtocolMessageType.PROTOCOL_MESSAGE_SERVER_UPDATE_STATUS);
                        ConnectionHandler.SendMessage(stream, serverUpdateStatusMessage);

                        ProtocolMessage serverUpdateRequiredMessage = ConnectionHandler.GetMessage(stream);
                        bUpdateRequired = serverUpdateRequiredMessage.GetParameterAsBool(0);

                        stopWatch = Stopwatch.StartNew();
                    }
                    else
                    {
                        bUpdateRequired = false;
                    }
                }
            }
        }

        public bool Start(NetworkStream stream)
        {
            if (m_runningThread != null)
            {
                return false;
            }

            m_runningThread = new Thread(RunningThread);
            m_runningThread.Start(stream);
            return true;
        }

        public bool Stop()
        {
            if (m_runningThread == null)
            {
                return false;
            }

            m_runningThread.Abort();
            return true;
        }

    }
}
