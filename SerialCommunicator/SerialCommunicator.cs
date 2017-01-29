using System;
using System.Linq;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Windows.Forms;

// <copyright file="SerialCommunicator.cs">
// Copyright (c) 2016, All Right Reserved
// </copyright>
// <author>Janis Langer</author>

//This file is part of SerialCommunicator.
//
//	SerialCommunicator is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//
//	SerialCommunicator is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//    GNU General Public License for more details.
//
//	You should have received a copy of the GNU General Public License
//	along with SerialCommunicator. If not, see<http://www.gnu.org/licenses/>.

namespace SerialCommunication {

	/// <summary>
	/// Small class library for easy and robust serial communication
	/// </summary>
	public class SerialCommunicator {
		private bool debugEnabled;
		private SerialPort serialPort;
		private Control control;
		private ReceiveMessageCallback receiveMessageCallback;
		
		public bool Connected { get { return serialPort != null && serialPort.IsOpen; } }

		public delegate void ReceiveMessageCallback(string msg);


		#region public methods
		public SerialPortInfo[] GetPortNames() {
			List<SerialPortInfo> portNames = new List<SerialPortInfo>();
			try {
				ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SerialPort");
				foreach (ManagementObject queryObj in searcher.Get()) {
					portNames.Add(new SerialPortInfo() {
						DeviceID = (string)queryObj["DeviceID"],
						Name = (string)queryObj["Name"]
					});
				}
			} catch {
				portNames = SerialPort.GetPortNames().Select(p => new SerialPortInfo() { DeviceID = p, Name = p }).ToList();
			}
			return portNames.ToArray();
		}

		/// <summary>
		/// Connects to a SerialPort with the specified settings. Also initializes the class if not done already.
		/// </summary>
		/// <param name="ctrl">Reference to the control where the callback will be invoked.</param>
		/// <param name="callback">Callback to invoke after receiving messages.</param>
		/// <param name="portName">Name of COM-Port to connect to.</param>
		/// <returns>Returns true, if connecting was successfull.</returns>
		public bool Connect(Control ctrl, ReceiveMessageCallback callback, string portName) {
			return Connect(ctrl, callback, portName, 9600, 2000, 500);
		}

		/// <summary>
		/// Connects to a SerialPort with the specified settings. Also initializes the class if not done already.
		/// </summary>
		/// <param name="ctrl">Reference to the control where the callback will be invoked.</param>
		/// <param name="callback">Callback to invoke after receiving messages.</param>
		/// <param name="portName">Name of COM-Port to connect to.</param>
		/// <param name="baudRate">Baudrate to use while communicating.</param>
		/// <param name="readTimeout">Timeout for reading messages.</param>
		/// <param name="writeTimeout">Timeout for sending messages.</param>
		/// <returns>Returns true, if connecting was successfull.</returns>
		public bool Connect(Control ctrl, ReceiveMessageCallback callback, string portName, int baudRate, int readTimeout, int writeTimeout) {
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

		/// <summary>
		/// Disconnects from the current SerialPort.
		/// </summary>
		public void Disconnect() {
			if (serialPort != null && serialPort.IsOpen) {
				serialPort.Close();
				ConsoleOutput("Disconnected.");
			}
		}

		/// <summary>
		/// Sends a message to the SerialPort.
		/// </summary>
		/// <param name="msg"></param>
		public void SendMessage(string msg) {
			if (!TrySendMessage(msg)) {
				throw new Exception("Not connected to SerialPort");
			}
		}

		/// <summary>
		/// Tries to send a message to the SerialPort. Returns true if successfull.
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public bool TrySendMessage(string msg) {
			if (serialPort != null && serialPort.IsOpen) {
				ConsoleOutput(String.Format("Sending Message: {0}", msg));
				serialPort.WriteLine(msg);
				return true;
			} else {
				ConsoleOutput("Can not send message. Not connected to SerialPort.");
				return false;
			}
		}

		/// <summary>
		/// Turn debug mode on or off. Default is off. Will print to console if on.
		/// </summary>
		/// <param name="newValue"></param>
		public void SetDebugMode(bool newValue) {
			debugEnabled = newValue;
		}
		#endregion

		#region private methods
		private void Initialize(Control ctrl, ReceiveMessageCallback callback, string portName, int baudRate, int readTimeout, int writeTimeout) {
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

		private void ReceiveMessageHandler(object sender, SerialDataReceivedEventArgs e) {
			string msg = serialPort.ReadLine();
			ConsoleOutput(String.Format("Reading Message: {0}", msg));
			control.Invoke(receiveMessageCallback, msg);
		}

		private void ConsoleOutput(string message) {
			if (debugEnabled) {
				Console.WriteLine(String.Format("SerialCommunicator: {0}", message));
			}
		}
		#endregion
	}
}
