using NeoArduinoSerial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace PixelMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Task.Run(() => { this.Begin(); });
        }

        private void Begin()
        {
            var startColor = new NeoColor { Red = 0, Blue = 0, Green = 10 };
            var endColor = new NeoColor { Red = 10, Blue = 0, Green = 0 };

            var container = new NeoContainer("COM1", 57600);
            var strip1 = new NeoSection(0, 64);
            // var strip2 = new NeoSection(32, 32);

            strip1.SetColor(startColor, endColor);
            // strip2.SetColor(endColor, startColor);

            container.Sections.Add(strip1);
            // container.Sections.Add(strip2);

            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            var ni = NetworkInterface.GetAllNetworkInterfaces().Last();

            while (true)
            {

                var speed = ni.Speed;

                strip1.Percentage = (int)cpuCounter.NextValue();
                // strip2.Percentage = Math.Min(100, (int) speed);

                Console.WriteLine(strip1.Percentage);

                try
                {
                    container.SendCommand();
                }
                catch (Exception ex)
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                }

                Thread.Sleep(200);
            }
        }

        private void Begin1()
        {
            SerialPort port = new SerialPort("COM1", 9600);
            byte[] commands = new byte[]
            {
                51, 48, 52, 59,
                51, 49, 53, 59,
                51, 50, 54, 59,
                51, 60, 54, 59,
                51, 70, 54, 59,
                51, 80, 54, 59,
                51, 90, 54, 59,
                50, 50 };
            port.Open();
            port.Write(commands, 0, commands.Length);
            port.Close();
            Thread.Sleep(1000);

        }
    }
}
