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
					serialPort = new(currentCom, 115200, Parity.None, 8, StopBits.One);
					serialPort.ReadTimeout = 500;
					serialPort.WriteTimeout = 500;

					serialPort.Open();
					Logger.Write($"Check port: {currentCom}", Logger.StatusCode.INFO);
					serialPort.Write("*IDN?" + Environment.NewLine);
					string response = serialPort.ReadLine().Trim();

					// if the answer is CPX200 then add to list selection or port verify is disabled
					if (response.Contains("CPX200"))
					{
						Console.WriteLine(currentCom);
						ListBoxComPorts.Items.Add(currentCom);
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
						Logger.Write("Port verify is disabled. Forcing add COM to list. Use with caution.", Logger.StatusCode.WARNING);
						ListBoxComPorts.Items.Add(currentCom);
					}
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

				TextBoxCurrentRefreshRate.Text = ConfigHandler.currentConfig.CurrentMonitorRate.ToString();
				Logger.Write("Config file found. Load data into window.", Logger.StatusCode.INFO);
			}
		}

		private void ButtonRun_Click(object sender, RoutedEventArgs e)
		{
			if (ListBoxComPorts.SelectedValue != null)
			{
				try
				{
					string? portName = ListBoxComPorts.SelectedValue.ToString();
					int baudrate = int.Parse(TextBoxBaudrate.Text);
					int stopbits = int.Parse(TextBoxStopBits.Text);
					int databits = int.Parse(TextBoxDataBits.Text);
					int parity = int.Parse(TextBoxParity.Text);

					int readTimeout = int.Parse(TextBoxReadTimeout.Text);
					int writeTimeout = int.Parse(TextBoxWriteTimeout.Text);

					int currentRefreshRate = int.Parse(TextBoxCurrentRefreshRate.Text);


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
							ConfigHandler.currentConfig.CurrentMonitorRate = currentRefreshRate;

							ConfigHandler.SaveConfig();
							Logger.Write("Config saved", Logger.StatusCode.INFO);
						}

						#region OPEN_PORT
						SerialSender.ConnectDevice();
						if (SerialSender.serialPort == null)
						{
							Logger.Write("Open serial port", Logger.StatusCode.ERROR);
						}
						#endregion

						// open mainwindow
						MainWindow mainWindow = new();
						mainWindow.Show();

						// open seperate window of current; place on topmost
						if (CheckBoxCurrentMon.IsChecked == true)
						{
							Logger.Write($"Set current refresh rate to {currentRefreshRate}ms", Logger.StatusCode.INFO);
							PanelWindow panelWindow = new();
							panelWindow.Show();
							panelWindow.Topmost = true;
						}

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
