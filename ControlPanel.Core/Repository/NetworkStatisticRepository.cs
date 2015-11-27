using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core;
using ControlPanel.Core.Entities;
using ControlPanel.Core.Extensions;
using System.Data.SqlClient;
using System.Data;

namespace ControlPanel.Repository
{
		
	public partial class NetworkStatisticRepository: NetworkStatisticRepositoryBase
	{

        public override NetworkStatistic InsertNetworkStatistic(NetworkStatistic entity)
        {
            NetworkStatistic other = new NetworkStatistic();
            other = entity;
            if (entity.IsTransient())
            {
                string sql = @"Insert into NetworkStatistic ( [InterfaceName]
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
                SqlParameter[] parameterArray = new SqlParameter[]{
					 new SqlParameter("@InterfaceName",entity.InterfaceName)
					, new SqlParameter("@IPAddress",entity.IpAddress)
					, new SqlParameter("@TotalUsage",entity.TotalUsage)
					, new SqlParameter("@Download",entity.Download)
					, new SqlParameter("@Upload",entity.Upload)
					, new SqlParameter("@CreationDate",entity.CreationDate)
					, new SqlParameter("@ServerIp",entity.ServerIp ?? (object)DBNull.Value)
					, new SqlParameter("@ServerName",entity.ServerName ?? (object)DBNull.Value)};
                var identity = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, parameterArray);
                if (identity == DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
                return GetNetworkStatistic(Convert.ToInt32(identity));
            }
            return entity;
        }


        public List<NetworkStatistic> GetNetworkPerformanceStatisticThreadByLastUpdatedDate(DateTime CreationDate)
        {
            string sql = GetNetworkStatisticSelectClause();
            sql += "from [NetworkStatistic]  where CreationDate>=@CreationDate ";
            SqlParameter parameter = new SqlParameter("@CreationDate", CreationDate);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<NetworkStatistic>(ds, NetworkStatisticFromDataRow);
        }


        public DateTime GetMaxCreationDate()
        {
            string sql = @"select max(CreationDate) from [NetworkStatistic] (nolock)";
            return Convert.ToDateTime(SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql));
        }


        public List<NetworkStatistic> GetAllSystemNetworkPerformanceByCreationDate(DateTime CreationDate)
        {
            string sql = GetNetworkStatisticSelectClause();
            sql += "from [NetworkStatistic]  where CreationDate>=@CreationDate ";
            SqlParameter parameter = new SqlParameter("@CreationDate", CreationDate);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<NetworkStatistic>(ds, NetworkStatisticFromDataRow);
        }

        public List<NetworkStatistic> GetAllSystemNetworkPerformanceByAllDate()
        {
            string sql = GetNetworkStatisticSelectClause();
            sql += "from [NetworkStatistic] (nolock)";
           
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql);
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<NetworkStatistic>(ds, NetworkStatisticFromDataRow);
        }
	}
	
	
}
