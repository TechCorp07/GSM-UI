using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;

namespace GSM_UI
{
    public partial class Form1 : Form
    {
        SerialPort aSerialPort = new SerialPort();
        public Form1()
        {
            InitializeComponent();
        }

        void receive_message()
        {
            aSerialPort.PortName = "COM12";
            aSerialPort.BaudRate = 9600;  //Baudrate of the serial communication system
            aSerialPort.DataBits = 8;
            aSerialPort.Parity = Parity.None;
            aSerialPort.StopBits = StopBits.One;
            aSerialPort.Handshake = Handshake.XOnXOff;
            aSerialPort.DtrEnable = true;
            aSerialPort.RtsEnable = true;
            aSerialPort.NewLine = Environment.NewLine;

            try
            {
                if (!aSerialPort.IsOpen)
                {
                    aSerialPort.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //aSerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            //aSerialPort.Write("AT" + System.Environment.NewLine);
            //Thread.Sleep(1000);

            //aSerialPort.WriteLine("AT+CMGF=1" + System.Environment.NewLine);
            //Thread.Sleep(1000);

            //aSerialPort.WriteLine("AT+CMGL=\"ALL\"\r" + System.Environment.NewLine); // ("AT+CMGL=\"REC UNREAD\"\r");
            //Thread.Sleep(3000);

                aSerialPort.WriteLine("AT+CMGF=1"); // Set mode to Text(1) or PDU(0)
                Thread.Sleep(1000); // Give a second to write

                aSerialPort.WriteLine("AT+CMGL=\"REC UNREAD\""); // What category to read ALL, REC READ, or REC UNREAD
                Thread.Sleep(1000);

                aSerialPort.Write("\r");
                Thread.Sleep(1000);

                string response = aSerialPort.ReadExisting();
                //string[] subVar = response.Split('*');
                int i = 0;
                string heartrate, temp, longi, lati, datetime;

                sbyte indexOf1 = Convert.ToSByte(response.IndexOf("@"));
                sbyte indexOf2 = Convert.ToSByte(response.IndexOf("*"));
                sbyte indexOf3 = Convert.ToSByte(response.IndexOf("$"));
                sbyte indexOf4 = Convert.ToSByte(response.IndexOf("&"));
                sbyte indexOf5 = Convert.ToSByte(response.IndexOf("%"));
                sbyte indexOf6 = Convert.ToSByte(response.IndexOf("?"));

                heartrate = response.Substring((indexOf1 + 1), (indexOf2 - indexOf1 - 1));
                temp = response.Substring((indexOf2 + 1), (indexOf3 - indexOf2 - 1));
                longi = response.Substring((indexOf3 + 1), (indexOf4 - indexOf3 - 1));
                lati = response.Substring((indexOf4 + 1), (indexOf5 - indexOf4 - 1));
                datetime = response.Substring((indexOf5 + 1), (indexOf6 - indexOf5 - 1));

                label1.Text = heartrate;
                label2.Text = temp;
                label3.Text = longi;
                label4.Text = lati;
                label5.Text = datetime;
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            //string[] arrList = sData.ReadLine().Split('/');
        }

        private void button1_Click(object sender, EventArgs e)
        {
            receive_message();
        }
    }
}
