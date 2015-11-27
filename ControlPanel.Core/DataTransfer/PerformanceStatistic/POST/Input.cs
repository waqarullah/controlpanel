using System;
using System.Runtime.Serialization;
using Validation;

namespace ControlPanel.Core.DataTransfer.PerformanceStatistic
{
    [DataContract]
	public class PostInput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public string PerformanceStatisticId{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Double)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string Cpu{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Double)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string Memory{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string CreationDate{ get; set; }

		[FieldLength(MaxLength = 255)]
		[FieldNullable(IsNullable = true)]
		[DataMember (EmitDefaultValue=false)]
		public string MachineName{ get; set; }

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

	}	
}
