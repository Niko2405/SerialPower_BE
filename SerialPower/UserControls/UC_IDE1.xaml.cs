using System.Globalization;
using System.Windows;
using System.Windows.Controls;


namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für UC_IDE1.xaml
	/// </summary>
	public partial class UC_IDE1 : UserControl
	{
		private static readonly float STEPS = 0.010f;

		public UC_IDE1()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Sende eingestellte Spannung am Kanal 2
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonCH2_SendVoltage_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("CH2 - Send voltage");
			string voltage = TextBoxCH2Voltage.Text;
			voltage = voltage.Replace(",", ".");
			Logger.PrintStatus($"[CH2] Set voltage to [{voltage}]", Logger.StatusCode.OK);
			SerialSender.SendData($"V2 {voltage}");
		}

		/// <summary>
		/// Sende eingestellte Spannung am Kanal 1
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonCH1_SendVoltage_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("CH1 - Send voltage");
			string voltage = TextBoxCH1Voltage.Text;
			voltage = voltage.Replace(",", ".");
			Logger.PrintStatus($"[CH1] Set voltage to [{voltage}]", Logger.StatusCode.OK);
			SerialSender.SendData($"V1 {voltage}");
		}

		/// <summary>
		/// Spannungswert bei Kanal 1 um 0.01 verringern
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonMinusCH1_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// Komma durch Punkt ersetzen
				float currentValue = float.Parse(TextBoxCH1Voltage.Text.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);

				// Wert um 0,001 verringern
				float newValue = currentValue - STEPS;
				newValue = (float)Math.Round(newValue, 3);
				Logger.PrintStatus($"{currentValue} => {newValue}", Logger.StatusCode.INFO);
				TextBoxCH1Voltage.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		/// <summary>
		/// Spannungswert bei Kanal 1 um STEPS erhöhen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonPlusCH1_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// Komma durch Punkt ersetzen
				float currentValue = float.Parse(TextBoxCH1Voltage.Text.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);

				// Wert um 0,001 verringern
				float newValue = currentValue + STEPS;
				newValue = (float)Math.Round(newValue, 3);
				Logger.PrintStatus($"{currentValue} => {newValue}", Logger.StatusCode.INFO);
				TextBoxCH1Voltage.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		/// <summary>
		/// Spannungswert bei Kanal 2 um STEPS verringern
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonMinusCH2_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// Komma durch Punkt ersetzen
				float currentValue = float.Parse(TextBoxCH2Voltage.Text.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);

				// Wert um 0,001 verringern
				float newValue = currentValue - STEPS;
				newValue = (float)Math.Round(newValue, 3);
				Logger.PrintStatus($"{currentValue} => {newValue}", Logger.StatusCode.INFO);
				TextBoxCH2Voltage.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		/// <summary>
		/// Spannungswert bei Kanal 2 um 0.01 erhöhen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonPlusCH2_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// Komma durch Punkt ersetzen
				float currentValue = float.Parse(TextBoxCH2Voltage.Text.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);

				// Wert um 0,001 verringern
				float newValue = currentValue + STEPS;
				newValue = (float)Math.Round(newValue, 3);
				Logger.PrintStatus($"{currentValue} => {newValue}", Logger.StatusCode.INFO);
				TextBoxCH2Voltage.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ListBox_CH1Temp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(100);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					// Remove System.ListBox:
					string command = ConvertListBoxItemToCommand(selectedItem);
					Logger.PrintStatus($"Set voltage of CH1 to {command}V", Logger.StatusCode.OK);

					// Set TextBox CH1
					TextBoxCH1Voltage.Text = command;
					SerialSender.SendData($"V1 {command}");
				}
			}
		}

		private void ListBox_CH1Pos_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(100);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					// Remove System.ListBox:
					string command = ConvertListBoxItemToCommand(selectedItem);
					Logger.PrintStatus($"Set voltage of CH1 to {command}V", Logger.StatusCode.OK);

					// Set TextBox CH1
					TextBoxCH1Voltage.Text = command;
					SerialSender.SendData($"V1 {command}");
				}
			}
		}

		private void ListBox_CH1Valve_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(100);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					// Remove System.ListBox:
					string command = ConvertListBoxItemToCommand(selectedItem);
					Logger.PrintStatus($"Set voltage of CH1 to {command}V", Logger.StatusCode.OK);

					// Set TextBox CH1
					TextBoxCH1Voltage.Text = command;
					SerialSender.SendData($"V1 {command}");
				}
			}
		}

		/// <summary>
		/// Convert the raw list box item to send able command
		/// </summary>
		/// <param name="selectedItem"></param>
		/// <returns></returns>
		private static string ConvertListBoxItemToCommand(string selectedItem)
		{
			string command = selectedItem.Split(":")[1].Trim();
			command = command.Replace(",", ".").Replace("V", "").Replace("set ", "");
			Logger.PrintStatus($"Convert object '{selectedItem}' to '{command}'", Logger.StatusCode.OK);
			return command;
		}

		private void CheckBoxCH1_Checked(object sender, RoutedEventArgs e)
		{
			SerialSender.SendData("I1 0.20; I2 0.20");
			SerialSender.SendData("OP1 1");
		}

		private void CheckBoxCH2_Checked(object sender, RoutedEventArgs e)
		{
			SerialSender.SendData("I1 0.20; I2 0.20");
			SerialSender.SendData("OP2 1");
		}

		private void CheckBoxCH2_Unchecked(object sender, RoutedEventArgs e)
		{
			SerialSender.SendData("OP2 0");
		}

		private void CheckBoxCH1_Unchecked(object sender, RoutedEventArgs e)
		{
			SerialSender.SendData("OP1 0");
		}
	}
}
