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
		private readonly int DELAY = 2000;

		public UC_ErnstLeitz1()
		{
			InitializeComponent();
			SerialSender.SetChannelState(0);
		}

		private void ButtonStart_Click(object sender, RoutedEventArgs e)
		{
			BackgroundWorker backgroundWorker = new BackgroundWorker();
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
			if (sender != null)
			{
				// Intern 0V
				this.Dispatcher.Invoke(() =>
				{
					TextBlockCurrentTestState.Text = "TEST: INTERN";
					ToggleButtonProgrammingMode.IsEnabled = false;
					ButtonStart.IsEnabled = false;
				});
				((BackgroundWorker)sender).ReportProgress(0);
				SerialSender.SetChannelState(0);
				SerialSender.SetPowerSupplyValues(0f, 0.5f, SerialSender.Channel.CH1);
				SerialSender.SetPowerSupplyValues(0f, 0.5f, SerialSender.Channel.CH2);
				Thread.Sleep(DELAY);

				// Intern 6V
				((BackgroundWorker)sender).ReportProgress(10);
				SerialSender.SetChannelState(SerialSender.Channel.CH1, SerialSender.State.ON);
				SerialSender.SetPowerSupplyValues(6f, 0.3f, SerialSender.Channel.CH1);
				Thread.Sleep(DELAY);

				// Intern 12V
				((BackgroundWorker)sender).ReportProgress(20);
				SerialSender.SetPowerSupplyValues(12f, 0.2f, SerialSender.Channel.CH1);
				Thread.Sleep(DELAY);

				// Intern 24V
				((BackgroundWorker)sender).ReportProgress(30);
				SerialSender.SetPowerSupplyValues(24f, 0.15f, SerialSender.Channel.CH1);
				Thread.Sleep(DELAY);

				// Intern 48V
				((BackgroundWorker)sender).ReportProgress(40);
				SerialSender.SetPowerSupplyValues(48f, 0.15f, SerialSender.Channel.CH1);
				Thread.Sleep(DELAY);

				/// ------------------------------------------------------ ///

				// Extern 0V
				this.Dispatcher.Invoke(() =>
				{
					TextBlockCurrentTestState.Text = "TEST: EXTERN";
				});
				((BackgroundWorker)sender).ReportProgress(50);
				SerialSender.SetChannelState(0);
				SerialSender.SetPowerSupplyValues(0f, 0.5f, SerialSender.Channel.CH1);
				SerialSender.SetPowerSupplyValues(0f, 0.5f, SerialSender.Channel.CH2);
				Thread.Sleep(DELAY);

				// Extern 6V
				((BackgroundWorker)sender).ReportProgress(60);
				SerialSender.SetChannelState(SerialSender.Channel.CH2, SerialSender.State.ON);
				SerialSender.SetPowerSupplyValues(6f, 0.3f, SerialSender.Channel.CH2);
				Thread.Sleep(DELAY);

				// Intern 12V
				((BackgroundWorker)sender).ReportProgress(70);
				SerialSender.SetPowerSupplyValues(12f, 0.2f, SerialSender.Channel.CH2);
				Thread.Sleep(DELAY);

				// Intern 24V
				((BackgroundWorker)sender).ReportProgress(80);
				SerialSender.SetPowerSupplyValues(24f, 0.15f, SerialSender.Channel.CH2);
				Thread.Sleep(DELAY);

				// Intern 48V
				((BackgroundWorker)sender).ReportProgress(90);
				SerialSender.SetPowerSupplyValues(48f, 0.15f, SerialSender.Channel.CH2);
				Thread.Sleep(DELAY);

				((BackgroundWorker)sender).ReportProgress(100);
				SerialSender.SetChannelState(0);
				this.Dispatcher.Invoke(() =>
				{
					TextBlockCurrentTestState.Text = "TEST: ENDED";
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
