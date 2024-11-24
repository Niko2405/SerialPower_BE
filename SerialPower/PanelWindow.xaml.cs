using System.Windows;

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
						// get voltage on channel 1
						data = SerialSender.SendDataAndRecv("V1O?", false);
						this.Dispatcher.Invoke(() =>
						{
							TextBoxCH1Voltage.Text = data;
						});

						// get current on channel 1
						data = SerialSender.SendDataAndRecv("I1O?", false);
						this.Dispatcher.Invoke(() =>
						{
							TextBoxCH1Current.Text = data;
						});

						// get voltage on channel 2
						data = SerialSender.SendDataAndRecv("V2O?", false);
						this.Dispatcher.Invoke(() =>
						{
							TextBoxCH2Voltage.Text = data;
						});

						// get current on channel 2
						data = SerialSender.SendDataAndRecv("I2O?", false);
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
			Logger.Write("PanelWindow closed. Thread killed.", Logger.StatusCode.INFO);
			currentWindowClosed = true;
		}
	}
}
