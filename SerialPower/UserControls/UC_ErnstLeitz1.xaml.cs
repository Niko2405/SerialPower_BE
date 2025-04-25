using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TLogger;

namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für UC_ErnstLeitz1.xaml
	/// </summary>
	public partial class UC_ErnstLeitz1 : UserControl
	{
		private static readonly short DEFAULT_DELAY = 2000;
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

			Logger.Info("Starting automatic test phase");
		}

		private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
		{
			ProgressBarTestPhase.Value = e.ProgressPercentage;
		}

		private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			short delay = DEFAULT_DELAY;
			try
			{
				this.Dispatcher.Invoke(() =>
				{
					delay = short.Parse(TextBoxDelay.Text);
					Logger.Debug("Given delay: " + delay);
					if (delay <= 1000)
					{
						Logger.Warn($"Given delay is to short. Set default value [{DEFAULT_DELAY}ms]");
						TextBoxDelay.Text = DEFAULT_DELAY.ToString();
						delay = DEFAULT_DELAY;
					}
				});
				Logger.Info("Delay per test is now set to: " + delay);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (sender != null)
			{
				// Intern 0V
				Logger.PrintHeader("INTERN");
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
				Logger.PrintHeader("EXTERN");
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
