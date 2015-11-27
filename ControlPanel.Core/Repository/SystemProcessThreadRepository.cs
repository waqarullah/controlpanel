using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ControlPanel.Core;


namespace ControlPanel.Repository
{
		
	public partial class SystemProcessThreadRepository: ControlPanel.Repository.SystemProcessThreadRepositoryBase
	{

        public override SystemProcessThread InsertSystemProcessThread(SystemProcessThread entity)
        {
            entity.LastUpdateDate = DateTime.UtcNow;
            return base.InsertSystemProcessThread(entity);
        }

        public override SystemProcessThread UpdateSystemProcessThread(SystemProcessThread entity)
        {
            entity.LastUpdateDate = DateTime.UtcNow;
            return base.UpdateSystemProcessThread(entity);
        }

        public SystemProcessThread GetSystemProcessThreadByName(string threadName)
        {
            string sql = GetSystemProcessThreadSelectClause();
            sql += "from SystemProcessThread  where Name=@Name ";
            SqlParameter parameter = new SqlParameter("@Name", threadName);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
            return SystemProcessThreadFromDataRow(ds.Tables[0].Rows[0]);
        }

        public List<SystemProcessThread> GetSystemProcessThreadByProcessID(int processID)
        {
            string sql = GetSystemProcessThreadSelectClause();
            sql += "from SystemProcessThread  where SystemProcessId=@ProcessId ";
            SqlParameter parameter = new SqlParameter("@ProcessId", processID);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<SystemProcessThread>(ds, SystemProcessThreadFromDataRow);
        }


        public bool ToggleSystemProcessThreadContinuous(int systemProcessThreadID)
        {
            var entity = GetSystemProcessThread(systemProcessThreadID);
            if (entity != null)
            {
                string sql = "update SystemProcessThread set Continuous=@Continuous,LastUpdateDate=@lastUpdateDate where SystemProcessThreadId=@ProcessId ";
                return SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { new SqlParameter("@ProcessId", systemProcessThreadID), new SqlParameter("@Continuous", !entity.Continuous), new SqlParameter("@lastUpdateDate", DateTime.UtcNow) }) > 0;
            }
            return false;
        }

        public bool ToggleSystemProcessThreadEnabled(int systemProcessThreadID)
        {
            var entity = GetSystemProcessThread(systemProcessThreadID);
            if (entity != null)
            {
                string sql = "update SystemProcessThread set Enabled=@Enabled,LastUpdateDate=@lastUpdateDate where SystemProcessThreadId=@ProcessId ";
                return SqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { new SqlParameter("@ProcessId", systemProcessThreadID), new SqlParameter("@Enabled", !entity.Enabled), new SqlParameter("@lastUpdateDate", DateTime.UtcNow) }) > 0;
            }
            return false;
        }

        public SystemProcessThread GetSystemProcessThreadsByTenantId(int TenantId , string connectionString = null)
        {
            string sql = GetSystemProcessThreadSelectClause();
            sql += "from SystemProcessThread  where TenantId = @TenantId";
            SqlParameter parameter = new SqlParameter("@TenantId", TenantId);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
            return SystemProcessThreadFromDataRow(ds.Tables[0].Rows[0]);
        }

        public List<SystemProcessThread> GetSystemProcessThreadsByLastUpdateDate(DateTime lastUpdateDate)
        {
            string sql = GetSystemProcessThreadSelectClause();
            sql += "from SystemProcessThread  where LastUpdateDate>@lastUpdateDate ";
            SqlParameter parameter = new SqlParameter("@lastUpdateDate", lastUpdateDate);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count == 0) return null;
            return CollectionFromDataSet<SystemProcessThread>(ds, SystemProcessThreadFromDataRow);
        }

        public SystemProcessThread GetSystemProcessThreadBySpringName(string springName)
        {
            string sql = GetSystemProcessThreadSelectClause();
            sql += "from SystemProcessThread  where SpringEntryName=@SpringEntryName";
            SqlParameter parameter = new SqlParameter("@SpringEntryName", springName);
            DataSet ds = SqlHelper.ExecuteDataset(this.ConnectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
            return SystemProcessThreadFromDataRow(ds.Tables[0].Rows[0]);
        }

        public SystemProcessThread GetSystemProcessThreadBySpringName(string springName,string connectionString=null)
        {
            string sql = GetSystemProcessThreadSelectClause();
            sql += "from SystemProcessThread  where SpringEntryName=@SpringEntryName";
            SqlParameter parameter = new SqlParameter("@SpringEntryName", springName);
            DataSet ds = SqlHelper.ExecuteDataset(string.IsNullOrEmpty(connectionString) ? this.ConnectionString : connectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
            return SystemProcessThreadFromDataRow(ds.Tables[0].Rows[0]);
        }

        public virtual SystemProcessThread UpdateSystemProcessThread(SystemProcessThread entity, string connectionString)
        {
            entity.LastUpdateDate = DateTime.UtcNow;
            string sql = @"Update SystemProcessThread set  [SystemProcessID]=@SystemProcessID
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
							 where SystemProcessThreadID=@SystemProcessThreadID";
            SqlParameter[] parameterArray = new SqlParameter[]{
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
					, new SqlParameter("@SystemProcessThreadID",entity.SystemProcessThreadId)};
            SqlHelper.ExecuteNonQuery(string.IsNullOrEmpty(connectionString) ? this.ConnectionString : connectionString, CommandType.Text, sql, parameterArray);
            return GetSystemProcessThread(entity.SystemProcessThreadId, connectionString);
        }

        public virtual SystemProcessThread GetSystemProcessThread(System.Int32 SystemProcessThreadId, string connectionString = null)
        {
            string sql = GetSystemProcessThreadSelectClause();
            sql += "from SystemProcessThread  where SystemProcessThreadID=@SystemProcessThreadID  AND IsDeleted='false'";
            SqlParameter parameter = new SqlParameter("@SystemProcessThreadID", SystemProcessThreadId);
            DataSet ds = SqlHelper.ExecuteDataset(string.IsNullOrEmpty(connectionString) ? this.ConnectionString : connectionString, CommandType.Text, sql, new SqlParameter[] { parameter });
            if (ds == null || ds.Tables.Count != 1 || ds.Tables[0].Rows.Count != 1) return null;
            return SystemProcessThreadFromDataRow(ds.Tables[0].Rows[0]);
        }
        
        public virtual SystemProcessThread InsertSystemProcessThread(SystemProcessThread entity, string connectionString)
        {
            SystemProcessThread other = new SystemProcessThread();
            other = entity;
            if (entity.IsTransient())
            {
                string sql = @"Insert into SystemProcessThread (  [TenantId],[SystemProcessID]
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
				,[ShowUpdateInLog] )
				Values
				( @TenantId
                , @SystemProcessID
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
				, @ShowUpdateInLog );
				Select scope_identity()";
                SqlParameter[] parameterArray = new SqlParameter[]{
                      new SqlParameter("@TenantId",entity.TenantId ?? (object)DBNull.Value)
					, new SqlParameter("@SystemProcessID",entity.SystemProcessId)
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
					, new SqlParameter("@ShowUpdateInLog",entity.ShowUpdateInLog ?? (object)DBNull.Value)};
                var identity = SqlHelper.ExecuteScalar(string.IsNullOrEmpty(connectionString) ? this.ConnectionString : connectionString, CommandType.Text, sql, parameterArray);
                if (identity == DBNull.Value) throw new DataException("Identity column was null as a result of the insert operation.");
                return GetSystemProcessThread(Convert.ToInt32(identity),connectionString);
            }
            return entity;
        }
    }
	
	
}
