using System;
using System.Runtime.Serialization;
using Validation;

namespace ControlPanel.Core.DataTransfer.DriveStatistic
{
    [DataContract]
	public class PostInput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public string DriveStatisticId{ get; set; }

		[FieldLength(MaxLength = 50)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string IpAddress{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Double)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string DriveSpaceAvailable{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Double)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string DriveTotalSpace{ get; set; }

		[FieldLength(MaxLength = 50)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string DriveName{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string CreationDate{ get; set; }

		[FieldLength(MaxLength = 50)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string MachineName{ get; set; }

	}	
}
