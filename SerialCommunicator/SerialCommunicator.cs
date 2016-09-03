using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SerialCommunicator {
	public class SerialCommunicator {

		public enum SerialMessage {
			Command1 = 1,
			Command2 = 2,
			Result1 = 8,
			Result2 = 9
		}

		private SerialPort serialPort;
		private Form1 form;

		public SerialCommunicator(Form1 _form) {
			serialPort = new SerialPort() {
				PortName = "COM5",
				BaudRate = 9600,
				ReadTimeout = 2000,
				WriteTimeout = 500
			};
			serialPort.DataReceived += new SerialDataReceivedEventHandler(ReadMessageHandler);
			form = _form;
		}

		public void Connect() {
			serialPort.Open();
			Console.WriteLine("Serial connected.");
		}

		public void Disconnect() {
			serialPort.Close();
			Console.WriteLine("Serial disconnected.");
		}

		public void SendMessage(SerialMessage _msg) {
			if (serialPort.IsOpen) {
				Console.WriteLine(String.Format("SendMessage: {0}", _msg));
				serialPort.WriteLine(((int)_msg).ToString());
			} else {
				Console.WriteLine("Can not send message, because SerialPort is closed.");
			}
		}

		private void ReadMessageHandler(object sender, SerialDataReceivedEventArgs e) {
			try {
				SerialMessage msg = (SerialMessage)(Int32.Parse(serialPort.ReadLine()));
				Console.WriteLine(String.Format("ReadMessage: {0}", msg));
				form.Invoke((Action)delegate { form.ReceiveMessage(msg); });
			} catch (Exception) {
				Console.WriteLine("Error reading message.");
			}
		}

	}
}
