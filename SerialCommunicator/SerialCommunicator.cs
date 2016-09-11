using System;
using System.IO.Ports;
using System.Windows.Forms;

	public static class SerialCommunicator {

		private static SerialPort serialPort;
		private static Control control;
		private static ReceiveMessageCallback receiveMessageCallback;

		public delegate void ReceiveMessageCallback(string msg);

		#region public methods
		public static bool Connect(Control ctrl, ReceiveMessageCallback callback, string portName) {
			return Connect(ctrl, callback, portName, 9600, 2000, 500);
		}

		public static bool Connect(Control ctrl, ReceiveMessageCallback callback, string portName, int baudRate, int readTimeout, int writeTimeout) {
			if (serialPort == null) {
				Initialize(ctrl, callback, portName, baudRate, readTimeout, writeTimeout);
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

		public static void SendMessage(string msg) {
			if (serialPort != null && serialPort.IsOpen) {
				ConsoleOutput(String.Format("Sending Message: {0}", msg));
				serialPort.WriteLine(msg);
			} else {
				ConsoleOutput("Can not send message. Not connected to SerialPort.");
			}
		}
		#endregion

		#region private methods
		private static void Initialize(Control ctrl, ReceiveMessageCallback callback, string portName, int baudRate, int readTimeout, int writeTimeout) {
			serialPort = new SerialPort() {
				PortName = portName,
				BaudRate = baudRate,
				ReadTimeout = readTimeout,
				WriteTimeout = writeTimeout
			};
			serialPort.DataReceived += new SerialDataReceivedEventHandler(ReceiveMessageHandler);
			control = ctrl;
			receiveMessageCallback = callback;
			ConsoleOutput("Initialized.");
		}

		private static void ReceiveMessageHandler(object sender, SerialDataReceivedEventArgs e) {
			string msg = serialPort.ReadLine();
			ConsoleOutput(String.Format("Reading Message: {0}", msg));
			control.Invoke(receiveMessageCallback, msg);	
		}

		private static void ConsoleOutput(string message) {
			Console.WriteLine(String.Format("SerialCommunicator: {0}", message));
		}
		#endregion
}
