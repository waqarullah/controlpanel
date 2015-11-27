using System;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ControlPanel.Core;
using ControlPanel.Core.Entities;
using ControlPanel.Core.Extensions;

namespace ControlPanel.Repository
{
		
	public abstract partial class PerformanceStatisticRepositoryBase : Repository
	{
        
        public PerformanceStatisticRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("PerformanceStatisticID",new SearchColumn(){Name="PerformanceStatisticID",Title="PerformanceStatisticID",SelectClause="PerformanceStatisticID",WhereClause="AllRecords.PerformanceStatisticID",DataType="System.Int32",IsForeignColumn=false,PropertyName="PerformanceStatisticId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CPU",new SearchColumn(){Name="CPU",Title="CPU",SelectClause="CPU",WhereClause="AllRecords.CPU",DataType="System.Double?",IsForeignColumn=false,PropertyName="Cpu",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Memory",new SearchColumn(){Name="Memory",Title="Memory",SelectClause="Memory",WhereClause="AllRecords.Memory",DataType="System.Double?",IsForeignColumn=false,PropertyName="Memory",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreationDate",new SearchColumn(){Name="CreationDate",Title="CreationDate",SelectClause="CreationDate",WhereClause="AllRecords.CreationDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("MachineName",new SearchColumn(){Name="MachineName",Title="MachineName",SelectClause="MachineName",WhereClause="AllRecords.MachineName",DataType="System.String",IsForeignColumn=false,PropertyName="MachineName",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IPAddress",new SearchColumn(){Name="IPAddress",Title="IPAddress",SelectClause="IPAddress",WhereClause="AllRecords.IPAddress",DataType="System.String",IsForeignColumn=false,PropertyName="IpAddress",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DriveSpaceAvailable",new SearchColumn(){Name="DriveSpaceAvailable",Title="DriveSpaceAvailable",SelectClause="DriveSpaceAvailable",WhereClause="AllRecords.DriveSpaceAvailable",DataType="System.Double?",IsForeignColumn=false,PropertyName="DriveSpaceAvailable",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DriveTotalSpace",new SearchColumn(){Name="DriveTotalSpace",Title="DriveTotalSpace",SelectClause="DriveTotalSpace",WhereClause="AllRecords.DriveTotalSpace",DataType="System.Double?",IsForeignColumn=false,PropertyName="DriveTotalSpace",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetPerformanceStatisticSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetPerformanceStatisticBasicSearchColumns()
        {
			Dictionary<string, string> columnList = new Dictionary<string, string>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                if (keyValuePair.Value.IsBasicSearchColumm)
                {
					keyValuePair.Value.Value = string.Empty;
                    columnList.Add(keyValuePair.Key, keyValuePair.Value.Title);
                }
            }
            return columnList;
        }

        public virtual List<SearchColumn> GetPerformanceStatisticAdvanceSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                if (keyValuePair.Value.IsAdvanceSearchColumn)
                {
					keyValuePair.Value.Value = string.Empty;
					searchColumns.Add(keyValuePair.Value);
                }
            }
            return searchColumns;
        }
        
        
        public virtual string GetPerformanceStatisticSelectClause()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            string selectQuery=string.Empty;
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                if (!keyValuePair.Value.IgnoreForDefaultSelect)
                {
					if (keyValuePair.Value.IsForeignColumn)
                	{
						if(string.IsNullOrEmpty(selectQuery))
						{
							selectQuery = "("+keyValuePair.Value.SelectClause+") as \""+keyValuePair.Key+"\"";
						}
						else
						{
							selectQuery += ",(" + keyValuePair.Value.SelectClause + ") as \"" + keyValuePair.Key + "\"";
						}
                	}
                	else
                	{
                    	if (string.IsNullOrEmpty(selectQuery))
                    	{
                        	selectQuery =  "[PerformanceStatistic].["+keyValuePair.Key+"]";
                    	}
                    	else
                    	{
                        	selectQuery += ",[PerformanceStatistic].["+keyValuePair.Key+"]";
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual PerformanceStatistic GetPerformanceStatistic(System.Int32 PerformanceStatisticId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetPerformanceStatisticSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [PerformanceStatistic] with (nolock)  where PerformanceStatisticID=@PerformanceStatisticID ";
			SqlParameter parameter=new SqlParameter("@PerformanceStatisticID",PerformanceStatisticId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return PerformanceStatisticFromDataRow(ds.Tables[0].Rows[0]);
		}

		public  List<PerformanceStatistic> GetPerformanceStatisticByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetPerformanceStatisticSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [PerformanceStatistic] with (nolock)  where {0} {1} '{2}' ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<PerformanceStatistic>(ds,PerformanceStatisticFromDataRow);
		}

		public virtual List<PerformanceStatistic> GetAllPerformanceStatistic(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetPerformanceStatisticSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [PerformanceStatistic] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<PerformanceStatistic>(ds, PerformanceStatisticFromDataRow);
		}

		public virtual List<PerformanceStatistic> GetPagedPerformanceStatistic(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetPerformanceStatisticCount(whereClause, searchColumns);
			if(count>0)
			{
			if (count < startIndex) startIndex = (count / pageSize) * pageSize;			
			
           	int PageLowerBound = startIndex;
            int PageUpperBound = PageLowerBound + pageSize;
            string sql = @"CREATE TABLE #PageIndex
				            (
				                [IndexId] int IDENTITY (1, 1) NOT NULL,
				                [PerformanceStatisticID] int				   
				            );";

            //Insert into the temp table
            string tempsql = "INSERT INTO #PageIndex ([PerformanceStatisticID])";
            tempsql += " SELECT ";
            if (pageSize > 0) tempsql += "TOP " + PageUpperBound.ToString();
            tempsql += " [PerformanceStatisticID] ";
            tempsql += " FROM [PerformanceStatistic] AllRecords with (NOLOCK)";
            if (!string.IsNullOrEmpty(whereClause) && whereClause.Length > 0) tempsql += " WHERE " + whereClause;
            if (orderByClause.Length > 0) 
			{
				tempsql += " ORDER BY " + orderByClause;
				if( !orderByClause.Contains("PerformanceStatisticID"))
					tempsql += " , (AllRecords.[PerformanceStatisticID])"; 
			}
			else 
			{
				tempsql  += " ORDER BY (AllRecords.[PerformanceStatisticID])"; 
			}           
            
            // Return paged results
            string pagedResultsSql =
                (string.IsNullOrEmpty(SelectClause)? GetPerformanceStatisticSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [PerformanceStatistic] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [PerformanceStatistic].[PerformanceStatisticID] = PageIndex.[PerformanceStatisticID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<PerformanceStatistic>(ds, PerformanceStatisticFromDataRow);
			}else{ return null;}
		}

		private int GetPerformanceStatisticCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM PerformanceStatistic as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM PerformanceStatistic as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(PerformanceStatistic))]
		public virtual PerformanceStatistic InsertPerformanceStatistic(PerformanceStatistic entity)
		{

			PerformanceStatistic other=new PerformanceStatistic();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into PerformanceStatistic ( [CPU]
				,[Memory]
				,[CreationDate]
				,[MachineName]
				,[IPAddress]
				,[DriveSpaceAvailable]
				,[DriveTotalSpace] )
				Values
				( @CPU
				, @Memory
				, @CreationDate
				, @MachineName
				, @IPAddress
				, @DriveSpaceAvailable
				, @DriveTotalSpace );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@CPU",entity.Cpu ?? (object)DBNull.Value)
					, new SqlParameter("@Memory",entity.Memory ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@MachineName",entity.MachineName ?? (object)DBNull.Value)
					, new SqlParameter("@IPAddress",entity.IpAddress ?? (object)DBNull.Value)
					, new SqlParameter("@DriveSpaceAvailable",entity.DriveSpaceAvailable ?? (object)DBNull.Value)
					, new SqlParameter("@DriveTotalSpace",entity.DriveTotalSpace ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetPerformanceStatistic(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(PerformanceStatistic))]
		public virtual PerformanceStatistic UpdatePerformanceStatistic(PerformanceStatistic entity)
		{

			if (entity.IsTransient()) return entity;
			PerformanceStatistic other = GetPerformanceStatistic(entity.PerformanceStatisticId);
			if (entity.Equals(other)) return entity;
			string sql=@"Update PerformanceStatistic set  [CPU]=@CPU
							, [Memory]=@Memory
							, [CreationDate]=@CreationDate
							, [MachineName]=@MachineName
							, [IPAddress]=@IPAddress
							, [DriveSpaceAvailable]=@DriveSpaceAvailable
							, [DriveTotalSpace]=@DriveTotalSpace 
							 where PerformanceStatisticID=@PerformanceStatisticID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@CPU",entity.Cpu ?? (object)DBNull.Value)
					, new SqlParameter("@Memory",entity.Memory ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@MachineName",entity.MachineName ?? (object)DBNull.Value)
					, new SqlParameter("@IPAddress",entity.IpAddress ?? (object)DBNull.Value)
					, new SqlParameter("@DriveSpaceAvailable",entity.DriveSpaceAvailable ?? (object)DBNull.Value)
					, new SqlParameter("@DriveTotalSpace",entity.DriveTotalSpace ?? (object)DBNull.Value)
					, new SqlParameter("@PerformanceStatisticID",entity.PerformanceStatisticId)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetPerformanceStatistic(entity.PerformanceStatisticId);
		}

		public virtual bool DeletePerformanceStatistic(System.Int32 PerformanceStatisticId)
		{

			string sql="delete from PerformanceStatistic where PerformanceStatisticID=@PerformanceStatisticID";
			SqlParameter parameter=new SqlParameter("@PerformanceStatisticID",PerformanceStatisticId);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(PerformanceStatistic))]
		public virtual PerformanceStatistic DeletePerformanceStatistic(PerformanceStatistic entity)
		{
			this.DeletePerformanceStatistic(entity.PerformanceStatisticId);
			return entity;
		}


		public virtual PerformanceStatistic PerformanceStatisticFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			PerformanceStatistic entity=new PerformanceStatistic();
			if (dr.Table.Columns.Contains("PerformanceStatisticID"))
			{
			entity.PerformanceStatisticId = (System.Int32)dr["PerformanceStatisticID"];
			}
			if (dr.Table.Columns.Contains("CPU"))
			{
			entity.Cpu = dr["CPU"]==DBNull.Value?(System.Double?)null:(System.Double?)dr["CPU"];
			}
			if (dr.Table.Columns.Contains("Memory"))
			{
			entity.Memory = dr["Memory"]==DBNull.Value?(System.Double?)null:(System.Double?)dr["Memory"];
			}
			if (dr.Table.Columns.Contains("CreationDate"))
			{
			entity.CreationDate = dr["CreationDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["CreationDate"];
			}
			if (dr.Table.Columns.Contains("MachineName"))
			{
			entity.MachineName = dr["MachineName"].ToString();
			}
			if (dr.Table.Columns.Contains("IPAddress"))
			{
			entity.IpAddress = dr["IPAddress"].ToString();
			}
			if (dr.Table.Columns.Contains("DriveSpaceAvailable"))
			{
			entity.DriveSpaceAvailable = dr["DriveSpaceAvailable"]==DBNull.Value?(System.Double?)null:(System.Double?)dr["DriveSpaceAvailable"];
			}
			if (dr.Table.Columns.Contains("DriveTotalSpace"))
			{
			entity.DriveTotalSpace = dr["DriveTotalSpace"]==DBNull.Value?(System.Double?)null:(System.Double?)dr["DriveTotalSpace"];
			}
			return entity;
		}

	}
	
	
}
