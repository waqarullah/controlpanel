using System;
using System.Runtime.Serialization;

namespace ControlPanel.Core.DataTransfer.SystemEventLog
{
    [DataContract]
	public class PutInput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public string EventLogId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string EventCode{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Title{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Description{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string DateOccured{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string ElmahErrorId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Source{ get; set; }

	}	
}
