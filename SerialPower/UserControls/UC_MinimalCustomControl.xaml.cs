using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TartarosLogger;

namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für UC_MinimalCustomControl.xaml
	/// </summary>
	public partial class UC_MinimalCustomControl : UserControl
	{
		private bool _channel1active;
		private bool _channel2active;

		public UC_MinimalCustomControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Property of Channel2 State
		/// </summary>
		public bool Channel1Active
		{
			get
			{
				Logger.Debug("Get Property of Channel1Active to: " + _channel1active);
				return _channel1active;
			}
			set
			{
				_channel1active = value;
				Logger.Debug("Set Property of Channel1Active to: " + value);
				if (value)
				{
					ImagePowerCH1.Source = (ImageSource)this.TryFindResource("ImagePowerOn");
					SerialSender.SetChannelState(SerialSender.Channel.CH1, SerialSender.State.ON);
				}
				else if (!value)
				{
					ImagePowerCH1.Source = (ImageSource)this.TryFindResource("ImagePowerOff");
					SerialSender.SetChannelState(SerialSender.Channel.CH1, SerialSender.State.OFF);
				}
			}
		}

		/// <summary>
		/// Property of Channel 2 State
		/// </summary>
		public bool Channel2Active
		{
			get
			{
				Logger.Debug("Get Property of Channel2Active to: " + _channel2active);
				return _channel2active;
			}
			set
			{
				_channel2active = value;
				Logger.Debug("Set Property of Channel2Active to: " + value);
				if (value)
				{
					ImagePowerCH2.Source = (ImageSource)this.TryFindResource("ImagePowerOn");
					SerialSender.SetChannelState(SerialSender.Channel.CH2, SerialSender.State.ON);
				}
				else
				{
					ImagePowerCH2.Source = (ImageSource)this.TryFindResource("ImagePowerOff");
					SerialSender.SetChannelState(SerialSender.Channel.CH2, SerialSender.State.OFF);
				}
			}
		}

		private void ButtonPowerCH1_Click(object sender, RoutedEventArgs e)
		{
			Channel1Active = !Channel1Active;
		}

		private void ButtonPowerCH2_Click(object sender, RoutedEventArgs e)
		{
			Channel2Active = !Channel2Active;
		}

		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (Convert.ToBoolean(e.NewValue.ToString()))
			{
				SerialSender.SetChannelState(SerialSender.State.OFF);
			}
		}
	}
}
