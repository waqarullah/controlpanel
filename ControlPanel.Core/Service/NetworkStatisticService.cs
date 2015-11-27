using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using ControlPanel.Core.Entities;
using ControlPanel.Core.DataInterfaces;
using ControlPanel.Core.IService;
using ControlPanel.Core.DataTransfer;
using Validation;
using System.Linq;
using ControlPanel.Repository;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ControlPanel.Core.Service
{
		
	public class NetworkStatisticService 
	{
        public List<NetworkStatistic> GetSystemNetworkStatistic()
        {
           

            NetworkInterface[] networkCards= System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface card in networkCards)
            {
         

                Console.WriteLine("Name:{0}, Status:{1}, Speed:{2}",card.Name,card.OperationalStatus,card.Speed);
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

            double utilization = (8 * (dataSent + dataReceived)) / (bandwidth * numberOfIterations) * 100;
            stats.TotalUsage = utilization;
            stats.InterfaceName = networkCard + " " + ((bandwidth / 1000.0) / 1000.0 / 1000.0).ToString() + "GB";

            //if(networkCard =='')
            //{
            
            //}

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
                else
                {
                    if (card.Name == networkCard)
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
