using System.Windows;

namespace SerialPower
{
	[Obsolete]
	/// <summary>
	/// Interaktionslogik für PanelWindow.xaml
	/// </summary>
	public partial class PanelWindow : Window
	{
		public static bool updaterRunning = false;
		public PanelWindow()
		{
			InitializeComponent();
			RunUpdaters();
		}

		/// <summary>
		/// Backgroundworker to update the current
		/// </summary>
		public async void RunUpdaters()
		{
			await Task.Factory.StartNew(() =>
			{
				string data = string.Empty;
				while (true)
				{
					Thread.Sleep(1000);
					//Logger.Write("Task running", Logger.StatusCode.WARNING);
					if (updaterRunning)
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
			Logger.Write("PanelWindow closed. Thread killed.", Logger.StatusCode.WARNING);
			updaterRunning = false;
		}
	}
}
