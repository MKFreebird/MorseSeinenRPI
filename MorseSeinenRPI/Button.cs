using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace MorseSeinenRPI
{
    public class Button : Hardware
    {
        /* Changes to pin value do not generate events during DebounceTimeout */
        private const int debounceTimeout = 50;

        public Button(int rpiPin, GpioPinDriveMode driveMode) : base(rpiPin, driveMode)
        {
            Pin.DebounceTimeout = TimeSpan.FromMilliseconds(debounceTimeout);
        }
    }
}
