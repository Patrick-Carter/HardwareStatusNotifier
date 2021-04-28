using HardwareStatusNotifier.Logic.Hardware.CPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareStatusUI
{
    class Program
    {
        static void Main(string[] args)
        {
            CPU cpu = CPU.GetInstance();
            Dictionary<string, float?> dictOfTemps = new Dictionary<string, float?>();

            while (true)
            {
                dictOfTemps = cpu.GetTemerature();
                
                foreach (var temp in dictOfTemps)
                {
                    Console.WriteLine("CPU Temp: " + temp.Value.ToString() + " Celsius");
                }

                Console.ReadKey();
            }
        }
    }
}
