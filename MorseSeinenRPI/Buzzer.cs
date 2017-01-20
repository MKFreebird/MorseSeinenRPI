using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace MorseSeinenRPI
{
    public class Buzzer : Hardware
    {
        private int characterDelay = 50;
        private int shortDelay = 100;
        private int longDelay = 300;

        public Buzzer(int rpiPin, GpioPinDriveMode driveMode) : base(rpiPin, driveMode)
        {
            Pin.Write(GpioPinValue.Low);
        }

        /* Play received morse message with buzzer */
        public void PlayMorseMessage(List<string> message)
        {
            foreach (string morseLetter in message)
            {
                for (int i = 0; i < morseLetter.Length; i++)
                {
                    switch (morseLetter[i].ToString())
                    {
                        case ".":
                            Buzz(shortDelay);
                            break;
                        case "-":
                            Buzz(longDelay);
                            break;
                    }
                }
                Task.Delay(100).Wait();
            }
        }

        /* Set buzzer on for a specified duration in milli seconds*/
        private void Buzz(int delay)
        {
            Pin.Write(GpioPinValue.High);
            Task.Delay(delay).Wait();
            Pin.Write(GpioPinValue.Low);
            Task.Delay(characterDelay).Wait();
        }
    }
}