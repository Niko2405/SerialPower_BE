using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TartarosLogger;

namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für UC_Sequencer.xaml
	/// </summary>
	public partial class UC_Sequencer : UserControl
	{
		private readonly BackgroundWorker BackgroundWorkerSequencer = new();
		public static ObservableCollection<Sequence> SequenceCollection = new();

		public UC_Sequencer()
		{
			InitializeComponent();
			BackgroundWorkerSequencer.DoWork += BackgroundWorkerSequencerWorker_DoWork;
			ListViewProgram.ItemsSource = SequenceCollection;

			if (SerialSender.TestingMode)
			{
				SequenceCollection.Add(new Sequence() { Id = 0, Comment = "Kanal 1 High", Channel = 1, Voltage = 5.0f, Current = 0.5f, State = 1, Delay = 5 });
				SequenceCollection.Add(new Sequence() { Id = 1, Comment = "Kanal 2 High", Channel = 2, Voltage = 5.0f, Current = 0.5f, State = 1, Delay = 5 });

				SequenceCollection.Add(new Sequence() { Id = 2, Comment = "Kanal 1 Low", Channel = 1, Voltage = 5.0f, Current = 0.5f, State = 0, Delay = 5 });
				SequenceCollection.Add(new Sequence() { Id = 3, Comment = "Kanal 2 Low", Channel = 2, Voltage = 5.0f, Current = 0.5f, State = 0, Delay = 5 });
			}
			BackgroundWorkerSequencer.WorkerSupportsCancellation = true;
		}

		private void BackgroundWorkerSequencerWorker_DoWork(object? sender, DoWorkEventArgs e)
		{
			int counter = 0;
			if (ConfigHandler.serialConfig != null)
			{
				// endless check if IsSequencerRunning is True
				while (true)
				{
					if (ConfigHandler.serialConfig.IsSequencerRunning)
					{
						counter++;
						for (int i = 0; i < SequenceCollection.Count; i++)
						{
							if (BackgroundWorkerSequencer.CancellationPending)
							{
								Logger.Warn("BackgroundWorkerSequencer: CancellationPending = true");
								e.Cancel = true;
								break;
							}

							Logger.PrintHeader($"ID [{SequenceCollection[i].Id}]\t{SequenceCollection[i].Comment}");

							Logger.Info($"Run: {counter}");
							SerialSender.SetChannelState(SequenceCollection[i].Channel, SequenceCollection[i].State);
							SerialSender.SetPowerSupplyValues(SequenceCollection[i].Voltage, SequenceCollection[i].Current, SequenceCollection[i].Channel);
							Thread.Sleep((int)SequenceCollection[i].Delay * 1000);
						}
						Logger.PrintHeader("Current sequence ended");
					}
					else if (!ConfigHandler.serialConfig.IsSequencerRunning)
					{
						Thread.Sleep(1000);
					}
				}
			}
		}

		public class Sequence
		{
			public required int Id { get; set; }
			public required string Comment { get; set; }
			public required int Channel { get; set; }
			public required float Voltage { get; set; }
			public required float Current { get; set; }
			public required int State { get; set; }
			public required float Delay { get; set; }
		}

		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (Convert.ToBoolean(e.NewValue.ToString()))
			{
				SerialSender.SetChannelState(SerialSender.State.OFF);
			}
		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int _id = SequenceCollection.Count;
				string _comment = TextBoxComment.Text;
				int _channel = int.Parse(TextBoxChannel.Text);
				float _voltage = float.Parse(TextBoxVoltage.Text);
				float _current = float.Parse(TextBoxCurrent.Text);
				int _state = int.Parse(TextBoxState.Text);
				float _delay = float.Parse(TextBoxDelay.Text);

				// check channel
				if (_channel != 1 && _channel != 2)
				{
					Logger.Error($"Channel: {_channel} is invalid");
					MessageBox.Show("Only Channel 1 or 2 can be used", $"Invalid channel: {_channel}.\nUse 1 or 2 instead!", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				// check voltage
				if (_voltage < 0.0f || _voltage > 60.0f)
				{
					Logger.Error($"Voltage: {_voltage} is invalid");
					MessageBox.Show("Voltage can only be from 0 to 60", $"Invalid voltage: {_voltage}.", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				// check current
				if (_current < 0.0f || _current > 10.0f)
				{
					Logger.Error($"Current: {_voltage} is invalid");
					MessageBox.Show("Current can only be from 0 to 10", $"Invalid current: {_current}.", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				// check state
				if (_state != 0 && _state != 1)
				{
					Logger.Error($"State: {_state} is invalid");
					MessageBox.Show("State 0 or 1 can be used", $"Invalid state: {_state}.", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				// add to list
				SequenceCollection.Add(new Sequence() { Id = _id, Comment = TextBoxComment.Text, Channel = _channel, Voltage = _voltage, Current = _current, State = _state, Delay = _delay });
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
				MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				TextBoxComment.Clear();
				TextBoxChannel.Clear();
				TextBoxVoltage.Clear();
				TextBoxCurrent.Clear();
				TextBoxState.Clear();
				TextBoxDelay.Clear();
			}
		}

		private void ButtonDelete_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SequenceCollection.RemoveAt(SequenceCollection.Count - 1);
			}
			catch (ArgumentOutOfRangeException)
			{
				Logger.Warn($"Cannot delete item in SequenceCollection. Index [{SequenceCollection.Count - 1}] is out of range.");
			}
		}

		private void ButtonStart_Click(object sender, RoutedEventArgs e)
		{
			if (SequenceCollection.Count == 0)
			{
				Logger.Warn("Program is empty");
				return;
			}

			if (ConfigHandler.serialConfig != null)
			{
				Logger.Info("Sequencer is running");
				ConfigHandler.serialConfig.IsSequencerRunning = true;
				ConfigHandler.Save();

				ButtonStart.IsEnabled = false;
				ButtonStop.IsEnabled = true;
				BackgroundWorkerSequencer.RunWorkerAsync();
			}
		}

		private void ButtonStop_Click(object sender, RoutedEventArgs e)
		{
			if (ConfigHandler.serialConfig != null)
			{
				Logger.Info("Sequencer stopping");
				ConfigHandler.serialConfig.IsSequencerRunning = false;
				ConfigHandler.Save();

				ButtonStart.IsEnabled = true;
				ButtonStop.IsEnabled = false;

				BackgroundWorkerSequencer.CancelAsync();
			}
		}

		private void ButtonReset_Click(object sender, RoutedEventArgs e)
		{
			if (ConfigHandler.serialConfig != null)
			{
				Logger.Info("Sequencer stopping");
				ConfigHandler.serialConfig.IsSequencerRunning = false;
				ConfigHandler.Save();

				ButtonStart.IsEnabled = true;
				ButtonStop.IsEnabled = false;

				BackgroundWorkerSequencer.CancelAsync();
			}
		}
	}
}
