using System;
using System.IO.Ports;
using System.Windows.Forms;

// <copyright file="Program.cs" company="">
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

/// <summary>
/// Small class library for easy and robust serial communication
/// </summary>
public static class SerialCommunicator {
	private static SerialPort serialPort;
	private static Control control;
	private static ReceiveMessageCallback receiveMessageCallback;

	public delegate void ReceiveMessageCallback(string msg);

	#region public methods
	/// <summary>
	/// Connects to a SerialPort with the specified settings. Also initializes the class if not done already.
	/// </summary>
	/// <param name="ctrl">Reference to the control where the callback will be invoked.</param>
	/// <param name="callback">Callback to invoke after receiving messages.</param>
	/// <param name="portName">Name of COM-Port to connect to.</param>
	/// <returns>Returns true, if connecting was successfull.</returns>
	public static bool Connect(Control ctrl, ReceiveMessageCallback callback, string portName) {
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

	/// <summary>
	/// Disconnects from the current SerialPort.
	/// </summary>
	public static void Disconnect() {
		if (serialPort != null && serialPort.IsOpen) {
			serialPort.Close();
			ConsoleOutput("Disconnected.");
		}
	}

	/// <summary>
	/// Sends a message to the SerialPort.
	/// </summary>
	/// <param name="msg"></param>
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
