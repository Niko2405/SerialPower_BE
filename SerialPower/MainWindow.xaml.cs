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
		/// <param name="state"></param>
		public void SetAllButtonState(bool state)
		{
			ButtonBaugruppe1.IsEnabled = state;
			ButtonBaugruppe2.IsEnabled = state;
			ButtonBaugruppe3.IsEnabled = state;
			ButtonBaugruppe4.IsEnabled = state;
			ButtonBaugruppe5.IsEnabled = state;
			ButtonBaugruppe6.IsEnabled = state;
			ButtonBaugruppe7.IsEnabled = state;
			ButtonBaugruppe8.IsEnabled = state;
			ButtonBaugruppe9.IsEnabled = state;
			ButtonBaugruppe10.IsEnabled = state;
			ButtonBaugruppe11.IsEnabled = state;
			ButtonBaugruppe12.IsEnabled = state;
			ButtonBaugruppe13.IsEnabled = state;

			ButtonCustom.IsEnabled = state;
			ButtonInfo.IsEnabled = state;
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

			Logger.PrintStatus($"Change UserControl to: {userControl.GetType().Name}", Logger.StatusCode.INFO);
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

		private void WindowClosed(object sender, EventArgs e)
		{
			Logger.PrintStatus("Exit", Logger.StatusCode.INFO);
			Environment.Exit(0);
		}
	}
}
