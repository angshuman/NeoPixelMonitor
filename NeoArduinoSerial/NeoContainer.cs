using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoArduinoSerial
{
    public class NeoContainer
    {
        public readonly List<NeoSection> Sections = new List<NeoSection>();

        private readonly SerialChannel channel;

        public NeoContainer(string name, int baudRate)
        {
            this.channel = new SerialChannel(name, baudRate);
            this.channel.Open();
        }

        public void SendCommand()
        {
            byte[] startCommand = { Constants.Start, Constants.Nil };
            byte[] endCommand = { Constants.End, Constants.Nil };

            this.channel.Write(startCommand);

            foreach(var section in this.Sections)
            {
                channel.Write(section.GetCommands());
            }

            this.channel.Write(endCommand);
            //this.channel.Close();
        }
    }
}
