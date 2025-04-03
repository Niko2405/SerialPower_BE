using System.IO.Ports;
using System.Windows;

namespace SerialPower
{
	/// <summary>
	/// Interaktionslogik für SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window
	{
		public SettingsWindow()
		{
			InitializeComponent();

			// scan all serial ports
			string[] coms = SerialPort.GetPortNames();
			SerialPort? serialPort = null;
			foreach (string currentCom in coms)
			{
				Logger.Write($"New serial device found: {currentCom}", Logger.StatusCode.INFO);
				try
				{
					// check if connected device is a power supply
					serialPort = new(currentCom, 115200, Parity.None, 8, StopBits.One)
					{
						ReadTimeout = 500,
						WriteTimeout = 500
					};

					serialPort.Open();
					Logger.Write($"Check port: {currentCom}", Logger.StatusCode.INFO);
					serialPort.WriteLine("*IDN?");
					string response = serialPort.ReadLine().Trim();

					// if the answer contains CPX200; add to selection list or port verify is disabled
					if (response.Contains("CPX200"))
					{
						Console.Beep(560, 500);

						// Auto disconnect
						serialPort.WriteLine("LOCAL");
						ComboBoxComPorts.Items.Add(currentCom);
					}
					serialPort.Close();
				}
				catch (TimeoutException)
				{
					if (SerialSender.DisablePortVerify)
					{
						if (serialPort != null)
						{
							if (serialPort.IsOpen)
							{
								serialPort.Close();
							}
						}
						Logger.Write("Port verify failed. But verify is disabled. Forcing add COM to list. Use with caution!", Logger.StatusCode.WARNING);
						ComboBoxComPorts.Items.Add(currentCom);
					}
				}
				catch (UnauthorizedAccessException)
				{
					if (serialPort != null)
					Logger.Write("Port " + serialPort.PortName + " cannot open", Logger.StatusCode.WARNING);
				}

				// Add DUMMY (Placeholder)
				if (SerialSender.DisableCommunication)
				{
					ComboBoxComPorts.Items.Add("PLACEHOLDER");
				}
				Thread.Sleep(1000);
			}

			// reading config file and load data
			if (ConfigHandler.currentConfig != null)
			{
				TextBoxBaudrate.Text = ConfigHandler.currentConfig.SerialPortBaudrate.ToString();
				TextBoxStopBits.Text = ConfigHandler.currentConfig.SerialPortStopBits.ToString();
				TextBoxDataBits.Text = ConfigHandler.currentConfig.SerialPortDataBits.ToString();
				TextBoxParity.Text = ConfigHandler.currentConfig.SerialPortParity.ToString();

				TextBoxReadTimeout.Text = ConfigHandler.currentConfig.SerialPortReadTimeOut.ToString();
				TextBoxWriteTimeout.Text = ConfigHandler.currentConfig.SerialPortWriteTimeOut.ToString();

				Logger.Write("Config file found. Load data into window.", Logger.StatusCode.INFO);
			}
		}

		private void ButtonConnect_Click(object sender, RoutedEventArgs e)
		{
			if (ComboBoxComPorts.SelectedValue != null)
			{
				try
				{
					string? portName = ComboBoxComPorts.SelectedValue.ToString();
					int baudrate = int.Parse(TextBoxBaudrate.Text);
					int stopbits = int.Parse(TextBoxStopBits.Text);
					int databits = int.Parse(TextBoxDataBits.Text);
					int parity = int.Parse(TextBoxParity.Text);

					int readTimeout = int.Parse(TextBoxReadTimeout.Text);
					int writeTimeout = int.Parse(TextBoxWriteTimeout.Text);

					if (portName != null)
					{
						// save current settings in config file
						if (ConfigHandler.currentConfig != null)
						{
							ConfigHandler.currentConfig.SerialPortName = portName;
							ConfigHandler.currentConfig.SerialPortBaudrate = baudrate;
							ConfigHandler.currentConfig.SerialPortStopBits = stopbits;
							ConfigHandler.currentConfig.SerialPortDataBits = databits;
							ConfigHandler.currentConfig.SerialPortParity = parity;

							ConfigHandler.currentConfig.SerialPortReadTimeOut = readTimeout;
							ConfigHandler.currentConfig.SerialPortWriteTimeOut = writeTimeout;

							ConfigHandler.SaveConfig();
							Logger.Write("Config saved", Logger.StatusCode.INFO);
						}

						SerialSender.ConnectDevice();

						// open mainwindow
						MainWindow mainWindow = new();
						mainWindow.Show();

						// hide current settings window
						this.Hide();
						return;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			else
			{
				MessageBox.Show("Port not selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
