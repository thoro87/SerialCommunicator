namespace WinFormsExample {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.buttonSendCommand1 = new System.Windows.Forms.Button();
			this.buttonSendCommand2 = new System.Windows.Forms.Button();
			this.portComboBox = new System.Windows.Forms.ComboBox();
			this.buttonConnect = new System.Windows.Forms.Button();
			this.textBox = new System.Windows.Forms.TextBox();
			this.textBoxFree = new System.Windows.Forms.TextBox();
			this.buttonSendText = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonSendCommand1
			// 
			this.buttonSendCommand1.Location = new System.Drawing.Point(144, 53);
			this.buttonSendCommand1.Name = "buttonSendCommand1";
			this.buttonSendCommand1.Size = new System.Drawing.Size(123, 23);
			this.buttonSendCommand1.TabIndex = 0;
			this.buttonSendCommand1.Text = "Send Command1";
			this.buttonSendCommand1.UseVisualStyleBackColor = true;
			this.buttonSendCommand1.Click += new System.EventHandler(this.buttonSendCommand1_Click);
			// 
			// buttonSendCommand2
			// 
			this.buttonSendCommand2.Location = new System.Drawing.Point(13, 53);
			this.buttonSendCommand2.Name = "buttonSendCommand2";
			this.buttonSendCommand2.Size = new System.Drawing.Size(125, 23);
			this.buttonSendCommand2.TabIndex = 1;
			this.buttonSendCommand2.Text = "Send Command2";
			this.buttonSendCommand2.UseVisualStyleBackColor = true;
			this.buttonSendCommand2.Click += new System.EventHandler(this.buttonSendCommand2_Click);
			// 
			// portComboBox
			// 
			this.portComboBox.FormattingEnabled = true;
			this.portComboBox.Location = new System.Drawing.Point(13, 13);
			this.portComboBox.Name = "portComboBox";
			this.portComboBox.Size = new System.Drawing.Size(159, 21);
			this.portComboBox.TabIndex = 3;
			// 
			// buttonConnect
			// 
			this.buttonConnect.Location = new System.Drawing.Point(178, 13);
			this.buttonConnect.Name = "buttonConnect";
			this.buttonConnect.Size = new System.Drawing.Size(89, 23);
			this.buttonConnect.TabIndex = 5;
			this.buttonConnect.Text = "Connect";
			this.buttonConnect.UseVisualStyleBackColor = true;
			this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
			// 
			// textBox
			// 
			this.textBox.Location = new System.Drawing.Point(12, 108);
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.ReadOnly = true;
			this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox.Size = new System.Drawing.Size(255, 170);
			this.textBox.TabIndex = 6;
			// 
			// textBoxFree
			// 
			this.textBoxFree.Location = new System.Drawing.Point(13, 82);
			this.textBoxFree.Name = "textBoxFree";
			this.textBoxFree.Size = new System.Drawing.Size(176, 20);
			this.textBoxFree.TabIndex = 7;
			// 
			// buttonSendText
			// 
			this.buttonSendText.Location = new System.Drawing.Point(195, 82);
			this.buttonSendText.Name = "buttonSendText";
			this.buttonSendText.Size = new System.Drawing.Size(72, 20);
			this.buttonSendText.TabIndex = 8;
			this.buttonSendText.Text = "Send";
			this.buttonSendText.UseVisualStyleBackColor = true;
			this.buttonSendText.Click += new System.EventHandler(this.buttonSendText_Click);
			// 
			// Form1
			// 
			this.AcceptButton = this.buttonSendText;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(279, 289);
			this.Controls.Add(this.buttonSendText);
			this.Controls.Add(this.textBoxFree);
			this.Controls.Add(this.textBox);
			this.Controls.Add(this.buttonConnect);
			this.Controls.Add(this.portComboBox);
			this.Controls.Add(this.buttonSendCommand2);
			this.Controls.Add(this.buttonSendCommand1);
			this.Name = "Form1";
			this.Text = "SerialCommunicator";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonSendCommand1;
		private System.Windows.Forms.Button buttonSendCommand2;
		private System.Windows.Forms.ComboBox portComboBox;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.TextBox textBoxFree;
		private System.Windows.Forms.Button buttonSendText;
	}
}

