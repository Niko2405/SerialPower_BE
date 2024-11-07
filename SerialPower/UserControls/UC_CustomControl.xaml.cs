using System.Windows;
using System.Windows.Controls;

namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für UC_CustomControl.xaml
	/// </summary>
	public partial class UC_CustomControl : UserControl
	{
		private static readonly int SELECT_DELAY = 1;

		public UC_CustomControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Removes V and A; Replace , to .
		/// </summary>
		/// <param name="rawData"></param>
		/// <returns></returns>
		private static string TrimData(string rawData)
		{
			return rawData.Replace(",", ".").Replace("V", "").Replace(" ", "").Replace("A", "");
		}

		/// <summary>
		/// Convert the data from listbox to useable command
		/// </summary>
		/// <param name="selectedItem"></param>
		/// <returns>Voltage and current of the listbox</returns>
		private static Tuple<string, string> ConvertListBoxItemData(string selectedItem)
		{
			// Remove System.ListBox:
			string data = selectedItem.Split(":")[1].Trim();

			// 12.000V - 0.100A => V1 12.000; I1 0.100A
			string dataVoltage = data.Split(" - ")[0].Trim();
			string dataCurrent = data.Split(" - ")[1].Trim();

			dataVoltage = dataVoltage.Replace("V", "");
			dataCurrent = dataCurrent.Replace("A", "");

			Logger.PrintStatus($"Convert object '{selectedItem}' => '{dataVoltage}' and '{dataCurrent}'", Logger.StatusCode.INFO);
			return Tuple.Create(dataVoltage, dataCurrent);
		}

		private void CheckBoxCH1_Checked(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Channel 1 - Online");
			SerialSender.SendData("OP1 1");
		}

		private void CheckBoxCH2_Checked(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Channel 2 - Online");
			SerialSender.SendData("OP2 1");
		}

		private void CheckBoxCH2_Unchecked(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Channel 2 - Offline");
			SerialSender.SendData("OP2 0");
		}

		private void CheckBoxCH1_Unchecked(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Channel 1 - Offline");
			SerialSender.SendData("OP1 0");
		}

		private void ButtonCH1_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Send data to Channel 1");

			// voltage setter
			string voltage = TextBox_CH1Voltage.Text;
			voltage = TrimData(voltage);
			SerialSender.SendData($"V1 {voltage}");

			// current setter
			string current = TextBox_CH1Current.Text;
			current = TrimData(current);
			SerialSender.SendData($"I1 {current}");
		}

		private void ButtonCH2_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Send data to Channel 2");

			// voltage setter
			string voltage = TextBox_CH2Voltage.Text;
			voltage = TrimData(voltage);
			SerialSender.SendData($"V2 {voltage}");

			// current setter
			string current = TextBox_CH2Current.Text;
			current = TrimData(current);
			SerialSender.SendData($"I2 {current}");
		}

		private void VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			// If current window is visible
			if (Convert.ToBoolean(e.NewValue.ToString()))
			{
				// get last data from device
				string voltageCH1 = SerialSender.SendDataAndRecv("V1?"); // V1 5.45 ...
				string currentCH1 = SerialSender.SendDataAndRecv("I1?"); // I1 2.15 ...
				string voltageCH2 = SerialSender.SendDataAndRecv("V2?");
				string currentCH2 = SerialSender.SendDataAndRecv("I2?");

				if (voltageCH1.StartsWith("V1") && voltageCH2.StartsWith("V2") && currentCH1.StartsWith("I1") && currentCH2.StartsWith("I2"))
				{
					voltageCH1 = voltageCH1.Split(' ')[1];
					currentCH1 = currentCH1.Split(' ')[1];
					voltageCH2 = voltageCH2.Split(' ')[1];
					currentCH2 = currentCH2.Split(' ')[1];
					Logger.PrintStatus("Get last given data from device", Logger.StatusCode.OK);

					Logger.PrintStatus($"[CH1] Voltage = {voltageCH1}", Logger.StatusCode.INFO);
					Logger.PrintStatus($"[CH1] Current = {currentCH1}", Logger.StatusCode.INFO);
					Logger.PrintStatus($"[CH2] Voltage = {voltageCH2}", Logger.StatusCode.INFO);
					Logger.PrintStatus($"[CH2] Current = {currentCH2}", Logger.StatusCode.INFO);

					// Remove V1 or I1 from data (V1 5.45 => 5.45)
					TextBox_CH1Voltage.Text = voltageCH1;
					TextBox_CH2Voltage.Text = voltageCH2;
					TextBox_CH1Current.Text = currentCH1;
					TextBox_CH2Current.Text = currentCH2;
					Logger.PrintStatus("Insert data into textboxes", Logger.StatusCode.OK);
					return;
				}
				else
				{
					Logger.PrintStatus("Reading data from device", Logger.StatusCode.FAILED);
				}
			}
		}
		#region Presets
		private void ListBoxCH1Presets33_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(SELECT_DELAY);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 1 - 3.300V");

					// Set voltage
					string commandVoltage = ConvertListBoxItemData(selectedItem).Item1;
					Logger.PrintStatus($"Set voltage of CH1 to {commandVoltage}V", Logger.StatusCode.OK);

					TextBox_CH1Voltage.Text = commandVoltage;
					SerialSender.SendData($"V1 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH1 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH1Current.Text = commandCurrent;
					SerialSender.SendData($"I1 {commandCurrent}");

					// clear selection
					ListBoxCH1Presets33.SelectedItem = null;
				}
			}
		}

		private void ListBoxCH1Presets50_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(SELECT_DELAY);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 1 - 5.000V");

					// Set voltage
					string commandVoltage = ConvertListBoxItemData(selectedItem).Item1;
					Logger.PrintStatus($"Set voltage of CH1 to {commandVoltage}V", Logger.StatusCode.OK);

					TextBox_CH1Voltage.Text = commandVoltage;
					SerialSender.SendData($"V1 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH1 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH1Current.Text = commandCurrent;
					SerialSender.SendData($"I1 {commandCurrent}");

					// clear selection
					ListBoxCH1Presets50.SelectedItem = null;
				}
			}
		}

		private void ListBoxCH1Presets120_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(SELECT_DELAY);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 1 - 12.000V");

					// Set voltage
					string commandVoltage = ConvertListBoxItemData(selectedItem).Item1;
					Logger.PrintStatus($"Set voltage of CH1 to {commandVoltage}V", Logger.StatusCode.OK);

					TextBox_CH1Voltage.Text = commandVoltage;
					SerialSender.SendData($"V1 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH1 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH1Current.Text = commandCurrent;
					SerialSender.SendData($"I1 {commandCurrent}");

					// clear selection
					ListBoxCH1Presets120.SelectedItem = null;
				}
			}
		}

		private void ListBoxCH1Presets180_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(SELECT_DELAY);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 1 - 18.000V");

					// Set voltage
					string commandVoltage = ConvertListBoxItemData(selectedItem).Item1;
					Logger.PrintStatus($"Set voltage of CH1 to {commandVoltage}V", Logger.StatusCode.OK);

					TextBox_CH1Voltage.Text = commandVoltage;
					SerialSender.SendData($"V1 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH1 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH1Current.Text = commandCurrent;
					SerialSender.SendData($"I1 {commandCurrent}");

					// clear selection
					ListBoxCH1Presets180.SelectedItem = null;
				}
			}
		}

		private void ListBoxCH1Presets240_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(SELECT_DELAY);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 1 - 24.000V");

					// Set voltage
					string commandVoltage = ConvertListBoxItemData(selectedItem).Item1;
					Logger.PrintStatus($"Set voltage of CH1 to {commandVoltage}V", Logger.StatusCode.OK);

					TextBox_CH1Voltage.Text = commandVoltage;
					SerialSender.SendData($"V1 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH1 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH1Current.Text = commandCurrent;
					SerialSender.SendData($"I1 {commandCurrent}");

					// clear selection
					ListBoxCH1Presets240.SelectedItem = null;
				}
			}
		}

		private void ListBoxCH2Presets33_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(SELECT_DELAY);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 2 - 3.300V");

					// Set voltage
					string commandVoltage = ConvertListBoxItemData(selectedItem).Item1;
					Logger.PrintStatus($"Set voltage of CH2 to {commandVoltage}V", Logger.StatusCode.OK);

					TextBox_CH2Voltage.Text = commandVoltage;
					SerialSender.SendData($"V2 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH2 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH2Current.Text = commandCurrent;
					SerialSender.SendData($"I2 {commandCurrent}");

					// clear selection
					ListBoxCH2Presets33.SelectedItem = null;
				}
			}
		}

		private void ListBoxCH2Presets50_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(SELECT_DELAY);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 2 - 5.000V");

					// Set voltage
					string commandVoltage = ConvertListBoxItemData(selectedItem).Item1;
					Logger.PrintStatus($"Set voltage of CH2 to {commandVoltage}V", Logger.StatusCode.OK);

					TextBox_CH2Voltage.Text = commandVoltage;
					SerialSender.SendData($"V2 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH2 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH2Current.Text = commandCurrent;
					SerialSender.SendData($"I2 {commandCurrent}");

					// clear selection
					ListBoxCH2Presets50.SelectedItem = null;
				}
			}
		}

		private void ListBoxCH2Presets120_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(SELECT_DELAY);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 2 - 12.000V");

					// Set voltage
					string commandVoltage = ConvertListBoxItemData(selectedItem).Item1;
					Logger.PrintStatus($"Set voltage of CH2 to {commandVoltage}V", Logger.StatusCode.OK);

					TextBox_CH2Voltage.Text = commandVoltage;
					SerialSender.SendData($"V2 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH2 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH2Current.Text = commandCurrent;
					SerialSender.SendData($"I2 {commandCurrent}");

					// clear selection
					ListBoxCH2Presets120.SelectedItem = null;
				}
			}
		}

		private void ListBoxCH2Presets180_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(SELECT_DELAY);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 2 - 18.000V");

					// Set voltage
					string commandVoltage = ConvertListBoxItemData(selectedItem).Item1;
					Logger.PrintStatus($"Set voltage of CH2 to {commandVoltage}V", Logger.StatusCode.OK);

					TextBox_CH2Voltage.Text = commandVoltage;
					SerialSender.SendData($"V2 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH2 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH2Current.Text = commandCurrent;
					SerialSender.SendData($"I2 {commandCurrent}");

					// clear selection
					ListBoxCH2Presets180.SelectedItem = null;
				}
			}
		}

		private void ListBoxCH2Presets240_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Thread.Sleep(SELECT_DELAY);
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 2 - 24.000V");

					// Set voltage
					string commandVoltage = ConvertListBoxItemData(selectedItem).Item1;
					Logger.PrintStatus($"Set voltage of CH2 to {commandVoltage}V", Logger.StatusCode.OK);

					TextBox_CH2Voltage.Text = commandVoltage;
					SerialSender.SendData($"V2 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH2 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH2Current.Text = commandCurrent;
					SerialSender.SendData($"I2 {commandCurrent}");

					// clear selection
					ListBoxCH2Presets240.SelectedItem = null;
				}
			}
		}
		#endregion
	}
}
