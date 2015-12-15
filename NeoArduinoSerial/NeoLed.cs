using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoArduinoSerial
{
    public class NeoLed
    {
        public int Position { get; }

        public NeoColor Color { get; set; }

        public NeoLed(int position)
        {
            if (position < 0)
            {
                throw new ArgumentException("position cannot be negative");
            }

            this.Position = position;
        }
    }
}
