using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace SerialCommunicator {

	public enum SerialMessage {
		Command1 = 1,
		Command2 = 2,
		Result1 = 8,
		Result2 = 9
	}

	public static class SerialCommunicator {

		private static SerialPort serialPort;
		private static Control control;
		private static ReadMessageCallback readMessageCallback;

		public delegate void ReadMessageCallback(SerialMessage msg);

		#region public methods
		public static bool Connect(Control ctrl, ReadMessageCallback callback, string portName) {
			if (serialPort == null) {
				Initialize(ctrl, callback, portName);
			}

			try {
				serialPort.Open();
				ConsoleOutput("Connected.");
				return true;
			} catch {
				ConsoleOutput("Unable to connect.");
				return false;
			}
		}

		public static void Disconnect() {
			if (serialPort != null && serialPort.IsOpen) {
				serialPort.Close();
				ConsoleOutput("Disconnected.");
			}
		}

		public static void SendMessage(SerialMessage msg) {
			if (serialPort != null && serialPort.IsOpen) {
				ConsoleOutput(String.Format("Sending Message: {0}", msg));
				serialPort.WriteLine(((int)msg).ToString());
			} else {
				ConsoleOutput("Can not send message. Not connected to SerialPort.");
			}
		}
		#endregion

		#region private methods
		private static void Initialize(Control ctrl, ReadMessageCallback callback, string portName) {
			serialPort = new SerialPort() {
				PortName = portName,
				BaudRate = 9600,
				ReadTimeout = 2000,
				WriteTimeout = 500
			};
			serialPort.DataReceived += new SerialDataReceivedEventHandler(ReceiveMessageHandler);
			control = ctrl;
			readMessageCallback = callback;
			ConsoleOutput("Initialized.");
		}

		private static void ReceiveMessageHandler(object sender, SerialDataReceivedEventArgs e) {
			try {
				SerialMessage msg = (SerialMessage)(Int32.Parse(serialPort.ReadLine()));
				ConsoleOutput(String.Format("Reading Message: {0}", msg));
				control.Invoke(readMessageCallback, msg);	
			} catch (Exception) {
				ConsoleOutput("Error reading message.");
			}
		}

		private static void ConsoleOutput(string message) {
			Console.WriteLine(String.Format("SerialCommunicator: {0}", message));
		}
		#endregion

	}
}
