using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using ControlPanel.Core.DataTransfer;
using ControlPanel.Core.DataTransfer.DriveStatistic;

namespace ControlPanel.Core.IService
{
		
	public interface IDriveStatisticService
	{
        Dictionary<string, string> GetDriveStatisticBasicSearchColumns();
        
        List<SearchColumn> GetDriveStatisticAdvanceSearchColumns();

		DriveStatistic GetDriveStatistic(System.Int32 DriveStatisticId);
		DataTransfer<List<GetOutput>> GetAll();
		DriveStatistic UpdateDriveStatistic(DriveStatistic entity);
		bool DeleteDriveStatistic(System.Int32 DriveStatisticId);
		List<DriveStatistic> GetAllDriveStatistic();
		DriveStatistic InsertDriveStatistic(DriveStatistic entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
