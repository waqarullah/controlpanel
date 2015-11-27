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

namespace MMS.ProcessThreads
{
    public class NetworkStatisticThread : ISystemProcessThread
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
            NetworkStatisticRepository nStatisticRepo = new NetworkStatisticRepository(); 
            try
            {
                List<NetworkStatistic> stats = new List<NetworkStatistic>();
                stats = GetSystemNetworkStatistic();

                if (stats.Count > 0 && stats != null)
                {
                    foreach (NetworkStatistic nstatistic in stats)
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
                return "Error";
            }
        }

        public List<NetworkStatistic> GetSystemNetworkStatistic()
        {


            NetworkInterface[] networkCards = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface card in networkCards)
            {
                Console.WriteLine("Name:{0}, Status:{1}, Speed:{2}", card.Name, card.OperationalStatus, card.Speed);
            }

            List<NetworkStatistic> statistics = new List<NetworkStatistic>();
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
            String[] instancename = category.GetInstanceNames();
            foreach (string name in instancename)
            {
                var stats = getNetworkUtilization(name);
                Console.WriteLine("Interface:{0}, Usage:{1}%, Send:{2} Bytes, Recieved:{3} Bytes", stats.InterfaceName, stats.TotalUsage, stats.Upload, stats.Download);
                statistics.Add(stats);
            }
            return statistics;
        }

        public void InsertNetworkStatistic(NetworkStatistic stats)
        {
            NetworkStatisticRepository repo = new NetworkStatisticRepository();
            repo.InsertNetworkStatistic(stats);
        }

        private NetworkStatistic getNetworkUtilization(string networkCard)
        {

            NetworkStatistic stats = new NetworkStatistic();
            const int numberOfIterations = 10;

            PerformanceCounter bandwidthCounter = new PerformanceCounter("Network Interface", "Current Bandwidth", networkCard);
            float bandwidth = bandwidthCounter.NextValue();//valor fixo 10Mb/100Mn/

            PerformanceCounter dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", networkCard);

            PerformanceCounter dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkCard);

            float sendSum = 0;
            float receiveSum = 0;

            for (int index = 0; index < numberOfIterations; index++)
            {
                sendSum += dataSentCounter.NextValue();
                receiveSum += dataReceivedCounter.NextValue();
            }
            float dataSent = sendSum;
            float dataReceived = receiveSum;
            stats.Download = Math.Round(dataReceived / 1000.0, 3);
            stats.Upload = Math.Round(dataSent / 1000.0, 3);
            stats.CreationDate = DateTime.UtcNow;

            double utilization = (8 * (dataSent + dataReceived)) / (bandwidth * numberOfIterations) * 100;
            stats.TotalUsage = utilization;
            stats.InterfaceName = networkCard + " " + ((bandwidth / 1000.0) / 1000.0 / 1000.0).ToString() + "GB";

            NetworkInterface[] networkCards = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface card in networkCards)
            {
                var ipProps = card.GetIPProperties();
                if (card.Name == "Local Area Connection")
                {
                    string networkIp = networkCard;
                    networkIp = networkIp.Replace("[", "(");
                    networkIp = networkIp.Replace("]", ")");


                    if (card.Description == networkIp)
                    {
                        foreach (var ip in ipProps.UnicastAddresses)
                        {
                            if ((ip.Address.AddressFamily == AddressFamily.InterNetwork))
                            {
                                Console.Out.WriteLine(ip.Address.ToString() + "|" + card.Description.ToString());
                                stats.IpAddress = ip.Address.ToString();
                            }
                        }
                    }

                }
            }
            return stats;
        }

    }
}
