using System;
using System.Runtime.Serialization;

namespace ControlPanel.Core.DataTransfer.SystemEventLog
{
    [DataContract]
	public class PutOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 EventLogId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? EventCode{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Title{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Description{ get; set; }

		[IgnoreDataMember]
		public System.DateTime DateOccured{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string DateOccuredStr
		{
			 get { return DateOccured.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); }
            set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { DateOccured = date.ToUniversalTime(); } } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.Guid? ElmahErrorId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Source{ get; set; }

	}	
}
