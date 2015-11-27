using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Runtime.Serialization;

namespace ControlPanel.Core.Entities
{
    [DataContract]
	public abstract partial class PerformanceStatisticBase:EntityBase, IEquatable<PerformanceStatisticBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("PerformanceStatisticID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 PerformanceStatisticId{ get; set; }

		[FieldNameAttribute("CPU",true,false,8)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Double? Cpu{ get; set; }

		[FieldNameAttribute("Memory",true,false,8)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Double? Memory{ get; set; }

		[FieldNameAttribute("CreationDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? CreationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string CreationDateStr
		{
			 get {if(CreationDate.HasValue) return CreationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("MachineName",true,false,255)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String MachineName{ get; set; }

		[FieldNameAttribute("IPAddress",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String IpAddress{ get; set; }

		[FieldNameAttribute("DriveSpaceAvailable",true,false,8)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Double? DriveSpaceAvailable{ get; set; }

		[FieldNameAttribute("DriveTotalSpace",true,false,8)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Double? DriveTotalSpace{ get; set; }

		
		public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<PerformanceStatisticBase> Members

        public virtual bool Equals(PerformanceStatisticBase other)
        {
			if(this.PerformanceStatisticId==other.PerformanceStatisticId  && this.Cpu==other.Cpu  && this.Memory==other.Memory  && this.CreationDate==other.CreationDate  && this.MachineName==other.MachineName  && this.IpAddress==other.IpAddress  && this.DriveSpaceAvailable==other.DriveSpaceAvailable  && this.DriveTotalSpace==other.DriveTotalSpace )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(PerformanceStatistic other)
        {
			if(other!=null)
			{
				this.PerformanceStatisticId=other.PerformanceStatisticId;
				this.Cpu=other.Cpu;
				this.Memory=other.Memory;
				this.CreationDate=other.CreationDate;
				this.MachineName=other.MachineName;
				this.IpAddress=other.IpAddress;
				this.DriveSpaceAvailable=other.DriveSpaceAvailable;
				this.DriveTotalSpace=other.DriveTotalSpace;
			}
			
		
		}

        #endregion
		
		
		
	}
	
	
}
