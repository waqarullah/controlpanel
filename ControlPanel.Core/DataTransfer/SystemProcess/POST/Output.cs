using System;
using System.Runtime.Serialization;

namespace ControlPanel.Core.DataTransfer.SystemProcess
{
    [DataContract]
	public class PostOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 SystemProcessId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Name{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Description{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean Enabled{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32 DisplayOrder{ get; set; }

	}	
}
