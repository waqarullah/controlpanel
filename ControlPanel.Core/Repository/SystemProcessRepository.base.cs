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
		
	public abstract partial class SystemProcessRepositoryBase : Repository
	{
        
        public SystemProcessRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("SystemProcessID",new SearchColumn(){Name="SystemProcessID",Title="SystemProcessID",SelectClause="SystemProcessID",WhereClause="AllRecords.SystemProcessID",DataType="System.Int32",IsForeignColumn=false,PropertyName="SystemProcessId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Name",new SearchColumn(){Name="Name",Title="Name",SelectClause="Name",WhereClause="AllRecords.Name",DataType="System.String",IsForeignColumn=false,PropertyName="Name",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Description",new SearchColumn(){Name="Description",Title="Description",SelectClause="Description",WhereClause="AllRecords.Description",DataType="System.String",IsForeignColumn=false,PropertyName="Description",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Enabled",new SearchColumn(){Name="Enabled",Title="Enabled",SelectClause="Enabled",WhereClause="AllRecords.Enabled",DataType="System.Boolean",IsForeignColumn=false,PropertyName="Enabled",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("DisplayOrder",new SearchColumn(){Name="DisplayOrder",Title="DisplayOrder",SelectClause="DisplayOrder",WhereClause="AllRecords.DisplayOrder",DataType="System.Int32",IsForeignColumn=false,PropertyName="DisplayOrder",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Ip",new SearchColumn(){Name="Ip",Title="Ip",SelectClause="Ip",WhereClause="AllRecords.Ip",DataType="System.String",IsForeignColumn=false,PropertyName="Ip",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Port",new SearchColumn(){Name="Port",Title="Port",SelectClause="Port",WhereClause="AllRecords.Port",DataType="System.Int32?",IsForeignColumn=false,PropertyName="Port",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetSystemProcessSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetSystemProcessBasicSearchColumns()
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


        public virtual List<SearchColumn> GetSystemProcessAdvanceSearchColumns()
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
        
        
        public virtual string GetSystemProcessSelectClause()
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
                        	selectQuery =  "SystemProcess."+keyValuePair.Key;
                    	}
                    	else
                    	{
                        	selectQuery += ",SystemProcess."+keyValuePair.Key;
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual SystemProcess GetSystemProcess(System.Int32 SystemProcessId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetSystemProcessSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from SystemProcess with (nolock)  where SystemProcessID=@SystemProcessID ";
			SqlParameter parameter=new SqlParameter("@SystemProcessID",SystemProcessId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return SystemProcessFromDataRow(ds.Tables[0].Rows[0]);
		}

		public  List<SystemProcess> GetSystemProcessByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetSystemProcessSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from SystemProcess with (nolock)  where {0} {1} '{2}' ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<SystemProcess>(ds,SystemProcessFromDataRow);
		}

		public virtual List<SystemProcess> GetAllSystemProcess(string SelectClause=null)
		{
			string sql=string.IsNullOrEmpty(SelectClause)?GetSystemProcessSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from SystemProcess with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<SystemProcess>(ds, SystemProcessFromDataRow);
		}

		public virtual List<SystemProcess> GetPagedSystemProcess(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetSystemProcessCount(whereClause, searchColumns);
			if(count>0)
			{
			if (count < startIndex) startIndex = (count / pageSize) * pageSize;			
			
           	int PageLowerBound = startIndex;
            int PageUpperBound = PageLowerBound + pageSize;
            string sql = @"CREATE TABLE #PageIndex
				            (
				                [IndexId] int IDENTITY (1, 1) NOT NULL,
				                [SystemProcessID] int				   
				            );";

            //Insert into the temp table
            string tempsql = "INSERT INTO #PageIndex ([SystemProcessID])";
            tempsql += " SELECT ";
            if (pageSize > 0) tempsql += "TOP " + PageUpperBound.ToString();
            tempsql += " [SystemProcessID] ";
            tempsql += " FROM [SystemProcess] AllRecords with (NOLOCK)";
            if (!string.IsNullOrEmpty(whereClause) && whereClause.Length > 0) tempsql += " WHERE " + whereClause;
            if (orderByClause.Length > 0) 
			{
				tempsql += " ORDER BY " + orderByClause;
				if( !orderByClause.Contains("SystemProcessID"))
					tempsql += " , (AllRecords.[SystemProcessID])"; 
			}
			else 
			{
				tempsql  += " ORDER BY (AllRecords.[SystemProcessID])"; 
			}           
            
            // Return paged results
            string pagedResultsSql =
                (string.IsNullOrEmpty(SelectClause)? GetSystemProcessSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [SystemProcess] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [SystemProcess].[SystemProcessID] = PageIndex.[SystemProcessID] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<SystemProcess>(ds, SystemProcessFromDataRow);
			}else{ return null;}
		}

		private int GetSystemProcessCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM SystemProcess as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM SystemProcess as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(SystemProcess))]
		public virtual SystemProcess InsertSystemProcess(SystemProcess entity)
		{

			SystemProcess other=new SystemProcess();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into SystemProcess ( [Name]
				,[Description]
				,[Enabled]
				,[DisplayOrder]
				,[Ip]
				,[Port] )
				Values
				( @Name
				, @Description
				, @Enabled
				, @DisplayOrder
				, @Ip
				, @Port );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@Name",entity.Name)
					, new SqlParameter("@Description",entity.Description ?? (object)DBNull.Value)
					, new SqlParameter("@Enabled",entity.Enabled)
					, new SqlParameter("@DisplayOrder",entity.DisplayOrder)
					, new SqlParameter("@Ip",entity.Ip ?? (object)DBNull.Value)
					, new SqlParameter("@Port",entity.Port ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetSystemProcess(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(SystemProcess))]
		public virtual SystemProcess UpdateSystemProcess(SystemProcess entity)
		{

			if (entity.IsTransient()) return entity;
			SystemProcess other = GetSystemProcess(entity.SystemProcessId);
			if (entity.Equals(other)) return entity;
			string sql=@"Update SystemProcess set  [Name]=@Name
							, [Description]=@Description
							, [Enabled]=@Enabled
							, [DisplayOrder]=@DisplayOrder
							, [Ip]=@Ip
							, [Port]=@Port 
							 where SystemProcessID=@SystemProcessID";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@Name",entity.Name)
					, new SqlParameter("@Description",entity.Description ?? (object)DBNull.Value)
					, new SqlParameter("@Enabled",entity.Enabled)
					, new SqlParameter("@DisplayOrder",entity.DisplayOrder)
					, new SqlParameter("@Ip",entity.Ip ?? (object)DBNull.Value)
					, new SqlParameter("@Port",entity.Port ?? (object)DBNull.Value)
					, new SqlParameter("@SystemProcessID",entity.SystemProcessId)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetSystemProcess(entity.SystemProcessId);
		}

		public virtual bool DeleteSystemProcess(System.Int32 SystemProcessId)
		{

			string sql="delete from SystemProcess where SystemProcessID=@SystemProcessID";
			SqlParameter parameter=new SqlParameter("@SystemProcessID",SystemProcessId);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(SystemProcess))]
		public virtual SystemProcess DeleteSystemProcess(SystemProcess entity)
		{
			this.DeleteSystemProcess(entity.SystemProcessId);
			return entity;
		}


		public virtual SystemProcess SystemProcessFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			SystemProcess entity=new SystemProcess();
			if (dr.Table.Columns.Contains("SystemProcessID"))
			{
			entity.SystemProcessId = (System.Int32)dr["SystemProcessID"];
			}
			if (dr.Table.Columns.Contains("Name"))
			{
			entity.Name = dr["Name"].ToString();
			}
			if (dr.Table.Columns.Contains("Description"))
			{
			entity.Description = dr["Description"].ToString();
			}
			if (dr.Table.Columns.Contains("Enabled"))
			{
			entity.Enabled = (System.Boolean)dr["Enabled"];
			}
			if (dr.Table.Columns.Contains("DisplayOrder"))
			{
			entity.DisplayOrder = (System.Int32)dr["DisplayOrder"];
			}
			if (dr.Table.Columns.Contains("Ip"))
			{
			entity.Ip = dr["Ip"].ToString();
			}
			if (dr.Table.Columns.Contains("Port"))
			{
			entity.Port = dr["Port"]==DBNull.Value?(System.Int32?)null:(System.Int32?)dr["Port"];
			}
			return entity;
		}

	}
	
	
}
