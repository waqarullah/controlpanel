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
	public abstract partial class SystemProcessBase:EntityBase, IEquatable<SystemProcessBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("SystemProcessID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 SystemProcessId{ get; set; }

		[FieldNameAttribute("Name",false,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Name{ get; set; }

		[FieldNameAttribute("Description",true,false,100)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Description{ get; set; }

		[FieldNameAttribute("Enabled",false,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean Enabled{ get; set; }

		[FieldNameAttribute("DisplayOrder",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 DisplayOrder{ get; set; }

		[FieldNameAttribute("Ip",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Ip{ get; set; }

		[FieldNameAttribute("Port",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? Port{ get; set; }

		
		public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<SystemProcessBase> Members

        public virtual bool Equals(SystemProcessBase other)
        {
			if(this.SystemProcessId==other.SystemProcessId  && this.Name==other.Name  && this.Description==other.Description  && this.Enabled==other.Enabled  && this.DisplayOrder==other.DisplayOrder  && this.Ip==other.Ip  && this.Port==other.Port )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(SystemProcess other)
        {
			if(other!=null)
			{
				this.SystemProcessId=other.SystemProcessId;
				this.Name=other.Name;
				this.Description=other.Description;
				this.Enabled=other.Enabled;
				this.DisplayOrder=other.DisplayOrder;
				this.Ip=other.Ip;
				this.Port=other.Port;
			}
			
		
		}

        #endregion
		
		
		
	}
	
	
}
