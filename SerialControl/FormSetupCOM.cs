using System.Diagnostics;

namespace SerialControl
{
	public partial class FormSetupCOM : Form
	{
		public FormSetupCOM()
		{
			InitializeComponent();
			comboBoxPortNames.Items.AddRange(SerialConnection.GetPortNames());
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			object? selectedPortItem = comboBoxPortNames.SelectedItem;

			try
			{
				if (selectedPortItem != null)
				{
					string portname = selectedPortItem.ToString();
					int baudrate = Convert.ToInt32(textBoxBaudrate.Text);
					int stopbits = Convert.ToInt32(textBoxStopBits.Text);
					int databits = Convert.ToInt32(textBoxDataBits.Text);

					Debug.WriteLine($"===> Port Settings <===\nPortName: {portname}\nBaudrate: {baudrate}\nStopBits: {stopbits}\nDataBits: {databits}");
					SerialConnection.SelectedPortName = portname;
					SerialConnection.SelectedBaudrate = baudrate;
					SerialConnection.SelectedStopBits = stopbits;
					SerialConnection.SelectedDataBits = databits;
					return;
				}
				else
				{
					MessageBox.Show("Port nicht ausgewählt", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonExit_Click(object sender, EventArgs e)
		{
			System.Environment.Exit(0);
		}
	}
}
