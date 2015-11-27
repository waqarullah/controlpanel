using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlPanel.Core
{
    

    public interface ISystemProcessThread
    {
        string ThreadName { get; set; }
        int? StartRange { get; set; }
        int? EndRange { get; set; }
        DateTime? LastSuccessfullyExecuted { get; set; }
        TimeSpan? ScheduledTime { get; set; }
        string Initialize();
        string Execute(string argument);
    }
}
