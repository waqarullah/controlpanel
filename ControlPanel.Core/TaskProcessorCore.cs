using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using ControlPanel.Core.Entities;
using MMS.ProcessThreads;

namespace ControlPanel.Core
{
    public class TaskProcessorCore
    {
        CPServiceClient webClient = new CPServiceClient();
        //ICPWebClient webClient = IoC.Resolve<ICPWebClient>("CPWebClient");
        private SystemProcess _systemProcess;
        private List<SystemProcessThread> processThreads;
        private bool isRunning = false;
        private bool isTerminating = false;
        private List<GenericServiceThread> _threads;
        private System.Timers.Timer mainTimer = new System.Timers.Timer();
        private int userAccountId;
        public TaskProcessorCore(SystemProcess systemProcess)
        {
            _systemProcess = systemProcess;
            processThreads = webClient.GetSystemProcessThreadsByProcessName(_systemProcess.Name);
        }

        public TaskProcessorCore(SystemProcess systemProcess, int _userAccountId)
        {
            _systemProcess = systemProcess;
            processThreads = webClient.GetSystemProcessThreadsByProcessName(_systemProcess.Name);
            userAccountId = _userAccountId;
        }

        public void StartEngines()
        {
            mainTimer.Elapsed += new System.Timers.ElapsedEventHandler(Go);
            mainTimer.Interval = 60000;
            mainTimer.Start();
        }

       
        private GenericServiceThread InitializeThread(SystemProcessThread serviceThread)
        {
            GenericServiceThread thread = new GenericServiceThread(serviceThread.Name,userAccountId);
            thread.OnExecute += new GenericServiceThread.GOEventHandler(Execute);
            thread.OnSetStatusMessage += new GenericServiceThread.SetStatusMessageEventHandler(thread_OnSetStatusMessage);
            thread.OnReportEngineState += new GenericServiceThread.ReportEngineStateEventHandler(thread_OnReportEngineState);
            thread.OnSetEngineState += new GenericServiceThread.SetEngineStateEventHandler(thread_OnSetEngineState);
            thread.Init(false, serviceThread.SleepTime, serviceThread.Enabled);
            return thread;
        }

        private void Go(object sender, System.Timers.ElapsedEventArgs e)
        {
            //bool checkForNewThread = false;
            //if (ConfigurationManager.AppSettings["CheckForNewThread"] != null)
            //    bool.TryParse(ConfigurationManager.AppSettings["CheckForNewThread"].ToString(), out checkForNewThread);
            processThreads = webClient.GetSystemProcessThreadsByProcessName(_systemProcess.Name);

            isRunning = true;
            if (_threads == null || _threads.Count==0)
            {
                _threads = new List<GenericServiceThread>();

                foreach (SystemProcessThread serviceThread in processThreads)
                {
                    _threads.Add(InitializeThread(serviceThread));
                }
            }
            else
            {
                //string systemProcess = string.Empty;
                //if (ConfigurationManager.AppSettings["SystemProcess"] != null) systemProcess = ConfigurationManager.AppSettings["SystemProcess"].ToString();
                //_systemProcess = webClient.GetSystemProcessByName(systemProcess);
                foreach (SystemProcessThread systemProcessThread in processThreads)
                {
                    if(_threads.Where(x=>x.ThreadName.ToLower()==systemProcessThread.Name.ToLower()).Count()==0)
                    {
                        _threads.Add(InitializeThread(systemProcessThread));
                    }
                }
                
                for(int i=0;i<_threads.Count;i++)
                {
                    GenericServiceThread serviceThread = _threads[i];
                    if (processThreads.Where(x => x.Name.ToLower() == serviceThread.ThreadName.ToLower()).Count() == 0)
                    {
                        _threads.Remove(serviceThread);
                        i--;
                        serviceThread.Dispose();
                        serviceThread = null;
                    }
                }
            }


        }

        void thread_OnSetEngineState(string threadName, bool? enabled, bool? continuous, bool resetName)
        {
            SystemProcessThread systemProcessThread= GetSystemProcessThread(threadName);

            if (resetName)
            {
                systemProcessThread.Name = threadName;
            }
            systemProcessThread.Enabled = enabled == null ? systemProcessThread.Enabled : (bool)enabled;
            systemProcessThread.Continuous = continuous == null ? systemProcessThread.Continuous : (bool)continuous;

            UpdateSystemProcessThread(_OnUpdateSystemProcess(systemProcessThread));
            //lastChecked = DateTime.Now;
        }

        void thread_OnReportEngineState(string threadName, GenericServiceThread.EngineStatusList state)
        {
            

            SystemProcessThread systemProcessThread = GetSystemProcessThread(threadName);
            if(systemProcessThread!=null)
            {
                systemProcessThread.Status = state.ToString();
                UpdateSystemProcessThread(_OnUpdateSystemProcess(systemProcessThread));    
            }
        }
        private SystemProcessThread GetSystemProcessThread(string threadName)
        {
            if (_OnGetSystemProcessThread != null)
                return _OnGetSystemProcessThread(threadName);
            else
            {
                return processThreads.Where(x => x.Name == threadName).FirstOrDefault();
            }
        }

        private void UpdateSystemProcessThread(SystemProcessThread systemProcessThread)
        {
            SystemProcessThread thread = processThreads.Where(x => x.SystemProcessThreadId == systemProcessThread.SystemProcessThreadId).FirstOrDefault();
            thread.LastUpdateDate = DateTime.UtcNow;
            thread = systemProcessThread;
        }
        void thread_OnSetStatusMessage(string threadName, string messageText)
        {
            SystemProcessThread systemProcessThread = GetSystemProcessThread(threadName);
            if(systemProcessThread!=null)
            {
                string status = messageText;
                if (string.IsNullOrEmpty(messageText))
                {
                  
                    systemProcessThread.Message = status;
                    UpdateSystemProcessThread(_OnUpdateSystemProcess(systemProcessThread));
                    return;
                }

                systemProcessThread.Message = status;
                UpdateSystemProcessThread(_OnUpdateSystemProcess(systemProcessThread));
              
            }
            


            
        }

        public delegate SystemProcessThread GetSystemProcessThreadEventHandler(string threadName);
        private event GetSystemProcessThreadEventHandler _OnGetSystemProcessThread;
        public event GetSystemProcessThreadEventHandler OnGetSystemProcessThread
        {
            add { _OnGetSystemProcessThread = value; }
            remove { _OnGetSystemProcessThread = value; }
        }

        public delegate SystemProcessThread UpdateSystemProcessThreadEventHandler(SystemProcessThread systemProcessThread);
        private event UpdateSystemProcessThreadEventHandler _OnUpdateSystemProcess;
        public event UpdateSystemProcessThreadEventHandler OnUpdateSystemProcess
        {
            add { _OnUpdateSystemProcess = value; }
            remove { _OnUpdateSystemProcess = value; }
        }
        private bool exiting;
        void Execute(string threadName,out bool executed)
        {
           // ILog log = MO.Core.ServiceLogManager.GetLogger(threadName);
            executed = false;
            try
            {
                SystemProcessThread systemProcessThread = _OnGetSystemProcessThread(threadName);
                if(systemProcessThread.Continuous && systemProcessThread.Enabled==false)
                {
                    
                    if(systemProcessThread.ScheduledTime.HasValue)
                    {                        
                        TimeSpan duration=DateTime.UtcNow-DateTime.UtcNow.Date;
                        if(duration.TotalSeconds>= systemProcessThread.ScheduledTime.Value.TotalSeconds && duration.TotalSeconds<=systemProcessThread.ScheduledTime.Value.TotalSeconds+systemProcessThread.SleepTime)
                        {
                            systemProcessThread.Enabled = true;
                            systemProcessThread = _OnUpdateSystemProcess(systemProcessThread);
                        }
                    }
                    else
                    {
                        if(systemProcessThread.LastSuccessfullyExecuted.HasValue)
                        {
                            TimeSpan duration = DateTime.UtcNow - systemProcessThread.LastSuccessfullyExecuted.Value;
                            if(duration.TotalSeconds>=systemProcessThread.ContinuousDelay)
                            {
                                systemProcessThread.Enabled = true;
                                systemProcessThread = _OnUpdateSystemProcess(systemProcessThread);
                            }
                        }
                        else
                        {
                            systemProcessThread.Enabled = true;
                            systemProcessThread = _OnUpdateSystemProcess(systemProcessThread);
                        }

                    }
                }
                if (systemProcessThread.Enabled)
                {
                    try

                    {
                        thread_OnReportEngineState(threadName, GenericServiceThread.EngineStatusList.Running);
                        ISystemProcessThread systemProcess=null;
                        if (systemProcessThread.SpringEntryName == "PerformanceStatisticThread")
                            systemProcess = new PerformanceStatisticThread();
                        else if (systemProcessThread.SpringEntryName == "DeleteDataThread")
                            systemProcess = new DeleteDataThread();
                        else systemProcess = IoC.Resolve<ISystemProcessThread>(systemProcessThread.SpringEntryName);
                        systemProcess.ThreadName = threadName;
                        systemProcess.LastSuccessfullyExecuted = systemProcessThread.LastSuccessfullyExecuted;
                        systemProcess.ScheduledTime = systemProcessThread.ScheduledTime;
                        systemProcess.StartRange = systemProcessThread.StartRange;
                        systemProcess.EndRange = systemProcessThread.EndRange;

                        executed = true;
                        try
                        {

                            DateTime dt = DateTime.UtcNow;
                            string message = systemProcess.Execute(systemProcessThread.Argument);
                            TimeSpan executionTime = DateTime.UtcNow.Subtract(dt);
                            systemProcessThread = _OnGetSystemProcessThread(threadName);
                            systemProcessThread.LastSuccessfullyExecuted = DateTime.UtcNow;
                            systemProcessThread.Status = GenericServiceThread.EngineStatusList.Sleeping.ToString();
                            systemProcessThread.Message = message;
                            systemProcessThread.ExecutionTime = executionTime.TotalMilliseconds;
                          //  log.Info(string.Format("{0} Worker Thread Completed", threadName));
                            StopThread(systemProcessThread);
                        }
                        catch(SqlException ex)
                        {
                            if(ex.Number==1205  || ex.Number==-2)
                            {
                            }
                            else
                            {
                                StopThread(systemProcessThread);
                                throw ex;
                            }
                        }
                        catch (Exception ex)
                        {
                            StopThread(systemProcessThread);
                            throw ex;
                        }
                      
                    }
                    catch (Exception ex)
                    {
                        systemProcessThread.Status = GenericServiceThread.EngineStatusList.Exception.ToString();
                        systemProcessThread.Message = string.Format("{0} at {1}", ex.Message, DateTime.UtcNow.ToISOString());
                        StopThread(systemProcessThread);
                        throw ex;
                    }
                }
                

            }
            catch (Exception ex)
            {
                LogException(ex);
                throw ex;
            }



        }

        public void LogException(Exception ex)
        {
            ControlPanel.Service.SystemEventLogService logService = new ControlPanel.Service.SystemEventLogService();
            logService.InsertSystemEventLog(ex.Message, ex.StackTrace, Enums.EventCodes.Error);
            if (ex.InnerException != null) LogException(ex.InnerException);
        }

        //static void exceptionManager_ErrorLog_Handler(object sender, ExceptionManagementEventArgs e)
        //{
        //    string errorID = string.Empty;
        //    try
        //    {
        //        errorID = WCFServiceContext<IMOService>.ServiceContext.LogException(e.ErrorToLog, true);
        //        //Logging to file
        //        ExceptionManagement exceptionManager = new ExceptionManagement();
        //        Log errorLog = exceptionManager.GetLog(e.ErrorToLog);
        //        errorLog.ElmahId = errorID;
        //        exceptionManager.LogToFile(errorLog);
        //    }
        //    catch (Exception ex)
        //    {                
        //        throw ex;
        //    }
        //}
        public void StopThread(SystemProcessThread systemProcessThread)
        {
            processThreads.Where(x => x.Name == systemProcessThread.Name).First().Enabled =
                               false;
            if (_OnUpdateSystemProcess != null)
            {
                systemProcessThread.Enabled = false;
                systemProcessThread=_OnUpdateSystemProcess(systemProcessThread);
            }
        }




        public void StopEngines()
        {
            isTerminating = true;

            mainTimer.Stop();




        }









      




    }
}
