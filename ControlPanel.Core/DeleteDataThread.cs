using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanel.Core;
using ControlPanel.Core.Enums;
using ControlPanel.Core.IService;
using System.Threading.Tasks;
using ControlPanel.Core.Entities;
using System.Diagnostics;
using System.IO;
using ControlPanel.Core.DataTransfer;
using ControlPanel.Repository;
using System.Net;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using ControlPanel.Core.Argument;


namespace MMS.ProcessThreads
{
    public class DeleteDataThread : ISystemProcessThread
    {
        private string _threadName;
        static Queue<double> cpuReading = new Queue<double>();
        DateTime lastAlertSendDateTime = DateTime.UtcNow.AddMinutes(-10);
        public string ThreadName
        {
            get
            {
                return _threadName;
            }
            set
            {
                _threadName = value;
            }
        }

        private int? _startRange;
        public int? StartRange
        {
            get
            {
                return _startRange;
            }
            set
            {
                _startRange = value;
            }
        }

        private int? _endRange;
        public int? EndRange
        {
            get
            {
                return _endRange;
            }
            set
            {
                _endRange = value;
            }
        }

        private DateTime? _lastSuccessfullyExecuted;
        public DateTime? LastSuccessfullyExecuted
        {
            get
            {
                return _lastSuccessfullyExecuted;
            }
            set
            {
                _lastSuccessfullyExecuted = value;
            }
        }

        private TimeSpan? _scheduleTime;
        public TimeSpan? ScheduledTime
        {
            get
            {
                return _scheduleTime;
            }
            set
            {
                _scheduleTime = value;
            }
        }

        public string Initialize()
        {

            return "Service Initialize " + DateTime.UtcNow.ToString("MMM dd,yyyy hh:mm:ss tt");
        }

        public string Execute(string argument)
        {
            PerformanceStatisticRepository pStatisticRepo = new PerformanceStatisticRepository();
            SystemEventLogRepository sysEventLog = new SystemEventLogRepository();
            FlushData arg=JSONhelper.GetObject<FlushData>(argument);

            try
            {
                int daysAgoPerformance = Convert.ToInt32(arg.PerformanceStatisticDays);
                DateTime dtPerformance = pStatisticRepo.GetMaxCreationDate();
                dtPerformance = dtPerformance.AddDays(-daysAgoPerformance);
                pStatisticRepo.FlushPerformanceStatistic(dtPerformance);

                int daysAgoSystemEvent = Convert.ToInt32(arg.SystemEventLogDays);
                DateTime dtSystemEvt = sysEventLog.GetMaxCreationDate();
                dtSystemEvt = dtSystemEvt.AddDays(-daysAgoSystemEvent);
                sysEventLog.FlushSystemEventLog(dtSystemEvt);

                return "Success";
            }
            catch (Exception exp)
            {
                return "Error";
            }
        }
    }
}
