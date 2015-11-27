using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlPanel.Core
{
   public class test : ISystemProcessThread
    {
        public string ThreadName { get; set; }
        public  int? StartRange { get; set; }
        public  int? EndRange { get; set; }
       public DateTime? LastSuccessfullyExecuted { get; set; }
       public TimeSpan? ScheduledTime { get; set; }
       public string Initialize()
       {
           return "";
       }
       public string Execute(string argument)
       {
           return "";
       }
    }

   
}
