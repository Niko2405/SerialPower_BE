using SerialPower.UserControls;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SerialPower
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// If settings set, activate all buttons
		/// </summary>
		/// <param name="enable"></param>
		public void SetAllButtonState(bool enable)
		{
			ButtonBaugruppe1.IsEnabled = enable;
			ButtonBaugruppe2.IsEnabled = enable;
			ButtonBaugruppe3.IsEnabled = enable;
			ButtonBaugruppe4.IsEnabled = enable;
			ButtonBaugruppe5.IsEnabled = enable;
			ButtonBaugruppe6.IsEnabled = enable;
			ButtonBaugruppe7.IsEnabled = enable;
			ButtonBaugruppe8.IsEnabled = enable;
			ButtonBaugruppe9.IsEnabled = enable;
			ButtonBaugruppe10.IsEnabled = enable;
			ButtonBaugruppe11.IsEnabled = enable;
			ButtonBaugruppe12.IsEnabled = enable;
			ButtonBaugruppe13.IsEnabled = enable;

			ButtonCustom.IsEnabled = enable;
			ButtonInfo.IsEnabled = enable;
		}

		/// <summary>
		/// Change primary user control
		/// </summary>
		/// <param name="userControl"></param>
		public void SetActiveUserControl(UserControl userControl)
		{
			// Disable Settings Button
			Console.Clear();
			ButtonSettings.IsEnabled = false;

			Logging.Warn($"Change UserControl to: {userControl.GetType().Name}");
			//BaugruppeOne.Visibility = Visibility.Collapsed;
			UserControlBaugruppe1.Visibility = Visibility.Collapsed;
			UserControlBaugruppe2.Visibility = Visibility.Collapsed;
			UserControlCustom.Visibility = Visibility.Collapsed;
			UserControlInfo.Visibility = Visibility.Collapsed;
			UserControlSettings.Visibility = Visibility.Collapsed;

			// only show current usercontrol
			userControl.Visibility = Visibility.Visible;
		}

		private void Baugruppe1_Click(object sender, RoutedEventArgs e)
		{
			// CWSO Ernst Leitz
			Console.Title = "85/15186";
			SetActiveUserControl(UserControlBaugruppe1);
		}

		private void Baugruppe2_Click(object sender, RoutedEventArgs e)
		{
			// IDE
			Console.Title = "87/10582";
			SetActiveUserControl(UserControlBaugruppe2);
		}

		private void Baugruppe3_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe4_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe5_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe6_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe7_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe8_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe9_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe10_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		private void Baugruppe11_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe12_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe13_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void ButtonSettings_Click(object sender, RoutedEventArgs e)
		{
			SetActiveUserControl(UserControlSettings);
		}

		private void ButtonExit_Click(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
		}

		private void ButtonInfo_Click(object sender, RoutedEventArgs e)
		{
			Console.Title = "Info";
			SetActiveUserControl(UserControlInfo);
        }

		private void ButtonCustom_Click(object sender, RoutedEventArgs e)
		{
			Console.Title = "Manual control";
			SetActiveUserControl(UserControlCustom);
		}
	}
}
