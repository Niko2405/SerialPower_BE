namespace SerialControl
{
	partial class FormMenu
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			flowLayoutPanel1 = new FlowLayoutPanel();
			buttonBaugruppe01 = new Button();
			buttonBaugruppe02 = new Button();
			buttonBaugruppe03 = new Button();
			flowLayoutPanel1.SuspendLayout();
			SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			flowLayoutPanel1.Controls.Add(buttonBaugruppe01);
			flowLayoutPanel1.Controls.Add(buttonBaugruppe02);
			flowLayoutPanel1.Controls.Add(buttonBaugruppe03);
			flowLayoutPanel1.Dock = DockStyle.Fill;
			flowLayoutPanel1.Location = new Point(0, 0);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Size = new Size(1488, 228);
			flowLayoutPanel1.TabIndex = 0;
			// 
			// buttonBaugruppe01
			// 
			buttonBaugruppe01.Location = new Point(3, 3);
			buttonBaugruppe01.Name = "buttonBaugruppe01";
			buttonBaugruppe01.Size = new Size(245, 45);
			buttonBaugruppe01.TabIndex = 0;
			buttonBaugruppe01.Text = "IDE - 85/10582";
			buttonBaugruppe01.UseVisualStyleBackColor = true;
			buttonBaugruppe01.Click += buttonBaugruppe01_Click;
			// 
			// buttonBaugruppe02
			// 
			buttonBaugruppe02.Location = new Point(254, 3);
			buttonBaugruppe02.Name = "buttonBaugruppe02";
			buttonBaugruppe02.Size = new Size(245, 45);
			buttonBaugruppe02.TabIndex = 1;
			buttonBaugruppe02.Text = "Ernst Leitz";
			buttonBaugruppe02.UseVisualStyleBackColor = true;
			buttonBaugruppe02.Click += buttonBaugruppe02_Click;
			// 
			// buttonBaugruppe03
			// 
			buttonBaugruppe03.Location = new Point(505, 3);
			buttonBaugruppe03.Name = "buttonBaugruppe03";
			buttonBaugruppe03.Size = new Size(245, 45);
			buttonBaugruppe03.TabIndex = 2;
			buttonBaugruppe03.Text = "button3";
			buttonBaugruppe03.UseVisualStyleBackColor = true;
			// 
			// FormMenu
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1488, 228);
			Controls.Add(flowLayoutPanel1);
			Name = "FormMenu";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "FormMenu";
			flowLayoutPanel1.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private FlowLayoutPanel flowLayoutPanel1;
		private Button buttonBaugruppe01;
		private Button buttonBaugruppe02;
		private Button buttonBaugruppe03;
	}
}