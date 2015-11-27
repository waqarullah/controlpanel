using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;

namespace ControlPanel.Core.DataInterfaces
{
		
	public interface IDriveStatisticRepositoryBase
	{
        
        Dictionary<string, string> GetDriveStatisticBasicSearchColumns();
        List<SearchColumn> GetDriveStatisticSearchColumns();
        List<SearchColumn> GetDriveStatisticAdvanceSearchColumns();
        

		DriveStatistic GetDriveStatistic(System.Int32 DriveStatisticId,string SelectClause=null);
		DriveStatistic UpdateDriveStatistic(DriveStatistic entity);
		bool DeleteDriveStatistic(System.Int32 DriveStatisticId);
		DriveStatistic DeleteDriveStatistic(DriveStatistic entity);
		List<DriveStatistic> GetPagedDriveStatistic(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<DriveStatistic> GetAllDriveStatistic(string SelectClause=null);
		DriveStatistic InsertDriveStatistic(DriveStatistic entity);
		List<DriveStatistic> GetDriveStatisticByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
