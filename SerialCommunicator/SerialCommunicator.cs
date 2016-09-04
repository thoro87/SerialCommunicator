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
		private static ReceiveMessageCallback readMessageCallback;

		public delegate void ReceiveMessageCallback(SerialMessage msg);

		#region public methods
		public static bool Connect(Control ctrl, ReceiveMessageCallback callback, string portName) {
			if (serialPort == null) {
				Initialize(ctrl, callback, portName);
			} else if (serialPort.IsOpen) {
				ConsoleOutput("Already connected.");
			} else {
				serialPort.PortName = portName;
			}

			try {
				serialPort.Open();
				ConsoleOutput(String.Format("Connected to port {0}", portName));
				return true;
			} catch {
				ConsoleOutput(String.Format("Unable to connect to port {0}", portName));
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
		private static void Initialize(Control ctrl, ReceiveMessageCallback callback, string portName) {
			Initialize(ctrl, callback, portName, 9600, 2000, 500);
		}

		private static void Initialize(Control ctrl, ReceiveMessageCallback callback, string portName, int baudRate, int readTimeout, int writeTimeout) {
			serialPort = new SerialPort() {
				PortName = portName,
				BaudRate = baudRate,
				ReadTimeout = readTimeout,
				WriteTimeout = writeTimeout
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
