using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using ControlPanel.Core.DataTransfer;
using ControlPanel.Core.DataTransfer.PerformanceStatistic;

namespace ControlPanel.Core.IService
{
		
	public interface IPerformanceStatisticService
	{
        Dictionary<string, string> GetPerformanceStatisticBasicSearchColumns();
        
        List<SearchColumn> GetPerformanceStatisticAdvanceSearchColumns();

		PerformanceStatistic GetPerformanceStatistic(System.Int32 PerformanceStatisticId);
		DataTransfer<List<GetOutput>> GetAll();
		PerformanceStatistic UpdatePerformanceStatistic(PerformanceStatistic entity);
		bool DeletePerformanceStatistic(System.Int32 PerformanceStatisticId);
		List<PerformanceStatistic> GetAllPerformanceStatistic();
		PerformanceStatistic InsertPerformanceStatistic(PerformanceStatistic entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
