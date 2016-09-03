using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialCommunicator {
	public partial class Form1 : Form {

		public Form1() {
			InitializeComponent();

			SerialCommunicator.Initialize(this, ReceiveMessage);
			SerialCommunicator.Connect();
		}

		public void ReceiveMessage(SerialMessage msg) {
			Console.WriteLine(String.Format("Form received Message: {0}", msg.ToString()));
			label1.Text = msg.ToString();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			SerialCommunicator.Disconnect();
		}

		private void button1_Click(object sender, EventArgs e) {
			SerialCommunicator.SendMessage(SerialMessage.Command1);
		}

		private void button2_Click(object sender, EventArgs e) {
			SerialCommunicator.SendMessage(SerialMessage.Command2);
		}
	}
}
