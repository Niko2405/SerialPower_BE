namespace SerialControl
{
	partial class FormModule01
	{
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Komponenten-Designer generierter Code

		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			labelTitle = new Label();
			panel1 = new Panel();
			panel2 = new Panel();
			buttonCH1Read = new Button();
			checkBoxCH1 = new CheckBox();
			label6 = new Label();
			textBoxCH1Current = new TextBox();
			buttonCH1Send = new Button();
			buttonCH1Plus = new Button();
			buttonCH1Minus = new Button();
			textBoxCH1Fine = new TextBox();
			labelCH1Fine = new Label();
			listBoxCH1Temperature = new ListBox();
			label3 = new Label();
			listBoxCH1PositionAmplifier = new ListBox();
			listBoxCH1ValveControl = new ListBox();
			label2 = new Label();
			label1 = new Label();
			panel3 = new Panel();
			checkBoxCH2 = new CheckBox();
			buttonCH2Read = new Button();
			buttonCH2Send = new Button();
			textBoxCH2Current = new TextBox();
			label5 = new Label();
			buttonCH2Plus = new Button();
			buttonCH2Minus = new Button();
			textBoxCH2Fine = new TextBox();
			label4 = new Label();
			timerRender = new System.Windows.Forms.Timer(components);
			panel1.SuspendLayout();
			panel2.SuspendLayout();
			panel3.SuspendLayout();
			SuspendLayout();
			// 
			// labelTitle
			// 
			labelTitle.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			labelTitle.Location = new Point(490, 17);
			labelTitle.Name = "labelTitle";
			labelTitle.Size = new Size(190, 45);
			labelTitle.TabIndex = 0;
			labelTitle.Text = "IDE - 87/10582";
			labelTitle.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			panel1.BackColor = SystemColors.Control;
			panel1.Controls.Add(labelTitle);
			panel1.Dock = DockStyle.Top;
			panel1.Location = new Point(0, 0);
			panel1.Name = "panel1";
			panel1.Size = new Size(1258, 67);
			panel1.TabIndex = 1;
			// 
			// panel2
			// 
			panel2.BackColor = SystemColors.Control;
			panel2.BorderStyle = BorderStyle.FixedSingle;
			panel2.Controls.Add(buttonCH1Read);
			panel2.Controls.Add(checkBoxCH1);
			panel2.Controls.Add(label6);
			panel2.Controls.Add(textBoxCH1Current);
			panel2.Controls.Add(buttonCH1Send);
			panel2.Controls.Add(buttonCH1Plus);
			panel2.Controls.Add(buttonCH1Minus);
			panel2.Controls.Add(textBoxCH1Fine);
			panel2.Controls.Add(labelCH1Fine);
			panel2.Controls.Add(listBoxCH1Temperature);
			panel2.Controls.Add(label3);
			panel2.Controls.Add(listBoxCH1PositionAmplifier);
			panel2.Controls.Add(listBoxCH1ValveControl);
			panel2.Controls.Add(label2);
			panel2.Controls.Add(label1);
			panel2.Dock = DockStyle.Left;
			panel2.Location = new Point(0, 67);
			panel2.Name = "panel2";
			panel2.Size = new Size(595, 657);
			panel2.TabIndex = 2;
			// 
			// buttonCH1Read
			// 
			buttonCH1Read.Location = new Point(330, 213);
			buttonCH1Read.Name = "buttonCH1Read";
			buttonCH1Read.Size = new Size(249, 34);
			buttonCH1Read.TabIndex = 13;
			buttonCH1Read.Text = "Lese Wert";
			buttonCH1Read.UseVisualStyleBackColor = true;
			buttonCH1Read.Click += buttonCH1Read_Click;
			// 
			// checkBoxCH1
			// 
			checkBoxCH1.AutoSize = true;
			checkBoxCH1.Location = new Point(379, 6);
			checkBoxCH1.Name = "checkBoxCH1";
			checkBoxCH1.Size = new Size(177, 29);
			checkBoxCH1.TabIndex = 8;
			checkBoxCH1.Text = "Kanal 1 - Aktiviert";
			checkBoxCH1.UseVisualStyleBackColor = true;
			checkBoxCH1.CheckedChanged += checkBoxCH1_CheckedChanged;
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			label6.Location = new Point(330, 145);
			label6.Name = "label6";
			label6.Size = new Size(147, 28);
			label6.TabIndex = 12;
			label6.Text = "Kanal 1 - Strom";
			// 
			// textBoxCH1Current
			// 
			textBoxCH1Current.Location = new Point(330, 176);
			textBoxCH1Current.Name = "textBoxCH1Current";
			textBoxCH1Current.ReadOnly = true;
			textBoxCH1Current.Size = new Size(249, 31);
			textBoxCH1Current.TabIndex = 11;
			// 
			// buttonCH1Send
			// 
			buttonCH1Send.Location = new Point(20, 598);
			buttonCH1Send.Name = "buttonCH1Send";
			buttonCH1Send.Size = new Size(249, 34);
			buttonCH1Send.TabIndex = 10;
			buttonCH1Send.Text = "Sende Spannung";
			buttonCH1Send.UseVisualStyleBackColor = true;
			buttonCH1Send.Click += buttonCH1Send_Click;
			// 
			// buttonCH1Plus
			// 
			buttonCH1Plus.Location = new Point(157, 558);
			buttonCH1Plus.Name = "buttonCH1Plus";
			buttonCH1Plus.Size = new Size(112, 34);
			buttonCH1Plus.TabIndex = 9;
			buttonCH1Plus.Text = "+";
			buttonCH1Plus.UseVisualStyleBackColor = true;
			buttonCH1Plus.Click += buttonCH1Plus_Click;
			// 
			// buttonCH1Minus
			// 
			buttonCH1Minus.Location = new Point(20, 558);
			buttonCH1Minus.Name = "buttonCH1Minus";
			buttonCH1Minus.Size = new Size(112, 34);
			buttonCH1Minus.TabIndex = 8;
			buttonCH1Minus.Text = "-";
			buttonCH1Minus.UseVisualStyleBackColor = true;
			buttonCH1Minus.Click += buttonCH1Minus_Click;
			// 
			// textBoxCH1Fine
			// 
			textBoxCH1Fine.Location = new Point(20, 521);
			textBoxCH1Fine.Name = "textBoxCH1Fine";
			textBoxCH1Fine.Size = new Size(249, 31);
			textBoxCH1Fine.TabIndex = 7;
			textBoxCH1Fine.Text = "12";
			// 
			// labelCH1Fine
			// 
			labelCH1Fine.AutoSize = true;
			labelCH1Fine.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			labelCH1Fine.Location = new Point(20, 490);
			labelCH1Fine.Name = "labelCH1Fine";
			labelCH1Fine.Size = new Size(225, 28);
			labelCH1Fine.TabIndex = 6;
			labelCH1Fine.Text = "Kanal 1 - Feineinstellung";
			// 
			// listBoxCH1Temperature
			// 
			listBoxCH1Temperature.FormattingEnabled = true;
			listBoxCH1Temperature.ItemHeight = 25;
			listBoxCH1Temperature.Items.AddRange(new object[] { "2,928V", "3,239V", "3,549V" });
			listBoxCH1Temperature.Location = new Point(20, 387);
			listBoxCH1Temperature.Name = "listBoxCH1Temperature";
			listBoxCH1Temperature.Size = new Size(249, 79);
			listBoxCH1Temperature.TabIndex = 5;
			listBoxCH1Temperature.SelectedIndexChanged += listBoxCH1Temperature_SelectedIndexChanged;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			label3.Location = new Point(20, 356);
			label3.Name = "label3";
			label3.Size = new Size(203, 28);
			label3.TabIndex = 4;
			label3.Text = "Kanal 1 - Temperature";
			// 
			// listBoxCH1PositionAmplifier
			// 
			listBoxCH1PositionAmplifier.FormattingEnabled = true;
			listBoxCH1PositionAmplifier.ItemHeight = 25;
			listBoxCH1PositionAmplifier.Items.AddRange(new object[] { "1,629V", "4,829V", "8,029V", "0,900V", "4,789V", "8,678V" });
			listBoxCH1PositionAmplifier.Location = new Point(20, 176);
			listBoxCH1PositionAmplifier.Name = "listBoxCH1PositionAmplifier";
			listBoxCH1PositionAmplifier.Size = new Size(249, 154);
			listBoxCH1PositionAmplifier.TabIndex = 3;
			listBoxCH1PositionAmplifier.SelectedIndexChanged += listBoxCH1PositionAmplifier_SelectedIndexChanged;
			// 
			// listBoxCH1ValveControl
			// 
			listBoxCH1ValveControl.FormattingEnabled = true;
			listBoxCH1ValveControl.ItemHeight = 25;
			listBoxCH1ValveControl.Items.AddRange(new object[] { "1V", "9V", "0V" });
			listBoxCH1ValveControl.Location = new Point(20, 35);
			listBoxCH1ValveControl.Name = "listBoxCH1ValveControl";
			listBoxCH1ValveControl.Size = new Size(249, 79);
			listBoxCH1ValveControl.TabIndex = 2;
			listBoxCH1ValveControl.SelectedIndexChanged += listBoxCH1ValveControl_SelectedIndexChanged;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			label2.Location = new Point(20, 145);
			label2.Name = "label2";
			label2.Size = new Size(249, 28);
			label2.TabIndex = 1;
			label2.Text = "Kanal 1 - Position Amplifier";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			label1.Location = new Point(20, 4);
			label1.Name = "label1";
			label1.Size = new Size(211, 28);
			label1.TabIndex = 0;
			label1.Text = "Kanal 1 - Valve Control";
			// 
			// panel3
			// 
			panel3.BackColor = SystemColors.Control;
			panel3.BorderStyle = BorderStyle.FixedSingle;
			panel3.Controls.Add(checkBoxCH2);
			panel3.Controls.Add(buttonCH2Read);
			panel3.Controls.Add(buttonCH2Send);
			panel3.Controls.Add(textBoxCH2Current);
			panel3.Controls.Add(label5);
			panel3.Controls.Add(buttonCH2Plus);
			panel3.Controls.Add(buttonCH2Minus);
			panel3.Controls.Add(textBoxCH2Fine);
			panel3.Controls.Add(label4);
			panel3.Dock = DockStyle.Right;
			panel3.Location = new Point(701, 67);
			panel3.Name = "panel3";
			panel3.Size = new Size(557, 657);
			panel3.TabIndex = 3;
			// 
			// checkBoxCH2
			// 
			checkBoxCH2.AutoSize = true;
			checkBoxCH2.Location = new Point(19, 6);
			checkBoxCH2.Name = "checkBoxCH2";
			checkBoxCH2.Size = new Size(177, 29);
			checkBoxCH2.TabIndex = 9;
			checkBoxCH2.Text = "Kanal 2 - Aktiviert";
			checkBoxCH2.UseVisualStyleBackColor = true;
			checkBoxCH2.CheckedChanged += checkBoxCH2_CheckedChanged;
			// 
			// buttonCH2Read
			// 
			buttonCH2Read.Location = new Point(296, 250);
			buttonCH2Read.Name = "buttonCH2Read";
			buttonCH2Read.Size = new Size(249, 34);
			buttonCH2Read.TabIndex = 7;
			buttonCH2Read.Text = "Lese Wert";
			buttonCH2Read.UseVisualStyleBackColor = true;
			buttonCH2Read.Click += buttonCH2Read_Click;
			// 
			// buttonCH2Send
			// 
			buttonCH2Send.Location = new Point(296, 108);
			buttonCH2Send.Name = "buttonCH2Send";
			buttonCH2Send.Size = new Size(249, 34);
			buttonCH2Send.TabIndex = 6;
			buttonCH2Send.Text = "Sende Spannung";
			buttonCH2Send.UseVisualStyleBackColor = true;
			buttonCH2Send.Click += buttonCH2Send_Click;
			// 
			// textBoxCH2Current
			// 
			textBoxCH2Current.Location = new Point(296, 213);
			textBoxCH2Current.Name = "textBoxCH2Current";
			textBoxCH2Current.ReadOnly = true;
			textBoxCH2Current.Size = new Size(249, 31);
			textBoxCH2Current.TabIndex = 5;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			label5.Location = new Point(398, 182);
			label5.Name = "label5";
			label5.Size = new Size(147, 28);
			label5.TabIndex = 4;
			label5.Text = "Kanal 2 - Strom";
			// 
			// buttonCH2Plus
			// 
			buttonCH2Plus.Location = new Point(433, 68);
			buttonCH2Plus.Name = "buttonCH2Plus";
			buttonCH2Plus.Size = new Size(112, 34);
			buttonCH2Plus.TabIndex = 3;
			buttonCH2Plus.Text = "+";
			buttonCH2Plus.UseVisualStyleBackColor = true;
			buttonCH2Plus.Click += buttonCH2Plus_Click;
			// 
			// buttonCH2Minus
			// 
			buttonCH2Minus.Location = new Point(296, 68);
			buttonCH2Minus.Name = "buttonCH2Minus";
			buttonCH2Minus.Size = new Size(112, 34);
			buttonCH2Minus.TabIndex = 2;
			buttonCH2Minus.Text = "-";
			buttonCH2Minus.UseVisualStyleBackColor = true;
			buttonCH2Minus.Click += buttonCH2Minus_Click;
			// 
			// textBoxCH2Fine
			// 
			textBoxCH2Fine.Location = new Point(296, 31);
			textBoxCH2Fine.Name = "textBoxCH2Fine";
			textBoxCH2Fine.Size = new Size(249, 31);
			textBoxCH2Fine.TabIndex = 1;
			textBoxCH2Fine.Text = "24";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
			label4.Location = new Point(323, 0);
			label4.Name = "label4";
			label4.Size = new Size(225, 28);
			label4.TabIndex = 0;
			label4.Text = "Kanal 2 - Feineinstellung";
			// 
			// timerRender
			// 
			timerRender.Interval = 500;
			// 
			// FormModule01
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.Control;
			ClientSize = new Size(1258, 724);
			Controls.Add(panel3);
			Controls.Add(panel2);
			Controls.Add(panel1);
			Name = "FormModule01";
			StartPosition = FormStartPosition.CenterScreen;
			panel1.ResumeLayout(false);
			panel2.ResumeLayout(false);
			panel2.PerformLayout();
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private Label labelTitle;
		private Panel panel1;
		private Panel panel2;
		private Panel panel3;
		private Label label2;
		private Label label1;
		private ListBox listBoxCH1PositionAmplifier;
		private ListBox listBoxCH1ValveControl;
		private ListBox listBoxCH1Temperature;
		private Label label3;
		private Button buttonCH1Plus;
		private Button buttonCH1Minus;
		private TextBox textBoxCH1Fine;
		private Label labelCH1Fine;
		private Button buttonCH1Send;
		private Button buttonCH2Send;
		private TextBox textBoxCH2Current;
		private Label label5;
		private Button buttonCH2Plus;
		private Button buttonCH2Minus;
		private TextBox textBoxCH2Fine;
		private Label label4;
		private Label label6;
		private TextBox textBoxCH1Current;
		private Button buttonCH1Read;
		private Button buttonCH2Read;
		private CheckBox checkBoxCH1;
		private CheckBox checkBoxCH2;
		public System.Windows.Forms.Timer timerRender;
	}
}
