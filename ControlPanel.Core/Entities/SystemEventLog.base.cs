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
	public abstract partial class SystemEventLogBase:EntityBase, IEquatable<SystemEventLogBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("EventLogID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 EventLogId{ get; set; }

		[FieldNameAttribute("EventCode",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? EventCode{ get; set; }

		[FieldNameAttribute("Title",true,false,500)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Title{ get; set; }

		[FieldNameAttribute("Description",true,false,1000)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Description{ get; set; }

		[FieldNameAttribute("DateOccured",false,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime DateOccured{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string DateOccuredStr
		{
			 get { return DateOccured.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); }
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { DateOccured = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("ElmahErrorID",true,false,16)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Guid? ElmahErrorId{ get; set; }

		[FieldNameAttribute("Source",true,false,60)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Source{ get; set; }

		
		public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<SystemEventLogBase> Members

        public virtual bool Equals(SystemEventLogBase other)
        {
			if(this.EventLogId==other.EventLogId  && this.EventCode==other.EventCode  && this.Title==other.Title  && this.Description==other.Description  && this.DateOccured==other.DateOccured  && this.ElmahErrorId==other.ElmahErrorId  && this.Source==other.Source )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(SystemEventLog other)
        {
			if(other!=null)
			{
				this.EventLogId=other.EventLogId;
				this.EventCode=other.EventCode;
				this.Title=other.Title;
				this.Description=other.Description;
				this.DateOccured=other.DateOccured;
				this.ElmahErrorId=other.ElmahErrorId;
				this.Source=other.Source;
			}
			
		
		}

        #endregion
		
		
		
	}
	
	
}
