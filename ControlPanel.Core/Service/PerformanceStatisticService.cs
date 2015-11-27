using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using ControlPanel.Core.DataInterfaces;
using ControlPanel.Core.IService;
using ControlPanel.Core.DataTransfer;
using ControlPanel.Core.DataTransfer.PerformanceStatistic;
using Validation;
using System.Linq;

namespace ControlPanel.Core.Service
{
		
	public class PerformanceStatisticService : IPerformanceStatisticService 
	{
		private IPerformanceStatisticRepository _iPerformanceStatisticRepository;
        
		public PerformanceStatisticService(IPerformanceStatisticRepository iPerformanceStatisticRepository)
		{
			this._iPerformanceStatisticRepository = iPerformanceStatisticRepository;
		}
        
        public Dictionary<string, string> GetPerformanceStatisticBasicSearchColumns()
        {
            
            return this._iPerformanceStatisticRepository.GetPerformanceStatisticBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetPerformanceStatisticAdvanceSearchColumns()
        {
            
            return this._iPerformanceStatisticRepository.GetPerformanceStatisticAdvanceSearchColumns();
           
        }
        

		public PerformanceStatistic GetPerformanceStatistic(System.Int32 PerformanceStatisticId)
		{
			return _iPerformanceStatisticRepository.GetPerformanceStatistic(PerformanceStatisticId);
		}

		public PerformanceStatistic UpdatePerformanceStatistic(PerformanceStatistic entity)
		{
			return _iPerformanceStatisticRepository.UpdatePerformanceStatistic(entity);
		}

		public bool DeletePerformanceStatistic(System.Int32 PerformanceStatisticId)
		{
			return _iPerformanceStatisticRepository.DeletePerformanceStatistic(PerformanceStatisticId);
		}

		public List<PerformanceStatistic> GetAllPerformanceStatistic()
		{
			return _iPerformanceStatisticRepository.GetAllPerformanceStatistic();
		}

		public PerformanceStatistic InsertPerformanceStatistic(PerformanceStatistic entity)
		{
			 return _iPerformanceStatisticRepository.InsertPerformanceStatistic(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 performancestatisticid=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out performancestatisticid))
            {
				PerformanceStatistic performancestatistic = _iPerformanceStatisticRepository.GetPerformanceStatistic(performancestatisticid);
                if(performancestatistic!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(performancestatistic);
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
            List<PerformanceStatistic> performancestatisticlist = _iPerformanceStatisticRepository.GetAllPerformanceStatistic();
            if (performancestatisticlist != null && performancestatisticlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(performancestatisticlist);
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
                PerformanceStatistic performancestatistic = new PerformanceStatistic();
                PostOutput output = new PostOutput();
                performancestatistic.CopyFrom(Input);
                performancestatistic = _iPerformanceStatisticRepository.InsertPerformanceStatistic(performancestatistic);
                output.CopyFrom(performancestatistic);
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
                PerformanceStatistic performancestatisticinput = new PerformanceStatistic();
                PerformanceStatistic performancestatisticoutput = new PerformanceStatistic();
                PutOutput output = new PutOutput();
                performancestatisticinput.CopyFrom(Input);
                PerformanceStatistic performancestatistic = _iPerformanceStatisticRepository.GetPerformanceStatistic(performancestatisticinput.PerformanceStatisticId);
                if (performancestatistic!=null)
                {
                    performancestatisticoutput = _iPerformanceStatisticRepository.UpdatePerformanceStatistic(performancestatisticinput);
                    if(performancestatisticoutput!=null)
                    {
                        output.CopyFrom(performancestatisticoutput);
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
            System.Int32 performancestatisticid=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out performancestatisticid))
            {
				 bool IsDeleted = _iPerformanceStatisticRepository.DeletePerformanceStatistic(performancestatisticid);
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
