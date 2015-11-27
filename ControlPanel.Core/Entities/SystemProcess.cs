
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;
using System.Collections.Generic;


namespace ControlPanel.Core.Entities
{
    [DataContract]
    public partial class SystemProcess : SystemProcessBase
    {
        [DataMember(EmitDefaultValue = false)]
        public List<SystemProcessThread> SystemProcessThreadList { get; set; }

    }
}
