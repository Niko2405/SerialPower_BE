﻿using System.ComponentModel;
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
		private readonly BackgroundWorker SequencerRunningIndicatorWorker = new();

		public MainWindow()
		{
			InitializeComponent();
			MeasurementWorker.DoWork += MeasurementWorker_DoWork;
			HeartbeatIndicatorWorker.DoWork += HeartbeatIndicatorWorker_DoWork;

			SequencerRunningIndicatorWorker.WorkerReportsProgress = true;
			SequencerRunningIndicatorWorker.DoWork += SequencerRunningIndicatorWorker_DoWork;
			SequencerRunningIndicatorWorker.ProgressChanged += SequencerRunningIndicatorWorker_ProgressChanged;
			
			MeasurementWorker.RunWorkerAsync();
			HeartbeatIndicatorWorker.RunWorkerAsync();

			// TODO: Implement LUA Sequencers
			//SequencerRunningIndicatorWorker.RunWorkerAsync();
		}

		private void SequencerRunningIndicatorWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
		{
			ProgressBarSequencer.Value = e.ProgressPercentage;
		}

		private void SequencerRunningIndicatorWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			if (sender != null)
			{
				((BackgroundWorker)sender).ReportProgress(0);
			}
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

		private void MenuItemLua_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Lua is currently not implemented", "Lua / Sequences", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		private void MenuItemSequencer_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Lua is currently not implemented", "Lua / Sequences", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
	}
}
