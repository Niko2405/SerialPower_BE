using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
				Logger.Write("Get Property of Channel1Active to: " + _channel1active, Logger.StatusCode.DEBUG);
				return _channel1active;
			}
			set
			{
				_channel1active = value;
				Logger.Write("Set Property of Channel1Active to: " + value, Logger.StatusCode.DEBUG);
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
				Logger.Write("Get Property of Channel2Active to: " + _channel2active, Logger.StatusCode.DEBUG);
				return _channel2active;
			}
			set
			{
				_channel2active = value;
				Logger.Write("Set Property of Channel2Active to: " + value, Logger.StatusCode.DEBUG);
				if (value)
				{
					ImagePowerCH2.Source = (ImageSource)this.TryFindResource("ImagePowerOn");
					SerialSender.SetChannelState(SerialSender.Channel.CH2, SerialSender.State.ON);
					return;
				}
				else
				{
					ImagePowerCH2.Source = (ImageSource)this.TryFindResource("ImagePowerOff");
					SerialSender.SetChannelState(SerialSender.Channel.CH2, SerialSender.State.OFF);
					return;
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

			Logger.Write($"Convert object [{selectedItem}] => [{dataVoltage}] and [{dataCurrent}]", Logger.StatusCode.DEBUG);
			return Tuple.Create(dataVoltage, dataCurrent);
		}

		private void ButtonCH1_Click(object sender, RoutedEventArgs e)
		{
			Logger.Write("Send current data to Channel 1", Logger.StatusCode.INFO);
			try
			{
				float voltage = float.Parse(TextBox_CH1Voltage.Text.Replace('.', ','));
				float current = float.Parse(TextBox_CH1Current.Text.Replace('.', ','));
				SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH1);
			}
			catch (Exception)
			{
				Logger.Write("Input is invalid", Logger.StatusCode.ERROR);
				MessageBox.Show("Your input is not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void ButtonCH2_Click(object sender, RoutedEventArgs e)
		{
			Logger.Write("Send current data to Channel 2", Logger.StatusCode.INFO);
			try
			{
				float voltage = float.Parse(TextBox_CH2Voltage.Text.Replace('.', ','));
				float current = float.Parse(TextBox_CH2Current.Text.Replace('.', ','));
				SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH2);
			}
			catch (Exception)
			{
				Logger.Write("Input is invalid", Logger.StatusCode.ERROR);
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

				if (voltageCH1.StartsWith("V1") && voltageCH2.StartsWith("V2") && currentCH1.StartsWith("I1") && currentCH2.StartsWith("I2"))
				{
					voltageCH1 = voltageCH1.Split(' ')[1];
					currentCH1 = currentCH1.Split(' ')[1];
					voltageCH2 = voltageCH2.Split(' ')[1];
					currentCH2 = currentCH2.Split(' ')[1];
					Logger.Write("Get last given data from device", Logger.StatusCode.INFO);

					Logger.Write($"[CH1] Voltage = {voltageCH1}", Logger.StatusCode.INFO);
					Logger.Write($"[CH1] Current = {currentCH1}", Logger.StatusCode.INFO);
					Logger.Write($"[CH2] Voltage = {voltageCH2}", Logger.StatusCode.INFO);
					Logger.Write($"[CH2] Current = {currentCH2}", Logger.StatusCode.INFO);

					// Remove V1 or I1 from data (V1 5.45 => 5.45)
					TextBox_CH1Voltage.Text = voltageCH1;
					TextBox_CH1Current.Text = currentCH1;

					TextBox_CH2Voltage.Text = voltageCH2;
					TextBox_CH2Current.Text = currentCH2;
					Logger.Write("Insert data into textboxes", Logger.StatusCode.INFO);
					return;
				}
				else
				{
					Logger.Write("Reading data from device", Logger.StatusCode.ERROR);
				}
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
						Logger.Write($"Set voltage of CH1 to {voltage}V", Logger.StatusCode.INFO);

						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);
						Logger.Write($"Set current of CH1 to {current}A", Logger.StatusCode.INFO);

						TextBox_CH1Voltage.Text = voltage.ToString();
						TextBox_CH1Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH1);

						// clear selection
						ListBoxCH1Presets33.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Write("Input is invalid", Logger.StatusCode.ERROR);
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
						Logger.Write($"Set voltage of CH1 to {voltage}V", Logger.StatusCode.INFO);

						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);
						Logger.Write($"Set current of CH1 to {current}A", Logger.StatusCode.INFO);

						TextBox_CH1Voltage.Text = voltage.ToString();
						TextBox_CH1Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH1);

						// clear selection
						ListBoxCH1Presets50.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Write("Input is invalid", Logger.StatusCode.ERROR);
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
						Logger.Write($"Set voltage of CH1 to {voltage}V", Logger.StatusCode.INFO);

						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);
						Logger.Write($"Set current of CH1 to {current}A", Logger.StatusCode.INFO);

						TextBox_CH1Voltage.Text = voltage.ToString();
						TextBox_CH1Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH1);

						// clear selection
						ListBoxCH1Presets120.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Write("Input is invalid", Logger.StatusCode.ERROR);
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
						Logger.Write($"Set voltage of CH1 to {voltage}V", Logger.StatusCode.INFO);

						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);
						Logger.Write($"Set current of CH1 to {current}A", Logger.StatusCode.INFO);

						TextBox_CH1Voltage.Text = voltage.ToString();
						TextBox_CH1Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH1);

						// clear selection
						ListBoxCH1Presets240.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Write("Input is invalid", Logger.StatusCode.ERROR);
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
						Logger.Write($"Set voltage of CH2 to {voltage}V", Logger.StatusCode.INFO);

						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);
						Logger.Write($"Set current of CH2 to {current}A", Logger.StatusCode.INFO);

						TextBox_CH2Voltage.Text = voltage.ToString();
						TextBox_CH2Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH2);

						// clear selection
						ListBoxCH2Presets33.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Write("Input is invalid", Logger.StatusCode.ERROR);
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
						Logger.Write($"Set voltage of CH2 to {voltage}V", Logger.StatusCode.INFO);

						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);
						Logger.Write($"Set current of CH2 to {current}A", Logger.StatusCode.INFO);

						TextBox_CH2Voltage.Text = voltage.ToString();
						TextBox_CH2Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH2);

						// clear selection
						ListBoxCH2Presets50.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Write("Input is invalid", Logger.StatusCode.ERROR);
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
						Logger.Write($"Set voltage of CH2 to {voltage}V", Logger.StatusCode.INFO);

						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);
						Logger.Write($"Set current of CH2 to {current}A", Logger.StatusCode.INFO);

						TextBox_CH2Voltage.Text = voltage.ToString();
						TextBox_CH2Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH2);

						// clear selection
						ListBoxCH2Presets120.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Write("Input is invalid", Logger.StatusCode.ERROR);
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
						Logger.Write($"Set voltage of CH2 to {voltage}V", Logger.StatusCode.INFO);

						float current = float.Parse(ConvertListBoxItemData(selectedItem).Item2);
						Logger.Write($"Set current of CH2 to {current}A", Logger.StatusCode.INFO);

						TextBox_CH2Voltage.Text = voltage.ToString();
						TextBox_CH2Current.Text = current.ToString();

						// send
						SerialSender.SetPowerSupplyValues(voltage, current, SerialSender.Channel.CH2);

						// clear selection
						ListBoxCH2Presets240.SelectedItem = null;
					}
					catch (Exception)
					{
						Logger.Write("Input is invalid", Logger.StatusCode.ERROR);
						MessageBox.Show("Your input is not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
		}
		#endregion

		#region Options
		private void CheckBoxSyncSwitch_Checked(object sender, RoutedEventArgs e)
		{
			Logger.Write("Synchronous activated", Logger.StatusCode.INFO);
			SerialSender.SetChannelState(SerialSender.State.ON);
			Channel1Active = true;
			Channel2Active = true;
		}

		private void CheckBoxSyncSwitch_Unchecked(object sender, RoutedEventArgs e)
		{
			Logger.Write("Synchronous deactivated", Logger.StatusCode.INFO);
			SerialSender.SetChannelState(SerialSender.State.OFF);
			Channel1Active = false;
			Channel2Active = false;
		}
		#endregion
	}
}
