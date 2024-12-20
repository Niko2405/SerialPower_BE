using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SerialPower
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			RunUpdater();
		}

		private async void RunUpdater()
		{
			await Task.Factory.StartNew(() =>
			{
				while (true)
				{
					Thread.Sleep(1000);
					this.Dispatcher.Invoke(() =>
					{
						if (ConfigHandler.currentConfig != null)
						{
							TextBlockPortName.Text = "Port: " + ConfigHandler.currentConfig.SerialPortName;
							TextBlockBaudrate.Text = "Baudrate: " + ConfigHandler.currentConfig.SerialPortBaudrate.ToString();
							TextBlockStopBits.Text = "StopBits: " + ConfigHandler.currentConfig.SerialPortStopBits.ToString();
							TextBlockDataBits.Text = "DataBits: " + ConfigHandler.currentConfig.SerialPortDataBits.ToString();
							TextBlockParity.Text = "Parity: " + ConfigHandler.currentConfig.SerialPortParity.ToString();
							TextBlockReadTimeout.Text = "ReadTimeout: " + ConfigHandler.currentConfig.SerialPortReadTimeOut.ToString();
							TextBlockWriteTimeout.Text = "WriteTimeout: " + ConfigHandler.currentConfig.SerialPortWriteTimeOut.ToString();

							// current and and voltage
							TextBlockVoltageCH1.Text = "Voltage CH1: " + SerialSender.SendDataAndRecv("V1O?", false);
							TextBlockCurrentCH1.Text = "Current CH1: " + SerialSender.SendDataAndRecv("I1O?", false);

							TextBlockVoltageCH2.Text = "Voltage CH2: " + SerialSender.SendDataAndRecv("V2O?", false);
							TextBlockCurrentCH2.Text = "Current CH2: " + SerialSender.SendDataAndRecv("I2O?", false);
							return;
						}
						else
						{
							Logger.Write("Reading config", Logger.StatusCode.ERROR);
							return;
						}
					});
				}
			});
		}

		/// <summary>
		/// Change primary user control
		/// </summary>
		/// <param name="userControl"></param>
		public void SetActiveUserControl(UserControl userControl)
		{
			Console.Clear();
			Logger.Write($"Change UserControl to: {userControl.GetType().FullName}", Logger.StatusCode.INFO);
			UserControlErnstLeitz1.Visibility = Visibility.Collapsed;
			UserControlIDE1.Visibility = Visibility.Collapsed;

			UserControlCustomControl.Visibility = Visibility.Collapsed;
			UserControlTerminal.Visibility = Visibility.Collapsed;
			UserControlInfo.Visibility = Visibility.Collapsed;

			// only show current usercontrol
			userControl.Visibility = Visibility.Visible;
		}

		private void WindowClosed(object sender, EventArgs e)
		{
			Logger.Write("Closing program. Disconnect device.", Logger.StatusCode.INFO);
			SerialSender.SendData("LOCAL");
			Thread.Sleep(1000);
			SerialSender.DisconnectDevice();

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

		private void MenuItemExit_Click(object sender, RoutedEventArgs e)
		{
			Logger.Write("Closing program. Disconnect device.", Logger.StatusCode.INFO);
			SerialSender.SendData("LOCAL");
			Thread.Sleep(1000);
			SerialSender.DisconnectDevice();

			Environment.Exit(0);
		}

		private void MenuItemOpenConfig_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("notepad.exe", ConfigHandler.CONFIG_FILE);
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
	}
}
