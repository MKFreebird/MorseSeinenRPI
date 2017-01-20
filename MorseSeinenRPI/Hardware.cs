using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace MorseSeinenRPI
{
    public abstract class Hardware
    {
        private GpioPin gpioPin;
        private GpioController gpio;

        /* Initialize GPIO pin */
        public Hardware(int rpiPin, GpioPinDriveMode driveMode)
        {
            gpio = GpioController.GetDefault();
            gpioPin = gpio.OpenPin(rpiPin);
            gpioPin.SetDriveMode(driveMode);
        }

        /* Make gpioPin available for valuechanged events */
        public GpioPin Pin
        {
            get { return gpioPin; }
        }
    }
}
