using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ControlPanel.Core.PerformanceStatisticThread thread = new ControlPanel.Core.PerformanceStatisticThread();
            thread.GetSystemNetworkStatistic();
            Console.Read();
        }
    }
}
