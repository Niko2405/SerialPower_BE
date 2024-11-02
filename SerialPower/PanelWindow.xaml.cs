using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SerialPower
{
	/// <summary>
	/// Interaktionslogik für PanelWindow.xaml
	/// </summary>
	public partial class PanelWindow : Window
	{
		private static bool currentWindowClosed = false;

		public PanelWindow()
		{
			InitializeComponent();
			RunUpdaters();
		}
		

		/// <summary>
		/// Backgroundworker to update the current
		/// </summary>
		private async void RunUpdaters()
		{
			await Task.Factory.StartNew(() =>
			{
				string data = string.Empty;

				while (true)
				{
					if (ConfigHandler.currentConfig != null)
					{
						Thread.Sleep(ConfigHandler.currentConfig.CurrentMonitorRate);
					}
					else if (ConfigHandler.currentConfig == null)
					{
						Thread.Sleep(1000);
					}
						
					// If current window closed. Kill backgroundworker
					if (currentWindowClosed)
						return;

					// Only check current when ComPort is selected and visibility is true
					if (this.Visibility == Visibility.Visible)
					{
						// get current on channel 1
						data = SerialSender.SendCommand("I1O?", true, false);
						this.Dispatcher.Invoke(() =>
						{
							TextBoxCH1Current.Text = data;
						});

						// get current on channel 2
						data = SerialSender.SendCommand("I2O?", true, false);
						this.Dispatcher.Invoke(() =>
						{
							TextBoxCH2Current.Text = data;
						});
					}
				}
			});
		}

		private void WindowClosed(object sender, EventArgs e)
		{
			Logger.PrintStatus("PanelWindow closed. Backgroundworker killed.", Logger.StatusCode.INFO);
			currentWindowClosed = true;
		}
	}
}
