using System.Diagnostics;
using System.Globalization;

namespace SerialControl
{
	public partial class FormModule01 : Form
	{
		private static readonly double STEPS = 0.01d;

		public FormModule01()
		{
			InitializeComponent();
			timerRender.Start();
			timerRender.Tick += Render_Tick;
		}

		private void RenderCurrent(string channel)
		{
			Debug.WriteLine($"Update current on channel {channel}");
			if (channel.Contains("CH1"))
			{
				textBoxCH1Current.Text = SerialConnection.SendCommand("I1O?", true);
			}
			else if (channel.Contains("CH2"))
			{
				textBoxCH2Current.Text = SerialConnection.SendCommand("I2O?", true);
			}
		}

		/// <summary>
		/// Convert the data to command
		/// </summary>
		/// <param name="selectedItem"></param>
		/// <returns>Returns the command</returns>
		private static string ConvertListBoxItemToCommand(string selectedItem)
		{
			string command = selectedItem.Replace(",", ".").Replace("V", "").Replace("set ", "");
			Debug.WriteLine($"Convert object from '{selectedItem}' to '{command}'");
			return command;
		}

		private void listBoxCH1ValveControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxCH1ValveControl.SelectedItem != null)
			{
				string? selectedValue = listBoxCH1ValveControl.SelectedItem.ToString();
				string? value = ConvertListBoxItemToCommand(selectedValue);
				if (value != null)
				{
					SerialConnection.SendCommand($"V1 {value}");
					textBoxCH1Fine.Text = value;
					Debug.WriteLine($"[CH1 - ValveControl] set to {value}");
				}
			}
		}

		private void listBoxCH1PositionAmplifier_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxCH1PositionAmplifier.SelectedItem != null)
			{
				string? selectedValue = listBoxCH1PositionAmplifier.SelectedItem.ToString();
				string? value = ConvertListBoxItemToCommand(selectedValue);
				if (value != null)
				{
					SerialConnection.SendCommand($"V1 {value}");
					textBoxCH1Fine.Text = value;
					Debug.WriteLine($"[CH1 - Position Amplifier] set to {value}");
				}
			}
		}

		private void listBoxCH1Temperature_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxCH1Temperature.SelectedItem != null)
			{
				string? selectedValue = listBoxCH1Temperature.SelectedItem.ToString();
				string? value = ConvertListBoxItemToCommand(selectedValue);
				if (value != null)
				{
					SerialConnection.SendCommand($"V1 {value}");
					textBoxCH1Fine.Text = value;
					Debug.WriteLine($"[CH1 - Temperature] set to {value}");
				}
			}
		}

		private void buttonCH1Minus_Click(object sender, EventArgs e)
		{
			try
			{
				// Komma durch Punkt ersetzen
				double currentValue = double.Parse(textBoxCH1Fine.Text.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);

				// Wert um 0,001 verringern
				double newValue = currentValue - STEPS;
				newValue = Math.Round(newValue, 3);
				Debug.WriteLine($"Change textBoxCH1Fine from {currentValue} to {newValue}");
				textBoxCH1Fine.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonCH1Plus_Click(object sender, EventArgs e)
		{
			try
			{
				// Komma durch Punkt ersetzen
				double currentValue = double.Parse(textBoxCH1Fine.Text.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);

				// Wert um 0,001 erhöhen
				double newValue = currentValue + STEPS;
				newValue = Math.Round(newValue, 3);
				Debug.WriteLine($"Change textBoxCH1Fine from {currentValue} to {newValue}");
				textBoxCH1Fine.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonCH1Send_Click(object sender, EventArgs e)
		{
			string voltage = textBoxCH1Fine.Text;
			voltage = voltage.Replace(",", ".");
			Debug.WriteLine($"[CH1] Set voltage to [{voltage}]");
			SerialConnection.SendCommand($"V1 {voltage}");
		}

		private void buttonCH2Minus_Click(object sender, EventArgs e)
		{
			try
			{
				// Komma durch Punkt ersetzen
				double currentValue = double.Parse(textBoxCH2Fine.Text.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);

				// Wert um 0,001 verringern
				double newValue = currentValue - STEPS;
				newValue = Math.Round(newValue, 3);
				Debug.WriteLine($"Change textBoxCH2Fine value from {currentValue} to {newValue}");
				textBoxCH2Fine.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonCH2Plus_Click(object sender, EventArgs e)
		{
			try
			{
				// Komma durch Punkt ersetzen
				double currentValue = double.Parse(textBoxCH2Fine.Text.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);

				// Wert um 0,001 erhöhen
				double newValue = currentValue + STEPS;
				newValue = Math.Round(newValue, 3);
				Debug.WriteLine($"Change textBoxCH2Fine value from {currentValue} to {newValue}");
				textBoxCH2Fine.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void buttonCH2Send_Click(object sender, EventArgs e)
		{
			string voltage = textBoxCH2Fine.Text;
			voltage = voltage.Replace(",", ".");
			Debug.WriteLine($"[CH2] Set voltage to [{voltage}]");
			SerialConnection.SendCommand($"V2 {voltage}");
		}

		private void Render_Tick(object? sender, EventArgs e)
		{
			RenderCurrent("CH1");
			RenderCurrent("CH2");
			Debug.WriteLine("Update Current");
		}
		private void buttonCH1Read_Click(object sender, EventArgs e)
		{
			RenderCurrent("CH1");
		}

		private void buttonCH2Read_Click(object sender, EventArgs e)
		{
			RenderCurrent("CH2");
		}

		private void checkBoxCH1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBoxCH1.Checked)
			{
				Debug.WriteLine("CH1 Online");
				SerialConnection.SendCommand("OP1 1");
				return;
			}
			else
			{
				Debug.WriteLine("CH1 Offline");
				SerialConnection.SendCommand("OP1 0");
				return;
			}
		}

		private void checkBoxCH2_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBoxCH2.Checked)
			{
				Debug.WriteLine("CH2 Online");
				SerialConnection.SendCommand("OP2 1");
				return;
			}
			else
			{
				Debug.WriteLine("CH2 Offline");
				SerialConnection.SendCommand("OP2 0");
				return;
			}
		}
	}
}
