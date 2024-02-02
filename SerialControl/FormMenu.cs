using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialControl
{
	public partial class FormMenu : Form
	{
		public FormMenu()
		{
			InitializeComponent();
		}

		private void buttonBaugruppe01_Click(object sender, EventArgs e)
		{
			Program.SelectedModule = "Module01";
			this.Dispose();
		}

		private void buttonBaugruppe02_Click(object sender, EventArgs e)
		{
			Program.SelectedModule = "Module02";
			this.Dispose();
		}
	}
}
