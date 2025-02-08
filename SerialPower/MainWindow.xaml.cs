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
			MeasurementWorker.DoWork += MeasurementWorker_DoWork;
			HeartbeatIndicatorWorker.DoWork += HeartbeatIndicatorWorker_DoWork;
			
			MeasurementWorker.RunWorkerAsync();
			HeartbeatIndicatorWorker.RunWorkerAsync();
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

		/// <summary>
		/// Measure voltage and current
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MeasurementWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			while (true)
			{
				Thread.Sleep(1000);
				this.Dispatcher.Invoke(() =>
				{
					var data = SerialSender.GetPowerSupplyValues();
					TextBlockVoltageCH1.Text = "CH1 Voltage: " + data.Item1;
					TextBlockCurrentCH1.Text = "CH1 Current: " + data.Item2;
					TextBlockVoltageCH2.Text = "CH2 Voltage: " + data.Item3;
					TextBlockCurrentCH2.Text = "CH2 Current: " + data.Item4;
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
			Thread.Sleep(100);
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
			Thread.Sleep(100);
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

		private void MenuItemLicense_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("MIT License\r\n\r\nCopyright (c) 2024 Niko2405\r\n\r\nPermission is hereby granted, free of charge, to any person obtaining a copy\r\nof this software and associated documentation files (the \"Software\"), to deal\r\nin the Software without restriction, including without limitation the rights\r\nto use, copy, modify, merge, publish, distribute, sublicense, and/or sell\r\ncopies of the Software, and to permit persons to whom the Software is\r\nfurnished to do so, subject to the following conditions:\r\n\r\nThe above copyright notice and this permission notice shall be included in all\r\ncopies or substantial portions of the Software.\r\n\r\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR\r\nIMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,\r\nFITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE\r\nAUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER\r\nLIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,\r\nOUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE\r\nSOFTWARE.", "MIT License - SerialPower_BE", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}
