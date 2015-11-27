using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;

namespace ControlPanel.Core.DataInterfaces
{
		
	public interface IPerformanceStatisticRepositoryBase
	{
        
        Dictionary<string, string> GetPerformanceStatisticBasicSearchColumns();
        List<SearchColumn> GetPerformanceStatisticSearchColumns();
        List<SearchColumn> GetPerformanceStatisticAdvanceSearchColumns();
        

		PerformanceStatistic GetPerformanceStatistic(System.Int32 PerformanceStatisticId,string SelectClause=null);
		PerformanceStatistic UpdatePerformanceStatistic(PerformanceStatistic entity);
		bool DeletePerformanceStatistic(System.Int32 PerformanceStatisticId);
		PerformanceStatistic DeletePerformanceStatistic(PerformanceStatistic entity);
		List<PerformanceStatistic> GetPagedPerformanceStatistic(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<PerformanceStatistic> GetAllPerformanceStatistic(string SelectClause=null);
		PerformanceStatistic InsertPerformanceStatistic(PerformanceStatistic entity);
		List<PerformanceStatistic> GetPerformanceStatisticByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
