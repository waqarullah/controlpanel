using System;
using System.Runtime.Serialization;

namespace ControlPanel.Core.DataTransfer.SystemProcess
{
    [DataContract]
	public class PostInput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public string SystemProcessId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Name{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Description{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string Enabled{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string DisplayOrder{ get; set; }

	}	
}
