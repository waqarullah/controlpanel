using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlPanel.Core;
using ControlPanel.Core.Enums;
using ControlPanel.Core.IService;
using System.Threading.Tasks;
using ControlPanel.Core.Entities;
using System.Diagnostics;
using System.IO;
using ControlPanel.Core.DataTransfer;
using ControlPanel.Repository;
using System.Net;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using ControlPanel.Service;

namespace ControlPanel.Core
{
    public class PerformanceStatisticThread : ISystemProcessThread
    {
        private string _threadName;
        static Queue<double> cpuReading = new Queue<double>();
        DateTime lastAlertSendDateTime = DateTime.UtcNow.AddMinutes(-10);
        public string ThreadName
        {
            get
            {
                return _threadName;
            }
            set
            {
                _threadName = value;
            }
        }

        private int? _startRange;
        public int? StartRange
        {
            get
            {
                return _startRange;
            }
            set
            {
                _startRange = value;
            }
        }

        private int? _endRange;
        public int? EndRange
        {
            get
            {
                return _endRange;
            }
            set
            {
                _endRange = value;
            }
        }

        private DateTime? _lastSuccessfullyExecuted;
        public DateTime? LastSuccessfullyExecuted
        {
            get
            {
                return _lastSuccessfullyExecuted;
            }
            set
            {
                _lastSuccessfullyExecuted = value;
            }
        }

        private TimeSpan? _scheduleTime;
        public TimeSpan? ScheduledTime
        {
            get
            {
                return _scheduleTime;
            }
            set
            {
                _scheduleTime = value;
            }
        }

        public string Initialize()
        {

            return "Service Initialize " + DateTime.UtcNow.ToString("MMM dd,yyyy hh:mm:ss tt");
        }

        public string Execute(string argument)
        {
            SystemEventLogService logService = new SystemEventLogService();
            PerformanceStatisticRepository pStatisticRepo = new PerformanceStatisticRepository();
            try
            {
                PerformanceStatistic stats = new PerformanceStatistic();
                stats = GetPCPerformance();

                PerformanceStatistic input = new PerformanceStatistic();
                input.CopyFrom(stats);

                if (input != null)
                {
                    pStatisticRepo.InsertPerformanceStatistic(input);
                }

                List<NetworkStatistic> Networkstats = new List<NetworkStatistic>();
                Networkstats = GetSystemNetworkStatistic();

                if (Networkstats.Count > 0 && Networkstats != null)
                {
                    foreach (NetworkStatistic nstatistic in Networkstats)
                    {
                        if (nstatistic.IpAddress != null)
                        {
                            //nstatistic.IpAddress = Convert.ToString(0);
                            InsertNetworkStatistic(nstatistic);
                        }
                    }
                }

                return "Success";
            }
            catch (Exception exp)
            {
                logService.InsertSystemEventLog(string.Format("Error in PerformanceStatisticThread: {0}", exp.Message), exp.StackTrace, EventCodes.Error);
                return "Error";
            }
        }

        public PerformanceStatistic GetPCPerformance()
        {
            PerformanceStatistic stats = new PerformanceStatistic();
            PerformanceCounter cpuCounter;
            PerformanceCounter ramCounter;
            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            Int64 phav = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
            Int64 tot = PerformanceInfo.GetTotalMemoryInMiB();
            stats.Memory = Math.Round((double)(100 - (((decimal)phav / (decimal)tot) * 100)), 2);
            stats.Cpu = cpuCounter.NextValue();
            Thread.Sleep(1000);
            stats.Cpu = cpuCounter.NextValue();
            stats.CreationDate = DateTime.UtcNow;

            stats.MachineName = System.Environment.MachineName.ToString();

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            DriveStatisticRepository driveRepo = new DriveStatisticRepository();
            if (allDrives.Count() > 0)
            {
                stats.DriveSpaceAvailable = 0;
                stats.DriveTotalSpace = 0;
                for (int i = 0; i < allDrives.Count(); i++)
                {
                    try
                    {
                        DriveStatistic dS = new DriveStatistic();
                        dS.CreationDate = stats.CreationDate;
                        dS.DriveName = allDrives[i].Name;
                        dS.MachineName = stats.MachineName + ":" + allDrives[i].Name;
                        dS.DriveSpaceAvailable = Math.Round(((allDrives[i].TotalFreeSpace / 1024.0) / 1024.0) / 1024.0, 2);
                        dS.DriveTotalSpace = Math.Round(((allDrives[i].TotalSize / 1024.0) / 1024.0) / 1024.0, 2);
                        string hostNames = Dns.GetHostName();
                        string myIPs = Dns.GetHostByName(hostNames).AddressList[0].ToString();
                        dS.IpAddress = myIPs + ":" + allDrives[i].Name;
                        driveRepo.InsertDriveStatistic(dS);
                        stats.DriveSpaceAvailable += dS.DriveSpaceAvailable;
                        stats.DriveTotalSpace += dS.DriveTotalSpace;
                    }
                    catch (Exception exp)
                    { }
                }
            }

            string hostName = Dns.GetHostName();
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

            stats.IpAddress = myIP;

            return stats;
        }

        double GetAverageReading(double newReading)
        {
            cpuReading.Enqueue(newReading);
            if (cpuReading.Count > 5)
                cpuReading.Dequeue();
            return cpuReading.Sum() / cpuReading.Count;
        }

        Queue<double> averageReadings = new Queue<double>();

        private void ReportIfThreshholdLevelReached(double cpuReading, double ramReading, double diskReading)
        {
            averageReadings.Enqueue(cpuReading);
            if (averageReadings.Count > 5)
                averageReadings.Dequeue();
            double reading = averageReadings.Sum() / averageReadings.Count;
            if (reading > Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["CPUThreshhold"]) || ramReading > Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["RAMThreshhold"]) || diskReading > Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["DiskThreshhold"]))
            {
                if (DateTime.UtcNow.Subtract(lastAlertSendDateTime).TotalMinutes > 10)
                {
                    try
                    {
                        // EmailHelper.SendEmail(System.Environment.MachineName.ToString(), reading, ramReading, diskReading);
                    }
                    catch { }

                    try
                    {
                        // SendSMS(string.Format("LivePerson: Server Threshhold Levels Reached. (CPU->{0}, Memory->{1}, Disk->{2})", reading.ToString("##.##"), ramReading.ToString("##.##"), diskReading.ToString("##.##")));
                    }
                    catch { }
                    lastAlertSendDateTime = DateTime.UtcNow;
                }
            }
        }

        public List<NetworkStatistic> GetSystemNetworkStatistic()
        {
            //NetworkInterface[] networkCards = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            //foreach (NetworkInterface card in networkCards)
            //{
            //    Console.WriteLine("Name:{0}, Status:{1}, Speed:{2}", card.Name, card.OperationalStatus, card.Speed);
            //}

            List<NetworkStatistic> statistics = new List<NetworkStatistic>();
            //PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
            //String[] instancename = category.GetInstanceNames();

            statistics = getNetworkUtilization();
            // Console.WriteLine("Interface:{0}, Usage:{1}%, Send:{2} Bytes, Recieved:{3} Bytes", stats.InterfaceName, stats.TotalUsage, stats.Upload, stats.Download);
            return statistics;
        }

        public void InsertNetworkStatistic(NetworkStatistic stats)
        {
            NetworkStatisticRepository repo = new NetworkStatisticRepository();
            repo.InsertNetworkStatistic(stats);
        }

        public List<NetworkStatistic> getNetworkUtilization()
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
            String[] instancename = category.GetInstanceNames();
            List<NetworkStatistic> Lststats = new List<NetworkStatistic>();

            List<string> lst = new List<string>();
            NetworkInterface[] networkCards = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface card in networkCards)
            {
                var ipProps = card.GetIPProperties();
                if (card.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Ethernet)
                {
                    foreach (var ip in ipProps.UnicastAddresses)
                    {
                        if ((ip.Address.AddressFamily == AddressFamily.InterNetwork))
                        {
                            var Name = card.Name.ToString();
                            if (!lst.Any(x => x == Name))
                            {
                                NetworkStatistic stats = new NetworkStatistic();
                                if (card.Speed > 0)
                                {
                                    var beforedownload = Math.Round(card.GetIPv4Statistics().BytesReceived / 1000.0, 3);
                                    var beforeupload = Math.Round(card.GetIPv4Statistics().BytesSent / 1000.0, 3);
                                    Thread.Sleep(1000);
                                    var afterdownload = Math.Round(card.GetIPv4Statistics().BytesReceived / 1000.0, 3);
                                    var afterupload = Math.Round(card.GetIPv4Statistics().BytesSent / 1000.0, 3);
                                    stats.Download = afterdownload - beforedownload;
                                    stats.Upload = afterupload - beforeupload;
                                    stats.CreationDate = DateTime.UtcNow;
                                    stats.InterfaceName = card.Name;
                                    string hostNames = Dns.GetHostName();
                                    string myIPs = Dns.GetHostByName(hostNames).AddressList[0].ToString();
                                    stats.ServerIp = System.Environment.MachineName.ToString();
                                    float utilization = (float)((stats.Upload + stats.Download) / ((card.Speed / 8) / 1000.0)) * 100;
                                    stats.TotalUsage = Math.Round(utilization, 3);
                                    stats.ServerName = Name;
                                    stats.IpAddress = ip.Address.ToString();
                                    Console.Out.WriteLine("\n" + ip.Address.ToString() + "|  " + card.Description.ToString() + "\n" + "OpeartionalStatus:" + card.OperationalStatus + "\n" + "speed:" + card.Speed + "\n" + "Name" + card.Name + "\n" + "NetworkInterfaceType:" + card.NetworkInterfaceType);
                                    Console.WriteLine("Download:{0}, Upload:{1}, TotalUsage{2}", stats.Download, stats.Upload, stats.TotalUsage);
                                    lst.Add(Name);
                                    Lststats.Add(stats);
                                }
                            }
                            //Console.Out.WriteLine(ip.Address.ToString() + "|" + card.Description.ToString());
                            //stats.IpAddress = ip.Address.ToString();
                        }

                    }
                }

            }
            return Lststats;
        }
    }
}
