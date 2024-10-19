using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für Settings.xaml
	/// </summary>
	public partial class Settings : UserControl
	{
		public Settings()
		{
			InitializeComponent();

			// COM Liste füllen
			string[] coms = SerialPort.GetPortNames();
			foreach (string com in coms)
			{
				Logger.PrintStatus($"New device found: {com}. Add to selection", Logger.StatusCode.INFO);
				ListBoxComPorts.Items.Add(com);
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
						SerialSender.SelectedPortName = portName;
						SerialSender.SelectedBaudrate = baudrate;
						SerialSender.SelectedStopBits = stopbits;
						SerialSender.SelectedDataBits = databits;
						SerialSender.SelectedParity = parity;
						SerialSender.SelectedReadTimeout = readTimeout;
						SerialSender.SelectedWriteTimeout = writeTimeout;

						// TODO: Add settings to config file

						// get instance of mainwindow to access functions.
						MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
						Logger.PrintStatus("Settings saved in runtime. Enabling all buttons", Logger.StatusCode.OK);
						mainWindow.SetAllButtonState(true);

						// Show all settings
						MessageBox.Show($"Port name: {SerialSender.SelectedPortName}\nBaudrate: {SerialSender.SelectedBaudrate}\nStopBits: {SerialSender.SelectedStopBits}\nDataBits: {SerialSender.SelectedDataBits}\nParity: {SerialSender.SelectedParity}\nReadTimeout: {SerialSender.SelectedReadTimeout}\nWriteTimeout: {SerialSender.SelectedWriteTimeout}\nCurrent - Refresh rate: {currentRefreshRate}", "SerialPower - Settings saved", MessageBoxButton.OK, MessageBoxImage.Information);

						// open seperate window of current; place on topmost
						CurrentWindow currentWindow = new CurrentWindow();
						Logger.PrintStatus($"Set current refresh rate to {currentRefreshRate}ms", Logger.StatusCode.OK);
						currentWindow.SetCurrentRefreshRate(currentRefreshRate);
						currentWindow.Show();
						currentWindow.Topmost = true;

						// deactivate run button
						Logger.PrintStatus("Disable Run button", Logger.StatusCode.OK);
						ButtonRun.Content = "Running...";
						ButtonRun.IsEnabled = false;

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
