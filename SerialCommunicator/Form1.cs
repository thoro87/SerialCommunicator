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

		public Form1() {
			InitializeComponent();

			portComboBox.Items.Clear();
			foreach (string portName in SerialPort.GetPortNames()) {
				portComboBox.Items.Add(portName);
			}
			portComboBox.SelectedIndex = portComboBox.Items.Count - 1;
		}

		public void ReceiveMessage(SerialMessage msg) {
			Console.WriteLine(String.Format("Form received Message: {0}", msg.ToString()));
			label1.Text = msg.ToString();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			SerialCommunicator.Disconnect();
		}


		#region buttons
		private void button1_Click(object sender, EventArgs e) {
			SerialCommunicator.SendMessage(SerialMessage.Command1);
		}

		private void button2_Click(object sender, EventArgs e) {
			SerialCommunicator.SendMessage(SerialMessage.Command2);
		}

		private void button4_Click(object sender, EventArgs e) {
			SerialCommunicator.Connect(this, ReceiveMessage, (string)portComboBox.SelectedItem);
		}
		#endregion
	}
}
