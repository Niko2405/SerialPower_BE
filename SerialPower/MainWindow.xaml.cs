using SerialPower.UserControls;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
				StatusBarConnectionInfo.Text = $"Port: {ConfigHandler.currentConfig.SerialPortName}\t\tBaudrate: {ConfigHandler.currentConfig.SerialPortBaudrate}\t\tStopBits: {ConfigHandler.currentConfig.SerialPortStopBits}\t\tDataBits: {ConfigHandler.currentConfig.SerialPortDataBits}\t\tParity: {ConfigHandler.currentConfig.SerialPortParity}\t\tReadTimeout: {ConfigHandler.currentConfig.SerialPortReadTimeOut}\t\tWriteTimeout: {ConfigHandler.currentConfig.SerialPortWriteTimeOut}\t\tCurrent Monitor Rate: {ConfigHandler.currentConfig.CurrentMonitorRate}";
				return;
			}
			else
			{
				StatusBarConnectionInfo.Text = "ERROR";
			}
			
		}

		/// <summary>
		/// Change primary user control
		/// </summary>
		/// <param name="userControl"></param>
		public void SetActiveUserControl(UserControl userControl)
		{
			Console.Clear();
			Logger.PrintStatus($"Change UserControl to: {userControl.GetType().FullName}", Logger.StatusCode.OK);
			UserControlErnstLeitz1.Visibility = Visibility.Collapsed;
			UserControlIDE1.Visibility = Visibility.Collapsed;
			UserControlCustomControl.Visibility = Visibility.Collapsed;
			UserControlInfo.Visibility = Visibility.Collapsed;

			// only show current usercontrol
			userControl.Visibility = Visibility.Visible;
		}


		private void WindowClosed(object sender, EventArgs e)
		{
			Logger.PrintStatus("Closing program. Disconnect target.", Logger.StatusCode.OK);
			SerialSender.SendCommand("LOCAL");
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

		}

		private void MenuItemInfo_Click(object sender, RoutedEventArgs e)
		{
			SetActiveUserControl(UserControlInfo);
		}

		private void MenuItemExit_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintStatus("Closing program. Disconnect target.", Logger.StatusCode.OK);
			SerialSender.SendCommand("LOCAL");
			Environment.Exit(0);
		}
	}
}
