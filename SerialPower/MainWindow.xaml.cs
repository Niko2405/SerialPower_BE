using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TartarosLogger;

namespace SerialPower
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly BackgroundWorker MeasurementWorker = new();
		private readonly BackgroundWorker HeartbeatIndicatorWorker = new();
		private readonly BackgroundWorker SeqIndicatorWorker = new();
		private static short updateDelay = 1000;

		public MainWindow()
		{
			InitializeComponent();
			try
			{
				TextBlockVersion.Text = "Version: " + File.ReadAllText(ConfigHandler.VERSION_FILE);
				TextBlockDebugState.Text = "DebugEnabled: " + Logger.DebugEnabled;
			}
			catch (Exception)
			{
				Logger.Error($"Reading version file: {ConfigHandler.VERSION_FILE}");
			}

			if (ConfigHandler.serialConfig != null)
			{
				updateDelay = ConfigHandler.serialConfig.MeasureUpdateInterval;
			}

			//MeasurementWorker.DoWork += MeasurementWorker_DoWork;
			HeartbeatIndicatorWorker.DoWork += HeartbeatIndicatorWorker_DoWork;
			SeqIndicatorWorker.DoWork += SequencerIndicatorWorker_DoWork;

			MeasurementWorker.RunWorkerAsync();
			HeartbeatIndicatorWorker.RunWorkerAsync();
			SeqIndicatorWorker.RunWorkerAsync();
		}

		/// <summary>
		/// Show's the user, that program is not stuck
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HeartbeatIndicatorWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			bool toggled = false;
			while (true)
			{
				Thread.Sleep(updateDelay / 2);
				this.Dispatcher.Invoke(() =>
				{
					switch (toggled)
					{
						case true:
							RectangleTick.Fill = new SolidColorBrush(Colors.LimeGreen);
							break;
						case false:
							RectangleTick.Fill = new SolidColorBrush(Colors.Black);
							break;
					}
					toggled = !toggled;
				});
			}
		}

		/// <summary>
		/// Show's the user, sequencer is running. Current only simulation!
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SequencerIndicatorWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			bool toggled = false;
			while (true)
			{
				Thread.Sleep(100);
				if (ConfigHandler.serialConfig != null && ConfigHandler.serialConfig.IsSequencerRunning)
				{
					this.Dispatcher.Invoke(() =>
					{
						switch (toggled)
						{
							case true:
								RectangleSequencer.Fill = new SolidColorBrush(Colors.LimeGreen);
								break;
							case false:
								RectangleSequencer.Fill = new SolidColorBrush(Colors.Black);
								break;
						}
						toggled = !toggled;
					});
				}
				if (ConfigHandler.serialConfig != null && !ConfigHandler.serialConfig.IsSequencerRunning)
				{
					this.Dispatcher.Invoke(() =>
					{
						RectangleSequencer.Fill = new SolidColorBrush(Colors.Black);
					});
				}
			}
		}
		/*
		/// <summary>
		/// Measure voltage and current
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MeasurementWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			while (true)
			{
				// get values
				Thread.Sleep(updateDelay);
				Logger.Debug("Asking for actual values");
				string textCH1Voltage = SerialSender.GetPowerSupplyActualValue(SerialSender.Channel.CH1, SerialSender.TargetType.V);
				string textCH1Current = SerialSender.GetPowerSupplyActualValue(SerialSender.Channel.CH1, SerialSender.TargetType.I);
				string textCH2Voltage = SerialSender.GetPowerSupplyActualValue(SerialSender.Channel.CH2, SerialSender.TargetType.V);
				string textCH2Current = SerialSender.GetPowerSupplyActualValue(SerialSender.Channel.CH2, SerialSender.TargetType.I);


				#region ShortProtection
				if (ConfigHandler.serialConfig != null)
				{
					if (ConfigHandler.serialConfig.ShortCircuitProtection)
					{
						float nominalCH1Current = float.Parse(SerialSender.GetPowerSupplyNominalValue(SerialSender.Channel.CH1, SerialSender.TargetType.I).Replace('A', ' '));
						float nominalCH2Current = float.Parse(SerialSender.GetPowerSupplyNominalValue(SerialSender.Channel.CH2, SerialSender.TargetType.I).Replace('A', ' '));

						float currentCH1 = float.Parse(textCH1Current.Replace('A', ' '));
						float currentCH2 = float.Parse(textCH2Current.Replace('A', ' '));

						Logger.Debug($"[SCP] Current CH1: {currentCH1}A\t max. current: {nominalCH1Current}A");
						Logger.Debug($"[SCP] Current CH2: {currentCH2}A\t max. current: {nominalCH2Current}A");

						if ((currentCH1 / nominalCH1Current) >= 1.00)
						{
							Logger.Warn($"[SCP] Current to high on CH1");
							SerialSender.SetChannelState(SerialSender.Channel.CH1, SerialSender.State.OFF);
							this.Dispatcher.Invoke(() =>
							{
								UserControlCustomControl.Channel1Active = false;
								UserControlMinimalCustomControl.Channel1Active = false;
							});

							MessageBox.Show("Die Alte Firma brennt.\nSag ich mal so: Der maximale Strom an Kanal 1 wurde erreicht. Ausgang deaktiviert.", "Channel 1 maximum current reached", MessageBoxButton.OK, MessageBoxImage.Warning);
						}
						if ((currentCH2 / nominalCH2Current) >= 1.00)
						{
							Logger.Warn($"[SCP] Current to high on CH2");
							SerialSender.SetChannelState(SerialSender.Channel.CH2, SerialSender.State.OFF);
							this.Dispatcher.Invoke(() =>
							{
								UserControlCustomControl.Channel2Active = false;
								UserControlMinimalCustomControl.Channel2Active = false;
							});

							MessageBox.Show("Die Alte Firma brennt.\nSag ich mal so: Der maximale Strom an Kanal 2 wurde erreicht. Ausgang deaktiviert.", "Channel 2 maximum current reached", MessageBoxButton.OK, MessageBoxImage.Warning);
						}
					}
				}
				#endregion

				this.Dispatcher.Invoke(() =>
				{
					TextBlockVoltageCH1.Text = "CH1 Voltage: " + textCH1Voltage;
					TextBlockCurrentCH1.Text = "CH1 Current: " + textCH1Current;
					TextBlockVoltageCH2.Text = "CH2 Voltage: " + textCH2Voltage;
					TextBlockCurrentCH2.Text = "CH2 Current: " + textCH2Current;
				});
			}
		}
		*/

		/// <summary>
		/// Change primary user control
		/// </summary>
		/// <param name="userControl"></param>
		public void SetActiveUserControl(UserControl userControl)
		{
			Console.Clear();
			Logger.Info($"Change UserControl to: {userControl.GetType().FullName}");
			UserControlErnstLeitz1.Visibility = Visibility.Collapsed;
			UserControlIDE1.Visibility = Visibility.Collapsed;

			UserControlCustomControl.Visibility = Visibility.Collapsed;
			UserControlTerminal.Visibility = Visibility.Collapsed;
			UserControlInfo.Visibility = Visibility.Collapsed;
			UserControlMinimalCustomControl.Visibility = Visibility.Collapsed;
			UserControlSequencer.Visibility = Visibility.Collapsed;

			// only show current usercontrol
			userControl.Visibility = Visibility.Visible;
		}

		private void WindowClosed(object sender, EventArgs e)
		{
			Logger.Info("Closing program. Disconnect power supply.");
			Thread.Sleep(100);
			SerialSender.DisconnectDevice();
			Thread.Sleep(100);

			Environment.Exit(0);
		}

		private void MenuItemErnstLeitz1_Click(object sender, RoutedEventArgs e)
		{
			SetActiveUserControl(UserControlErnstLeitz1);
		}

		private void MenuItemIDE1_Click(object sender, RoutedEventArgs e)
		{
			SetActiveUserControl(UserControlIDE1);
		}

		private void MenuItemCustomControl_Click(object sender, RoutedEventArgs e)
		{
			SetActiveUserControl(UserControlCustomControl);
		}

		private void MenuItemTerminal_Click(object sender, RoutedEventArgs e)
		{
			SetActiveUserControl(UserControlTerminal);
		}

		private void MenuItemInfo_Click(object sender, RoutedEventArgs e)
		{
			SetActiveUserControl(UserControlInfo);
		}

		private void MenuItemMinimalControl_Click(object sender, RoutedEventArgs e)
		{
			SetActiveUserControl(UserControlMinimalCustomControl);
		}

		private void MenuItemSequencer_Click(object sender, RoutedEventArgs e)
		{
			OperatingSystem os = Environment.OSVersion;
			if (os.Platform == PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 1)
			{
				MessageBox.Show("Windows 7 is not supported. This function doesn't work on current machine", "Not supported", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			SetActiveUserControl(UserControlSequencer);
		}

		private void MenuItemExit_Click(object sender, RoutedEventArgs e)
		{
			Logger.Info("Closing program. Disconnect power supply.");
			Thread.Sleep(100);
			SerialSender.DisconnectDevice();
			Thread.Sleep(100);

			Environment.Exit(0);
		}

		private void MenuItemOpenSerialConfig_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("notepad.exe", ConfigHandler.FILE_CONFIG_SERIAL);
		}

		private void MenuItemOpenDeviceManager_Click(object sender, RoutedEventArgs e)
		{
			var startInfo = new ProcessStartInfo
			{
				FileName = "cmd.exe",
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = false,
			};

			var process = new Process { StartInfo = startInfo };

			process.Start();
			process.StandardInput.WriteLine("devmgmt.msc");
			process.StandardInput.WriteLine("exit");
		}

		private void MenuItemLicense_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("MIT License\r\n\r\nCopyright (c) 2025 Niko2405\r\n\r\nPermission is hereby granted, free of charge, to any person obtaining a copy\r\nof this software and associated documentation files (the \"Software\"), to deal\r\nin the Software without restriction, including without limitation the rights\r\nto use, copy, modify, merge, publish, distribute, sublicense, and/or sell\r\ncopies of the Software, and to permit persons to whom the Software is\r\nfurnished to do so, subject to the following conditions:\r\n\r\nThe above copyright notice and this permission notice shall be included in all\r\ncopies or substantial portions of the Software.\r\n\r\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR\r\nIMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,\r\nFITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE\r\nAUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER\r\nLIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,\r\nOUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE\r\nSOFTWARE.", "MIT License - SerialPower_BE", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void MenuItemGetCurrentInstanceSettings_Click(object sender, RoutedEventArgs e)
		{
			if (ConfigHandler.serialConfig != null)
			{
				MessageBox.Show($"PortName: {ConfigHandler.serialConfig.SerialPortName}\nBaudrate: {ConfigHandler.serialConfig.Baudrate}\nDataBits: {ConfigHandler.serialConfig.DataBits}\nStopBits: {ConfigHandler.serialConfig.StopBits}\nParity: {ConfigHandler.serialConfig.Parity}\nWriteTimeout: {ConfigHandler.serialConfig.WriteTimeout}\nReadTimeout: {ConfigHandler.serialConfig.ReadTimeout}\nMeasureUpdateInterval: {ConfigHandler.serialConfig.MeasureUpdateInterval}\nShortCircuitProtection: {ConfigHandler.serialConfig.ShortCircuitProtection}\nIsSequencerRunning: {ConfigHandler.serialConfig.IsSequencerRunning}", "Serial config", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		private void MenuItemShortCircuitProtection_Checked(object sender, RoutedEventArgs e)
		{
			if (ConfigHandler.serialConfig != null)
			{
				Logger.Info("ShortCircuitProtection = true");
				ConfigHandler.serialConfig.ShortCircuitProtection = true;
				ConfigHandler.Save();
			}
		}

		private void MenuItemShortCircuitProtection_Unchecked(object sender, RoutedEventArgs e)
		{
			if (ConfigHandler.serialConfig != null)
			{
				Logger.Info("ShortCircuitProtection = false");
				ConfigHandler.serialConfig.ShortCircuitProtection = false;
				ConfigHandler.Save();
			}
		}

		/*
+------------------------------------------------------------------------------+
|                    |   PlatformID    |   Major version   |   Minor version   |
+------------------------------------------------------------------------------+
| Windows 95         |  Win32Windows   |         4         |          0        |
| Windows 98         |  Win32Windows   |         4         |         10        |
| Windows Me         |  Win32Windows   |         4         |         90        |
| Windows NT 4.0     |  Win32NT        |         4         |          0        |
| Windows 2000       |  Win32NT        |         5         |          0        |
| Windows XP         |  Win32NT        |         5         |          1        |
| Windows 2003       |  Win32NT        |         5         |          2        |
| Windows Vista      |  Win32NT        |         6         |          0        |
| Windows 2008       |  Win32NT        |         6         |          0        |
| Windows 7          |  Win32NT        |         6         |          1        |
| Windows 2008 R2    |  Win32NT        |         6         |          1        |
| Windows 8          |  Win32NT        |         6         |          2        |
| Windows 8.1        |  Win32NT        |         6         |          3        |
+------------------------------------------------------------------------------+
| Windows 10         |  Win32NT        |        10         |          0        |
+------------------------------------------------------------------------------+
		*/
	}
}
