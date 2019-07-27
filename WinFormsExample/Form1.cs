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
using SerialCommunication;

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
			buttonSendLED13ON.Enabled = connected;
			buttonSendLED13OFF.Enabled = connected;
			buttonSendText.Enabled = connected;
		}

		private void ReceiveMessage(string msg) {
            switch (msg)
            {
                case "LED13State1":
                    txtLEDState.Text = "LED: ON";
                    break;

                case "LED13State0":
                    txtLEDState.Text = "LED: OFF";
                    break;

                case "LED13 switched on":
                case "LED13 switched off":
                    textBox.AppendText("<<< " + msg + "\n");
                    break;

                default:
                    textBox.AppendText("<<< " + msg + "\n");
                    break;
            }
		}

		private void SendMessage(String msg) {
            try
            {
                textBox.AppendText(">>> " + msg + "\n");
                sCom1.TrySendMessage(msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Es ist ein Fehler aufgetreten");
            }
		}

        private void ChangeLEDState()
        {

        }
		#endregion

		#region buttons
		private void buttonConnect_Click(object sender, EventArgs e) {
			if (connected) {
				sCom1.Disconnect();
				connected = false;
                textBox.Clear();
			} else {
				connected = sCom1.Connect(this, ReceiveMessage, ((SerialPortInfo)portComboBox.SelectedItem).DeviceID);
			}
			UpdateGui();
		}

		private void buttonSendCommand1_Click(object sender, EventArgs e) {
			SendMessage("LED13ON");
		}

		private void buttonSendCommand2_Click(object sender, EventArgs e) {
			SendMessage("LED13OFF");
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
