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

		public void ReceiveMessage(SerialMessage msg) {
			Console.WriteLine(String.Format("Form received Message: {0}", msg.ToString()));
			textBox.AppendText("<<< " + msg.ToString() + "\n");
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
			textBox.AppendText(">>> " + SerialMessage.Command1 + "\n");
			SerialCommunicator.SendMessage(SerialMessage.Command1);
		}

		private void buttonSendCommand2_Click(object sender, EventArgs e) {
			textBox.AppendText(">>> " + SerialMessage.Command2 + "\n");
			SerialCommunicator.SendMessage(SerialMessage.Command2);
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
