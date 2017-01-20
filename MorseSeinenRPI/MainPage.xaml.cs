using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.SerialCommunication;
using System.Diagnostics;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;


namespace MorseSeinenRPI
{
    public sealed partial class MainPage : Page
    {
        /* GPIO Pins
        LCD         */
        private const int enablePin = 16;
        private const int RWPin = 5;
        private const int registerSelectPin = 20;
        private const int dataPin07 = 6;
        private const int dataPin08 = 13;
        private const int dataPin09 = 19;
        private const int dataPin10 = 26;
        private const int dataPin11 = 22;
        private const int dataPin12 = 27;
        private const int dataPin13 = 4;
        private const int dataPin14 = 17;
        /* Button */
        private const int btnPinShort = 23;
        private const int btnPinLong = 24;
        private const int btnPinConfirm = 25;
        private const int btnPinRemove = 18;
        private const int btnPinSend = 12;
        /* Buzzer */
        private const int pinBuzzer = 21;

        private Button btnShort;
        private Button btnLong;
        private Button btnConfirm;
        private Button btnRemove;
        private Button btnSend;
        private Buzzer buzzer;

        private HD44780Controller lcd = new HD44780Controller();
        private MorseLibrary morse = new MorseLibrary();
        private Serial serial = new Serial();

        private List<string> rcvMorseMessage = new List<string>();
        private List<string> sendMorseMessage = new List<string>();
        private string morseChar;

        public MainPage()
        {
            this.InitializeComponent();
            Init();
           // Listen();
        }

        public void Init()
        {
            /* int rs, int rw, int enable, int d7, int d6, int d5, int d4, int d3, int d2, int d1, int d0 */
            lcd.Init(registerSelectPin, RWPin, enablePin, dataPin14, dataPin13, dataPin12, dataPin11, dataPin10, dataPin09, dataPin08, dataPin07);
            /* Initialize buttons and buzzer */
            btnShort = new Button(btnPinShort, GpioPinDriveMode.InputPullUp);
            btnLong = new Button(btnPinLong, GpioPinDriveMode.InputPullUp);
            btnConfirm = new Button(btnPinConfirm, GpioPinDriveMode.InputPullUp);
            btnRemove = new Button(btnPinRemove, GpioPinDriveMode.InputPullUp);
            btnSend = new Button(btnPinSend, GpioPinDriveMode.InputPullUp);
            buzzer = new Buzzer(pinBuzzer, GpioPinDriveMode.Output);
            /* Bind events to buttons*/
            btnLong.Pin.ValueChanged += MorseInput;
            btnShort.Pin.ValueChanged += MorseInput;
            btnConfirm.Pin.ValueChanged += ConfirmMorseChar;
            btnRemove.Pin.ValueChanged += RemoveLastChar;
            btnSend.Pin.ValueChanged += SendMessage;
            serial.SerialPort.PinChanged += readMessage;
            lcd.Write("Morse Seinen RPI");       
        }

        private async void readMessage(SerialDevice sender, PinChangedEventArgs args)
        {
            string message = await serial.Read();
            lcd.Write(message);
        }

        /* Get character input and add to character string */
        private void MorseInput(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.FallingEdge)
            {
                if (sender == btnLong.Pin)
                {
                    morseChar += "-";
                }
                else if (sender == btnShort.Pin)
                {
                    morseChar += ".";
                }
                UpdateScreen();
            }
        }

        /* Confirm the morse character and add to send list */
        private void ConfirmMorseChar(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.FallingEdge)
            {
                if (morse.IsValidChar(morseChar))
                {
                    sendMorseMessage.Add(morseChar);
                }
                morseChar = "";
                UpdateScreen();
            }
        }

        /* Remove last typed character */
        private void RemoveLastChar(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.FallingEdge)
            {
                if (morseChar.Length > 0)
                {
                    morseChar = morseChar.Substring(0, morseChar.Length - 1);
                }
                else if (sendMorseMessage.Count > 0)
                {
                    sendMorseMessage.RemoveAt(sendMorseMessage.Count - 1);
                }
                UpdateScreen();
            }
        }

        /* Send morse message over serial connection */
        private void SendMessage(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.FallingEdge && sendMorseMessage.Count > 0)
            {
                string message = string.Join("", sendMorseMessage.ToArray());
                serial.Write(message);
                sendMorseMessage.Clear();
                UpdateScreen();
            }
        }



        /* Write changes to LCD */
        private void UpdateScreen()
        {
            lcd.ClearDisplay();
            lcd.SetCursorPosition(0, 0);
            lcd.Write(morse.Translate(sendMorseMessage));
            lcd.SetCursorPosition(1, 0);
            lcd.Write(morseChar);
        }

        private async void Listen()
        {
            if (serial.SerialPort != null)
            {
                string message = await serial.Read();
            }

        }

    }
}
