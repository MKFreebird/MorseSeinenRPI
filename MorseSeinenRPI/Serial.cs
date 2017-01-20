using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace MorseSeinenRPI
{
    class Serial
    {
        public SerialDevice SerialPort;

        public Serial()
        {
            Task task = new Task(InitSerial);
            task.Start();
            task.Wait();
        }

        /* Initialize serial connection */
        public async void InitSerial()
        {
            string aqs = SerialDevice.GetDeviceSelector("UART0");      /* Find the selector string for the serial device   */
            var dis = await DeviceInformation.FindAllAsync(aqs);       /* Find the serial device with our selector string  */
            SerialPort = await SerialDevice.FromIdAsync(dis[0].Id);    /* Create an serial device with our selected device */
            SerialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
            SerialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
            SerialPort.BaudRate = 9600;
            SerialPort.Parity = SerialParity.None;
            SerialPort.StopBits = SerialStopBitCount.One;
            SerialPort.DataBits = 8;
        }

        /* Read data in from the serial port */
        public async Task<string> Read()
        {
            using (DataReader dataReader = new DataReader(SerialPort.InputStream))
            {
                const uint maxReadLength = 1024;
                uint bytesToRead = await dataReader.LoadAsync(maxReadLength);
                string rxBuffer = dataReader.ReadString(bytesToRead);
                return rxBuffer;  
            }  
        }

        /* Write a string out over serial */
        public async void Write(string message)
        {
            using (DataWriter dataWriter = new DataWriter())
            {
                string txBuffer = message;
                dataWriter.WriteString(txBuffer);
                uint bytesWritten = await SerialPort.OutputStream.WriteAsync(dataWriter.DetachBuffer());
            }
        }
    }
}