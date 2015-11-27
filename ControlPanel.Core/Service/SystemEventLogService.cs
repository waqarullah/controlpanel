using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using ControlPanel.Core.DataTransfer;
using ControlPanel.Core.DataTransfer.SystemEventLog;
using ControlPanel.Core;
using System.Linq;
using ControlPanel.Repository;
using ControlPanel.Core.IService;
using System.Configuration;


namespace ControlPanel.Service
{

    public class SystemEventLogService : ISystemEventLogService
	{
		private SystemEventLogRepository _iSystemEventLogRepository;
        
		public SystemEventLogService()
		{
            _iSystemEventLogRepository = new SystemEventLogRepository();
		}
        
        public Dictionary<string, string> GetSystemEventLogBasicSearchColumns()
        {
            
            return this._iSystemEventLogRepository.GetSystemEventLogBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetSystemEventLogAdvanceSearchColumns()
        {
            
            return this._iSystemEventLogRepository.GetSystemEventLogAdvanceSearchColumns();
           
        }
        

		public SystemEventLog GetSystemEventLog(System.Int32 EventLogId)
		{
			return _iSystemEventLogRepository.GetSystemEventLog(EventLogId);
		}

		public SystemEventLog UpdateSystemEventLog(SystemEventLog entity)
		{
			return _iSystemEventLogRepository.UpdateSystemEventLog(entity);
		}

		public bool DeleteSystemEventLog(System.Int32 EventLogId)
		{
			return _iSystemEventLogRepository.DeleteSystemEventLog(EventLogId);
		}

		public List<SystemEventLog> GetAllSystemEventLog()
		{
			return _iSystemEventLogRepository.GetAllSystemEventLog();
		}

		public SystemEventLog InsertSystemEventLog(SystemEventLog entity)
		{
			 return _iSystemEventLogRepository.InsertSystemEventLog(entity);
		}


        public DataTransfer<GetOutput> Get(string id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 eventlogid=0;
            if (!string.IsNullOrEmpty(id) && System.Int32.TryParse(id,out eventlogid))
            {
				SystemEventLog systemeventlog = _iSystemEventLogRepository.GetSystemEventLog(eventlogid);
                if(systemeventlog!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(systemeventlog);
                    tranfer.Data=output ;

                }
                else
                {
                    tranfer.IsSuccess = false;
                    tranfer.Errors = new string[1];
                    tranfer.Errors[0] = "Error: No record found.";
                }             
                
            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: Invalid request.";
            }
            return tranfer;
        }   
        public DataTransfer<List<GetOutput>> GetAll()
        {
            DataTransfer<List<GetOutput>> tranfer = new DataTransfer<List<GetOutput>>();
            List<SystemEventLog> systemeventloglist = _iSystemEventLogRepository.GetAllSystemEventLog();
            if (systemeventloglist != null && systemeventloglist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(systemeventloglist);
                tranfer.Data = outputlist;

            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: No record found.";
            }
            return tranfer;
        }
        public DataTransfer<PostOutput> Insert(PostInput Input)
        {
            throw new NotImplementedException();
        }

        public DataTransfer<PutOutput> Update(PutInput Input)
        {
            DataTransfer<PutOutput> transer = new DataTransfer<PutOutput>();

            SystemEventLog systemeventloginput = new SystemEventLog();
            SystemEventLog systemeventlogoutput = new SystemEventLog();
            PutOutput output = new PutOutput();
            systemeventloginput.CopyFrom(Input);
            SystemEventLog systemeventlog = _iSystemEventLogRepository.GetSystemEventLog(systemeventloginput.EventLogId);
            if (systemeventlog != null)
            {
                systemeventlogoutput = _iSystemEventLogRepository.UpdateSystemEventLog(systemeventloginput);
                if (systemeventlogoutput != null)
                {
                    output.CopyFrom(systemeventlogoutput);
                    transer.IsSuccess = true;
                    transer.Data = output;
                }
                else
                {
                    transer.IsSuccess = false;
                    transer.Errors = new string[1];
                    transer.Errors[0] = "Error: Could not update.";
                }
            }
            else
            {
                transer.IsSuccess = false;
                transer.Errors = new string[1];
                transer.Errors[0] = "Error: Record not found.";
            }

            return transer;
        }

         public DataTransfer<string> Delete(string id)
         {
            DataTransfer<string> tranfer = new DataTransfer<string>();
            System.Int32 eventlogid=0;
            if (!string.IsNullOrEmpty(id) && System.Int32.TryParse(id,out eventlogid))
            {
				 bool IsDeleted = _iSystemEventLogRepository.DeleteSystemEventLog(eventlogid);
                if(IsDeleted)
                {
                    tranfer.IsSuccess = true;
                    tranfer.Data=IsDeleted.ToString().ToLower() ;

                }
                else
                {
                    tranfer.IsSuccess = false;
                    tranfer.Errors = new string[1];
                    tranfer.Errors[0] = "Error: No record found.";
                }             
                
            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: Invalid request.";
            }
            return tranfer;
        }


         public List<SystemEventLog> GetSystemEventLogsByLastUpdateDate(DateTime lastUpdateDate)
         {
             return _iSystemEventLogRepository.GetSystemEventLogsByLastUpdateDate(lastUpdateDate);
         }


         public bool InsertSystemEventLog(string title, string description, Core.Enums.EventCodes code)
         {
             SystemEventLog evlog = new SystemEventLog();
             evlog.DateOccured = DateTime.UtcNow;
             evlog.Description = description;
             evlog.EventCode = (int)code;
             if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SystemProcess"]))
             {
                 var servicename = ConfigurationManager.AppSettings["SystemProcess"];
                 evlog.Title = title + "- " + servicename;
             }
             else
             {
                 evlog.Title = title;
             }
             var result = _iSystemEventLogRepository.InsertSystemEventLog(evlog);
             if (result != null && result.EventLogId > 0)
                 return true;
             else return false;
         }


         public List<SearchColumn> GetSystemEventLogSearchColumns()
         {
             return this._iSystemEventLogRepository.GetSystemEventLogSearchColumns();
         }


         public List<SystemEventLog> GetPagedSystemEventLog(string sort, int maximumRows, int startRow, out int count, List<SearchColumn> searchColumns)
         {
             return _iSystemEventLogRepository.GetPagedSystemEventLog(sort,  maximumRows,startRow, out count, searchColumns);
         }
    }
	
	
}
