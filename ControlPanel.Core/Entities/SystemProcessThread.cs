
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;


namespace ControlPanel.Core.Entities
{
    [DataContract]
	public partial class SystemProcessThread : SystemProcessThreadBase 
	{
        [DataMember(EmitDefaultValue = false)]
        public string LastExecutedSeconds { get; set; }
	
		
	}
}
