using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für UC_CustomControl.xaml
	/// </summary>
	public partial class UC_CustomControl : UserControl
	{
		private static readonly int SELECT_DELAY = 100;

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
			SerialSender.SendCommand("OP1 1");
		}

		private void CheckBoxCH2_Checked(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Channel 2 - Online");
			SerialSender.SendCommand("OP2 1");
		}

		private void CheckBoxCH2_Unchecked(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Channel 2 - Offline");
			SerialSender.SendCommand("OP2 0");
		}

		private void CheckBoxCH1_Unchecked(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Channel 1 - Offline");
			SerialSender.SendCommand("OP1 0");
		}

		private void ButtonCH1_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Send data to Channel 1");

			// voltage setter
			string voltage = TextBox_CH1Voltage.Text;
			voltage = TrimData(voltage);
			SerialSender.SendCommand($"V1 {voltage}");

			// current setter
			string current = TextBox_CH1Current.Text;
			current = TrimData(current);
			SerialSender.SendCommand($"I1 {current}");
		}

		private void ButtonCH2_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Send data to Channel 2");

			// voltage setter
			string voltage = TextBox_CH2Voltage.Text;
			voltage = TrimData(voltage);
			SerialSender.SendCommand($"V2 {voltage}");

			// current setter
			string current = TextBox_CH2Current.Text;
			current = TrimData(current);
			SerialSender.SendCommand($"I2 {current}");
		}

		private void VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			// If current window is visible
			if (Convert.ToBoolean(e.NewValue.ToString()))
			{
				// set auto voltage and current
				string voltageCH1 = SerialSender.SendCommand("V1?"); // V1 5.45 ...
				string voltageCH2 = SerialSender.SendCommand("V2?");
				string currentCH1 = SerialSender.SendCommand("I1?"); // I1 2.15 ...
				string currentCH2 = SerialSender.SendCommand("I2?");

				if (voltageCH1.StartsWith("V1") && voltageCH2.StartsWith("V2") && currentCH1.StartsWith("I1") && currentCH2.StartsWith("I2"))
				{
					// Remove V1 or I1 from data (V1 5.45 => 5.45)
					TextBox_CH1Voltage.Text = voltageCH1.Split(' ')[1];
					TextBox_CH2Voltage.Text = voltageCH2.Split(' ')[1];
					TextBox_CH1Current.Text = currentCH1.Split(' ')[1];
					TextBox_CH2Current.Text = currentCH2.Split(' ')[1];
					return;
				}
				else
				{
					Logger.PrintStatus("Reading data from target", Logger.StatusCode.FAILED);
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
					SerialSender.SendCommand($"V1 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH1 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH1Current.Text = commandCurrent;
					SerialSender.SendCommand($"I1 {commandCurrent}");

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
					SerialSender.SendCommand($"V1 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH1 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH1Current.Text = commandCurrent;
					SerialSender.SendCommand($"I1 {commandCurrent}");

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
					SerialSender.SendCommand($"V1 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH1 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH1Current.Text = commandCurrent;
					SerialSender.SendCommand($"I1 {commandCurrent}");

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
					SerialSender.SendCommand($"V1 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH1 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH1Current.Text = commandCurrent;
					SerialSender.SendCommand($"I1 {commandCurrent}");

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
					SerialSender.SendCommand($"V1 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH1 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH1Current.Text = commandCurrent;
					SerialSender.SendCommand($"I1 {commandCurrent}");

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
					SerialSender.SendCommand($"V2 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH2 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH2Current.Text = commandCurrent;
					SerialSender.SendCommand($"I2 {commandCurrent}");

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
					SerialSender.SendCommand($"V2 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH2 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH2Current.Text = commandCurrent;
					SerialSender.SendCommand($"I2 {commandCurrent}");

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
					SerialSender.SendCommand($"V2 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH2 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH2Current.Text = commandCurrent;
					SerialSender.SendCommand($"I2 {commandCurrent}");

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
					SerialSender.SendCommand($"V2 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH2 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH2Current.Text = commandCurrent;
					SerialSender.SendCommand($"I2 {commandCurrent}");

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
					SerialSender.SendCommand($"V2 {commandVoltage}");

					// Set current
					string commandCurrent = ConvertListBoxItemData(selectedItem).Item2;
					Logger.PrintStatus($"Set current of CH2 to {commandCurrent}A", Logger.StatusCode.OK);

					TextBox_CH2Current.Text = commandCurrent;
					SerialSender.SendCommand($"I2 {commandCurrent}");

					// clear selection
					ListBoxCH2Presets240.SelectedItem = null;
				}
			}
		}
		#endregion
	}
}
