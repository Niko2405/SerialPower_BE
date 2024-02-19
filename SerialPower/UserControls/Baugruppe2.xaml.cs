using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
	/// Interaktionslogik für Baugruppe2.xaml
	/// </summary>
	public partial class Baugruppe2 : UserControl
	{
		public Baugruppe2()
		{
			InitializeComponent();
		}

		private static readonly double STEPS = 0.010d;

		/// <summary>
		/// Sende eingestellte Spannung am Kanal 2
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonCH2_SendVoltage_Click(object sender, RoutedEventArgs e)
		{
			string voltage = TextBoxCH2Voltage.Text;
			voltage = voltage.Replace(",", ".");
			Debug.WriteLine($"[CH2] Set voltage to [{voltage}]");
			SerialSender.SendCommand($"V2 {voltage}");
		}

		/// <summary>
		/// Sende eingestellte Spannung am Kanal 1
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonCH1_SendVoltage_Click(object sender, RoutedEventArgs e)
		{
			string voltage = TextBoxCH1Voltage.Text;
			voltage = voltage.Replace(",", ".");
			Debug.WriteLine($"[CH1] Set voltage to [{voltage}]");
			SerialSender.SendCommand($"V1 {voltage}");
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
				double currentValue = float.Parse(TextBoxCH1Voltage.Text.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);

				// Wert um 0,001 verringern
				double newValue = currentValue - STEPS;
				newValue = Math.Round(newValue, 3);
				Debug.WriteLine($"{currentValue} => {newValue}");
				TextBoxCH1Voltage.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
				double currentValue = float.Parse(TextBoxCH1Voltage.Text.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);

				// Wert um 0,001 verringern
				double newValue = currentValue + STEPS;
				newValue = Math.Round(newValue, 3);
				Debug.WriteLine($"{currentValue} => {newValue}");
				TextBoxCH1Voltage.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
				double currentValue = float.Parse(TextBoxCH2Voltage.Text.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);

				// Wert um 0,001 verringern
				double newValue = currentValue - STEPS;
				newValue = Math.Round(newValue, 3);
				Debug.WriteLine($"{currentValue} => {newValue}");
				TextBoxCH2Voltage.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
				double currentValue = float.Parse(TextBoxCH2Voltage.Text.Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);

				// Wert um 0,001 verringern
				double newValue = currentValue + STEPS;
				newValue = Math.Round(newValue, 3);
				Debug.WriteLine($"{currentValue} => {newValue}");
				TextBoxCH2Voltage.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ListBox_CH1Temp_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				string selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					// Remove System.ListBox:
					string command = ConvertListBoxItemToCommand(selectedItem);
					Debug.WriteLine($"Set voltage of CH1 to {command}V");

					// Set TextBox CH1
					TextBoxCH1Voltage.Text = command;
					SerialSender.SendCommand($"V1 {command}");
				}
			}
		}

		private void ListBox_CH1Pos_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				string selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					// Remove System.ListBox:
					string command = ConvertListBoxItemToCommand(selectedItem);
					Debug.WriteLine($"Set voltage of CH1 to {command}V");

					// Set TextBox CH1
					TextBoxCH1Voltage.Text = command;
					SerialSender.SendCommand($"V1 {command}");
				}
			}
		}

		private void ListBox_CH1Valve_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				string selectedItem = e.AddedItems[0].ToString();
				if (selectedItem != null)
				{
					// Remove System.ListBox:
					string command = ConvertListBoxItemToCommand(selectedItem);	
					Debug.WriteLine($"Set voltage of CH1 to {command}V");

					// Set TextBox CH1
					TextBoxCH1Voltage.Text = command;
					SerialSender.SendCommand($"V1 {command}");
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
			Debug.WriteLine($"Convert object '{selectedItem}' to '{command}'");
			return command;
		}

		private void CheckBoxCH1_Checked(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("CH1 Online");
			SerialSender.SendCommand("OP1 1");
		}

		private void CheckBoxCH2_Checked(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("CH2 Online");
			SerialSender.SendCommand("OP2 1");
		}

		private void CheckBoxCH2_Unchecked(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("CH1 Offline");
			SerialSender.SendCommand("OP2 0");
		}

		private void CheckBoxCH1_Unchecked(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("CH1 Offline");
			SerialSender.SendCommand("OP1 0");
		}
		public void StartUpdateCH2Current()
		{
			/*
			string data = string.Empty;
			while (true)
			{
				Thread.Sleep(1000);
				if (SerialSender.SelectedPortName != string.Empty)
				{
					Debug.WriteLine("Ask for current");
					data = SerialSender.ReadNextCommand("I2O?");
					Debug.WriteLine($"Current: {data}");
					this.Dispatcher.Invoke(() =>
					{
						TextBoxCH2Current.Text = data;
					});
				}
			}
			*/
		}
	}
}
