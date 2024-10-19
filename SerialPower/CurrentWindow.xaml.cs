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
	/// Interaktionslogik für CurrentWindow.xaml
	/// </summary>
	public partial class CurrentWindow : Window
	{
		private static int currentRefreshRate = 500;
		private static bool windowsClosed = false;

		public CurrentWindow()
		{
			InitializeComponent();
			RunUpdaters();
		}
		
		public void SetCurrentRefreshRate(int refreshRate)
		{
			currentRefreshRate = refreshRate;
		}

		private async void RunUpdaters()
		{
			await Task.Factory.StartNew(() =>
			{
				string data = string.Empty;

				while (true)
				{
					Thread.Sleep(currentRefreshRate);

					// If current window closed. Kill background worker
					if (windowsClosed)
						return;

					// Only check current when ComPort is selected and visibility is true
					if (SerialSender.SelectedPortName != string.Empty && this.Visibility == Visibility.Visible)
					{
						// get current on channel 1
						data = SerialSender.SendCommand("I1O?", true);
						this.Dispatcher.Invoke(() =>
						{
							TextBoxCH1Current.Text = data;
						});

						// get current on channel 2
						data = SerialSender.SendCommand("I2O?", true);
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
			Logger.PrintStatus("Current Window closed. Disable background worker", Logger.StatusCode.INFO);
			windowsClosed = true;
		}
	}
}
