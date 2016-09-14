using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// <copyright file="Form1.cs" company="">
// Copyright (c) 2016 All Right Reserved
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

namespace WinFormsExample {
	public partial class Form1 : Form {

		private bool connected;
		private SerialCommunicator sCom1;
		private SerialPortInfo[] portInfos;


		public Form1() {
			InitializeComponent();

			sCom1 = new SerialCommunicator();
			portInfos = sCom1.GetPortNames();

			portComboBox.Items.Clear();
			portComboBox.Items.AddRange(portInfos);
			SerialPortInfo arduinoPort = portInfos.FirstOrDefault(p => p.Name.Contains("Arduino"));
			if (arduinoPort != null) {
				portComboBox.SelectedItem = arduinoPort;
			}

			UpdateGui();
		}

		#region private methods
		private void UpdateGui() {
			buttonConnect.Text = connected ? "Disconnect" : "Connect";
			portComboBox.Enabled = !connected;
			buttonSendCommand1.Enabled = connected;
			buttonSendCommand2.Enabled = connected;
			buttonSendText.Enabled = connected;
		}

		private void ReceiveMessage(string msg) {
			Console.WriteLine(String.Format("Form received Message: {0}", msg));
			textBox.AppendText("<<< " + msg + "\n");
		}

		private void SendMessage(String msg) {
			textBox.AppendText(">>> " + msg + "\n");
			sCom1.SendMessage(msg);
		}
		#endregion

		#region buttons
		private void buttonConnect_Click(object sender, EventArgs e) {
			if (connected) {
				sCom1.Disconnect();
				connected = false;
			} else {
				connected = sCom1.Connect(this, ReceiveMessage, ((SerialPortInfo)portComboBox.SelectedItem).DeviceID);
			}
			UpdateGui();
		}

		private void buttonSendCommand1_Click(object sender, EventArgs e) {
			SendMessage("command1");
		}

		private void buttonSendCommand2_Click(object sender, EventArgs e) {
			SendMessage("command2");
        }

		private void buttonSendText_Click(object sender, EventArgs e) {
			if (!String.IsNullOrEmpty(textBoxFree.Text)) {
				SendMessage(textBoxFree.Text);
				textBoxFree.Text = String.Empty;
			}
		}
		#endregion

		#region events
		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			sCom1.Disconnect();
		}
		#endregion
	}
}
