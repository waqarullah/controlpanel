using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using ControlPanel.Core.DataTransfer;
using ControlPanel.Core.DataTransfer.SystemProcess;

namespace ControlPanel.Core.IService
{
		
	public interface ISystemProcessService
	{
        Dictionary<string, string> GetSystemProcessBasicSearchColumns();
        
		SystemProcess GetSystemProcess(System.Int32 SystemProcessId);
		DataTransfer<List<GetOutput>> GetAll();
		SystemProcess UpdateSystemProcess(SystemProcess entity);
		bool DeleteSystemProcess(System.Int32 SystemProcessId);
		List<SystemProcess> GetAllSystemProcess();
		SystemProcess InsertSystemProcess(SystemProcess entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);

        DataTransfer<GetOutput> GetSystemProcessByName(string name);
        List<SystemProcessThread> GetSystemProcessThreadsByProcessName(string name);
        List<SystemProcess> GetSystemProcessByIp(string ip, string connectionString);
	}
	
	
}
