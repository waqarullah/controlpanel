using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using ControlPanel.Core.DataTransfer;
using ControlPanel.Core.DataTransfer.SystemProcess;
using ControlPanel.Core;
using System.Linq;
using ControlPanel.Repository;

namespace ControlPanel.Service
{
	
	public class SystemProcessService
	{
		private SystemProcessRepository _iSystemProcessRepository;
        
		public SystemProcessService()
		{
			this._iSystemProcessRepository = new SystemProcessRepository();
		}
        
        public Dictionary<string, string> GetSystemProcessBasicSearchColumns()
        {
            
            return this._iSystemProcessRepository.GetSystemProcessBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetSystemProcessAdvanceSearchColumns()
        {
            
            return this._iSystemProcessRepository.GetSystemProcessAdvanceSearchColumns();
           
        }
        

		public SystemProcess GetSystemProcess(System.Int32 SystemProcessId)
		{
			return _iSystemProcessRepository.GetSystemProcess(SystemProcessId);
		}

		public SystemProcess UpdateSystemProcess(SystemProcess entity)
		{
			return _iSystemProcessRepository.UpdateSystemProcess(entity);
		}

		public bool DeleteSystemProcess(System.Int32 SystemProcessId)
		{
			return _iSystemProcessRepository.DeleteSystemProcess(SystemProcessId);
		}

		public List<SystemProcess> GetAllSystemProcess()
		{
			return _iSystemProcessRepository.GetAllSystemProcess();
		}

		public SystemProcess InsertSystemProcess(SystemProcess entity)
		{
			 return _iSystemProcessRepository.InsertSystemProcess(entity);
		}


        public DataTransfer<GetOutput> Get(string id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 systemprocessid=0;
            if (!string.IsNullOrEmpty(id) && System.Int32.TryParse(id,out systemprocessid))
            {
				SystemProcess systemprocess = _iSystemProcessRepository.GetSystemProcess(systemprocessid);
                if(systemprocess!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(systemprocess);
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
            List<SystemProcess> systemprocesslist = _iSystemProcessRepository.GetAllSystemProcess();
            if (systemprocesslist != null && systemprocesslist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(systemprocesslist);
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

        public DataTransfer<PutOutput> Update(PutInput Input)
        {
            DataTransfer<PutOutput> transer = new DataTransfer<PutOutput>();
            SystemProcess systemprocessinput = new SystemProcess();
            SystemProcess systemprocessoutput = new SystemProcess();
            PutOutput output = new PutOutput();
            systemprocessinput.CopyFrom(Input);
            SystemProcess systemprocess = _iSystemProcessRepository.GetSystemProcess(systemprocessinput.SystemProcessId);
            if (systemprocess != null)
            {
                systemprocessoutput = _iSystemProcessRepository.UpdateSystemProcess(systemprocessinput);
                if (systemprocessoutput != null)
                {
                    output.CopyFrom(systemprocessoutput);
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
            System.Int32 systemprocessid=0;
            if (!string.IsNullOrEmpty(id) && System.Int32.TryParse(id,out systemprocessid))
            {
				 bool IsDeleted = _iSystemProcessRepository.DeleteSystemProcess(systemprocessid);
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

         public DataTransfer<GetOutput> GetSystemProcessByName(string name)
         {
             DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
             System.Int32 systemprocessid = 0;
             if (!string.IsNullOrEmpty(name))
             {
                 SystemProcess systemprocess = _iSystemProcessRepository.GetSystemProcessByName(name);
                 if (systemprocess != null)
                 {
                     tranfer.IsSuccess = true;
                     GetOutput output = new GetOutput();
                     output.CopyFrom(systemprocess);
                     tranfer.Data = output;

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

         public List<SystemProcessThread> GetSystemProcessThreadsByProcessName(string name)
         {
             return _iSystemProcessRepository.GetSystemProcessThreadsByProcessName(name);
         }


         public DataTransfer<PostOutput> Insert(PostInput Input)
         {
             throw new NotImplementedException();
         }

         public List<SystemProcess> GetSystemProcessByIp(string ip, string connectionString)
         {
             return _iSystemProcessRepository.GetSystemProcessByIp(ip, connectionString);
         }
    }
	
	
}
