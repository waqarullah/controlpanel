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
using ControlPanel.Repository;
using ControlPanel.Core.DataTransfer;

namespace ControlPanel.Core
{
    /// <summary>
    /// Summary description for ControlPanelHandler
    /// </summary>
    public class PerformanceStatisticHandler : IHttpHandler
    {
        string jsondata = string.Empty;
        string jsonNetworkdata = string.Empty;
        string jsonDrivesdata = string.Empty;
        string lastupdatedate = string.Empty;
        string NetworkLastUpdateDate = string.Empty;
        string DrivesLastUpdateDate = string.Empty;
        int i = 0;
        string binPath = Path.GetDirectoryName((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath);

        ControlPanelMethods cpm = new ControlPanelMethods();

        public void ProcessRequest(HttpContext context)
        {

            if (context.Request.Path.EndsWith("GetUpdatedSystemPerformanceStatistic"))
            {
                try
                {
                    byte[] data = new byte[context.Request.InputStream.Length];
                    context.Request.InputStream.Read(data, 0, data.Length);
                    string str = Encoding.UTF8.GetString(data);
                    JavaScriptSerializer ser = new JavaScriptSerializer();

                    updatedates upd = ser.Deserialize<updatedates>(str);
                    context.Response.ContentType = "application/javascript";
                    context.Response.Write(GetUpdatedSystemPerformanceStatistic(upd.ThreadLastUpdateDateStr));
                }
                catch (Exception ex)
                {
                    var a = ex;
                }

            }

            else if (context.Request.Path.EndsWith("GetUpdatedSystemPerformanceDrivesStatistic"))
            {
                byte[] data = new byte[context.Request.InputStream.Length];
                context.Request.InputStream.Read(data, 0, data.Length);
                string str = Encoding.UTF8.GetString(data);
                JavaScriptSerializer ser = new JavaScriptSerializer();

                updatedates upd = ser.Deserialize<updatedates>(str);
                context.Response.ContentType = "application/javascript";
                context.Response.Write(GetUpdatedSystemPerformanceDrivesStatistic(upd.ThreadLastUpdateDateStr));

            }

            else if (context.Request.Path.EndsWith("GetUpdatedSystemPerformanceNetworkStatistic"))
            {
                byte[] data = new byte[context.Request.InputStream.Length];
                context.Request.InputStream.Read(data, 0, data.Length);
                string str = Encoding.UTF8.GetString(data);
                JavaScriptSerializer ser = new JavaScriptSerializer();

                updatedates upd = ser.Deserialize<updatedates>(str);
                context.Response.ContentType = "application/javascript";
                context.Response.Write(GetUpdatedSystemPerformanceNetworkStatistic(upd.ThreadLastUpdateDateStr));

            }

                
            else if (context.Request.QueryString.Count == 0)
            {
                if (i == 0)
                { OnLoad(context);}
                i = i + 1;
                
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
        }
        public double GetSeconds(Int32 hours, Int32 minutes, Int32 seconds)
        {
            return ((hours * 60) + minutes) * 60 + seconds;
        }

        public void GetAllSystemPerformanceStatistic()
        {
            List<ControlPanel.Core.Entities.PerformanceStatistic> systemProcesses = new List<ControlPanel.Core.Entities.PerformanceStatistic>();
            ControlPanel.Repository.PerformanceStatisticRepository prepository = new Repository.PerformanceStatisticRepository();
            DateTime dtime = prepository.GetMaxCreationDate();

            double TotalSeconds = GetSeconds(dtime.Hour,dtime.Minute,dtime.Second);
            TotalSeconds = TotalSeconds - 300;

            TimeSpan t = TimeSpan.FromSeconds(TotalSeconds);

            string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds
                           );
            //DateTime date = DateTime.UtcNow;
            //if (!DateTime.TryParse(answer, out date))
            //    date = DateTime.UtcNow;


            systemProcesses = prepository.GetAllSystemPerformanceByCreationDate(Convert.ToDateTime(answer));
            DateTime? lastUpdateDate = null;
            if (systemProcesses != null && systemProcesses.Count > 0)
            {
                systemProcesses = systemProcesses.OrderBy(x => x.CreationDate).ToList();
                lastUpdateDate = systemProcesses[0].CreationDate;
                lastupdatedate = lastUpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            }
           
            jsondata = new JavaScriptSerializer().Serialize(systemProcesses);
        }

        public void GetAllSystemPerformanceNetworkStatistic()
        {
            List<ControlPanel.Core.Entities.NetworkStatistic> systemProcesses = new List<ControlPanel.Core.Entities.NetworkStatistic>();
            ControlPanel.Repository.NetworkStatisticRepository prepository = new Repository.NetworkStatisticRepository();
            DateTime dtime = DateTime.UtcNow;

            double TotalSeconds = GetSeconds(dtime.Hour, dtime.Minute, dtime.Second);
            TotalSeconds = TotalSeconds - 300;

            TimeSpan t = TimeSpan.FromSeconds(TotalSeconds);

            string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds
                           );

            systemProcesses = prepository.GetAllSystemNetworkPerformanceByCreationDate(Convert.ToDateTime(answer));
            DateTime? lastUpdateDate = null;
            if (systemProcesses != null && systemProcesses.Count > 0)
            {
                systemProcesses = systemProcesses.OrderBy(x => x.CreationDate).ToList();
                lastUpdateDate = systemProcesses[0].CreationDate;
                NetworkLastUpdateDate = lastUpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

                for (int i = 0; i < systemProcesses.Count; i++)
                {
                    if (systemProcesses[i].TotalUsage == 0 || systemProcesses[i].TotalUsage == 0.0)
                    {
                        systemProcesses[i].TotalUsage = 1;
                    }
                }
            }

           

            jsonNetworkdata = new JavaScriptSerializer().Serialize(systemProcesses);
        }

        public void GetAllSystemPerformanceDrivesStatistic()
        {
            List<ControlPanel.Core.Entities.DriveStatistic> systemProcesses = new List<ControlPanel.Core.Entities.DriveStatistic>();
            ControlPanel.Repository.DriveStatisticRepository prepository = new Repository.DriveStatisticRepository();
            DateTime dtime = prepository.GetMaxCreationDate();

            double TotalSeconds = GetSeconds(dtime.Hour, dtime.Minute, dtime.Second);
            TotalSeconds = TotalSeconds - 300;

            TimeSpan t = TimeSpan.FromSeconds(TotalSeconds);

            string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds
                           );

            systemProcesses = prepository.GetAllSystemDrivesPerformanceByCreationDate(Convert.ToDateTime(answer));
            DateTime? lastUpdateDate = null;
            if (systemProcesses != null && systemProcesses.Count > 0)
            {
                systemProcesses = systemProcesses.OrderBy(x => x.CreationDate).ToList();
                lastUpdateDate = systemProcesses[0].CreationDate;
                DrivesLastUpdateDate = lastUpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            }



            jsonDrivesdata = new JavaScriptSerializer().Serialize(systemProcesses);
        }

        public static string GetUpdatedSystemPerformanceStatistic(string ThreadLastUpdateDateStr)
        {


            DataTransfer<List<ControlPanel.Core.Entities.PerformanceStatistic>> dt = new DataTransfer<List<ControlPanel.Core.Entities.PerformanceStatistic>>();
            try
            {
                PerformanceStatisticRepository repo = new PerformanceStatisticRepository();
                DateTime date = DateTime.UtcNow;
                if (!DateTime.TryParse(ThreadLastUpdateDateStr, out date))
                    date = DateTime.UtcNow;
                List<ControlPanel.Core.Entities.PerformanceStatistic> stats = repo.GetPerformanceStatisticThreadByLastUpdatedDate(date.ToUniversalTime());
                if (stats != null) stats = stats.OrderBy(x => x.CreationDate).ToList();
                dt.Data = stats;
            }
            catch (Exception exp)
            {
                dt.IsSuccess = false;
                dt.Errors = new string[] { exp.Message };
            }
            var a = JSONhelper.GetString(dt);
            return JSONhelper.GetString(dt);
        }
        public static string GetUpdatedSystemPerformanceDrivesStatistic(string ThreadLastUpdateDateStr)
        {
            DataTransfer<List<ControlPanel.Core.Entities.DriveStatistic>> dt = new DataTransfer<List<ControlPanel.Core.Entities.DriveStatistic>>();
            try
            {
                DriveStatisticRepository repo = new DriveStatisticRepository();
                DateTime date = DateTime.UtcNow;
                if (!DateTime.TryParse(ThreadLastUpdateDateStr, out date))
                    date = DateTime.UtcNow;
                List<ControlPanel.Core.Entities.DriveStatistic> stats = repo.GetPerformanceDrivesStatisticThreadByLastUpdatedDate(date.ToUniversalTime());
                if (stats != null) stats = stats.OrderBy(x => x.CreationDate).ToList();
                dt.Data = stats;
            }
            catch (Exception exp)
            {
                dt.IsSuccess = false;
                dt.Errors = new string[] { exp.Message };
            }

            return JSONhelper.GetString(dt);
        }
        
        public static string GetUpdatedSystemPerformanceNetworkStatistic(string ThreadLastUpdateDateStr)
        {


            DataTransfer<List<ControlPanel.Core.Entities.NetworkStatistic>> dt = new DataTransfer<List<ControlPanel.Core.Entities.NetworkStatistic>>();
            try
            {
                NetworkStatisticRepository repo = new NetworkStatisticRepository();
                DateTime date = DateTime.UtcNow;
                if (!DateTime.TryParse(ThreadLastUpdateDateStr, out date))
                    date = DateTime.UtcNow;
                List<ControlPanel.Core.Entities.NetworkStatistic> stats = repo.GetNetworkPerformanceStatisticThreadByLastUpdatedDate(date.ToUniversalTime());


                for(int i=0;i<stats.Count;i++)
                {
                    if (stats[i].TotalUsage == 0 || stats[i].TotalUsage == 0.0)
                    {
                        stats[i].TotalUsage = 1; 
                    }
                }
                if (stats != null) stats = stats.OrderBy(x => x.CreationDate).ToList();
                dt.Data = stats;
            }
            catch (Exception exp)
            {
                dt.IsSuccess = false;
                dt.Errors = new string[] { exp.Message };
            }

            return JSONhelper.GetString(dt);
        }

        public class updatedates
        {
            public string ThreadLastUpdateDateStr { get; set; }
            public string id { get; set; }
        }

        public void OnLoad(HttpContext context)
        {
            string path = binPath.Replace("\\bin", "");

            string text = string.Empty;
            var assembly = Assembly.GetExecutingAssembly();
            string filename = "PerformanceStatisticHtml.txt";
            var resourcename = "ControlPanel.Core." + filename;
            using (Stream stream = assembly.GetManifestResourceStream(resourcename))
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            ConfigurationManager.AppSettings["SpringFilePath"] = binPath + "\\" + "ControlPanelSpring.cfg.xml";

            GetAllSystemPerformanceStatistic();
            GetAllSystemPerformanceNetworkStatistic();
            GetAllSystemPerformanceDrivesStatistic();

            StringBuilder sb = new StringBuilder(text);
            sb.Replace("<jsondata>", jsondata);
            sb.Replace("<jsonNetworkData>", jsonNetworkdata);
            sb.Replace("<jsondrivesData>", jsonDrivesdata);
            sb.Replace("<threadLastUpdateDate>", lastupdatedate);
            sb.Replace("<eventLoglastUpdateDate>", lastupdatedate);
            sb.Replace("<threadNetworkLastUpdateDate>", NetworkLastUpdateDate);
            sb.Replace("<threadDrivesLastUpdateDate>", DrivesLastUpdateDate);
            sb.Replace("<currentpath>", context.Request.Path);

            
            context.Response.ContentType = "text/Html";
            context.Response.Write(sb);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}