using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace ControlPanel.Repository
{
		
	public partial class SystemEventLogRepository: ControlPanel.Repository.SystemEventLogRepositoryBase
	{

        public List<SystemEventLog> GetSystemEventLogsByLastUpdateDate(DateTime lastUpdateDate)
        {
            string sql = GetSystemEventLogSelectClause();
            sql += "from SystemEventLog  where DateOccured>@lastUpdateDate ";
            SqlParameter parameter = new SqlParameter("@lastUpdateDate", lastUpdateDate);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<SystemEventLog>(ds, SystemEventLogFromDataRow);
        }

        public int FlushSystemEventLog(DateTime dt)
        {
            string sql = "delete from SystemEventLog where DateOccured<@date";
            SqlParameter parameter = new SqlParameter("@date", dt);
            return SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
        }

        public DateTime GetMaxCreationDate()
        {
            string sql = @"select max(DateOccured) from [SystemEventLog] (nolock)";
            return Convert.ToDateTime(SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql));
        }
    }
	
	
}
