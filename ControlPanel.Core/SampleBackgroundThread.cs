using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ControlPanel.Core
{
    public class SampleBackgroundThread : ISystemProcessThread
    {
        private string _threadName;
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
            _lastSuccessfullyExecuted = DateTime.UtcNow;
            Thread.Sleep(5000);
            return "Service Running " + DateTime.UtcNow.ToString("MMM dd,yyyy hh:mm:ss tt");
        }
    }
}
