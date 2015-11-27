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
		
	public abstract partial class SystemEventLogRepositoryBase : Repository 
	{
        
        public SystemEventLogRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("EventLogID",new SearchColumn(){Name="EventLogID",Title="EventLogID",SelectClause="EventLogID",WhereClause="AllRecords.EventLogID",DataType="System.Int32",IsForeignColumn=false,PropertyName="EventLogId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("EventCode",new SearchColumn(){Name="EventCode",Title="EventCode",SelectClause="EventCode",WhereClause="AllRecords.EventCode",DataType="System.Int32?",IsForeignColumn=false,PropertyName="EventCode",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Title",new SearchColumn(){Name="Title",Title="Title",SelectClause="Title",WhereClause="AllRecords.Title",DataType="System.String",IsForeignColumn=false,PropertyName="Title",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Description",new SearchColumn(){Name="Description",Title="Description",SelectClause="Description",WhereClause="AllRecords.Description",DataType="System.String",IsForeignColumn=false,PropertyName="Description",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DateOccured",new SearchColumn(){Name="DateOccured",Title="DateOccured",SelectClause="DateOccured",WhereClause="AllRecords.DateOccured",DataType="System.DateTime",IsForeignColumn=false,PropertyName="DateOccured",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ElmahErrorID",new SearchColumn(){Name="ElmahErrorID",Title="ElmahErrorID",SelectClause="ElmahErrorID",WhereClause="AllRecords.ElmahErrorID",DataType="System.Guid?",IsForeignColumn=false,PropertyName="ElmahErrorId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Source",new SearchColumn(){Name="Source",Title="Source",SelectClause="Source",WhereClause="AllRecords.Source",DataType="System.String",IsForeignColumn=false,PropertyName="Source",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetSystemEventLogSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetSystemEventLogBasicSearchColumns()
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

        public virtual List<SearchColumn> GetSystemEventLogAdvanceSearchColumns()
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
        
        
        public virtual string GetSystemEventLogSelectClause()
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
                        	selectQuery =  "SystemEventLog."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",SystemEventLog."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual SystemEventLog GetSystemEventLog(System.Int32 EventLogId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetSystemEventLogSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from SystemEventLog with (nolock)  where EventLogID=@EventLogID ";
			SqlParameter parameter=new SqlParameter("@EventLogID",EventLogId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return SystemEventLogFromDataRow(ds.Tables[0].Rows[0]);
		}

		public  List<SystemEventLog> GetSystemEventLogByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetSystemEventLogSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from SystemEventLog with (nolock)  where {0} {1} '{2}' ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<SystemEventLog>(ds,SystemEventLogFromDataRow);
		}

		public virtual List<SystemEventLog> GetAllSystemEventLog(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetSystemEventLogSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from SystemEventLog with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<SystemEventLog>(ds, SystemEventLogFromDataRow);
		}

		public virtual List<SystemEventLog> GetPagedSystemEventLog(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetSystemEventLogCount(whereClause, searchColumns);
			if(count>0)
			{
			if (count < startIndex) startIndex = (count / pageSize) * pageSize;			
			
           	int PageLowerBound = startIndex;
            int PageUpperBound = PageLowerBound + pageSize;
            string sql = @"CREATE TABLE #PageIndex
				            (
				                [IndexId] int IDENTITY (1, 1) NOT NULL,
				                [EventLogID] int				   
				            );";

            //Insert into the temp table
            string tempsql = "INSERT INTO #PageIndex ([EventLogID])";
            tempsql += " SELECT ";
            if (pageSize > 0) tempsql += "TOP " + PageUpperBound.ToString();
            tempsql += " [EventLogID] ";
            tempsql += " FROM [SystemEventLog] AllRecords with (NOLOCK)";
            if (!string.IsNullOrEmpty(whereClause) && whereClause.Length > 0) tempsql += " WHERE " + whereClause;
            if (orderByClause.Length > 0) 
			{
				tempsql += " ORDER BY " + orderByClause;
				if( !orderByClause.Contains("EventLogID"))
					tempsql += " , (AllRecords.[EventLogID])"; 
			}
			else 
			{
				tempsql  += " ORDER BY (AllRecords.[EventLogID])"; 
			}           
            
            // Return paged results
            string pagedResultsSql =
                (string.IsNullOrEmpty(SelectClause)? GetSystemEventLogSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [SystemEventLog] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [SystemEventLog].[EventLogID] = PageIndex.[EventLogID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<SystemEventLog>(ds, SystemEventLogFromDataRow);
			}else{ return null;}
		}

		private int GetSystemEventLogCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM SystemEventLog as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM SystemEventLog as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(SystemEventLog))]
		public virtual SystemEventLog InsertSystemEventLog(SystemEventLog entity)
		{

			SystemEventLog other=new SystemEventLog();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into SystemEventLog ( [EventCode]
				,[Title]
				,[Description]
				,[DateOccured]
				,[ElmahErrorID]
				,[Source] )
				Values
				( @EventCode
				, @Title
				, @Description
				, @DateOccured
				, @ElmahErrorID
				, @Source );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@EventCode",entity.EventCode ?? (object)DBNull.Value)
					, new SqlParameter("@Title",entity.Title ?? (object)DBNull.Value)
					, new SqlParameter("@Description",entity.Description ?? (object)DBNull.Value)
					, new SqlParameter("@DateOccured",entity.DateOccured)
					, new SqlParameter("@ElmahErrorID",entity.ElmahErrorId ?? (object)DBNull.Value)
					, new SqlParameter("@Source",entity.Source ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetSystemEventLog(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(SystemEventLog))]
		public virtual SystemEventLog UpdateSystemEventLog(SystemEventLog entity)
		{

			if (entity.IsTransient()) return entity;
			SystemEventLog other = GetSystemEventLog(entity.EventLogId);
			if (entity.Equals(other)) return entity;
			string sql=@"Update SystemEventLog set  [EventCode]=@EventCode
							, [Title]=@Title
							, [Description]=@Description
							, [DateOccured]=@DateOccured
							, [ElmahErrorID]=@ElmahErrorID
							, [Source]=@Source 
							 where EventLogID=@EventLogID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@EventCode",entity.EventCode ?? (object)DBNull.Value)
					, new SqlParameter("@Title",entity.Title ?? (object)DBNull.Value)
					, new SqlParameter("@Description",entity.Description ?? (object)DBNull.Value)
					, new SqlParameter("@DateOccured",entity.DateOccured)
					, new SqlParameter("@ElmahErrorID",entity.ElmahErrorId ?? (object)DBNull.Value)
					, new SqlParameter("@Source",entity.Source ?? (object)DBNull.Value)
					, new SqlParameter("@EventLogID",entity.EventLogId)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetSystemEventLog(entity.EventLogId);
		}

		public virtual bool DeleteSystemEventLog(System.Int32 EventLogId)
		{

			string sql="delete from SystemEventLog with (nolock) where EventLogID=@EventLogID";
			SqlParameter parameter=new SqlParameter("@EventLogID",EventLogId);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(SystemEventLog))]
		public virtual SystemEventLog DeleteSystemEventLog(SystemEventLog entity)
		{
			this.DeleteSystemEventLog(entity.EventLogId);
			return entity;
		}


		public virtual SystemEventLog SystemEventLogFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			SystemEventLog entity=new SystemEventLog();
			if (dr.Table.Columns.Contains("EventLogID"))
			{
			entity.EventLogId = (System.Int32)dr["EventLogID"];
			}
			if (dr.Table.Columns.Contains("EventCode"))
			{
			entity.EventCode = dr["EventCode"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["EventCode"];
			}
			if (dr.Table.Columns.Contains("Title"))
			{
			entity.Title = dr["Title"].ToString();
			}
			if (dr.Table.Columns.Contains("Description"))
			{
			entity.Description = dr["Description"].ToString();
			}
			if (dr.Table.Columns.Contains("DateOccured"))
			{
			entity.DateOccured = (System.DateTime)dr["DateOccured"];
			}
			if (dr.Table.Columns.Contains("ElmahErrorID"))
			{
			entity.ElmahErrorId = dr["ElmahErrorID"]==DBNull.Value?(System.Guid?)null:(System.Guid?)dr["ElmahErrorID"];
			}
			if (dr.Table.Columns.Contains("Source"))
			{
			entity.Source = dr["Source"].ToString();
			}
			return entity;
		}

	}
	
	
}
