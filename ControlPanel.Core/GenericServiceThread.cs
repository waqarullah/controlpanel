using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Timer = System.Timers.Timer;

namespace ControlPanel.Core
{
    public enum TaskEngineExitCode
    {
        Ok,
        Disable,
        PanicExit
    }

    public enum PartnerEngine
    {
        Jobs2Web = 100
    }
    public class GenericServiceThread : IDisposable
    {

        public static bool PanicExit = false;
        public enum EngineStatusList
        {
            Initializing,
            Sleeping,
            Stopped,
            Running,
            Offline,
            Exception
        }
        static GenericServiceThread()
        {
        }

        //private bool paused;
        private bool exiting;
        private bool continuous;
        private bool napping;
        private int sleepInterval = 60;
        private int defaultSleepInterval = 60;
        private string _threadName;
        private bool enabled;
        private bool autoStart;
        private int _userAccountId;

        System.Timers.Timer workThread;
        
        public string ThreadName
        {
            get { return _threadName; }
        }


        public delegate void SetStatusMessageEventHandler(string threadName, string messageText);

        private event SetStatusMessageEventHandler _OnSetStatusMessage;
        public event SetStatusMessageEventHandler OnSetStatusMessage
        {
            add { _OnSetStatusMessage = value; }
            remove { _OnSetStatusMessage = value; }
        }

        public delegate void ReportEngineStateEventHandler(string threadName, EngineStatusList state);

        private event ReportEngineStateEventHandler _OnReportEngineState;
        public event ReportEngineStateEventHandler OnReportEngineState
        {
            add { _OnReportEngineState = value; }
            remove { _OnReportEngineState = value; }
        }

        public delegate void SetEngineStateEventHandler(string threadName, bool? enabled, bool? continuous, bool resetName);
        private event SetEngineStateEventHandler _OnSetEngineState;
        public event SetEngineStateEventHandler OnSetEngineState
        {
            add { _OnSetEngineState = value; }
            remove { _OnSetEngineState = value; }
        }

        public GenericServiceThread(string threadName)
        {
            this._threadName = threadName;
        }

        public GenericServiceThread(string threadName, int userAccountId)
        {
            this._threadName = threadName;
            this._userAccountId = userAccountId;
        }

        public void Shutdown()
        {
            exiting = true;
            if (_OnReportEngineState != null) _OnReportEngineState(_threadName, EngineStatusList.Offline);
            if (_OnSetEngineState != null) _OnSetEngineState(_threadName, false, null, false);
          
            workThread.Stop();
        }




        public void Pause()
        {
            if (_OnSetEngineState != null) 
                _OnSetEngineState(_threadName, false, null, false);
        }

        public void Resume()
        {
            if (_OnSetEngineState != null) _OnSetEngineState(_threadName, true, null, false);
        }

        public delegate void GOEventHandler(string engine,out bool processExecuted);

        private event GOEventHandler _OnExecute;
        public event GOEventHandler OnExecute
        {
            add { _OnExecute = value; }
            remove { _OnExecute = value; }
        }

        public void Init(bool autoStart, int timeInterval,bool enabled)
        {
            if(enabled)
                _OnReportEngineState(_threadName, GenericServiceThread.EngineStatusList.Initializing);
            else
                _OnReportEngineState(_threadName, GenericServiceThread.EngineStatusList.Sleeping);
           
            this.autoStart = autoStart;
            this.enabled = enabled;
            this.sleepInterval = timeInterval;
            this.defaultSleepInterval = timeInterval;
            workThread=new Timer();
            workThread = new System.Timers.Timer();
            workThread.Interval = timeInterval*1000;
            workThread.Elapsed+=new System.Timers.ElapsedEventHandler(Go);
            workThread.Start();
        }

        public void logException(Exception exp)
        {
            File.AppendAllLines("c:\\logservice.txt", new string[] { exp.Message, exp.StackTrace });
            if (exp.InnerException != null) logException(exp.InnerException);
        }

        private void Go(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool processExecuted = false;

            if (_OnExecute != null)
            {
                workThread.Stop();
        
                try
                {
                    _OnExecute(_threadName, out processExecuted);
                }
                catch(Exception exp)
                {

                    File.AppendAllLines("c:\\log.txt", new string[] { exp.Message, exp.StackTrace });
                    if (exp.InnerException != null) logException(exp.InnerException);

                    throw exp;
                }
                finally
                {
                    workThread.Start();
                }
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            workThread.Stop();
            workThread.Dispose();
            //GC.SuppressFinalize(this);

        }

        #endregion
    }
}
