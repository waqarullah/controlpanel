﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ControlPanel.Core.Entities
{
	[DataContract]
    public abstract class EntityBase
    {
        [IgnoreDataMember]
        public bool IsUpdated { get; set; }
    }
}