using System;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ControlPanel.Core;
using ControlPanel.Core.Entities;
using ControlPanel.Core.Extensions;
using ControlPanel.Core.DataInterfaces;


namespace ControlPanel.Repository
{
		
	public partial class DriveStatisticRepository: DriveStatisticRepositoryBase, IDriveStatisticRepository
	{


        public DateTime GetMaxCreationDate()
        {
            string sql = @"select max(CreationDate) from [DriveStatistic] (nolock)";
            return Convert.ToDateTime(SqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, sql));
        }

        public List<DriveStatistic> GetAllSystemDrivesPerformanceByCreationDate(DateTime CreationDate)
        {
            string sql = GetDriveStatisticSelectClause();
            sql += "from [DriveStatistic]  where CreationDate>=@CreationDate ";
            SqlParameter parameter = new SqlParameter("@CreationDate", CreationDate);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<DriveStatistic>(ds, DriveStatisticFromDataRow);
        }

        public List<DriveStatistic> GetPerformanceDrivesStatisticThreadByLastUpdatedDate(DateTime CreationDate)
        {
            string sql = GetDriveStatisticSelectClause();
            sql += "from [DriveStatistic]  where CreationDate>=@CreationDate ";
            SqlParameter parameter = new SqlParameter("@CreationDate", CreationDate);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<DriveStatistic>(ds, DriveStatisticFromDataRow);
        }
	}
	
	
}
