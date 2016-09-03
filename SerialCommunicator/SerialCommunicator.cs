using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialCommunicator {
	public class SerialCommunicator {

		public enum SerialMessage {
			Command1 = 1,
			Command2 = 2,
			Result1 = 8,
			Result2 = 9
		}

		public delegate void ReadMessageCallback(SerialMessage msg);

		private SerialPort serialPort;
		private Control control;
		private ReadMessageCallback readMessageCallback;

		public SerialCommunicator(Control ctrl, ReadMessageCallback callback) {
			serialPort = new SerialPort() {
				PortName = "COM5",
				BaudRate = 9600,
				ReadTimeout = 2000,
				WriteTimeout = 500
			};
			serialPort.DataReceived += new SerialDataReceivedEventHandler(ReadMessageHandler);
			control = ctrl;
			readMessageCallback = callback;
		}

		public void Connect() {
			serialPort.Open();
			Console.WriteLine("Serial connected.");
		}

		public void Disconnect() {
			serialPort.Close();
			Console.WriteLine("Serial disconnected.");
		}

		public void SendMessage(SerialMessage msg) {
			if (serialPort.IsOpen) {
				Console.WriteLine(String.Format("SendMessage: {0}", msg));
				serialPort.WriteLine(((int)msg).ToString());
			} else {
				Console.WriteLine("Can not send message, because SerialPort is closed.");
			}
		}

		private void ReadMessageHandler(object sender, SerialDataReceivedEventArgs e) {
			try {
				SerialMessage msg = (SerialMessage)(Int32.Parse(serialPort.ReadLine()));
				Console.WriteLine(String.Format("ReadMessage: {0}", msg));
				control.Invoke(readMessageCallback, msg);	
			} catch (Exception) {
				Console.WriteLine("Error reading message.");
			}
		}

	}
}
