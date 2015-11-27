using System;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ControlPanel.Core;
using ControlPanel.Core.Entities;
using ControlPanel.Core.DataInterfaces;
using ControlPanel.Core.Extensions;

namespace ControlPanel.Repository
{
		
	public abstract partial class DriveStatisticRepositoryBase : Repository, IDriveStatisticRepositoryBase 
	{
        
        public DriveStatisticRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("DriveStatisticId",new SearchColumn(){Name="DriveStatisticId",Title="DriveStatisticId",SelectClause="DriveStatisticId",WhereClause="AllRecords.DriveStatisticId",DataType="System.Int32",IsForeignColumn=false,PropertyName="DriveStatisticId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IPAddress",new SearchColumn(){Name="IPAddress",Title="IPAddress",SelectClause="IPAddress",WhereClause="AllRecords.IPAddress",DataType="System.String",IsForeignColumn=false,PropertyName="IpAddress",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DriveSpaceAvailable",new SearchColumn(){Name="DriveSpaceAvailable",Title="DriveSpaceAvailable",SelectClause="DriveSpaceAvailable",WhereClause="AllRecords.DriveSpaceAvailable",DataType="System.Double?",IsForeignColumn=false,PropertyName="DriveSpaceAvailable",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DriveTotalSpace",new SearchColumn(){Name="DriveTotalSpace",Title="DriveTotalSpace",SelectClause="DriveTotalSpace",WhereClause="AllRecords.DriveTotalSpace",DataType="System.Double?",IsForeignColumn=false,PropertyName="DriveTotalSpace",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DriveName",new SearchColumn(){Name="DriveName",Title="DriveName",SelectClause="DriveName",WhereClause="AllRecords.DriveName",DataType="System.String",IsForeignColumn=false,PropertyName="DriveName",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreationDate",new SearchColumn(){Name="CreationDate",Title="CreationDate",SelectClause="CreationDate",WhereClause="AllRecords.CreationDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="CreationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("MachineName",new SearchColumn(){Name="MachineName",Title="MachineName",SelectClause="MachineName",WhereClause="AllRecords.MachineName",DataType="System.String",IsForeignColumn=false,PropertyName="MachineName",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetDriveStatisticSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetDriveStatisticBasicSearchColumns()
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

        public virtual List<SearchColumn> GetDriveStatisticAdvanceSearchColumns()
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
        
        
        public virtual string GetDriveStatisticSelectClause()
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
                        	selectQuery =  "[DriveStatistic].["+keyValuePair.Key+"]";
                    	}
                    	else
                    	{
                        	selectQuery += ",[DriveStatistic].["+keyValuePair.Key+"]";
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual DriveStatistic GetDriveStatistic(System.Int32 DriveStatisticId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetDriveStatisticSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [DriveStatistic] with (nolock)  where DriveStatisticId=@DriveStatisticId ";
			SqlParameter parameter=new SqlParameter("@DriveStatisticId",DriveStatisticId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return DriveStatisticFromDataRow(ds.Tables[0].Rows[0]);
		}

		public  List<DriveStatistic> GetDriveStatisticByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetDriveStatisticSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [DriveStatistic] with (nolock)  where {0} {1} '{2}' ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<DriveStatistic>(ds,DriveStatisticFromDataRow);
		}

		public virtual List<DriveStatistic> GetAllDriveStatistic(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetDriveStatisticSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [DriveStatistic] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<DriveStatistic>(ds, DriveStatisticFromDataRow);
		}

		public virtual List<DriveStatistic> GetPagedDriveStatistic(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetDriveStatisticCount(whereClause, searchColumns);
			if(count>0)
			{
			if (count < startIndex) startIndex = (count / pageSize) * pageSize;			
			
           	int PageLowerBound = startIndex;
            int PageUpperBound = PageLowerBound + pageSize;
            string sql = @"CREATE TABLE #PageIndex
				            (
				                [IndexId] int IDENTITY (1, 1) NOT NULL,
				                [DriveStatisticId] int				   
				            );";

            //Insert into the temp table
            string tempsql = "INSERT INTO #PageIndex ([DriveStatisticId])";
            tempsql += " SELECT ";
            if (pageSize > 0) tempsql += "TOP " + PageUpperBound.ToString();
            tempsql += " [DriveStatisticId] ";
            tempsql += " FROM [DriveStatistic] AllRecords with (NOLOCK)";
            if (!string.IsNullOrEmpty(whereClause) && whereClause.Length > 0) tempsql += " WHERE " + whereClause;
            if (orderByClause.Length > 0) 
			{
				tempsql += " ORDER BY " + orderByClause;
				if( !orderByClause.Contains("DriveStatisticId"))
					tempsql += " , (AllRecords.[DriveStatisticId])"; 
			}
			else 
			{
				tempsql  += " ORDER BY (AllRecords.[DriveStatisticId])"; 
			}           
            
            // Return paged results
            string pagedResultsSql =
                (string.IsNullOrEmpty(SelectClause)? GetDriveStatisticSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [DriveStatistic] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [DriveStatistic].[DriveStatisticId] = PageIndex.[DriveStatisticId] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<DriveStatistic>(ds, DriveStatisticFromDataRow);
			}else{ return null;}
		}

		private int GetDriveStatisticCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM DriveStatistic as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM DriveStatistic as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(DriveStatistic))]
		public virtual DriveStatistic InsertDriveStatistic(DriveStatistic entity)
		{

			DriveStatistic other=new DriveStatistic();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into DriveStatistic ( [IPAddress]
				,[DriveSpaceAvailable]
				,[DriveTotalSpace]
				,[DriveName]
				,[CreationDate]
				,[MachineName] )
				Values
				( @IPAddress
				, @DriveSpaceAvailable
				, @DriveTotalSpace
				, @DriveName
				, @CreationDate
				, @MachineName );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@IPAddress",entity.IpAddress ?? (object)DBNull.Value)
					, new SqlParameter("@DriveSpaceAvailable",entity.DriveSpaceAvailable ?? (object)DBNull.Value)
					, new SqlParameter("@DriveTotalSpace",entity.DriveTotalSpace ?? (object)DBNull.Value)
					, new SqlParameter("@DriveName",entity.DriveName ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@MachineName",entity.MachineName ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetDriveStatistic(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(DriveStatistic))]
		public virtual DriveStatistic UpdateDriveStatistic(DriveStatistic entity)
		{

			if (entity.IsTransient()) return entity;
			DriveStatistic other = GetDriveStatistic(entity.DriveStatisticId);
			if (entity.Equals(other)) return entity;
			string sql=@"Update DriveStatistic set  [IPAddress]=@IPAddress
							, [DriveSpaceAvailable]=@DriveSpaceAvailable
							, [DriveTotalSpace]=@DriveTotalSpace
							, [DriveName]=@DriveName
							, [CreationDate]=@CreationDate
							, [MachineName]=@MachineName 
							 where DriveStatisticId=@DriveStatisticId";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@IPAddress",entity.IpAddress ?? (object)DBNull.Value)
					, new SqlParameter("@DriveSpaceAvailable",entity.DriveSpaceAvailable ?? (object)DBNull.Value)
					, new SqlParameter("@DriveTotalSpace",entity.DriveTotalSpace ?? (object)DBNull.Value)
					, new SqlParameter("@DriveName",entity.DriveName ?? (object)DBNull.Value)
					, new SqlParameter("@CreationDate",entity.CreationDate ?? (object)DBNull.Value)
					, new SqlParameter("@MachineName",entity.MachineName ?? (object)DBNull.Value)
					, new SqlParameter("@DriveStatisticId",entity.DriveStatisticId)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetDriveStatistic(entity.DriveStatisticId);
		}

		public virtual bool DeleteDriveStatistic(System.Int32 DriveStatisticId)
		{

			string sql="delete from DriveStatistic where DriveStatisticId=@DriveStatisticId";
			SqlParameter parameter=new SqlParameter("@DriveStatisticId",DriveStatisticId);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(DriveStatistic))]
		public virtual DriveStatistic DeleteDriveStatistic(DriveStatistic entity)
		{
			this.DeleteDriveStatistic(entity.DriveStatisticId);
			return entity;
		}


		public virtual DriveStatistic DriveStatisticFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			DriveStatistic entity=new DriveStatistic();
			if (dr.Table.Columns.Contains("DriveStatisticId"))
			{
			entity.DriveStatisticId = (System.Int32)dr["DriveStatisticId"];
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
			if (dr.Table.Columns.Contains("DriveName"))
			{
			entity.DriveName = dr["DriveName"].ToString();
			}
			if (dr.Table.Columns.Contains("CreationDate"))
			{
			entity.CreationDate = dr["CreationDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["CreationDate"];
			}
			if (dr.Table.Columns.Contains("MachineName"))
			{
			entity.MachineName = dr["MachineName"].ToString();
			}
			return entity;
		}

	}
	
	
}
