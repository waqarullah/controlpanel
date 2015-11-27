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
	public abstract partial class SystemProcessThreadBase:EntityBase, IEquatable<SystemProcessThreadBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("SystemProcessThreadID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 SystemProcessThreadId{ get; set; }

		[FieldNameAttribute("TenantId",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? TenantId{ get; set; }

		[FieldNameAttribute("SystemProcessID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 SystemProcessId{ get; set; }

		[FieldNameAttribute("Name",false,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Name{ get; set; }

		[FieldNameAttribute("SpringEntryName",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String SpringEntryName{ get; set; }

		[FieldNameAttribute("Description",true,false,100)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Description{ get; set; }

		[FieldNameAttribute("Enabled",false,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean Enabled{ get; set; }

		[FieldNameAttribute("Continuous",false,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean Continuous{ get; set; }

		[FieldNameAttribute("SleepTime",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 SleepTime{ get; set; }

		[FieldNameAttribute("AutoStart",false,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean AutoStart{ get; set; }

		[FieldNameAttribute("Status",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Status{ get; set; }

		[FieldNameAttribute("Message",true,false,500)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Message{ get; set; }

		[FieldNameAttribute("ScheduledTime",true,false,5)]
		[DataMember (EmitDefaultValue=false)]
		public virtual TimeSpan? ScheduledTime{ get; set; }

		[FieldNameAttribute("StartRange",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? StartRange{ get; set; }

		[FieldNameAttribute("EndRange",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? EndRange{ get; set; }

		[FieldNameAttribute("LastSuccessfullyExecuted",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? LastSuccessfullyExecuted{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string LastSuccessfullyExecutedStr
		{
			 get {if(LastSuccessfullyExecuted.HasValue) return LastSuccessfullyExecuted.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { LastSuccessfullyExecuted = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("ContinuousDelay",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 ContinuousDelay{ get; set; }

		[FieldNameAttribute("IsDeleted",false,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean IsDeleted{ get; set; }

		[FieldNameAttribute("DisplayOrder",false,false,10)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String DisplayOrder{ get; set; }

		[FieldNameAttribute("Argument",true,false,0)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Argument{ get; set; }

		[FieldNameAttribute("LastUpdateDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? LastUpdateDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string LastUpdateDateStr
		{
			 get {if(LastUpdateDate.HasValue) return LastUpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { LastUpdateDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("ExecutionTime",true,false,8)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Double? ExecutionTime{ get; set; }

		[FieldNameAttribute("EstimatedExecutionTime",true,false,8)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Double? EstimatedExecutionTime{ get; set; }

		[FieldNameAttribute("ShowUpdateInLog",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? ShowUpdateInLog{ get; set; }

		
		public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<SystemProcessThreadBase> Members

        public virtual bool Equals(SystemProcessThreadBase other)
        {
			if(this.SystemProcessThreadId==other.SystemProcessThreadId  && this.TenantId==other.TenantId  && this.SystemProcessId==other.SystemProcessId  && this.Name==other.Name  && this.SpringEntryName==other.SpringEntryName  && this.Description==other.Description  && this.Enabled==other.Enabled  && this.Continuous==other.Continuous  && this.SleepTime==other.SleepTime  && this.AutoStart==other.AutoStart  && this.Status==other.Status  && this.Message==other.Message  && this.ScheduledTime==other.ScheduledTime  && this.StartRange==other.StartRange  && this.EndRange==other.EndRange  && this.LastSuccessfullyExecuted==other.LastSuccessfullyExecuted  && this.ContinuousDelay==other.ContinuousDelay  && this.IsDeleted==other.IsDeleted  && this.DisplayOrder==other.DisplayOrder  && this.Argument==other.Argument  && this.LastUpdateDate==other.LastUpdateDate  && this.ExecutionTime==other.ExecutionTime  && this.EstimatedExecutionTime==other.EstimatedExecutionTime  && this.ShowUpdateInLog==other.ShowUpdateInLog )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(SystemProcessThread other)
        {
			if(other!=null)
			{
				this.SystemProcessThreadId=other.SystemProcessThreadId;
				this.TenantId=other.TenantId;
				this.SystemProcessId=other.SystemProcessId;
				this.Name=other.Name;
				this.SpringEntryName=other.SpringEntryName;
				this.Description=other.Description;
				this.Enabled=other.Enabled;
				this.Continuous=other.Continuous;
				this.SleepTime=other.SleepTime;
				this.AutoStart=other.AutoStart;
				this.Status=other.Status;
				this.Message=other.Message;
				this.ScheduledTime=other.ScheduledTime;
				this.StartRange=other.StartRange;
				this.EndRange=other.EndRange;
				this.LastSuccessfullyExecuted=other.LastSuccessfullyExecuted;
				this.ContinuousDelay=other.ContinuousDelay;
				this.IsDeleted=other.IsDeleted;
				this.DisplayOrder=other.DisplayOrder;
				this.Argument=other.Argument;
				this.LastUpdateDate=other.LastUpdateDate;
				this.ExecutionTime=other.ExecutionTime;
				this.EstimatedExecutionTime=other.EstimatedExecutionTime;
				this.ShowUpdateInLog=other.ShowUpdateInLog;
			}
			
		
		}

        #endregion
		
		
		
	}
	
	
}
