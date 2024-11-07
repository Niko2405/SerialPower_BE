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
			foreach (string com in coms)
			{
				Logger.PrintStatus($"New serial device found: {com}. Add to selection list.", Logger.StatusCode.OK);
				ListBoxComPorts.Items.Add(com);
			}

			// reading config file
			if (ConfigHandler.currentConfig != null)
			{
				TextBoxBaudrate.Text = ConfigHandler.currentConfig.SerialPortBaudrate.ToString();
				TextBoxStopBits.Text = ConfigHandler.currentConfig.SerialPortStopBits.ToString();
				TextBoxDataBits.Text = ConfigHandler.currentConfig.SerialPortDataBits.ToString();
				TextBoxParity.Text = ConfigHandler.currentConfig.SerialPortParity.ToString();

				TextBoxReadTimeout.Text = ConfigHandler.currentConfig.SerialPortReadTimeOut.ToString();
				TextBoxWriteTimeout.Text = ConfigHandler.currentConfig.SerialPortWriteTimeOut.ToString();

				TextBoxCurrentRefreshRate.Text = ConfigHandler.currentConfig.CurrentMonitorRate.ToString();
				Logger.PrintStatus("Config file found. Load data into window.", Logger.StatusCode.OK);
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
							Logger.PrintStatus("Config saved", Logger.StatusCode.OK);
						}

						#region OPEN_PORT
						SerialSender.ConnectDevice();
						if (SerialSender.serialPort == null)
						{
							Logger.PrintStatus("Open serial port", Logger.StatusCode.FAILED);
						}
						#endregion

						MainWindow mainWindow = new();
						mainWindow.Show();

						// open seperate window of current; place on topmost
						Logger.PrintHeader("MainWindow");

						if (CheckBoxCurrentMon.IsChecked == true)
						{
							Logger.PrintStatus($"Set current refresh rate to {currentRefreshRate}ms", Logger.StatusCode.OK);
							PanelWindow panelWindow = new();
							panelWindow.Show();
							panelWindow.Topmost = true;
						}

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
