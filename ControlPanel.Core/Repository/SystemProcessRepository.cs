using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using System.Data.SqlClient;
using System.Data;
using ControlPanel.Core;
using System.Configuration;

namespace ControlPanel.Repository
{
		
	public partial class SystemProcessRepository: SystemProcessRepositoryBase
	{
    
        public SystemProcess GetSystemProcessByName(string name)
        {
            string sql = GetSystemProcessSelectClause();
            sql += "from SystemProcess  where Name=@Name ";
            SqlParameter parameter = new SqlParameter("@Name", name);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
            return SystemProcessFromDataRow(ds.Tables[0].Rows[0]);
        }

        public SystemProcess GetSystemProcessByName(string name,string connectionString)
        {
            string sql = GetSystemProcessSelectClause();
            sql += "from SystemProcess  where Name=@Name ";
            SqlParameter parameter = new SqlParameter("@Name", name);
            DataSet ds = SqlHelper.ExecuteDataset(string.IsNullOrEmpty(connectionString) ? this.ConnectionString : connectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
            return SystemProcessFromDataRow(ds.Tables[0].Rows[0]);
        }

        public List<SystemProcess> GetSystemProcessByIp(string ip, string connectionString)
        {
            string sql = GetSystemProcessSelectClause();
            sql += "from SystemProcess  where Ip=@ip ";
            SqlParameter parameter = new SqlParameter("@ip", ip);
            DataSet ds = SqlHelper.ExecuteDataset(string.IsNullOrEmpty(connectionString) ? this.ConnectionString : connectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<SystemProcess>(ds, SystemProcessFromDataRow);
        }

        public List<SystemProcessThread> GetSystemProcessThreadsByProcessName(string name)
        {
            SystemProcess process = GetSystemProcessByName(name);
            //ISystemProcessThreadRepository systemProcessThreadRepository = IoC.Resolve<ISystemProcessThreadRepository>("SystemProcessThreadRepository");
            SystemProcessThreadRepository systemProcessThreadRepository = new SystemProcessThreadRepository();
            return systemProcessThreadRepository.GetSystemProcessThreadBySystemProcessId(process.SystemProcessId);
        }
    }
}
