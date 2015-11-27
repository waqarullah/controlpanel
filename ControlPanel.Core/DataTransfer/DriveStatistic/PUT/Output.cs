using System;
using System.Runtime.Serialization;
using Validation;

namespace ControlPanel.Core.DataTransfer.DriveStatistic
{
    [DataContract]
	public class PutOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 DriveStatisticId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String IpAddress{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Double? DriveSpaceAvailable{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Double? DriveTotalSpace{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String DriveName{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? CreationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string CreationDateStr
		{
			 get {if(CreationDate.HasValue) return CreationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.String MachineName{ get; set; }

	}	
}
