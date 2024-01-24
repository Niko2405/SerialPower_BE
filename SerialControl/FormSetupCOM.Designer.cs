namespace SerialControl
{
	partial class FormSetupCOM
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			panelSettings = new Panel();
			textBoxDataBits = new TextBox();
			textBoxStopBits = new TextBox();
			textBoxBaudrate = new TextBox();
			comboBoxPortNames = new ComboBox();
			labelDataBits = new Label();
			labelStopBits = new Label();
			labelBaudrate = new Label();
			labelPortName = new Label();
			buttonOK = new Button();
			buttonExit = new Button();
			panelSettings.SuspendLayout();
			SuspendLayout();
			// 
			// panelSettings
			// 
			panelSettings.Controls.Add(textBoxDataBits);
			panelSettings.Controls.Add(textBoxStopBits);
			panelSettings.Controls.Add(textBoxBaudrate);
			panelSettings.Controls.Add(comboBoxPortNames);
			panelSettings.Controls.Add(labelDataBits);
			panelSettings.Controls.Add(labelStopBits);
			panelSettings.Controls.Add(labelBaudrate);
			panelSettings.Controls.Add(labelPortName);
			panelSettings.Dock = DockStyle.Top;
			panelSettings.Location = new Point(0, 0);
			panelSettings.Name = "panelSettings";
			panelSettings.Size = new Size(898, 363);
			panelSettings.TabIndex = 0;
			// 
			// textBoxDataBits
			// 
			textBoxDataBits.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			textBoxDataBits.Location = new Point(453, 268);
			textBoxDataBits.Name = "textBoxDataBits";
			textBoxDataBits.Size = new Size(182, 34);
			textBoxDataBits.TabIndex = 7;
			textBoxDataBits.Text = "8";
			// 
			// textBoxStopBits
			// 
			textBoxStopBits.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			textBoxStopBits.Location = new Point(453, 213);
			textBoxStopBits.Name = "textBoxStopBits";
			textBoxStopBits.Size = new Size(182, 34);
			textBoxStopBits.TabIndex = 6;
			textBoxStopBits.Text = "1";
			// 
			// textBoxBaudrate
			// 
			textBoxBaudrate.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			textBoxBaudrate.Location = new Point(453, 154);
			textBoxBaudrate.Name = "textBoxBaudrate";
			textBoxBaudrate.Size = new Size(182, 34);
			textBoxBaudrate.TabIndex = 5;
			textBoxBaudrate.Text = "9600";
			// 
			// comboBoxPortNames
			// 
			comboBoxPortNames.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			comboBoxPortNames.FormattingEnabled = true;
			comboBoxPortNames.Location = new Point(453, 102);
			comboBoxPortNames.Name = "comboBoxPortNames";
			comboBoxPortNames.Size = new Size(182, 36);
			comboBoxPortNames.TabIndex = 4;
			// 
			// labelDataBits
			// 
			labelDataBits.AutoSize = true;
			labelDataBits.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			labelDataBits.Location = new Point(231, 268);
			labelDataBits.Name = "labelDataBits";
			labelDataBits.Size = new Size(88, 28);
			labelDataBits.TabIndex = 3;
			labelDataBits.Text = "DataBits:";
			// 
			// labelStopBits
			// 
			labelStopBits.AutoSize = true;
			labelStopBits.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			labelStopBits.Location = new Point(231, 213);
			labelStopBits.Name = "labelStopBits";
			labelStopBits.Size = new Size(89, 28);
			labelStopBits.TabIndex = 2;
			labelStopBits.Text = "Stopbits:";
			// 
			// labelBaudrate
			// 
			labelBaudrate.AutoSize = true;
			labelBaudrate.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			labelBaudrate.Location = new Point(225, 157);
			labelBaudrate.Name = "labelBaudrate";
			labelBaudrate.Size = new Size(94, 28);
			labelBaudrate.TabIndex = 1;
			labelBaudrate.Text = "Baudrate:";
			// 
			// labelPortName
			// 
			labelPortName.AutoSize = true;
			labelPortName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			labelPortName.Location = new Point(214, 102);
			labelPortName.Name = "labelPortName";
			labelPortName.Size = new Size(105, 28);
			labelPortName.TabIndex = 0;
			labelPortName.Text = "Port name:";
			// 
			// buttonOK
			// 
			buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonOK.Cursor = Cursors.Hand;
			buttonOK.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			buttonOK.Location = new Point(774, 409);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new Size(112, 43);
			buttonOK.TabIndex = 1;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonExit
			// 
			buttonExit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonExit.Cursor = Cursors.Hand;
			buttonExit.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			buttonExit.Location = new Point(12, 409);
			buttonExit.Name = "buttonExit";
			buttonExit.Size = new Size(112, 43);
			buttonExit.TabIndex = 2;
			buttonExit.Text = "Exit";
			buttonExit.UseVisualStyleBackColor = true;
			// 
			// FormSetupCOM
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(898, 464);
			Controls.Add(buttonExit);
			Controls.Add(buttonOK);
			Controls.Add(panelSettings);
			Name = "FormSetupCOM";
			Text = "Setup COM";
			panelSettings.ResumeLayout(false);
			panelSettings.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private Panel panelSettings;
		private Label labelStopBits;
		private Label labelBaudrate;
		private Label labelPortName;
		private Button buttonOK;
		private Button buttonExit;
		private Label labelDataBits;
		private ComboBox comboBoxPortNames;
		private TextBox textBoxDataBits;
		private TextBox textBoxStopBits;
		private TextBox textBoxBaudrate;
	}
}
