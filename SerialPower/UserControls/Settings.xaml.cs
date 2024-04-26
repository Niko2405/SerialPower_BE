using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			string[] coms = System.IO.Ports.SerialPort.GetPortNames();
			foreach (string com in coms)
			{
				Debug.WriteLine($"{com} found. Add to list");
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
					int baudrate = int.Parse(TextBox_Baudrate.Text);
					int stopbits = int.Parse(TextBox_StopBits.Text);
					int databits = int.Parse(TextBox_DataBits.Text);
					int parity = int.Parse(TextBox_Parity.Text);
					int readTimeout = int.Parse(TextBox_ReadTimeout.Text);
					int writeTimeout = int.Parse(TextBox_WriteTimeout.Text);

					if (portName != null)
					{
						SerialSender.SelectedPortName = portName;
						SerialSender.SelectedBaudrate = baudrate;
						SerialSender.SelectedStopBits = stopbits;
						SerialSender.SelectedDataBits = databits;
						SerialSender.SelectedParity = parity;
						SerialSender.SelectedReadTimeout = readTimeout;
						SerialSender.SelectedWriteTimeout = writeTimeout;
						MessageBox.Show($"Port name: {SerialSender.SelectedPortName}\nBaudrate: {SerialSender.SelectedBaudrate}\nStopBits: {SerialSender.SelectedStopBits}\nDataBits: {SerialSender.SelectedDataBits}\nParity: {SerialSender.SelectedParity}\nReadTimeout: {SerialSender.SelectedReadTimeout}\nWriteTimeout: {SerialSender.SelectedWriteTimeout}", "Settings saved", MessageBoxButton.OK, MessageBoxImage.Information);
						
						// get instance of mainwindow to access functions.
						MainWindow mainWindow = (MainWindow) Window.GetWindow(this);
						Debug.WriteLine("Enable all buttons");
						mainWindow.SetAllButtonState(true);
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
