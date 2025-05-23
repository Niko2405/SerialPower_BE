using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using TartarosLogger;

namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für UC_IDE1.xaml
	/// </summary>
	public partial class UC_IDE1 : UserControl
	{
		private static readonly float STEPS = 0.010f;
		private static readonly float CURRENT_LIMIT = 0.1f;

		public UC_IDE1()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Sende eingestellte Spannung am Kanal 1
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonCH1_SendVoltage_Click(object sender, RoutedEventArgs e)
		{
			Logger.Info("CH1 - Send voltage");
			float voltage = float.Parse(TextBoxCH1Voltage.Text);
			Logger.Info($"Set voltage to [{voltage}]");
			SerialSender.SetPowerSupplyValues(voltage, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		/// <summary>
		/// Sende eingestellte Spannung am Kanal 2
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ButtonCH2_SendVoltage_Click(object sender, RoutedEventArgs e)
		{
			Logger.Info("CH2 - Send voltage");
			float voltage = float.Parse(TextBoxCH2Voltage.Text);
			Logger.Info($"Set voltage to [{voltage}]");
			SerialSender.SetPowerSupplyValues(voltage, CURRENT_LIMIT, SerialSender.Channel.CH2);
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
				float currentValue = float.Parse(TextBoxCH1Voltage.Text);

				// Wert um 0,001 verringern
				float newValue = currentValue - STEPS;
				newValue = (float)Math.Round(newValue, 3);
				Logger.Info($"[CH1] {currentValue} => {newValue}");
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
				float currentValue = float.Parse(TextBoxCH1Voltage.Text);

				// Wert um 0,001 verringern
				float newValue = currentValue + STEPS;
				newValue = (float)Math.Round(newValue, 3);
				Logger.Info($"[CH1] {currentValue} => {newValue}");
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
				Logger.Info($"[CH2] {currentValue} => {newValue}");
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
				Logger.Info($"[CH2] {currentValue} => {newValue}");
				TextBoxCH2Voltage.Text = newValue.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void RadioButtonValveControl1_Click(object sender, RoutedEventArgs e)
		{
			TextBoxCH1Voltage.Text = "1,000";
			SerialSender.SetPowerSupplyValues(1f, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		private void RadioButtonValveControl2_Click(object sender, RoutedEventArgs e)
		{
			TextBoxCH1Voltage.Text = "9,000";
			SerialSender.SetPowerSupplyValues(9f, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		private void RadioButtonValveControl3_Click(object sender, RoutedEventArgs e)
		{
			TextBoxCH1Voltage.Text = "0,000";
			SerialSender.SetPowerSupplyValues(0f, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		private void RadioButtonPosAmp1_Click(object sender, RoutedEventArgs e)
		{
			TextBoxCH1Voltage.Text = "1,629";
			SerialSender.SetPowerSupplyValues(1.629f, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		private void RadioButtonPosAmp2_Click(object sender, RoutedEventArgs e)
		{
			TextBoxCH1Voltage.Text = "4,829";
			SerialSender.SetPowerSupplyValues(4.829f, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		private void RadioButtonPosAmp3_Click(object sender, RoutedEventArgs e)
		{
			TextBoxCH1Voltage.Text = "8,029";
			SerialSender.SetPowerSupplyValues(8.029f, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		private void RadioButtonPosAmp4_Click(object sender, RoutedEventArgs e)
		{
			TextBoxCH1Voltage.Text = "0,900";
			SerialSender.SetPowerSupplyValues(0.9f, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		private void RadioButtonPosAmp5_Click(object sender, RoutedEventArgs e)
		{
			TextBoxCH1Voltage.Text = "4,789";
			SerialSender.SetPowerSupplyValues(4.789f, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		private void RadioButtonPosAmp6_Click(object sender, RoutedEventArgs e)
		{
			TextBoxCH1Voltage.Text = "8,678";
			SerialSender.SetPowerSupplyValues(8.678f, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		private void RadioButtonTempAmp1_Click(object sender, RoutedEventArgs e)
		{
			TextBoxCH1Voltage.Text = "2,928";
			SerialSender.SetPowerSupplyValues(2.928f, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		private void RadioButtonTempAmp2_Click(object sender, RoutedEventArgs e)
		{
			TextBoxCH1Voltage.Text = "3,239";
			SerialSender.SetPowerSupplyValues(3.239f, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		private void RadioButtonTempAmp3_Click(object sender, RoutedEventArgs e)
		{
			TextBoxCH1Voltage.Text = "3,549";
			SerialSender.SetPowerSupplyValues(3.549f, CURRENT_LIMIT, SerialSender.Channel.CH1);
		}

		private void CheckBoxCH1_Checked(object sender, RoutedEventArgs e)
		{
			SerialSender.SetChannelState(SerialSender.Channel.CH1, SerialSender.State.ON);
		}

		private void CheckBoxCH1_Unchecked(object sender, RoutedEventArgs e)
		{
			SerialSender.SetChannelState(SerialSender.Channel.CH1, SerialSender.State.OFF);
		}

		private void CheckBoxCH2_Checked(object sender, RoutedEventArgs e)
		{
			SerialSender.SetChannelState(SerialSender.Channel.CH2, SerialSender.State.ON);
		}

		private void CheckBoxCH2_Unchecked(object sender, RoutedEventArgs e)
		{
			SerialSender.SetChannelState(SerialSender.Channel.CH2, SerialSender.State.OFF);
		}
	}
}
