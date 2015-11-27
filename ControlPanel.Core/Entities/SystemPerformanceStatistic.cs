using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlPanel.Core.Entities
{
    public class SystemPerformanceStatistic
    {
        public virtual System.Int32 PerformanceStatisticId { get; set; }
      
        public virtual System.Double? Cpu { get; set; }
      
        public virtual System.Double? Memory { get; set; }

        public virtual System.DateTime? CreationDate { get; set; }

        public virtual System.String MachineName { get; set; }

        public virtual System.String IpAddress { get; set; }

        public virtual System.Double? DriveSpaceAvailable { get; set; }

        public virtual System.Double? DriveTotalSpace { get; set; }

        public virtual System.Int32 NetworkStatisticId { get; set; }

        public virtual System.String InterfaceName { get; set; }

        public virtual System.String IpAddressNetwork { get; set; }

        public virtual System.Double TotalUsage { get; set; }

        public virtual System.Double Download { get; set; }

        public virtual System.Double Upload { get; set; }

        public virtual System.DateTime CreationDateNetwork { get; set; }



    }
}
