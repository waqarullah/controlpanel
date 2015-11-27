using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ControlPanel.Core
{
    public class DatabaseHelper
    {
        public static bool CreateDatabaseFromDbMigrate(string clientCode)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\migrate.exe";
                process.StartInfo.Arguments = string.Format("nobackup {0}", clientCode);
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.EnableRaisingEvents = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.RedirectStandardInput = true;
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit(200000);
            }
            return true;
        }

        public static bool CreateTables(string dbName, string connectionString)
        {

        string sqlCreateDBQuery =

@"USE [<DatabaseName>]
GO
CREATE TABLE [dbo].[SystemEventLog](
	[EventLogID] [int] IDENTITY(1,1) NOT NULL,
	[EventCode] [int] NULL,
	[Title] [varchar](5000) NULL,
	[Description] [varchar](5000) NULL,
	[DateOccured] [datetime] NOT NULL,
	[ElmahErrorID] [uniqueidentifier] NULL,
	[Source] [nvarchar](60) NULL,
 CONSTRAINT [PK_EventLog] PRIMARY KEY CLUSTERED 
(
	[EventLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[SystemEventLog] ADD  CONSTRAINT [DF_EventLog_DateOccured]  DEFAULT (getdate()) FOR [DateOccured]
GO
CREATE TABLE [dbo].[SystemProcess](
	[SystemProcessID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[Enabled] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
    [Ip] [varchar](255) NULL,
    [Port] int
 CONSTRAINT [PK_SystemProcess] PRIMARY KEY CLUSTERED 
(
	[SystemProcessID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SystemProcess] ADD  CONSTRAINT [DF_SystemProcess_DisplayOrder]  DEFAULT ((0)) FOR [DisplayOrder]
GO
CREATE TABLE [dbo].[SystemProcessThread](
	[SystemProcessThreadID] [int] IDENTITY(1,1) NOT NULL,
	[SystemProcessID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SpringEntryName] [nvarchar](50) NULL,
	[Description] [nvarchar](100) NULL,
	[Enabled] [bit] NOT NULL,
	[Continuous] [bit] NOT NULL,
	[SleepTime] [int] NOT NULL,
	[AutoStart] [bit] NOT NULL,
	[Status] [nvarchar](50) NULL,
	[Message] [nvarchar](500) NULL,
	[ScheduledTime] [time](7) NULL,
	[StartRange] [int] NULL,
	[EndRange] [int] NULL,
	[LastSuccessfullyExecuted] [datetime] NULL,
	[ContinuousDelay] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DisplayOrder] [varchar](10) NOT NULL,
	[Argument] [nvarchar](500) NULL,
	[LastUpdateDate] [datetime] NULL,
	[ExecutionTime] [float] NULL,
	[EstimatedExecutionTime] [float] NULL,
	[ShowUpdateInLog] [bit] NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [PK_SystemProcessThread] PRIMARY KEY CLUSTERED 
(
	[SystemProcessThreadID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SystemProcessThread]  WITH CHECK ADD  CONSTRAINT [FK_SystemProcessThread_SystemProcess] FOREIGN KEY([SystemProcessID])
REFERENCES [dbo].[SystemProcess] ([SystemProcessID])
GO
ALTER TABLE [dbo].[SystemProcessThread] CHECK CONSTRAINT [FK_SystemProcessThread_SystemProcess]
GO
ALTER TABLE [dbo].[SystemProcessThread] ADD  CONSTRAINT [DF_SystemProcessThread_SleepTime]  DEFAULT ((60)) FOR [SleepTime]
GO
ALTER TABLE [dbo].[SystemProcessThread] ADD  CONSTRAINT [DF_SystemProcessThread_ContinuousDelay]  DEFAULT ((60)) FOR [ContinuousDelay]
GO
ALTER TABLE [dbo].[SystemProcessThread] ADD  CONSTRAINT [DF_SystemProcessThread_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[SystemProcessThread] ADD  CONSTRAINT [DF_SystemProcessThread_DisplayOrder]  DEFAULT ('0') FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[SystemProcessThread] ADD  CONSTRAINT [DF_SystemProcessThread_ShowUpdateInLog]  DEFAULT ((0)) FOR [ShowUpdateInLog]
GO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PerformanceStatistic](
	[PerformanceStatisticID] [int] IDENTITY(1,1) NOT NULL,
	[CPU] [float] NULL,
	[Memory] [float] NULL,
	[CreationDate] [datetime] NULL,
	[MachineName] [varchar](255) NULL,
	[IPAddress] [varchar](50) NULL,
	[DriveSpaceAvailable] [float] NULL,
	[DriveTotalSpace] [float] NULL,
 CONSTRAINT [PK_PerformanceStatistic] PRIMARY KEY CLUSTERED 
(
	[PerformanceStatisticID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO


GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[NetworkStatistic](
	[NetworkStatisticId] [int] IDENTITY(1,1) NOT NULL,
	[InterfaceName] [varchar](500) NOT NULL,
	[IPAddress] [varchar](50) NOT NULL,
	[TotalUsage] [float] NOT NULL,
	[Download] [float] NOT NULL,
	[Upload] [float] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ServerIp] [varchar](50) NULL,
	[ServerName] [varchar](50) NULL,
 CONSTRAINT [PK_NetworkStatistic] PRIMARY KEY CLUSTERED 
(
	[NetworkStatisticId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DriveStatistic](
	[DriveStatisticId] [int] IDENTITY(1,1) NOT NULL,
	[IPAddress] [varchar](50) NULL,
	[DriveSpaceAvailable] [float] NULL,
	[DriveTotalSpace] [float] NULL,
	[DriveName] [varchar](50) NULL,
	[CreationDate] [datetime] NULL,
	[MachineName] [varchar](50) NULL,
 CONSTRAINT [PK_DriveStatistic] PRIMARY KEY CLUSTERED 
(
	[DriveStatisticId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


";

            using (SqlConnection tmpConn = new SqlConnection())
            {
                tmpConn.ConnectionString = connectionString;
                sqlCreateDBQuery = sqlCreateDBQuery.Replace("<DatabaseName>", dbName);

                SqlCommand myCommand = new SqlCommand(sqlCreateDBQuery.Replace("\r\nGO",";"), tmpConn);
                try
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.CommandTimeout = 0;
                    tmpConn.Open();
                    myCommand.ExecuteNonQuery();
                    return true;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

            }

        }

        public static bool CreateDatabase(string dbName, string connectionString)
        {

            string sqlCreateDBQuery = @"CREATE DATABASE [<DatabaseName>]";

            using (SqlConnection tmpConn = new SqlConnection())
            {  
                tmpConn.ConnectionString = connectionString;
                var DB = tmpConn.Database;
                tmpConn.ConnectionString = connectionString.Replace(DB, "master");

                sqlCreateDBQuery = sqlCreateDBQuery.Replace("<DatabaseName>", dbName);

                SqlCommand myCommand = new SqlCommand(sqlCreateDBQuery, tmpConn);
                try
                {
                    myCommand.CommandTimeout = 0;
                    tmpConn.Open();
                    myCommand.ExecuteNonQuery();
                    return true;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

            }

        }

        public static bool RestoreDatabase(string databaseName, string backupPath, string filePath, string connectionString)
        {
            using (SqlConnection tmpConn = new SqlConnection())
            {
                tmpConn.ConnectionString = connectionString;
                string sql = string.Format(@"
DECLARE @Table TABLE (LogicalName varchar(128),[PhysicalName] varchar(128), [Type] varchar, [FileGroupName] varchar(128), [Size] varchar(128), 
            [MaxSize] varchar(128), [FileId]varchar(128), [CreateLSN]varchar(128), [DropLSN]varchar(128), [UniqueId]varchar(128), [ReadOnlyLSN]varchar(128), [ReadWriteLSN]varchar(128), 
            [BackupSizeInBytes]varchar(128), [SourceBlockSize]varchar(128), [FileGroupId]varchar(128), [LogGroupGUID]varchar(128), [DifferentialBaseLSN]varchar(128), [DifferentialBaseGUID]varchar(128), [IsReadOnly]varchar(128), [IsPresent]varchar(128), [TDEThumbprint]varchar(128)
)

INSERT INTO @Table
EXEC ('restore filelistonly
 from disk= ''{1}''')

declare @DbName varchar(255);
declare @LogName varchar(255);
set @DbName =(select LogicalName from @Table where [type]='D')
set @LogName =(select  LogicalName from @Table where [type]='L')

ALTER DATABASE {0} SET Single_User WITH Rollback Immediate
 RESTORE DATABASE {0} FROM DISK='{1}' With MOVE @DbName TO '{2}{0}.mdf', MOVE @LogName TO '{2}{0}_log.ldf', REPLACE ALTER DATABASE {0} SET Multi_User
", databaseName, backupPath, filePath);

                SqlCommand myCommand = new SqlCommand(sql, tmpConn);
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandTimeout = 0;
                try
                {
                    tmpConn.Open();
                    myCommand.ExecuteNonQuery();
                    return true;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

            }
        }

        public static bool BackupDatabase(string databaseName, string backupPath, string connectionString)
        {
            string message = BackupDB(databaseName, backupPath, connectionString);
            if (message.Equals("success"))
                return true;
            else
                return false;
        }

        public static string BackupDB(string databaseName, string backupPath, string connectionString)
        {
            using (SqlConnection tmpConn = new SqlConnection())
            {
                tmpConn.ConnectionString = connectionString;
                string sql = string.Format("USE MASTER BACKUP DATABASE {0} TO DISK='{1}'", databaseName, backupPath);

                SqlCommand myCommand = new SqlCommand(sql, tmpConn);
                try
                {
                    myCommand.CommandTimeout = 0;
                    tmpConn.Open();
                    myCommand.ExecuteNonQuery();
                    return "success";
                }
                catch (System.Exception ex)
                {
                    //XChatLiveProException exception = new XChatLiveProException(ex.Message + "====" + ex.InnerException + "====" + ex.StackTrace);
                    //ExceptionHandler.LogElmahException(exception);
                    return ex.Message;
                }
            }
        }

        public static bool ShrinkDBLog(string connectionString)
        {
            using (SqlConnection tmpConn = new SqlConnection())
            {
                tmpConn.ConnectionString = connectionString;
                string sql = string.Format(@"
DECLARE @SqlStatement2 as nvarchar(max)
SET @SqlStatement2 = 'SELECT [name], [recovery_model_desc] FROM ' + DB_NAME() + '.sys.databases WHERE [name] = ''' + DB_NAME() + '''';
EXEC ( @SqlStatement2 )
DECLARE @SqlStatement as nvarchar(max)
DECLARE @LogFileLogicalName as sysname
SET @SqlStatement = 'ALTER DATABASE ' + DB_NAME() + ' SET RECOVERY SIMPLE'
EXEC ( @SqlStatement )
SELECT [name], [recovery_model_desc] FROM sys.databases WHERE [name] = DB_NAME()
SELECT @LogFileLogicalName = [Name] FROM sys.database_files WHERE type = 1
DBCC Shrinkfile(@LogFileLogicalName, 1)
SET @SqlStatement = 'ALTER DATABASE ' + DB_NAME() + ' SET RECOVERY FULL'
EXEC ( @SqlStatement )
SET @SqlStatement = 'SELECT [name], [recovery_model_desc] FROM ' + DB_NAME() + '.sys.databases WHERE [name] = ''' + DB_NAME() + ''''
EXEC ( @SqlStatement )
");

                SqlCommand myCommand = new SqlCommand(sql, tmpConn);
                myCommand.CommandType = CommandType.Text;
                try
                {
                    tmpConn.Open();
                    myCommand.ExecuteNonQuery();
                    return true;
                }
                catch (System.Exception ex)
                {
                    //XChatLiveProException exception = new XChatLiveProException(ex.Message + "====" + ex.InnerException + "====" + ex.StackTrace);
                    //ExceptionHandler.LogElmahException(exception);
                    return false;
                }
            }
        }

        public static bool DatabaseExists(string databaseName, string masterConString)
        {
            string sqlCreateDBQuery;
            bool result = false;

            try
            {
                using (SqlConnection tmpConn = new SqlConnection())
                {
                    tmpConn.ConnectionString = masterConString;

                    sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);

                    using (tmpConn)
                    {
                        using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                        {
                            tmpConn.Open();
                            sqlCmd.CommandTimeout = 0;
                            var r = sqlCmd.ExecuteScalar();
                            tmpConn.Close();

                            result = (r != null ? true : false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public static bool TableExists(string TableName, string masterConString)
        {
            string sqlCreateDBQuery;
            bool result = false;

            try
            {
                using (SqlConnection tmpConn = new SqlConnection())
                {
                    tmpConn.ConnectionString = masterConString;

                    sqlCreateDBQuery = string.Format("SELECT COUNT(*) FROM information_schema.tables WHERE table_name = '{0}'", TableName);

                    using (tmpConn)
                    {
                        using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                        {
                            tmpConn.Open();
                            sqlCmd.CommandTimeout = 0;
                            var r = Convert.ToInt32(sqlCmd.ExecuteScalar());
                            
                            tmpConn.Close();

                            result = (r > 0 ? true : false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;

        }

    }
}
