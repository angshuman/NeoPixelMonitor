using System;

namespace NeoArduinoSerial
{
    public class NeoSection
    {
        private NeoColor startColor = new NeoColor();
        private NeoColor endColor = new NeoColor();
        private int percentage = 0;
        public int SectionCount
        {
            get
            {
                return (int)Math.Round(this.SectionLength * this.Percentage / 100.0);
            }
        }

        public int Start { get; set; }
        public int SectionLength { get; set; }
        public int Percentage
        {
            get
            {
                return this.percentage;
            }
            set
            {
                this.percentage = value; this.SetLeds();
            }
        }

        private readonly NeoLed[] leds;

        public NeoSection(int start, int length)
        {
            if (start < 0)
            {
                throw new ArgumentException("start cannot be negative");
            }

            if (length < 0)
            {
                throw new ArgumentException("length cannot be negative");
            }

            this.leds = new NeoLed[length];
            for (int i = 0; i < length; i++)
            {
                this.leds[i] = new NeoLed(i);
            }

            this.Start = start;
            this.SectionLength = length;
        }

        public void SetColor(NeoColor start, NeoColor end)
        {
            this.startColor = start;
            this.endColor = end;
            this.SetLeds();
        }

        public void SetLeds()
        {
            double dr = (double)(this.endColor.Red - this.startColor.Red) / SectionLength;
            double dg = (double)(this.endColor.Green - this.startColor.Green) / SectionLength;
            double db = (double)(this.endColor.Blue - this.startColor.Blue) / SectionLength;

            for (int i = 0; i < this.SectionCount; i++)
            {
                var color = new NeoColor();
                color.Red = (byte)(this.startColor.Red + i * dr);
                color.Green = (byte)(this.startColor.Green + i * dg);
                color.Blue = (byte)(this.startColor.Blue + i * db);

                this.leds[i].Color = color;
            }

            var black = new NeoColor();

            for (int i = this.SectionCount; i < this.SectionLength; i++)
            {
                this.leds[i].Color = black;
            }
        }

        public byte[] GetCommands()
        {
            var command = new byte[this.SectionLength * 8];

            for (int i = 0; i < this.SectionLength; i++)
            {
                var commandPos = i * 8;
                command[commandPos + 0] = Constants.Position;
                command[commandPos + 1] = (byte)(this.Start + i);

                command[commandPos + 2] = Constants.Red;
                command[commandPos + 3] = (byte)(this.leds[i].Color.Red);

                command[commandPos + 4] = Constants.Green;
                command[commandPos + 5] = (byte)(this.leds[i].Color.Green);

                command[commandPos + 6] = Constants.Blue;
                command[commandPos + 7] = (byte)(this.leds[i].Color.Blue);
            }

            return command;
        }
    }
}
