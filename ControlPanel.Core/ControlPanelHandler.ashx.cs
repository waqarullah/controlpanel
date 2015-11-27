using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Runtime.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using ControlPanel.Core;
using System.ServiceProcess;

namespace ControlPanel.Core
{
    /// <summary>
    /// Summary description for ControlPanelHandler
    /// </summary>
    public class ControlPanelHandler : IHttpHandler
    {
        string jsondata = string.Empty;
        string lastupdatedate = string.Empty;
        string binPath = Path.GetDirectoryName((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath);

        ControlPanelMethods cpm = new ControlPanelMethods();

        public void ProcessRequest(HttpContext context)
        {
            System.Data.SqlClient.SqlConnection conn =new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["controlpanel"].ConnectionString);
            if (!DatabaseHelper.DatabaseExists(conn.Database, ConfigurationManager.ConnectionStrings["controlpanel"].ConnectionString))
            {
                DatabaseHelper.CreateDatabase(conn.Database, ConfigurationManager.ConnectionStrings["controlpanel"].ConnectionString);
            }
            
            if (!DatabaseHelper.TableExists("SystemProcess", ConfigurationManager.ConnectionStrings["controlpanel"].ConnectionString))
            {
                DatabaseHelper.CreateTables(conn.Database, ConfigurationManager.ConnectionStrings["controlpanel"].ConnectionString);
            }
           

            if (context.Request.Path.EndsWith("GetUpdatedSystemProcessThreads"))
            {
                byte[] data =new byte[context.Request.InputStream.Length];
                context.Request.InputStream.Read(data,0,data.Length);
                string str = Encoding.UTF8.GetString(data);
                JavaScriptSerializer ser = new JavaScriptSerializer();

                 updatedates upd = ser.Deserialize<updatedates>(str);
               //updatedates upd = (updatedates)JsonConvert.DeserializeObject(str, typeof(updatedates));
                context.Response.ContentType = "application/javascript";
                context.Response.Write(GetUpdatedSystemProcessThreads(upd.ThreadLastUpdateDateStr, upd.EventLogLastUpdateDateStr));
            }

            else if (context.Request.Path.EndsWith("getdata"))
            {
                byte[] data = new byte[context.Request.InputStream.Length];
                context.Request.InputStream.Read(data, 0, data.Length);
                string str = Encoding.UTF8.GetString(data);
                JavaScriptSerializer ser = new JavaScriptSerializer();
                SystemProcessThreadModel sptm = ser.Deserialize<SystemProcessThreadModel>(str);

                getdata(sptm);
                
            }
            else if (context.Request.Path.EndsWith("GetAllVersions"))
            {
               context.Response.Write(GetAllVersions());
            }

            else if (context.Request.Path.EndsWith("CopyAllFiles"))
            {
                CopyAllFiles();
            }

            else if (context.Request.Path.EndsWith("removeEntries"))
            {
                byte[] data = new byte[context.Request.InputStream.Length];
                context.Request.InputStream.Read(data, 0, data.Length);
                string str = Encoding.UTF8.GetString(data);
                JavaScriptSerializer ser = new JavaScriptSerializer();
                updatedates upd = ser.Deserialize<updatedates>(str);
                removeEntries(upd.id);
                OnAdd(context);
            }
            else if (context.Request.Path.EndsWith("getallsystemprocessddl"))
            {
                context.Response.Write(getallsystemprocessddl());
            }
            else if (context.Request.Path.EndsWith("SpringEntryNames"))
            {
                context.Response.Write(SpringEntryNames());
            }
            else if (context.Request.QueryString.Count == 0)
            {
                OnLoad(context);
            }
            else if (context.Request.QueryString["opt"] == "enabled")
            {
                ToggleEnabled(context.Request["id"]);

                OnLoad(context);
            }
            else if (context.Request.QueryString["action"] == "add")
            {
                OnAdd(context);
            }
            
            else if (context.Request.QueryString["action"] == "publish")
            {
                OnPublish(context);
            }
            else if (!string.IsNullOrEmpty(context.Request.QueryString["script"]))
            {
                string result = string.Empty;
                var assembly = Assembly.GetExecutingAssembly();
                string filename = context.Request.QueryString["script"];
                var resourcename = "ControlPanel.Core." + filename;
                using (Stream stream = assembly.GetManifestResourceStream(resourcename))
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
                context.Response.ContentType = "application/javascript";
                context.Response.Write(result);

            }
            else if (context.Request.QueryString["opt"] == "cont")
            {
                ToggleContinuous(context.Request["id"]);
                OnLoad(context);
            }
          
        }

        public void GetAllSystemProcess()
        {  
            CPServiceClient webClient = new CPServiceClient();
            List<ControlPanel.Core.Entities.SystemProcess> systemProcesses = webClient.GetAllSystemProcess();
            DateTime? lastUpdateDate = null;
            if (systemProcesses != null && systemProcesses.Count > 0)
            {
                systemProcesses = systemProcesses.OrderBy(x => x.DisplayOrder).ToList();
                foreach (ControlPanel.Core.Entities.SystemProcess systemProcess in systemProcesses)
                {
                    systemProcess.SystemProcessThreadList = webClient.GetSystemProcessThreadsByProcessID(systemProcess.SystemProcessId);
                    if (systemProcess.SystemProcessThreadList != null)
                    {
                        systemProcess.SystemProcessThreadList = systemProcess.SystemProcessThreadList.OrderBy(x => x.DisplayOrder.Length).ThenBy(x => x.DisplayOrder).ToList();
                        var currentMax = systemProcess.SystemProcessThreadList.Select(x => x.LastUpdateDate).Max();
                        foreach (var dt in systemProcess.SystemProcessThreadList)
                        {
                            if (dt.LastSuccessfullyExecuted != null)
                                dt.LastExecutedSeconds = (DateTime.UtcNow - dt.LastSuccessfullyExecuted.Value.ToUniversalTime()).ToString(@"hh\:mm\:ss");
                        }
                        if (!lastUpdateDate.HasValue || currentMax > lastUpdateDate)
                            lastUpdateDate = currentMax;
                    }
                }

                jsondata = new JavaScriptSerializer().Serialize(systemProcesses);

                if (lastUpdateDate.HasValue)
                lastupdatedate = lastUpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            }

            //jsondata = new JavaScriptSerializer().Serialize(systemProcesses);

            //lastupdatedate = lastUpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        [DataContract]
        public class UpdateDate
        {

            [IgnoreDataMember]
            public virtual System.DateTime ThreadLastUpdateDate { get; set; }


            [DataMember(EmitDefaultValue = false)]
            public virtual string ThreadLastUpdateDateStr
            {
                get { return ThreadLastUpdateDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); }
                set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { ThreadLastUpdateDate = date.ToUniversalTime(); } }
            }

            [IgnoreDataMember]
            public virtual System.DateTime EventLogLastUpdateDate { get; set; }


            [DataMember(EmitDefaultValue = false)]
            public virtual string EventLogLastUpdateDateStr
            {
                get { return EventLogLastUpdateDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); }
                set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { EventLogLastUpdateDate = date.ToUniversalTime(); } }
            }
        }

        [DataContract]
        public class ControlPanelUpdateModel
        {
            [DataMember(EmitDefaultValue = false)]
            public List<ControlPanel.Core.Entities.SystemProcessThread> Threads { get; set; }
            [DataMember(EmitDefaultValue = false)]
            public List<ControlPanel.Core.Entities.SystemEventLog> EventLogs { get; set; }

            [IgnoreDataMember]
            public virtual System.DateTime? ThreadLastUpdateDate { get; set; }


            [DataMember(EmitDefaultValue = false)]
            public virtual string ThreadLastUpdateDateStr
            {
                get { return (ThreadLastUpdateDate.HasValue) ? ThreadLastUpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : string.Empty; }
                set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { ThreadLastUpdateDate = date.ToUniversalTime(); } }
            }

            [IgnoreDataMember]
            public virtual System.DateTime? EventLogLastUpdateDate { get; set; }


            [DataMember(EmitDefaultValue = false)]
            public virtual string EventLogLastUpdateDateStr
            {
                get { return (EventLogLastUpdateDate.HasValue) ? EventLogLastUpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : String.Empty; }
                set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { EventLogLastUpdateDate = date.ToUniversalTime(); } }
            }
        }
                
        public static string GetUpdatedSystemProcessThreads(string ThreadLastUpdateDateStr, string EventLogLastUpdateDateStr)
        {
            try
            {
                DateTime ThreadLastUpdateDate = DateTime.UtcNow;
                if (DateTime.TryParse(ThreadLastUpdateDateStr, out ThreadLastUpdateDate))
                {
                    ThreadLastUpdateDate = ThreadLastUpdateDate.ToUniversalTime();
                }
                DateTime EventLogLastUpdateDate = DateTime.UtcNow;

                if (DateTime.TryParse(EventLogLastUpdateDateStr, out EventLogLastUpdateDate))
                {
                    EventLogLastUpdateDate = EventLogLastUpdateDate.ToUniversalTime();
                }
               // ControlPanel.Core.ICPWebClient webClient = ControlPanel.Core.IoC.Resolve<ControlPanel.Core.ICPWebClient>("CPWebClient");
                CPServiceClient webClient = new CPServiceClient();
                var threads = webClient.GetSystemProcessThreadsByLastUpdateDate(ThreadLastUpdateDate);
                var eventLogs = webClient.GetEventLogByLastUpdateDate(EventLogLastUpdateDate);
                DateTime maxThreadDate = ThreadLastUpdateDate;
                DateTime maxEventLogDate = EventLogLastUpdateDate;

                if (threads.Data == null)
                    threads.Data = new List<ControlPanel.Core.Entities.SystemProcessThread>();
                else if (threads.Data.Count > 0)
                    maxThreadDate = threads.Data.Select(x => x.LastUpdateDate).Max().Value;

                foreach (var dt in threads.Data)
                {
                    if (dt.LastSuccessfullyExecuted != null)
                        dt.LastExecutedSeconds = (DateTime.UtcNow - dt.LastSuccessfullyExecuted.Value.ToUniversalTime()).ToString(@"hh\:mm\:ss");
                }

                if (eventLogs == null) eventLogs = new List<ControlPanel.Core.Entities.SystemEventLog>();
                else if (eventLogs.Count > 0)
                    maxEventLogDate = eventLogs.Select(x => x.DateOccured).Max();

                if (threads.Data != null && threads.Data.Count > 0)

                    return JSONhelper.GetString(new ControlPanelUpdateModel() { Threads = threads.Data, ThreadLastUpdateDate = maxThreadDate, EventLogs = eventLogs, EventLogLastUpdateDate = maxEventLogDate });
                else return JSONhelper.GetString(new ControlPanelUpdateModel() { ThreadLastUpdateDate = ThreadLastUpdateDate, EventLogLastUpdateDate = EventLogLastUpdateDate });
            }
            catch (Exception exp) { return exp.Message; }
        }

        public void ToggleEnabled(string id)
        {
            int systemProcessThreadID = 0;
            int.TryParse(id, out systemProcessThreadID);
            if (systemProcessThreadID > 0)
            {
                //ControlPanel.Core.ICPWebClient webClient = ControlPanel.Core.IoC.Resolve<ControlPanel.Core.ICPWebClient>("CPWebClient");
                CPServiceClient webClient = new CPServiceClient();
                webClient.ToggleSystemProcessThreadEnabled(systemProcessThreadID);
            }
            GetAllSystemProcess();
            var u = HttpContext.Current.Request.Url.AbsoluteUri;
            var url =  HttpContext.Current.Request.Url.Query;
            HttpContext.Current.Response.Redirect(u.Replace(url,""));
            
        }

        public void ToggleContinuous(string id)
        {
            int systemProcessThreadID = 0;
            int.TryParse(id, out systemProcessThreadID);
            if (systemProcessThreadID > 0)
            {
                //ControlPanel.Core.ICPWebClient webClient = ControlPanel.Core.IoC.Resolve<ControlPanel.Core.ICPWebClient>("CPWebClient");
                CPServiceClient webClient = new CPServiceClient();
                webClient.ToggleSystemProcessThreadContinuous(systemProcessThreadID);
            }
            GetAllSystemProcess();
        }

        public class updatedates
        {
            public string ThreadLastUpdateDateStr { get; set; }
            public string EventLogLastUpdateDateStr { get; set; }
            public string id { get; set; }
        }

        public void OnLoad(HttpContext context)
        {
            string path = binPath.Replace("\\bin", "");

            string text = string.Empty;
            var assembly = Assembly.GetExecutingAssembly();
            string filename = "ControlPanelHtml.txt";
            var resourcename = "ControlPanel.Core." + filename;
            using (Stream stream = assembly.GetManifestResourceStream(resourcename))
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
           
            string springfilepath = binPath + "\\" + "ControlPanelSpring.cfg.xml";

            cpm.WriteFile(binPath, springfilepath);

            ConfigurationManager.AppSettings["SpringFilePath"] = binPath + "\\" +"ControlPanelSpring.cfg.xml";

            GetAllSystemProcess();

            StringBuilder sb = new StringBuilder(text);
            sb.Replace("<jsondata>", jsondata);
            sb.Replace("<threadLastUpdateDate>", lastupdatedate);
            sb.Replace("<eventLoglastUpdateDate>", lastupdatedate);
            sb.Replace("<currentpath>", context.Request.Path);

            context.Response.ContentType = "text/Html";
            context.Response.Write(sb);
        }

        //public void MergeDll(string resourceName)
        //{
        //    AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        //    {       
        //        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ControlPnel.web."+resourceName))
        //        {
        //            Byte[] assemblyData = new Byte[stream.Length];
        //            stream.Read(assemblyData, 0, assemblyData.Length);
        //            return Assembly.Load(assemblyData);
        //        }
        //    };
        //}

        private bool IsNameSpaceAlreadyAdded(string nameSpace)
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                  from type in assembly.GetTypes()
                                  where type.Namespace == nameSpace
                                  select type).Any();
        }

        public void OnAdd(HttpContext context)
        {
            string path = binPath.Replace("\\bin", "");
            string text = string.Empty;
            var assembly = Assembly.GetExecutingAssembly();
            string filename = "ControlPanelAdd.txt";
            var resourcename = "ControlPanel.Core." + filename;
            using (Stream stream = assembly.GetManifestResourceStream(resourcename))
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
           
            GetAllSystemProcess();

            StringBuilder sb = new StringBuilder(text);
            sb.Replace("<jsondata>", jsondata);
            sb.Replace("<threadLastUpdateDate>", lastupdatedate);
            sb.Replace("<eventLoglastUpdateDate>", lastupdatedate);
            sb.Replace("<currentpath>", context.Request.Path);
            context.Response.ContentType = "text/Html";
            context.Response.Write(sb);
        }
               
        public static void getdata(SystemProcessThreadModel obj)
        {
            
            ControlPanel.Core.Entities.SystemProcessThread SystemProcessThreadEntity = new ControlPanel.Core.Entities.SystemProcessThread();
            var continuouschk = obj.Continuous;
            var isnewsysprocess = obj.SystemProcessId;

            if (string.IsNullOrEmpty(isnewsysprocess) || isnewsysprocess == "-1")
            {
                var sysId = addsystemprocess(obj);
                SystemProcessThreadEntity.SystemProcessId = sysId;
            }
            else
            {
                SystemProcessThreadEntity.SystemProcessId = Convert.ToInt16(obj.SystemProcessId);
            }
            
            TimeSpan scheduleTime;
            SystemProcessThreadEntity.Name = obj.Name;
            SystemProcessThreadEntity.Description = obj.Description;
            SystemProcessThreadEntity.SpringEntryName = obj.SpringEntryName;
            TimeSpan.TryParse(obj.ScheduledTime, out scheduleTime);

            if (scheduleTime.Hours != 00)
            SystemProcessThreadEntity.ScheduledTime = scheduleTime;

            SystemProcessThreadEntity.Enabled = false;

            if (continuouschk == "on")
            SystemProcessThreadEntity.Continuous = true;

            else
            SystemProcessThreadEntity.Continuous = false;


            SystemProcessThreadEntity.SleepTime = 10;
            SystemProcessThreadEntity.AutoStart = false;

            if (obj.ContinuousDelay != null && !string.IsNullOrEmpty(obj.ContinuousDelay))
            {
                SystemProcessThreadEntity.ContinuousDelay = Convert.ToInt16(obj.ContinuousDelay);
            }
            else
            {
                SystemProcessThreadEntity.ContinuousDelay = 0;
            }

            SystemProcessThreadEntity.IsDeleted = false;
            SystemProcessThreadEntity.DisplayOrder = "0";
            SystemProcessThreadEntity.Argument = obj.Argument;


            ControlPanel.Repository.SystemProcessThreadRepository sptrepo = new ControlPanel.Repository.SystemProcessThreadRepository();

            var threadId = obj.HiddenField;

            if (threadId == null || string.IsNullOrEmpty(threadId))
            {
                sptrepo.InsertSystemProcessThread(SystemProcessThreadEntity);
            }
            else
            {
                SystemProcessThreadEntity.SystemProcessThreadId = Convert.ToInt16(threadId);
                sptrepo.UpdateSystemProcessThread(SystemProcessThreadEntity);
            }

        }
                
        public static string getallsystemprocessddl()
        {
            ControlPanel.Repository.SystemProcessRepository sprepo = new ControlPanel.Repository.SystemProcessRepository();
            List<ControlPanel.Core.Entities.SystemProcess> splist = sprepo.GetAllSystemProcess();
            var jsondatalist = new JavaScriptSerializer().Serialize(splist);
            return jsondatalist;
        }
                
        public static void removeEntries(string id)
        {
            ControlPanel.Repository.SystemProcessThreadRepository sptr = new ControlPanel.Repository.SystemProcessThreadRepository();
            var thread = sptr.GetSystemProcessThread(Convert.ToInt16(id));
            bool result =  sptr.DeleteSystemProcessThread(Convert.ToInt16(id));
            if (result)
            {   
                var threadsofprocess = sptr.GetSystemProcessThreadBySystemProcessId(thread.SystemProcessId);

                if (threadsofprocess == null)
                {
                    ControlPanel.Repository.SystemProcessRepository spr = new Repository.SystemProcessRepository();
                    spr.DeleteSystemProcess(thread.SystemProcessId);
                }
                
            }
            
        }

        public static int addsystemprocess(SystemProcessThreadModel obj)
        {
            ControlPanel.Core.Entities.SystemProcess sysprocesentity = new ControlPanel.Core.Entities.SystemProcess();
            ControlPanel.Repository.SystemProcessRepository sprepo = new ControlPanel.Repository.SystemProcessRepository();
            sysprocesentity.Name = obj.SystemProcessName;
            sysprocesentity.Description = obj.SystemProcessDesc;
            sysprocesentity.Enabled = true;
            sysprocesentity.DisplayOrder = 10;
            sysprocesentity.Ip = obj.SystemProcessIp;
            if (obj.SystemProcessPort != null && obj.SystemProcessPort != "")
            {
                sysprocesentity.Port = Convert.ToInt16(obj.SystemProcessPort);
            }
            var entry = sprepo.InsertSystemProcess(sysprocesentity);
            return entry.SystemProcessId;

        }

        public class SystemProcessThreadModel
        {

            public string SystemProcessId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string SpringEntryName { get; set; }
            public string ScheduledTime { get; set; }
            public string Continuous { get; set; }
            public string ContinuousDelay { get; set; }
            public string Argument { get; set; }
            public string HiddenField { get; set;}
            public string SystemProcessName { get; set; }
            public string SystemProcessDesc { get; set; }
            public string SystemProcessIp { get; set; }
            public string SystemProcessPort { get; set; }

        }

        public void OnPublish(HttpContext context)
        {   
            string text = string.Empty;
            var assembly = Assembly.GetExecutingAssembly();
            string filename = "ControlPanelPublish.txt";
            var resourcename = "ControlPanel.Core." + filename;
            using (Stream stream = assembly.GetManifestResourceStream(resourcename))
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            GetAllSystemProcess();

            StringBuilder sb = new StringBuilder(text);
            sb.Replace("<jsondata>", jsondata);
            sb.Replace("<threadLastUpdateDate>", lastupdatedate);
            sb.Replace("<eventLoglastUpdateDate>", lastupdatedate);
            sb.Replace("<currentpath>", context.Request.Path);
            context.Response.ContentType = "text/Html";
            context.Response.Write(sb);

        }

        public static string GetAllVersions()
        {
            string path = ConfigurationManager.AppSettings["FilePath"];
            if (path != null)
            {
                string[] Folders = Directory.GetDirectories(path);
                List<string> foldername = new List<string>();
                foreach (var folder in Folders)
                {
                    DirectoryInfo di = new DirectoryInfo(folder);

                    if (di.Name.Contains("Deployment"))
                        foldername.Add(di.Name);
                }
                var jsondatalist = new JavaScriptSerializer().Serialize(foldername);

                return jsondatalist;
            }
            return "Fail";
        }

        public static void CopyAllFiles()
        {
            int maxint;
            string[] folders = Directory.GetDirectories(ConfigurationManager.AppSettings["FilePath"]);
            List<int> tts = new List<int>();
            foreach (var folder in folders)
            {
                DirectoryInfo di = new DirectoryInfo(folder);
                var foldername = di.Name;

                if (foldername.Contains("Deployment-"))
                {
                    int fnum = Convert.ToInt16(foldername.Replace("Deployment-", ""));
                    tts.Add(fnum);
                }
            }

            if (tts.Count > 0)
                maxint = tts.Max();

            else
                maxint = 0;

            string SourcePath = ConfigurationManager.AppSettings["FilePath"];
            string fldname = "Deployment-"+ (maxint + 1);
            Directory.CreateDirectory(ConfigurationManager.AppSettings["FilePath"] + fldname);
            
            string DestinationPath = ConfigurationManager.AppSettings["FilePath"] + fldname +"\\";

            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
            SearchOption.AllDirectories))
            {
                DirectoryInfo di = new DirectoryInfo(dirPath);
                string name = di.Name;
                if (!name.Contains("Deployment") && !dirPath.Contains("Deployment"))
                {
                    Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
                }
            }
           
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
            {
                DirectoryInfo di = new DirectoryInfo(newPath);
                var pname = di.Parent;
                if (!pname.Name.Contains("Deployment"))
                {
                    try
                    {
                        File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

        }

        public static string SpringEntryNames()
        {   
            var type = typeof(ISystemProcessThread);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            List<springtype> Names = new List<springtype>();
            foreach (var name in types)
            {
                if (name.Name != "ISystemProcessThread")
                Names.Add(new springtype { Name = name.Name, Type = name.FullName, Namespace = name.Namespace });
            }
            var jsondatalist = new JavaScriptSerializer().Serialize(Names);
            return jsondatalist;
        }

        public class springtype
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Namespace { get; set; }
        }
    }
}