using System;
using System.Runtime.Serialization;

namespace ControlPanel.Core.DataTransfer.SystemProcessThread
{
    [DataContract]
	public class PutOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 SystemProcessThreadId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32 SystemProcessId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Name{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String SpringEntryName{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Description{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean Enabled{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean Continuous{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32 SleepTime{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean AutoStart{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Status{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Message{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public TimeSpan? ScheduledTime{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? StartRange{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? EndRange{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? LastSuccessfullyExecuted{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string LastSuccessfullyExecutedStr
		{
			 get {if(LastSuccessfullyExecuted.HasValue) return LastSuccessfullyExecuted.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { LastSuccessfullyExecuted = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.Int32 ContinuousDelay{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean IsDeleted{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String DisplayOrder{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Argument{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? LastUpdateDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string LastUpdateDateStr
		{
			 get {if(LastUpdateDate.HasValue) return LastUpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { LastUpdateDate = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.Double? ExecutionTime{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Double? EstimatedExecutionTime{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? ShowUpdateInLog{ get; set; }

	}	
}
