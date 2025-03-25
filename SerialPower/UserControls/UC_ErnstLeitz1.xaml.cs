using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für UC_ErnstLeitz1.xaml
	/// </summary>
	public partial class UC_ErnstLeitz1 : UserControl
	{
		public UC_ErnstLeitz1()
		{
			InitializeComponent();
			SerialSender.SetChannelState(SerialSender.State.OFF);
		}

		private void ButtonStart_Click(object sender, RoutedEventArgs e)
		{
			BackgroundWorker backgroundWorker = new();
			backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.DoWork += BackgroundWorker_DoWork;
			backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
			backgroundWorker.RunWorkerAsync();

			Logger.Write("Starting automatic test phase", Logger.StatusCode.INFO);
		}

		private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
		{
			ProgressBarTestPhase.Value = e.ProgressPercentage;
		}

		private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			short delay = 2000;
			try
			{
				this.Dispatcher.Invoke(() =>
				{
					delay = short.Parse(TextBoxDelay.Text);
				});
				if (delay <= 1000)
				{
					MessageBox.Show("Junge übertreib es nicht! Delay sollte min. 2000ms benötigen. Setze delay auf 2000ms.", "Verarsch mich nicht!", MessageBoxButton.OK, MessageBoxImage.Warning);
					TextBoxDelay.Text = "2000";
					delay = 2000;
				}
				Logger.Write("Delay per test is set to: " + delay, Logger.StatusCode.DEBUG);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (sender != null)
			{
				// Intern 0V
				this.Dispatcher.Invoke(() =>
				{
					TextBlockCurrentTestState.Text = "Current Test: Intern";
					ToggleButtonProgrammingMode.IsEnabled = false;
					ButtonStart.IsEnabled = false;
				});
				((BackgroundWorker)sender).ReportProgress(0);
				SerialSender.SetChannelState(SerialSender.State.OFF);
				SerialSender.SetPowerSupplyValues(0f, 0.5f, SerialSender.Channel.CH1);
				SerialSender.SetPowerSupplyValues(0f, 0.5f, SerialSender.Channel.CH2);
				Thread.Sleep(delay);

				// Intern 6V
				((BackgroundWorker)sender).ReportProgress(10);
				SerialSender.SetChannelState(SerialSender.Channel.CH1, SerialSender.State.ON);
				SerialSender.SetPowerSupplyValues(6f, 0.3f, SerialSender.Channel.CH1);
				Thread.Sleep(delay);

				// Intern 12V
				((BackgroundWorker)sender).ReportProgress(20);
				SerialSender.SetPowerSupplyValues(12f, 0.2f, SerialSender.Channel.CH1);
				Thread.Sleep(delay);

				// Intern 24V
				((BackgroundWorker)sender).ReportProgress(30);
				SerialSender.SetPowerSupplyValues(24f, 0.15f, SerialSender.Channel.CH1);
				Thread.Sleep(delay);

				// Intern 48V
				((BackgroundWorker)sender).ReportProgress(40);
				SerialSender.SetPowerSupplyValues(48f, 0.15f, SerialSender.Channel.CH1);
				Thread.Sleep(delay);

				/// ------------------------------------------------------ ///

				// Extern 0V
				this.Dispatcher.Invoke(() =>
				{
					TextBlockCurrentTestState.Text = "Current Test: Extern";
				});
				((BackgroundWorker)sender).ReportProgress(50);
				SerialSender.SetChannelState(SerialSender.State.OFF);
				SerialSender.SetPowerSupplyValues(0f, 0.5f, SerialSender.Channel.CH1);
				SerialSender.SetPowerSupplyValues(0f, 0.5f, SerialSender.Channel.CH2);
				Thread.Sleep(delay);

				// Extern 6V
				((BackgroundWorker)sender).ReportProgress(60);
				SerialSender.SetChannelState(SerialSender.Channel.CH2, SerialSender.State.ON);
				SerialSender.SetPowerSupplyValues(6f, 0.3f, SerialSender.Channel.CH2);
				Thread.Sleep(delay);

				// Intern 12V
				((BackgroundWorker)sender).ReportProgress(70);
				SerialSender.SetPowerSupplyValues(12f, 0.2f, SerialSender.Channel.CH2);
				Thread.Sleep(delay);

				// Intern 24V
				((BackgroundWorker)sender).ReportProgress(80);
				SerialSender.SetPowerSupplyValues(24f, 0.15f, SerialSender.Channel.CH2);
				Thread.Sleep(delay);

				// Intern 48V
				((BackgroundWorker)sender).ReportProgress(90);
				SerialSender.SetPowerSupplyValues(48f, 0.15f, SerialSender.Channel.CH2);
				Thread.Sleep(delay);

				((BackgroundWorker)sender).ReportProgress(100);
				SerialSender.SetChannelState(0);
				this.Dispatcher.Invoke(() =>
				{
					TextBlockCurrentTestState.Text = "Current: Finish";
					ButtonStart.IsEnabled = true;
					ToggleButtonProgrammingMode.IsEnabled = true;
				});
				MessageBox.Show("Test is now finished", "Test phase ended", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		private void ToggleButtonProgrammingMode_Click(object sender, RoutedEventArgs e)
		{
			if (ToggleButtonProgrammingMode.IsChecked == true)
			{
				SerialSender.SetPowerSupplyValues(12f, 0.2f, SerialSender.Channel.CH1);
				SerialSender.SetChannelState(SerialSender.Channel.CH1, SerialSender.State.ON);
				ToggleButtonProgrammingMode.Content = "Enabled";
				ButtonStart.IsEnabled = false;
			}
			if (ToggleButtonProgrammingMode.IsChecked == false)
			{
				SerialSender.SetPowerSupplyValues(0f, 0.2f, SerialSender.Channel.CH1);
				SerialSender.SetChannelState(SerialSender.Channel.CH1, SerialSender.State.OFF);
				ToggleButtonProgrammingMode.Content = "Disabled";
				ButtonStart.IsEnabled = true;
			}
		}
	}
}
