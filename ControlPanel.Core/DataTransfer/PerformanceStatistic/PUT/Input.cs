using System;
using System.Runtime.Serialization;
using Validation;

namespace ControlPanel.Core.DataTransfer.PerformanceStatistic
{
    [DataContract]
	public class PutInput
	{
			
		[FieldTypeValidation(DataType=DataTypes.Integer)]
		[FieldNullable(IsNullable = false)]
		[DataMember (EmitDefaultValue=false)]
		public string PerformanceStatisticId{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Double)]
		[DataMember (EmitDefaultValue=false)]
		public string Cpu{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Double)]
		[DataMember (EmitDefaultValue=false)]
		public string Memory{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public string CreationDate{ get; set; }

		[FieldLength(MaxLength = 255)]
		[DataMember (EmitDefaultValue=false)]
		public string MachineName{ get; set; }

		[FieldLength(MaxLength = 50)]
		[DataMember (EmitDefaultValue=false)]
		public string IpAddress{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Double)]
		[DataMember (EmitDefaultValue=false)]
		public string DriveSpaceAvailable{ get; set; }

		[FieldTypeValidation(DataType=DataTypes.Double)]
		[DataMember (EmitDefaultValue=false)]
		public string DriveTotalSpace{ get; set; }

	}	
}
