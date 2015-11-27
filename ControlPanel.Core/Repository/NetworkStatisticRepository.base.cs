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
		
	public abstract partial class NetworkStatisticRepositoryBase : Repository 
	{
        
        public NetworkStatisticRepositoryBase()
        {   
            this.SearchColumns=new Dictionary<string, SearchColumn>();

			this.SearchColumns.Add("NetworkStatisticId",new SearchColumn(){Name="NetworkStatisticId",Title="NetworkStatisticId",SelectClause="NetworkStatisticId",WhereClause="AllRecords.NetworkStatisticId",DataType="System.Int32",IsForeignColumn=false,PropertyName="NetworkStatisticId",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("InterfaceName",new SearchColumn(){Name="InterfaceName",Title="InterfaceName",SelectClause="InterfaceName",WhereClause="AllRecords.InterfaceName",DataType="System.String",IsForeignColumn=false,PropertyName="InterfaceName",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("IPAddress",new SearchColumn(){Name="IPAddress",Title="IPAddress",SelectClause="IPAddress",WhereClause="AllRecords.IPAddress",DataType="System.String",IsForeignColumn=false,PropertyName="IpAddress",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("TotalUsage",new SearchColumn(){Name="TotalUsage",Title="TotalUsage",SelectClause="TotalUsage",WhereClause="AllRecords.TotalUsage",DataType="System.Double",IsForeignColumn=false,PropertyName="TotalUsage",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Download",new SearchColumn(){Name="Download",Title="Download",SelectClause="Download",WhereClause="AllRecords.Download",DataType="System.Double",IsForeignColumn=false,PropertyName="Download",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("Upload",new SearchColumn(){Name="Upload",Title="Upload",SelectClause="Upload",WhereClause="AllRecords.Upload",DataType="System.Double",IsForeignColumn=false,PropertyName="Upload",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("CreationDate",new SearchColumn(){Name="CreationDate",Title="CreationDate",SelectClause="CreationDate",WhereClause="AllRecords.CreationDate",DataType="System.DateTime",IsForeignColumn=false,PropertyName="CreationDate",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ServerIp",new SearchColumn(){Name="ServerIp",Title="ServerIp",SelectClause="ServerIp",WhereClause="AllRecords.ServerIp",DataType="System.String",IsForeignColumn=false,PropertyName="ServerIp",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});
			this.SearchColumns.Add("ServerName",new SearchColumn(){Name="ServerName",Title="ServerName",SelectClause="ServerName",WhereClause="AllRecords.ServerName",DataType="System.String",IsForeignColumn=false,PropertyName="ServerName",IsAdvanceSearchColumn = false,IsBasicSearchColumm = false});        
        }
        
		public virtual List<SearchColumn> GetNetworkStatisticSearchColumns()
        {
            List<SearchColumn> searchColumns = new List<SearchColumn>();
            foreach (KeyValuePair<string, SearchColumn> keyValuePair in this.SearchColumns)
            {
                searchColumns.Add(keyValuePair.Value);
            }
            return searchColumns;
        }
		
		
		
        public virtual Dictionary<string, string> GetNetworkStatisticBasicSearchColumns()
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

        public virtual List<SearchColumn> GetNetworkStatisticAdvanceSearchColumns()
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
        
        
        public virtual string GetNetworkStatisticSelectClause()
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
                        	selectQuery =  "[NetworkStatistic].["+keyValuePair.Key+"]";
                    	}
                    	else
                    	{
                        	selectQuery += ",[NetworkStatistic].["+keyValuePair.Key+"]";
                    	}
                	}
            	}
            }
            return "Select "+selectQuery+" ";
        }
        

		public virtual NetworkStatistic GetNetworkStatistic(System.Int32 NetworkStatisticId,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetNetworkStatisticSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [NetworkStatistic] with (nolock)  where NetworkStatisticId=@NetworkStatisticId ";
			SqlParameter parameter=new SqlParameter("@NetworkStatisticId",NetworkStatisticId);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
			return NetworkStatisticFromDataRow(ds.Tables[0].Rows[0]);
		}

		public  List<NetworkStatistic> GetNetworkStatisticByKeyValue(string Key,string Value,Operands operand,string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetNetworkStatisticSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+= string.Format("from [NetworkStatistic] with (nolock)  where {0} {1} '{2}' ",Key,operand.ToOperandString(),Value);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<NetworkStatistic>(ds,NetworkStatisticFromDataRow);
		}

		public virtual List<NetworkStatistic> GetAllNetworkStatistic(string SelectClause=null)
		{

			string sql=string.IsNullOrEmpty(SelectClause)?GetNetworkStatisticSelectClause():(string.Format("Select {0} ",SelectClause));
			sql+="from [NetworkStatistic] with (nolock)  ";
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql,null);
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<NetworkStatistic>(ds, NetworkStatisticFromDataRow);
		}

		public virtual List<NetworkStatistic> GetPagedNetworkStatistic(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null)
		{

			string whereClause = base.GetAdvancedWhereClauseByColumn(searchColumns, GetSearchColumns());
               if (!String.IsNullOrEmpty(orderByClause))
               {
                   KeyValuePair<string, string> parsedOrderByClause = base.ParseOrderByClause(orderByClause);
                   orderByClause = base.GetBasicSearchOrderByClauseByColumn(parsedOrderByClause.Key, parsedOrderByClause.Value, this.SearchColumns);
               }

            count=GetNetworkStatisticCount(whereClause, searchColumns);
			if(count>0)
			{
			if (count < startIndex) startIndex = (count / pageSize) * pageSize;			
			
           	int PageLowerBound = startIndex;
            int PageUpperBound = PageLowerBound + pageSize;
            string sql = @"CREATE TABLE #PageIndex
				            (
				                [IndexId] int IDENTITY (1, 1) NOT NULL,
				                [NetworkStatisticId] int				   
				            );";

            //Insert into the temp table
            string tempsql = "INSERT INTO #PageIndex ([NetworkStatisticId])";
            tempsql += " SELECT ";
            if (pageSize > 0) tempsql += "TOP " + PageUpperBound.ToString();
            tempsql += " [NetworkStatisticId] ";
            tempsql += " FROM [NetworkStatistic] AllRecords with (NOLOCK)";
            if (!string.IsNullOrEmpty(whereClause) && whereClause.Length > 0) tempsql += " WHERE " + whereClause;
            if (orderByClause.Length > 0) 
			{
				tempsql += " ORDER BY " + orderByClause;
				if( !orderByClause.Contains("NetworkStatisticId"))
					tempsql += " , (AllRecords.[NetworkStatisticId])"; 
			}
			else 
			{
				tempsql  += " ORDER BY (AllRecords.[NetworkStatisticId])"; 
			}           
            
            // Return paged results
            string pagedResultsSql =
                (string.IsNullOrEmpty(SelectClause)? GetNetworkStatisticSelectClause():(string.Format("Select {0} ",SelectClause)))+@" FROM [NetworkStatistic] , #PageIndex PageIndex WHERE ";
            pagedResultsSql += " PageIndex.IndexId > " + PageLowerBound.ToString(); 
            pagedResultsSql += @" AND [NetworkStatistic].[NetworkStatisticId] = PageIndex.[NetworkStatisticId] 
				                  ORDER BY PageIndex.IndexId;";
            pagedResultsSql += " drop table #PageIndex";
            sql = sql + tempsql + pagedResultsSql;
			sql = string.Format(sql, whereClause, pageSize, startIndex, orderByClause);
			DataSet ds=SqlHelper.ExecuteDataset(this.ConnectionString,CommandType.Text,sql, GetSQLParamtersBySearchColumns(searchColumns));
			if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
			return CollectionFromDataSet<NetworkStatistic>(ds, NetworkStatisticFromDataRow);
			}else{ return null;}
		}

		private int GetNetworkStatisticCount(string whereClause, List<SearchColumn> searchColumns)
		{

			string sql=string.Empty;
			if(string.IsNullOrEmpty(whereClause))
				sql = "SELECT Count(*) FROM NetworkStatistic as AllRecords  ";
			else
				sql = "SELECT Count(*) FROM NetworkStatistic as AllRecords  where  " +whereClause;
			var rowCount = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, GetSQLParamtersBySearchColumns(searchColumns));
			return rowCount == DBNull.Value ? 0 :(int)rowCount;
		}

		[MOLog(AuditOperations.Create,typeof(NetworkStatistic))]
		public virtual NetworkStatistic InsertNetworkStatistic(NetworkStatistic entity)
		{

			NetworkStatistic other=new NetworkStatistic();
			other = entity;
			if(entity.IsTransient())
			{
				string sql=@"Insert into NetworkStatistic ( [InterfaceName]
				,[IPAddress]
				,[TotalUsage]
				,[Download]
				,[Upload]
				,[CreationDate]
				,[ServerIp]
				,[ServerName] )
				Values
				( @InterfaceName
				, @IPAddress
				, @TotalUsage
				, @Download
				, @Upload
				, @CreationDate
				, @ServerIp
				, @ServerName );
				Select scope_identity()";
				SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@InterfaceName",entity.InterfaceName)
					, new SqlParameter("@IPAddress",entity.IpAddress)
					, new SqlParameter("@TotalUsage",entity.TotalUsage)
					, new SqlParameter("@Download",entity.Download)
					, new SqlParameter("@Upload",entity.Upload)
					, new SqlParameter("@CreationDate",entity.CreationDate)
					, new SqlParameter("@ServerIp",entity.ServerIp ?? (object)DBNull.Value)
					, new SqlParameter("@ServerName",entity.ServerName ?? (object)DBNull.Value)};
				var identity=SqlHelper.ExecuteScalar(this.ConnectionString,CommandType.Text,sql,parameterArray);
				if(identity==DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
				return GetNetworkStatistic(Convert.ToInt32(identity));
			}
			return entity;
		}

		[MOLog(AuditOperations.Update,typeof(NetworkStatistic))]
		public virtual NetworkStatistic UpdateNetworkStatistic(NetworkStatistic entity)
		{

			if (entity.IsTransient()) return entity;
			NetworkStatistic other = GetNetworkStatistic(entity.NetworkStatisticId);
			if (entity.Equals(other)) return entity;
			string sql=@"Update NetworkStatistic set  [InterfaceName]=@InterfaceName
							, [IPAddress]=@IPAddress
							, [TotalUsage]=@TotalUsage
							, [Download]=@Download
							, [Upload]=@Upload
							, [CreationDate]=@CreationDate
							, [ServerIp]=@ServerIp
							, [ServerName]=@ServerName 
							 where NetworkStatisticId=@NetworkStatisticId";
			SqlParameter[] parameterArray=new SqlParameter[]{
					 new SqlParameter("@InterfaceName",entity.InterfaceName)
					, new SqlParameter("@IPAddress",entity.IpAddress)
					, new SqlParameter("@TotalUsage",entity.TotalUsage)
					, new SqlParameter("@Download",entity.Download)
					, new SqlParameter("@Upload",entity.Upload)
					, new SqlParameter("@CreationDate",entity.CreationDate)
					, new SqlParameter("@ServerIp",entity.ServerIp ?? (object)DBNull.Value)
					, new SqlParameter("@ServerName",entity.ServerName ?? (object)DBNull.Value)
					, new SqlParameter("@NetworkStatisticId",entity.NetworkStatisticId)};
			SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,parameterArray);
			return GetNetworkStatistic(entity.NetworkStatisticId);
		}

		public virtual bool DeleteNetworkStatistic(System.Int32 NetworkStatisticId)
		{

			string sql="delete from NetworkStatistic where NetworkStatisticId=@NetworkStatisticId";
			SqlParameter parameter=new SqlParameter("@NetworkStatisticId",NetworkStatisticId);
			var identity=SqlHelper.ExecuteNonQuery(this.ConnectionString,CommandType.Text,sql,new SqlParameter[] { parameter });
			return (Convert.ToInt32(identity))==1? true: false;
		}

		[MOLog(AuditOperations.Delete,typeof(NetworkStatistic))]
		public virtual NetworkStatistic DeleteNetworkStatistic(NetworkStatistic entity)
		{
			this.DeleteNetworkStatistic(entity.NetworkStatisticId);
			return entity;
		}


		public virtual NetworkStatistic NetworkStatisticFromDataRow(DataRow dr)
		{
			if(dr==null) return null;
			NetworkStatistic entity=new NetworkStatistic();
			if (dr.Table.Columns.Contains("NetworkStatisticId"))
			{
			entity.NetworkStatisticId = (System.Int32)dr["NetworkStatisticId"];
			}
			if (dr.Table.Columns.Contains("InterfaceName"))
			{
			entity.InterfaceName = dr["InterfaceName"].ToString();
			}
			if (dr.Table.Columns.Contains("IPAddress"))
			{
			entity.IpAddress = dr["IPAddress"].ToString();
			}
			if (dr.Table.Columns.Contains("TotalUsage"))
			{
			entity.TotalUsage = (System.Double)dr["TotalUsage"];
			}
			if (dr.Table.Columns.Contains("Download"))
			{
			entity.Download = (System.Double)dr["Download"];
			}
			if (dr.Table.Columns.Contains("Upload"))
			{
			entity.Upload = (System.Double)dr["Upload"];
			}
			if (dr.Table.Columns.Contains("CreationDate"))
			{
			entity.CreationDate = (System.DateTime)dr["CreationDate"];
			}
			if (dr.Table.Columns.Contains("ServerIp"))
			{
			entity.ServerIp = dr["ServerIp"].ToString();
			}
			if (dr.Table.Columns.Contains("ServerName"))
			{
			entity.ServerName = dr["ServerName"].ToString();
			}
			return entity;
		}

	}
	
	
}
