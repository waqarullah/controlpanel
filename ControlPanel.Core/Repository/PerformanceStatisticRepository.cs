using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using ControlPanel.Core.DataInterfaces;
using System.Data.SqlClient;
using System.Data;

namespace ControlPanel.Repository
{
		
	public partial class PerformanceStatisticRepository: PerformanceStatisticRepositoryBase, IPerformanceStatisticRepository
	{
        public override PerformanceStatistic InsertPerformanceStatistic(PerformanceStatistic entity)
        {
            PerformanceStatistic other = new PerformanceStatistic();
            other = entity;
            if (entity.IsTransient())
            {
                string sql = @"Insert into PerformanceStatistic ( [CPU],[Memory],[CreationDate],[MachineName],[IPAddress],[DriveSpaceAvailable],[DriveTotalSpace] ) Values
( @CPU, @Memory, getutcdate(), @MachineName, @IPAddress, @DriveSpaceAvailable, @DriveTotalSpace ); Select scope_identity()";
                SqlParameter[] parameterArray = new SqlParameter[]{
					 new SqlParameter("@CPU",entity.Cpu ?? (object)DBNull.Value)
					, new SqlParameter("@Memory",entity.Memory ?? (object)DBNull.Value)
					, new SqlParameter("@MachineName",entity.MachineName ?? (object)DBNull.Value)
					, new SqlParameter("@IPAddress",entity.IpAddress ?? (object)DBNull.Value)
					, new SqlParameter("@DriveSpaceAvailable",entity.DriveSpaceAvailable ?? (object)DBNull.Value)
					, new SqlParameter("@DriveTotalSpace",entity.DriveTotalSpace ?? (object)DBNull.Value)};
                var identity = SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql, parameterArray);
                if (identity == DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
                return GetPerformanceStatistic(Convert.ToInt32(identity));
            }
            return entity;
        }

        public List<PerformanceStatistic> GetPerformanceStatisticThreadByLastUpdatedDate(DateTime CreationDate)
        {
            string sql = GetPerformanceStatisticSelectClause();
            sql += "from [PerformanceStatistic]  where CreationDate>=@CreationDate ";
            SqlParameter parameter = new SqlParameter("@CreationDate", CreationDate);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<PerformanceStatistic>(ds, PerformanceStatisticFromDataRow);
        }

        public List<PerformanceStatistic> GetAllSystemPerformanceByCreationDate(DateTime CreationDate)
        {
            string sql = GetPerformanceStatisticSelectClause();
            sql += "from [PerformanceStatistic]  where CreationDate>=@CreationDate ";
            SqlParameter parameter = new SqlParameter("@CreationDate", CreationDate);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<PerformanceStatistic>(ds, PerformanceStatisticFromDataRow);
        }

        public DateTime GetMaxCreationDate()
        {
            string sql = @"select max(CreationDate) from [PerformanceStatistic] (nolock)";
            return Convert.ToDateTime(SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql));
        }

        public int FlushPerformanceStatistic(DateTime dt)
        {
            string sql = "delete from [PerformanceStatistic] where CreationDate<@date";
            SqlParameter parameter = new SqlParameter("@date", dt);
            return SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
        }
	}
	
	
}
