using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareStatusNotifier.Logic.Visitors;
using OpenHardwareMonitor.Hardware;

namespace HardwareStatusNotifier.Logic.Hardware.CPU
{
    public class CPU
    {
        private static CPU cpu;
        private Computer computer;
        private IVisitor hardwareVisitor;

        private CPU(Computer computer = null, IVisitor hardwareVisitor = null)
        {
            this.computer = computer ?? new Computer();
            this.hardwareVisitor = hardwareVisitor ?? new HardwareVisitor();
        }

        public static CPU GetInstance()
        {
            if (cpu == null)
            {
                cpu = new CPU();
            }
            return cpu;
        }

        public Dictionary<string, float?> GetTemerature()
        {
            // create dictionary to return
            Dictionary<string, float?> dictOfTemps = new Dictionary<string, float?>();


            // open access to CPU, enable reading CPU, send in visitor to get info
            computer.Open();
            computer.CPUEnabled = true;
            computer.Accept(hardwareVisitor);

            // Get a list of all CPU
            List<IHardware> hardware = new List<IHardware>();
            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                if (computer.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    hardware.Add(computer.Hardware[i]);
                }
            }

            // Go thorugh each CPU and get the tempature sensor
            List<ISensor> tempSensors = new List<ISensor>();
            foreach (var piece in hardware)
            {
                for (int i = 0; i < piece.Sensors.Length; i++)
                {
                    if (piece.Sensors[i].SensorType == SensorType.Temperature)
                    {
                        tempSensors.Add(piece.Sensors[i]);
                    }
                }
            }

            // for each temp sensor return the tempature
            foreach (var sensor in tempSensors)
            {
                dictOfTemps.Add(sensor.Name, sensor.Value);
            }

            // close computor
            computer.Close();

            return dictOfTemps;
        }
    }
}
