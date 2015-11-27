using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using ControlPanel.Core.DataTransfer;
using ControlPanel.Core.DataTransfer.SystemProcessThread;
using ControlPanel.Core;
using System.Linq;
using ControlPanel.Repository;

namespace ControlPanel.Service
{
		
	public class SystemProcessThreadService
	{
		private SystemProcessThreadRepository _iSystemProcessThreadRepository;
        
		public SystemProcessThreadService()
		{
			this._iSystemProcessThreadRepository = new SystemProcessThreadRepository();
		}
        
        public Dictionary<string, string> GetSystemProcessThreadBasicSearchColumns()
        {
            
            return this._iSystemProcessThreadRepository.GetSystemProcessThreadBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetSystemProcessThreadAdvanceSearchColumns()
        {
            
            return this._iSystemProcessThreadRepository.GetSystemProcessThreadAdvanceSearchColumns();
           
        }
        

		public virtual List<SystemProcessThread> GetSystemProcessThreadBySystemProcessId(System.Int32 SystemProcessId)
		{
			return _iSystemProcessThreadRepository.GetSystemProcessThreadBySystemProcessId(SystemProcessId);
		}

		public SystemProcessThread GetSystemProcessThread(System.Int32 SystemProcessThreadId)
		{
			return _iSystemProcessThreadRepository.GetSystemProcessThread(SystemProcessThreadId);
		}

		public SystemProcessThread UpdateSystemProcessThread(SystemProcessThread entity)
		{
			return _iSystemProcessThreadRepository.UpdateSystemProcessThread(entity);
		}

		public bool DeleteSystemProcessThread(System.Int32 SystemProcessThreadId)
		{
			return _iSystemProcessThreadRepository.DeleteSystemProcessThread(SystemProcessThreadId);
		}

		public List<SystemProcessThread> GetAllSystemProcessThread()
		{
			return _iSystemProcessThreadRepository.GetAllSystemProcessThread();
		}

		public SystemProcessThread InsertSystemProcessThread(SystemProcessThread entity)
		{
			 return _iSystemProcessThreadRepository.InsertSystemProcessThread(entity);
		}


        public DataTransfer<GetOutput> Get(string id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 systemprocessthreadid=0;
            if (!string.IsNullOrEmpty(id) && System.Int32.TryParse(id,out systemprocessthreadid))
            {
				SystemProcessThread systemprocessthread = _iSystemProcessThreadRepository.GetSystemProcessThread(systemprocessthreadid);
                if(systemprocessthread!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(systemprocessthread);
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
            List<SystemProcessThread> systemprocessthreadlist = _iSystemProcessThreadRepository.GetAllSystemProcessThread();
            if (systemprocessthreadlist != null && systemprocessthreadlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(systemprocessthreadlist);
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
            SystemProcessThread systemprocessthreadinput = new SystemProcessThread();
            SystemProcessThread systemprocessthreadoutput = new SystemProcessThread();
            PutOutput output = new PutOutput();
            systemprocessthreadinput.CopyFrom(Input);
            SystemProcessThread systemprocessthread = _iSystemProcessThreadRepository.GetSystemProcessThread(systemprocessthreadinput.SystemProcessThreadId);
            if (systemprocessthread != null)
            {
                systemprocessthreadoutput = _iSystemProcessThreadRepository.UpdateSystemProcessThread(systemprocessthreadinput);
                if (systemprocessthreadoutput != null)
                {
                    output.CopyFrom(systemprocessthreadoutput);
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
            System.Int32 systemprocessthreadid=0;
            if (!string.IsNullOrEmpty(id) && System.Int32.TryParse(id,out systemprocessthreadid))
            {
				 bool IsDeleted = _iSystemProcessThreadRepository.DeleteSystemProcessThread(systemprocessthreadid);
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


         public SystemProcessThread GetSystemProcessThreadByName(string threadName)
         {
             return _iSystemProcessThreadRepository.GetSystemProcessThreadByName(threadName);
         }


         public List<SystemProcessThread> GetSystemProcessThreadByProcessID(int processID)
         {
             return _iSystemProcessThreadRepository.GetSystemProcessThreadByProcessID(processID);
         }


         public bool ToggleSystemProcessThreadContinuous(int systemProcessThreadID)
         {
             return _iSystemProcessThreadRepository.ToggleSystemProcessThreadContinuous(systemProcessThreadID);
         }

         public bool ToggleSystemProcessThreadEnabled(int systemProcessThreadID)
         {
             return _iSystemProcessThreadRepository.ToggleSystemProcessThreadEnabled(systemProcessThreadID);
         }


         public DataTransfer<List<SystemProcessThread>> GetSystemProcessThreadsByLastUpdateDate(DateTime lastUpdateDate)
         {
             List<SystemProcessThread> threads = new List<SystemProcessThread>();
             try
             {
                 return new DataTransfer<List<SystemProcessThread>>() { Data = _iSystemProcessThreadRepository.GetSystemProcessThreadsByLastUpdateDate(lastUpdateDate), IsSuccess = true };
             }
             catch (Exception exp)
             { return new DataTransfer<List<SystemProcessThread>>() { IsSuccess = false, Errors = new string[] { exp.Message } }; }
         }

         public SystemProcessThread GetSystemProcessThreadBySpringName(string springName)
         {
             return _iSystemProcessThreadRepository.GetSystemProcessThreadBySpringName(springName);
         }


         public SystemProcessThread GetSystemProcessThread(int SystemProcessThreadId, string connectionString = null)
         {
             return _iSystemProcessThreadRepository.GetSystemProcessThread(SystemProcessThreadId, connectionString);
         }

         public SystemProcessThread UpdateSystemProcessThread(SystemProcessThread entity, string connectionString)
         {
             return _iSystemProcessThreadRepository.UpdateSystemProcessThread(entity, connectionString);
         }

         public SystemProcessThread GetSystemProcessThreadBySpringName(string springName, string connectionString = null)
         {
             return _iSystemProcessThreadRepository.GetSystemProcessThreadBySpringName(springName, connectionString);
         }


         public SystemProcessThread InsertSystemProcessThread(SystemProcessThread entity, string connectionString)
         {
             return _iSystemProcessThreadRepository.InsertSystemProcessThread(entity, connectionString);
         }

         public SystemProcessThread GetSystemProcessThreadsByTenantId(int TenantId, string connectionString = null)
         {

             return _iSystemProcessThreadRepository.GetSystemProcessThreadsByTenantId(TenantId, connectionString);

         }
    }
	
	
}
