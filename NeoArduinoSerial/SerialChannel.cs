using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NeoArduinoSerial
{
    public class SerialChannel
    {
        private SerialPort port;

        private readonly int offset = 1;

        public SerialChannel(string name, int baudRate)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name has to be a port name");
            }
            
            if (baudRate <= 0)
            {
                throw new ArgumentException("baudRate cannot be that value");
            }

            this.port = new SerialPort(name, baudRate);
        }

        public void Open()
        {
            this.port.Open();
        }

        public void Close()
        {
            this.port.Close();
        }

        public void Write(byte [] commands)
        {
            if (commands == null || commands.Length == 0)
            {
                throw new ArgumentException("Invalid command array");
            }

            byte[] offsetCommands = new byte[commands.Length];

            for (int i = 0; i < commands.Length; i++)
            {
                offsetCommands[i] = (byte)(commands[i] + offset);
            }

            this.port.Write(offsetCommands, 0, offsetCommands.Length);
        }
    }
}
