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
			if (ConfigHandler.currentConfig != null)
			{
				TextBlockPortName.Text = "Port: " + ConfigHandler.currentConfig.SerialPortName;
				TextBlockBaudrate.Text = "Baudrate: " + ConfigHandler.currentConfig.SerialPortBaudrate.ToString();
				TextBlockStopBits.Text = "StopBits: " + ConfigHandler.currentConfig.SerialPortStopBits.ToString();
				TextBlockDataBits.Text = "DataBits: " + ConfigHandler.currentConfig.SerialPortDataBits.ToString();
				TextBlockParity.Text = "Parity: " + ConfigHandler.currentConfig.SerialPortParity.ToString();
				TextBlockReadTimeout.Text = "ReadTimeout: " + ConfigHandler.currentConfig.SerialPortReadTimeOut.ToString();
				TextBlockWriteTimeout.Text = "WriteTimeout: " + ConfigHandler.currentConfig.SerialPortWriteTimeOut.ToString();
				TextBlockCurrentRefreshRate.Text = "CurrentRefreshRate: " + ConfigHandler.currentConfig.CurrentMonitorRate.ToString();
				return;
			}
			else
			{
				Logger.Write("Reading config", Logger.StatusCode.ERROR);
				return;
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

		private void MenuItemRecreateConfig_Click(object sender, RoutedEventArgs e)
		{

		}

		private void MenuItemReloadConfig_Click(object sender, RoutedEventArgs e)
		{
			//ConfigHandler.Init();
			ConfigHandler.SaveConfig();
			ConfigHandler.Init();
			MessageBox.Show("Config reloaded", "Config", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}
