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

namespace SerialCommunicator {
	public partial class Form1 : Form {

		private bool connected;

		public Form1() {
			InitializeComponent();

			portComboBox.Items.Clear();
			foreach (string portName in SerialPort.GetPortNames()) {
				portComboBox.Items.Add(portName);
			}
			portComboBox.SelectedIndex = portComboBox.Items.Count - 1;

			UpdateGui();
		}

		public void ReceiveMessage(string msg) {
			Console.WriteLine(String.Format("Form received Message: {0}", msg));
			textBox.AppendText("<<< " + msg + "\n");
		}

		private void UpdateGui() {
			buttonConnect.Text = connected ? "Disconnect" : "Connect";
			portComboBox.Enabled = !connected;
			buttonSendCommand1.Enabled = connected;
			buttonSendCommand2.Enabled = connected;
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			SerialCommunicator.Disconnect();
		}


		#region buttons
		private void buttonSendCommand1_Click(object sender, EventArgs e) {
			string cmd = "command1";
			textBox.AppendText(">>> " + cmd + "\n");
			SerialCommunicator.SendMessage(cmd);
		}

		private void buttonSendCommand2_Click(object sender, EventArgs e) {
			string cmd = "command2";
			textBox.AppendText(">>> " + cmd + "\n");
			SerialCommunicator.SendMessage(cmd);
		}

		private void buttonConnect_Click(object sender, EventArgs e) {
			if (connected) {
				SerialCommunicator.Disconnect();
				connected = false;
			} else {
				connected = SerialCommunicator.Connect(this, ReceiveMessage, (string)portComboBox.SelectedItem);
			}
			UpdateGui();
		}
		#endregion
	}
}
