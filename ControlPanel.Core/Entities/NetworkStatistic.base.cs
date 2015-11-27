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
	public abstract partial class NetworkStatisticBase:EntityBase, IEquatable<NetworkStatisticBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("NetworkStatisticId",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 NetworkStatisticId{ get; set; }

		[FieldNameAttribute("InterfaceName",false,false,500)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String InterfaceName{ get; set; }

		[FieldNameAttribute("IPAddress",false,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String IpAddress{ get; set; }

		[FieldNameAttribute("TotalUsage",false,false,8)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Double TotalUsage{ get; set; }

		[FieldNameAttribute("Download",false,false,8)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Double Download{ get; set; }

		[FieldNameAttribute("Upload",false,false,8)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Double Upload{ get; set; }

		[FieldNameAttribute("CreationDate",false,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime CreationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string CreationDateStr
		{
			 get { return CreationDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); }
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("ServerIp",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String ServerIp{ get; set; }

		[FieldNameAttribute("ServerName",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String ServerName{ get; set; }

		
		public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<NetworkStatisticBase> Members

        public virtual bool Equals(NetworkStatisticBase other)
        {
			if(this.NetworkStatisticId==other.NetworkStatisticId  && this.InterfaceName==other.InterfaceName  && this.IpAddress==other.IpAddress  && this.TotalUsage==other.TotalUsage  && this.Download==other.Download  && this.Upload==other.Upload  && this.CreationDate==other.CreationDate  && this.ServerIp==other.ServerIp  && this.ServerName==other.ServerName )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(NetworkStatistic other)
        {
			if(other!=null)
			{
				this.NetworkStatisticId=other.NetworkStatisticId;
				this.InterfaceName=other.InterfaceName;
				this.IpAddress=other.IpAddress;
				this.TotalUsage=other.TotalUsage;
				this.Download=other.Download;
				this.Upload=other.Upload;
				this.CreationDate=other.CreationDate;
				this.ServerIp=other.ServerIp;
				this.ServerName=other.ServerName;
			}
			
		
		}

        #endregion
		
		
		
	}
	
	
}
