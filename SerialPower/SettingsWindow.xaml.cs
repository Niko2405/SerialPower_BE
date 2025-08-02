using System.IO.Ports;
using System.Windows;
using TartarosLogger;

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
				Logger.Info($"New serial device found: {currentCom}");
				try
				{
					// check if connected device is a power supply
					serialPort = new(currentCom, 9600, Parity.None, 8, StopBits.One)
					{
						ReadTimeout = 250,
						WriteTimeout = 250
					};

					serialPort.Open();
					Logger.Info($"Check port: {currentCom}");
					serialPort.WriteLine("*IDN?");
					string response = serialPort.ReadLine().Trim();

					// if the answer contains CPX200; add to selection list or port verify is disabled
					if (response.Contains("CPX200"))
					{
						Console.Beep(560, 500);
						Console.Beep(620, 500);
						Console.Beep(720, 500);

						// Auto disconnect
						serialPort.WriteLine("LOCAL");
						ComboBoxComPorts.Items.Add(currentCom);
					}
					serialPort.Close();
				}
				catch (TimeoutException)
				{
					if (SerialSender.DisablePortVerify || SerialSender.TestingMode)
					{
						if (serialPort != null)
						{
							if (serialPort.IsOpen)
							{
								serialPort.Close();
							}
						}
						Logger.Warn("Port verify failed. But verify is disabled. Forcing add COM to list. Use with caution!");
						ComboBoxComPorts.Items.Add(currentCom);
					}
				}
				catch (UnauthorizedAccessException)
				{
					if (serialPort != null)
						Logger.Warn("Port " + serialPort.PortName + " cannot open");
				}
				Thread.Sleep(1000);
			}

			// Add DUMMY (Placeholder)
			if (SerialSender.TestingMode)
			{
				Logger.Info("Add virtual serial port: TestPort");
				ComboBoxComPorts.Items.Add("TestPort");
			}

			// reading config file and load data
			if (ConfigHandler.serialConfig != null)
			{
				TextBoxBaudrate.Text = ConfigHandler.serialConfig.Baudrate.ToString();
				TextBoxStopBits.Text = ConfigHandler.serialConfig.StopBits.ToString();
				TextBoxDataBits.Text = ConfigHandler.serialConfig.DataBits.ToString();
				TextBoxParity.Text = ConfigHandler.serialConfig.Parity.ToString();

				TextBoxReadTimeout.Text = ConfigHandler.serialConfig.ReadTimeout.ToString();
				TextBoxWriteTimeout.Text = ConfigHandler.serialConfig.WriteTimeout.ToString();

				Logger.Info("Config file found. Load data into window.");
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
						if (ConfigHandler.serialConfig != null)
						{
							ConfigHandler.serialConfig.SerialPortName = portName;
							ConfigHandler.serialConfig.Baudrate = baudrate;
							ConfigHandler.serialConfig.StopBits = stopbits;
							ConfigHandler.serialConfig.DataBits = databits;
							ConfigHandler.serialConfig.Parity = parity;

							ConfigHandler.serialConfig.ReadTimeout = readTimeout;
							ConfigHandler.serialConfig.WriteTimeout = writeTimeout;

							ConfigHandler.Save();
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
