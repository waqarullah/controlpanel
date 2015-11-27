using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using ControlPanel.Core.DataTransfer;
using ControlPanel.Core.DataTransfer.SystemEventLog;
using ControlPanel.Core.Enums;

namespace ControlPanel.Core.IService
{
		
	public interface ISystemEventLogService
	{
        Dictionary<string, string> GetSystemEventLogBasicSearchColumns();

		SystemEventLog GetSystemEventLog(System.Int32 EventLogId);
		DataTransfer<List<GetOutput>> GetAll();
		SystemEventLog UpdateSystemEventLog(SystemEventLog entity);
		bool DeleteSystemEventLog(System.Int32 EventLogId);
		List<SystemEventLog> GetAllSystemEventLog();
		SystemEventLog InsertSystemEventLog(SystemEventLog entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);

        List<SystemEventLog> GetSystemEventLogsByLastUpdateDate(DateTime lastUpdateDate);

        bool InsertSystemEventLog(string title,string description,EventCodes code);

        List<SearchColumn> GetSystemEventLogSearchColumns();

        List<SystemEventLog> GetPagedSystemEventLog(string sort, int maximumRows, int startRow, out int count, List<SearchColumn> searchColumns);
    }
	
	
}
