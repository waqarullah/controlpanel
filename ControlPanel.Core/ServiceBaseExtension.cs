using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using ControlPanel.Core.Entities;
using System.IO;



namespace ControlPanel.Core
{
    public abstract class ServiceBaseExtension:ServiceBase
    {
        TaskProcessorCore processor;
        CPServiceClient webClient;
       
        protected override void OnStart(string[] args)
        {
            try
            {                               
                //Thread.Sleep(15000);
                var displayName = GetServiceName();
                string configFile = AppDomain.CurrentDomain.BaseDirectory + "\\" + displayName + ".exe.config";
                if (File.Exists(configFile))
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configFile);
                }
                webClient = new CPServiceClient();
                string systemProcess = string.Empty;
                if (ConfigurationManager.AppSettings["SystemProcess"] != null) systemProcess = ConfigurationManager.AppSettings["SystemProcess"].ToString();

                //ServiceLogManager.GetLogger(this.GetType().Name).Info(string.Format("Started {0} Background Service", systemProcess));
                processor = new TaskProcessorCore(webClient.GetSystemProcessByName(systemProcess));
                processor.OnGetSystemProcessThread += new TaskProcessorCore.GetSystemProcessThreadEventHandler(processor_OnGetSystemProcessThread);
                processor.OnUpdateSystemProcess += new TaskProcessorCore.UpdateSystemProcessThreadEventHandler(processor_OnUpdateSystemProcess);
                processor.StartEngines();

                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        protected String GetServiceName()
        {
            // Calling System.ServiceProcess.ServiceBase::ServiceNamea allways returns
            // an empty string,
            // see https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=387024

            // So we have to do some more work to find out our service name, this only works if
            // the process contains a single service, if there are more than one services hosted
            // in the process you will have to do something else

            int processId = System.Diagnostics.Process.GetCurrentProcess().Id;
            String query = "SELECT * FROM Win32_Service where ProcessId = " + processId;
            System.Management.ManagementObjectSearcher searcher =
                new System.Management.ManagementObjectSearcher(query);

            foreach (System.Management.ManagementObject queryObj in searcher.Get())
            {
                return queryObj["DisplayName"].ToString();
            }

            throw new Exception("Can not get the ServiceName");
        }

        static void CurrentDomain_UnhandledException(object sender,
                                      UnhandledExceptionEventArgs e)
        {

        }

        SystemProcessThread processor_OnUpdateSystemProcess(SystemProcessThread systemProcessThread)
        {
            systemProcessThread.LastUpdateDate = DateTime.UtcNow;
            return webClient.UpdateSystemProcessThread(systemProcessThread);
        }

        SystemProcessThread processor_OnGetSystemProcessThread(string threadName)
        {
            return webClient.GetSystemProcessThreadByName(threadName);
        }

        protected override void OnStop()
        {
            string systemProcess = string.Empty;
            if (ConfigurationManager.AppSettings["SystemProcess"] != null) systemProcess = ConfigurationManager.AppSettings["SystemProcess"].ToString();

            List<SystemProcessThread> processThreads = webClient.GetSystemProcessThreadsByProcessName(systemProcess);
            foreach (SystemProcessThread systemProcessThread in processThreads)
            {
                systemProcessThread.Status = GenericServiceThread.EngineStatusList.Stopped.ToString();
                systemProcessThread.Enabled = false;
                systemProcessThread.LastUpdateDate = DateTime.UtcNow;
                webClient.UpdateSystemProcessThread(systemProcessThread);
   
            }
        //    ServiceLogManager.GetLogger(this.GetType().Name).Info(string.Format("Stopped {0} Background Service", systemProcess));

            processor.StopEngines();
        }
    }
}
