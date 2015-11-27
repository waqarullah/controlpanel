using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using ControlPanel.Core.DataInterfaces;
using ControlPanel.Core.IService;
using ControlPanel.Core.DataTransfer;
using ControlPanel.Core.DataTransfer.DriveStatistic;
using Validation;
using System.Linq;

namespace ControlPanel.Core.Service
{
		
	public class DriveStatisticService : IDriveStatisticService 
	{
		private IDriveStatisticRepository _iDriveStatisticRepository;
        
		public DriveStatisticService(IDriveStatisticRepository iDriveStatisticRepository)
		{
			this._iDriveStatisticRepository = iDriveStatisticRepository;
		}
        
        public Dictionary<string, string> GetDriveStatisticBasicSearchColumns()
        {
            
            return this._iDriveStatisticRepository.GetDriveStatisticBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetDriveStatisticAdvanceSearchColumns()
        {
            
            return this._iDriveStatisticRepository.GetDriveStatisticAdvanceSearchColumns();
           
        }
        

		public DriveStatistic GetDriveStatistic(System.Int32 DriveStatisticId)
		{
			return _iDriveStatisticRepository.GetDriveStatistic(DriveStatisticId);
		}

		public DriveStatistic UpdateDriveStatistic(DriveStatistic entity)
		{
			return _iDriveStatisticRepository.UpdateDriveStatistic(entity);
		}

		public bool DeleteDriveStatistic(System.Int32 DriveStatisticId)
		{
			return _iDriveStatisticRepository.DeleteDriveStatistic(DriveStatisticId);
		}

		public List<DriveStatistic> GetAllDriveStatistic()
		{
			return _iDriveStatisticRepository.GetAllDriveStatistic();
		}

		public DriveStatistic InsertDriveStatistic(DriveStatistic entity)
		{
			 return _iDriveStatisticRepository.InsertDriveStatistic(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 drivestatisticid=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out drivestatisticid))
            {
				DriveStatistic drivestatistic = _iDriveStatisticRepository.GetDriveStatistic(drivestatisticid);
                if(drivestatistic!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(drivestatistic);
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
            List<DriveStatistic> drivestatisticlist = _iDriveStatisticRepository.GetAllDriveStatistic();
            if (drivestatisticlist != null && drivestatisticlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(drivestatisticlist);
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
           DataTransfer<PostOutput> transer = new DataTransfer<PostOutput>();
            IList<string> errors = Validator.Validate(Input);
            if(errors.Count==0)
            {
                DriveStatistic drivestatistic = new DriveStatistic();
                PostOutput output = new PostOutput();
                drivestatistic.CopyFrom(Input);
                drivestatistic = _iDriveStatisticRepository.InsertDriveStatistic(drivestatistic);
                output.CopyFrom(drivestatistic);
                transer.IsSuccess = true;
                transer.Data = output;
            }
            else
            {
                transer.IsSuccess = false;
                transer.Errors = errors.ToArray<string>();
            }
            return transer;
        }

        public DataTransfer<PutOutput> Update(PutInput Input)
        {
            DataTransfer<PutOutput> transer = new DataTransfer<PutOutput>();
            IList<string> errors = Validator.Validate(Input);
            if (errors.Count == 0)
            {
                DriveStatistic drivestatisticinput = new DriveStatistic();
                DriveStatistic drivestatisticoutput = new DriveStatistic();
                PutOutput output = new PutOutput();
                drivestatisticinput.CopyFrom(Input);
                DriveStatistic drivestatistic = _iDriveStatisticRepository.GetDriveStatistic(drivestatisticinput.DriveStatisticId);
                if (drivestatistic!=null)
                {
                    drivestatisticoutput = _iDriveStatisticRepository.UpdateDriveStatistic(drivestatisticinput);
                    if(drivestatisticoutput!=null)
                    {
                        output.CopyFrom(drivestatisticoutput);
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
            }
            else
            {
                transer.IsSuccess = false;
                transer.Errors = errors.ToArray<string>();
            }
            return transer;
        }

         public DataTransfer<string> Delete(string _id)
         {
            DataTransfer<string> tranfer = new DataTransfer<string>();
            System.Int32 drivestatisticid=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out drivestatisticid))
            {
				 bool IsDeleted = _iDriveStatisticRepository.DeleteDriveStatistic(drivestatisticid);
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
	}
	
	
}
