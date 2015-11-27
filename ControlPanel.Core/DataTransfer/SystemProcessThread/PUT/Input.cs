using System;
using System.Runtime.Serialization;

namespace ControlPanel.Core.DataTransfer.SystemProcessThread
{
    [DataContract]
	public class PutInput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public string SystemProcessThreadId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string SystemProcessId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Name{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string SpringEntryName{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Description{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Enabled{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Continuous{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string SleepTime{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string AutoStart{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Status{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Message{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string ScheduledTime{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string StartRange{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string EndRange{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string LastSuccessfullyExecuted{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string ContinuousDelay{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string IsDeleted{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string DisplayOrder{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Argument{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string LastUpdateDate{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string ExecutionTime{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string EstimatedExecutionTime{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string ShowUpdateInLog{ get; set; }

	}	
}
