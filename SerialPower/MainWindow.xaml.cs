using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SerialPower
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly BackgroundWorker MeasurementWorker = new();
		private readonly BackgroundWorker HeartbeatIndicatorWorker = new();

		public MainWindow()
		{
			InitializeComponent();
			if (ConfigHandler.currentConfig != null)
			{
				TextBlockPortName.Text = "Port: " + ConfigHandler.currentConfig.SerialPortName;
				TextBlockBaudrate.Text = "Baudrate: " + ConfigHandler.currentConfig.SerialPortBaudrate.ToString();
				TextBlockStopBits.Text = "StopBits: " + ConfigHandler.currentConfig.SerialPortStopBits.ToString();
				TextBlockDataBits.Text = "DataBits: " + ConfigHandler.currentConfig.SerialPortDataBits.ToString();
				TextBlockParity.Text = "Parity: " + ConfigHandler.currentConfig.SerialPortParity.ToString();
				TextBlockReadTimeout.Text = "ReadTimeout: " + ConfigHandler.currentConfig.SerialPortReadTimeOut.ToString();
				TextBlockWriteTimeout.Text = "WriteTimeout: " + ConfigHandler.currentConfig.SerialPortWriteTimeOut.ToString();
			}
			MeasurementWorker.DoWork += MeasurementWorker_DoWork;
			HeartbeatIndicatorWorker.DoWork += HeartbeatIndicatorWorker_DoWork;

			MeasurementWorker.RunWorkerAsync();
			HeartbeatIndicatorWorker.RunWorkerAsync();
		}

		private void HeartbeatIndicatorWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			bool toggled = false;
			while (true)
			{
				Thread.Sleep(500);
				this.Dispatcher.Invoke(() =>
				{
					if (toggled)
						RectangleData.Fill = new SolidColorBrush(Colors.Green);
					if (!toggled)
						RectangleData.Fill = new SolidColorBrush(Colors.Red);
					toggled = !toggled;
				});
			}
		}

		private void MeasurementWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			while (true)
			{
				Thread.Sleep(1000);
				this.Dispatcher.Invoke(() =>
				{
					string rawData = SerialSender.SendDataAndRecv("V1O?; I1O?; V2O?; I2O?", false).Replace("\r", "");
					string[] data = rawData.Split("\n");
					try
					{
						TextBlockVoltageCH1.Text = data[0];
						TextBlockCurrentCH1.Text = data[1];
						TextBlockVoltageCH2.Text = data[2];
						TextBlockCurrentCH2.Text = data[3];
					}
					catch (Exception)
					{
						TextBlockVoltageCH1.Text = "No";
						TextBlockCurrentCH1.Text = "Connection";
						TextBlockVoltageCH2.Text = "No";
						TextBlockCurrentCH2.Text = "Connection";
					}
				});
			}
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
			Thread.Sleep(500);
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
			Logger.Write("Closing program. Disconnect power supply.", Logger.StatusCode.INFO);
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
