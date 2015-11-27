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
		
	public abstract partial class SystemProcessThreadRepositoryBase : Repository
	{
        
        public SystemProcessThreadRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("SystemProcessThreadID",new SearchColumn(){Name="SystemProcessThreadID",Title="SystemProcessThreadID",SelectClause="SystemProcessThreadID",WhereClause="AllRecords.SystemProcessThreadID",DataType="System.Int32",IsForeignColumn=false,PropertyName="SystemProcessThreadId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("SystemProcessID",new SearchColumn(){Name="SystemProcessID",Title="SystemProcessID",SelectClause="SystemProcessID",WhereClause="AllRecords.SystemProcessID",DataType="System.Int32",IsForeignColumn=false,PropertyName="SystemProcessId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Name",new SearchColumn(){Name="Name",Title="Name",SelectClause="Name",WhereClause="AllRecords.Name",DataType="System.String",IsForeignColumn=false,PropertyName="Name",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("SpringEntryName",new SearchColumn(){Name="SpringEntryName",Title="SpringEntryName",SelectClause="SpringEntryName",WhereClause="AllRecords.SpringEntryName",DataType="System.String",IsForeignColumn=false,PropertyName="SpringEntryName",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Description",new SearchColumn(){Name="Description",Title="Description",SelectClause="Description",WhereClause="AllRecords.Description",DataType="System.String",IsForeignColumn=false,PropertyName="Description",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Enabled",new SearchColumn(){Name="Enabled",Title="Enabled",SelectClause="Enabled",WhereClause="AllRecords.Enabled",DataType="System.Boolean",IsForeignColumn=false,PropertyName="Enabled",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Continuous",new SearchColumn(){Name="Continuous",Title="Continuous",SelectClause="Continuous",WhereClause="AllRecords.Continuous",DataType="System.Boolean",IsForeignColumn=false,PropertyName="Continuous",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("SleepTime",new SearchColumn(){Name="SleepTime",Title="SleepTime",SelectClause="SleepTime",WhereClause="AllRecords.SleepTime",DataType="System.Int32",IsForeignColumn=false,PropertyName="SleepTime",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("AutoStart",new SearchColumn(){Name="AutoStart",Title="AutoStart",SelectClause="AutoStart",WhereClause="AllRecords.AutoStart",DataType="System.Boolean",IsForeignColumn=false,PropertyName="AutoStart",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Status",new SearchColumn(){Name="Status",Title="Status",SelectClause="Status",WhereClause="AllRecords.Status",DataType="System.String",IsForeignColumn=false,PropertyName="Status",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Message",new SearchColumn(){Name="Message",Title="Message",SelectClause="Message",WhereClause="AllRecords.Message",DataType="System.String",IsForeignColumn=false,PropertyName="Message",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ScheduledTime",new SearchColumn(){Name="ScheduledTime",Title="ScheduledTime",SelectClause="ScheduledTime",WhereClause="AllRecords.ScheduledTime",DataType="System.TimeSpan?",IsForeignColumn=false,PropertyName="ScheduledTime",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("StartRange",new SearchColumn(){Name="StartRange",Title="StartRange",SelectClause="StartRange",WhereClause="AllRecords.StartRange",DataType="System.Int32?",IsForeignColumn=false,PropertyName="StartRange",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("EndRange",new SearchColumn(){Name="EndRange",Title="EndRange",SelectClause="EndRange",WhereClause="AllRecords.EndRange",DataType="System.Int32?",IsForeignColumn=false,PropertyName="EndRange",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("LastSuccessfullyExecuted",new SearchColumn(){Name="LastSuccessfullyExecuted",Title="LastSuccessfullyExecuted",SelectClause="LastSuccessfullyExecuted",WhereClause="AllRecords.LastSuccessfullyExecuted",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="LastSuccessfullyExecuted",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ContinuousDelay",new SearchColumn(){Name="ContinuousDelay",Title="ContinuousDelay",SelectClause="ContinuousDelay",WhereClause="AllRecords.ContinuousDelay",DataType="System.Int32",IsForeignColumn=false,PropertyName="ContinuousDelay",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IsDeleted",new SearchColumn(){Name="IsDeleted",Title="IsDeleted",SelectClause="IsDeleted",WhereClause="AllRecords.IsDeleted",DataType="System.Boolean",IsForeignColumn=false,PropertyName="IsDeleted",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DisplayOrder",new SearchColumn(){Name="DisplayOrder",Title="DisplayOrder",SelectClause="DisplayOrder",WhereClause="AllRecords.DisplayOrder",DataType="System.String",IsForeignColumn=false,PropertyName="DisplayOrder",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Argument",new SearchColumn(){Name="Argument",Title="Argument",SelectClause="Argument",WhereClause="AllRecords.Argument",DataType="System.String",IsForeignColumn=false,PropertyName="Argument",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("LastUpdateDate",new SearchColumn(){Name="LastUpdateDate",Title="LastUpdateDate",SelectClause="LastUpdateDate",WhereClause="AllRecords.LastUpdateDate",DataType="System.DateTime?",IsForeignColumn=false,PropertyName="LastUpdateDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ExecutionTime",new SearchColumn(){Name="ExecutionTime",Title="ExecutionTime",SelectClause="ExecutionTime",WhereClause="AllRecords.ExecutionTime",DataType="System.Double?",IsForeignColumn=false,PropertyName="ExecutionTime",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("EstimatedExecutionTime",new SearchColumn(){Name="EstimatedExecutionTime",Title="EstimatedExecutionTime",SelectClause="EstimatedExecutionTime",WhereClause="AllRecords.EstimatedExecutionTime",DataType="System.Double?",IsForeignColumn=false,PropertyName="EstimatedExecutionTime",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ShowUpdateInLog",new SearchColumn(){Name="ShowUpdateInLog",Title="ShowUpdateInLog",SelectClause="ShowUpdateInLog",WhereClause="AllRecords.ShowUpdateInLog",DataType="System.Boolean?",IsForeignColumn=false,PropertyName="ShowUpdateInLog",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("TenantId",new SearchColumn(){Name="TenantId",Title="TenantId",SelectClause="TenantId",WhereClause="AllRecords.TenantId",DataType="System.Int32?",IsForeignColumn=false,PropertyName="TenantId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetSystemProcessThreadSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetSystemProcessThreadBasicSearchColumns()
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

        public virtual List<SearchColumn> GetSystemProcessThreadAdvanceSearchColumns()
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
        
        
        public virtual string GetSystemProcessThreadSelectClause()
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
                        	selectQuery =  "SystemProcessThread."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",SystemProcessThread."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual List<SystemProcessThread> GetSystemProcessThreadBySystemProcessId(System.Int32 SystemProcessId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetSystemProcessThreadSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from SystemProcessThread with (nolock)  where SystemProcessID=@SystemProcessID   AND IsDeleted='false'";
			SqlParameter parameter=new SqlParameter("@SystemProcessID",SystemProcessId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<SystemProcessThread>(ds,SystemProcessThreadFromDataRow);
		}

		public virtual SystemProcessThread GetSystemProcessThread(System.Int32 SystemProcessThreadId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetSystemProcessThreadSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from SystemProcessThread with (nolock)  where SystemProcessThreadID=@SystemProcessThreadID  AND IsDeleted='false'";
			SqlParameter parameter=new SqlParameter("@SystemProcessThreadID",SystemProcessThreadId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return SystemProcessThreadFromDataRow(ds.Tables[0].Rows[0]);
		}

		public  List<SystemProcessThread> GetSystemProcessThreadByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetSystemProcessThreadSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from SystemProcessThread with (nolock)  where {0} {1} '{2}'  AND IsDeleted='false'",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<SystemProcessThread>(ds,SystemProcessThreadFromDataRow);
		}

		public virtual List<SystemProcessThread> GetAllSystemProcessThread(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetSystemProcessThreadSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from SystemProcessThread with (nolock)   WHERE IsDeleted='false'";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<SystemProcessThread>(ds, SystemProcessThreadFromDataRow);
		}

		public virtual List<SystemProcessThread> GetPagedSystemProcessThread(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetSystemProcessThreadCount(whereClause, searchColumns);
			if(count>0)
			{
			if (count < startIndex) startIndex = (count / pageSize) * pageSize;			
			
           	int PageLowerBound = startIndex;
            int PageUpperBound = PageLowerBound + pageSize;
            string sql = @"CREATE TABLE #PageIndex
				            (
				                [IndexId] int IDENTITY (1, 1) NOT NULL,
				                [SystemProcessThreadID] int				   
				            );";

            //Insert into the temp table
            string tempsql = "INSERT INTO #PageIndex ([SystemProcessThreadID])";
            tempsql += " SELECT ";
            if (pageSize > 0) tempsql += "TOP " + PageUpperBound.ToString();
            tempsql += " [SystemProcessThreadID] ";
            tempsql += " FROM [SystemProcessThread] AllRecords with (NOLOCK)";
            if (!string.IsNullOrEmpty(whereClause) && whereClause.Length > 0) tempsql += " WHERE " + whereClause;
            if (orderByClause.Length > 0) 
			{
				tempsql += " ORDER BY " + orderByClause;
				if( !orderByClause.Contains("SystemProcessThreadID"))
					tempsql += " , (AllRecords.[SystemProcessThreadID])"; 
			}
			else 
			{
				tempsql  += " ORDER BY (AllRecords.[SystemProcessThreadID])"; 
			}           
            
            // Return paged results
            string pagedResultsSql =
                (string.IsNullOrEmpty(SelectClause)? GetSystemProcessThreadSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [SystemProcessThread] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [SystemProcessThread].[SystemProcessThreadID] = PageIndex.[SystemProcessThreadID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<SystemProcessThread>(ds, SystemProcessThreadFromDataRow);
			}else{ return null;}
		}

		private int GetSystemProcessThreadCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM SystemProcessThread as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM SystemProcessThread as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(SystemProcessThread))]
		public virtual SystemProcessThread InsertSystemProcessThread(SystemProcessThread entity)
		{

			SystemProcessThread other=new SystemProcessThread();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into SystemProcessThread ( [SystemProcessID]
				,[Name]
				,[SpringEntryName]
				,[Description]
				,[Enabled]
				,[Continuous]
				,[SleepTime]
				,[AutoStart]
				,[Status]
				,[Message]
				,[ScheduledTime]
				,[StartRange]
				,[EndRange]
				,[LastSuccessfullyExecuted]
				,[ContinuousDelay]
				,[IsDeleted]
				,[DisplayOrder]
				,[Argument]
				,[LastUpdateDate]
				,[ExecutionTime]
				,[EstimatedExecutionTime]
				,[ShowUpdateInLog]
				,[TenantId] )
				Values
				( @SystemProcessID
				, @Name
				, @SpringEntryName
				, @Description
				, @Enabled
				, @Continuous
				, @SleepTime
				, @AutoStart
				, @Status
				, @Message
				, @ScheduledTime
				, @StartRange
				, @EndRange
				, @LastSuccessfullyExecuted
				, @ContinuousDelay
				, @IsDeleted
				, @DisplayOrder
				, @Argument
				, @LastUpdateDate
				, @ExecutionTime
				, @EstimatedExecutionTime
				, @ShowUpdateInLog
				, @TenantId );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@SystemProcessID",entity.SystemProcessId)
					, new SqlParameter("@Name",entity.Name)
					, new SqlParameter("@SpringEntryName",entity.SpringEntryName ?? (object)DBNull.Value)
					, new SqlParameter("@Description",entity.Description ?? (object)DBNull.Value)
					, new SqlParameter("@Enabled",entity.Enabled)
					, new SqlParameter("@Continuous",entity.Continuous)
					, new SqlParameter("@SleepTime",entity.SleepTime)
					, new SqlParameter("@AutoStart",entity.AutoStart)
					, new SqlParameter("@Status",entity.Status ?? (object)DBNull.Value)
					, new SqlParameter("@Message",entity.Message ?? (object)DBNull.Value)
					, new SqlParameter("@ScheduledTime",entity.ScheduledTime ?? (object)DBNull.Value)
					, new SqlParameter("@StartRange",entity.StartRange ?? (object)DBNull.Value)
					, new SqlParameter("@EndRange",entity.EndRange ?? (object)DBNull.Value)
					, new SqlParameter("@LastSuccessfullyExecuted",entity.LastSuccessfullyExecuted ?? (object)DBNull.Value)
					, new SqlParameter("@ContinuousDelay",entity.ContinuousDelay)
					, new SqlParameter("@IsDeleted",entity.IsDeleted)
					, new SqlParameter("@DisplayOrder",entity.DisplayOrder)
					, new SqlParameter("@Argument",entity.Argument ?? (object)DBNull.Value)
					, new SqlParameter("@LastUpdateDate",entity.LastUpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@ExecutionTime",entity.ExecutionTime ?? (object)DBNull.Value)
					, new SqlParameter("@EstimatedExecutionTime",entity.EstimatedExecutionTime ?? (object)DBNull.Value)
					, new SqlParameter("@ShowUpdateInLog",entity.ShowUpdateInLog ?? (object)DBNull.Value)
					, new SqlParameter("@TenantId",entity.TenantId ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetSystemProcessThread(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(SystemProcessThread))]
		public virtual SystemProcessThread UpdateSystemProcessThread(SystemProcessThread entity)
		{

			if (entity.IsTransient()) return entity;
			SystemProcessThread other = GetSystemProcessThread(entity.SystemProcessThreadId);
			if (entity.Equals(other)) return entity;
			string sql=@"Update SystemProcessThread set  [SystemProcessID]=@SystemProcessID
							, [Name]=@Name
							, [SpringEntryName]=@SpringEntryName
							, [Description]=@Description
							, [Enabled]=@Enabled
							, [Continuous]=@Continuous
							, [SleepTime]=@SleepTime
							, [AutoStart]=@AutoStart
							, [Status]=@Status
							, [Message]=@Message
							, [ScheduledTime]=@ScheduledTime
							, [StartRange]=@StartRange
							, [EndRange]=@EndRange
							, [LastSuccessfullyExecuted]=@LastSuccessfullyExecuted
							, [ContinuousDelay]=@ContinuousDelay
							, [IsDeleted]=@IsDeleted
							, [DisplayOrder]=@DisplayOrder
							, [Argument]=@Argument
							, [LastUpdateDate]=@LastUpdateDate
							, [ExecutionTime]=@ExecutionTime
							, [EstimatedExecutionTime]=@EstimatedExecutionTime
							, [ShowUpdateInLog]=@ShowUpdateInLog
							, [TenantId]=@TenantId 
							 where SystemProcessThreadID=@SystemProcessThreadID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@SystemProcessID",entity.SystemProcessId)
					, new SqlParameter("@Name",entity.Name)
					, new SqlParameter("@SpringEntryName",entity.SpringEntryName ?? (object)DBNull.Value)
					, new SqlParameter("@Description",entity.Description ?? (object)DBNull.Value)
					, new SqlParameter("@Enabled",entity.Enabled)
					, new SqlParameter("@Continuous",entity.Continuous)
					, new SqlParameter("@SleepTime",entity.SleepTime)
					, new SqlParameter("@AutoStart",entity.AutoStart)
					, new SqlParameter("@Status",entity.Status ?? (object)DBNull.Value)
					, new SqlParameter("@Message",entity.Message ?? (object)DBNull.Value)
					, new SqlParameter("@ScheduledTime",entity.ScheduledTime ?? (object)DBNull.Value)
					, new SqlParameter("@StartRange",entity.StartRange ?? (object)DBNull.Value)
					, new SqlParameter("@EndRange",entity.EndRange ?? (object)DBNull.Value)
					, new SqlParameter("@LastSuccessfullyExecuted",entity.LastSuccessfullyExecuted ?? (object)DBNull.Value)
					, new SqlParameter("@ContinuousDelay",entity.ContinuousDelay)
					, new SqlParameter("@IsDeleted",entity.IsDeleted)
					, new SqlParameter("@DisplayOrder",entity.DisplayOrder)
					, new SqlParameter("@Argument",entity.Argument ?? (object)DBNull.Value)
					, new SqlParameter("@LastUpdateDate",entity.LastUpdateDate ?? (object)DBNull.Value)
					, new SqlParameter("@ExecutionTime",entity.ExecutionTime ?? (object)DBNull.Value)
					, new SqlParameter("@EstimatedExecutionTime",entity.EstimatedExecutionTime ?? (object)DBNull.Value)
					, new SqlParameter("@ShowUpdateInLog",entity.ShowUpdateInLog ?? (object)DBNull.Value)
					, new SqlParameter("@TenantId",entity.TenantId ?? (object)DBNull.Value)
					, new SqlParameter("@SystemProcessThreadID",entity.SystemProcessThreadId)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetSystemProcessThread(entity.SystemProcessThreadId);
		}

		public virtual bool DeleteSystemProcessThread(System.Int32 SystemProcessThreadId)
		{

			string sql="delete from SystemProcessThread where SystemProcessThreadID=@SystemProcessThreadID";
			SqlParameter parameter=new SqlParameter("@SystemProcessThreadID",SystemProcessThreadId);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(SystemProcessThread))]
		public virtual SystemProcessThread DeleteSystemProcessThread(SystemProcessThread entity)
		{
			this.DeleteSystemProcessThread(entity.SystemProcessThreadId);
			return entity;
		}


		public virtual SystemProcessThread SystemProcessThreadFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			SystemProcessThread entity=new SystemProcessThread();
			if (dr.Table.Columns.Contains("SystemProcessThreadID"))
			{
			entity.SystemProcessThreadId = (System.Int32)dr["SystemProcessThreadID"];
			}
			if (dr.Table.Columns.Contains("SystemProcessID"))
			{
			entity.SystemProcessId = (System.Int32)dr["SystemProcessID"];
			}
			if (dr.Table.Columns.Contains("Name"))
			{
			entity.Name = dr["Name"].ToString();
			}
			if (dr.Table.Columns.Contains("SpringEntryName"))
			{
			entity.SpringEntryName = dr["SpringEntryName"].ToString();
			}
			if (dr.Table.Columns.Contains("Description"))
			{
			entity.Description = dr["Description"].ToString();
			}
			if (dr.Table.Columns.Contains("Enabled"))
			{
			entity.Enabled = (System.Boolean)dr["Enabled"];
			}
			if (dr.Table.Columns.Contains("Continuous"))
			{
			entity.Continuous = (System.Boolean)dr["Continuous"];
			}
			if (dr.Table.Columns.Contains("SleepTime"))
			{
			entity.SleepTime = (System.Int32)dr["SleepTime"];
			}
			if (dr.Table.Columns.Contains("AutoStart"))
			{
			entity.AutoStart = (System.Boolean)dr["AutoStart"];
			}
			if (dr.Table.Columns.Contains("Status"))
			{
			entity.Status = dr["Status"].ToString();
			}
			if (dr.Table.Columns.Contains("Message"))
			{
			entity.Message = dr["Message"].ToString();
			}
			if (dr.Table.Columns.Contains("ScheduledTime"))
			{
			entity.ScheduledTime = dr["ScheduledTime"]==DBNull.Value?(TimeSpan?)null:(TimeSpan?)dr["ScheduledTime"];
			}
			if (dr.Table.Columns.Contains("StartRange"))
			{
			entity.StartRange = dr["StartRange"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["StartRange"];
			}
			if (dr.Table.Columns.Contains("EndRange"))
			{
			entity.EndRange = dr["EndRange"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["EndRange"];
			}
			if (dr.Table.Columns.Contains("LastSuccessfullyExecuted"))
			{
			entity.LastSuccessfullyExecuted = dr["LastSuccessfullyExecuted"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["LastSuccessfullyExecuted"];
			}
			if (dr.Table.Columns.Contains("ContinuousDelay"))
			{
			entity.ContinuousDelay = (System.Int32)dr["ContinuousDelay"];
			}
			if (dr.Table.Columns.Contains("IsDeleted"))
			{
			entity.IsDeleted = (System.Boolean)dr["IsDeleted"];
			}
			if (dr.Table.Columns.Contains("DisplayOrder"))
			{
			entity.DisplayOrder = dr["DisplayOrder"].ToString();
			}
			if (dr.Table.Columns.Contains("Argument"))
			{
			entity.Argument = dr["Argument"].ToString();
			}
			if (dr.Table.Columns.Contains("LastUpdateDate"))
			{
			entity.LastUpdateDate = dr["LastUpdateDate"]==DBNull.Value?(System.DateTime?)null:(System.DateTime?)dr["LastUpdateDate"];
			}
			if (dr.Table.Columns.Contains("ExecutionTime"))
			{
			entity.ExecutionTime = dr["ExecutionTime"]==DBNull.Value?(System.Double?)null:(System.Double?)dr["ExecutionTime"];
			}
			if (dr.Table.Columns.Contains("EstimatedExecutionTime"))
			{
			entity.EstimatedExecutionTime = dr["EstimatedExecutionTime"]==DBNull.Value?(System.Double?)null:(System.Double?)dr["EstimatedExecutionTime"];
			}
			if (dr.Table.Columns.Contains("ShowUpdateInLog"))
			{
			entity.ShowUpdateInLog = dr["ShowUpdateInLog"]==DBNull.Value?(System.Boolean?)null:(System.Boolean?)dr["ShowUpdateInLog"];
			}
			if (dr.Table.Columns.Contains("TenantId"))
			{
			entity.TenantId = dr["TenantId"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["TenantId"];
			}
			return entity;
		}

	}
	
	
}
