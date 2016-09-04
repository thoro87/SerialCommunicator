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
		private static bool initialized;

		public delegate void ReadMessageCallback(SerialMessage msg);

		public static void Initialize(Control ctrl, ReadMessageCallback callback) {
			serialPort = new SerialPort() {
				PortName = "COM5",
				BaudRate = 9600,
				ReadTimeout = 2000,
				WriteTimeout = 500
			};
			serialPort.DataReceived += new SerialDataReceivedEventHandler(ReceiveMessageHandler);
			control = ctrl;
			readMessageCallback = callback;
			initialized = true;
			ConsoleOutput("Initialized.");
		}

		#region public methods
		public static void Connect() {
			Assert(initialized, "SerialCommunicator has not been initialized yet.");
			serialPort.Open();
			ConsoleOutput("Connected.");
		}

		public static void Disconnect() {
			Assert(initialized, "SerialCommunicator has not been initialized yet.");
			serialPort.Close();
			ConsoleOutput("Disconnected.");
		}

		public static void SendMessage(SerialMessage msg) {
			Assert(initialized, "SerialCommunicator has not been initialized yet.");
			if (serialPort.IsOpen) {
				ConsoleOutput(String.Format("Sending Message: {0}", msg));
				serialPort.WriteLine(((int)msg).ToString());
			} else {
				Console.WriteLine("Can not send message, because SerialPort is closed.");
			}
		}
		#endregion

		#region private methods
		private static void ReceiveMessageHandler(object sender, SerialDataReceivedEventArgs e) {
			try {
				SerialMessage msg = (SerialMessage)(Int32.Parse(serialPort.ReadLine()));
				ConsoleOutput(String.Format("Reading Message: {0}", msg));
				control.Invoke(readMessageCallback, msg);	
			} catch (Exception) {
				ConsoleOutput("Error reading message.");
			}
		}

		private static void Assert(bool statement, string message) {
			if (!statement) {
				throw new Exception(message);
			}
		}

		private static void ConsoleOutput(string message) {
			Console.WriteLine(String.Format("SerialCommunicator: {0}", message));
		}
		#endregion

	}
}
