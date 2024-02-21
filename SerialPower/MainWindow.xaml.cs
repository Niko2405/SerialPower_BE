﻿using SerialPower.UserControls;
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
		public void SetActiveUserControl(UserControl userControl)
		{
			Debug.WriteLine($"Change UserControl to: {userControl.GetType().Name}");
			//BaugruppeOne.Visibility = Visibility.Collapsed;
			UserControlBaugruppe1.Visibility = Visibility.Collapsed;
			UserControlBaugruppe2.Visibility = Visibility.Collapsed;
			UserControlCustom.Visibility = Visibility.Collapsed;
			UserControlSettings.Visibility = Visibility.Collapsed;

			// only show current usercontrol
			userControl.Visibility = Visibility.Visible;
		}

		private void Baugruppe1_Click(object sender, RoutedEventArgs e)
		{
			// CWSO Ernst Leitz
			SetActiveUserControl(UserControlBaugruppe1);
			UserControlBaugruppe2.active = false;
		}

		private void Baugruppe2_Click(object sender, RoutedEventArgs e)
		{
			// IDE
			SetActiveUserControl(UserControlBaugruppe2);
			UserControlBaugruppe2.active = true;
		}

		private void Baugruppe3_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe4_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe5_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe6_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe7_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe8_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe9_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void Baugruppe10_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Keine Baugruppe registriert", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void ButtonSettings_Click(object sender, RoutedEventArgs e)
		{
			SetActiveUserControl(UserControlSettings);
			UserControlBaugruppe2.active = false;
		}

		private void ButtonExit_Click(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
		}

		private void ButtonInfo_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Delevoper: Nils Kornmann\nVersion: 2.3a\nCompiled: 22.02.2024\nCompiled with dotnet SDK 8.0\nLicense: MIT", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

		private void ButtonCustom_Click(object sender, RoutedEventArgs e)
		{
			SetActiveUserControl(UserControlCustom);
			UserControlBaugruppe2.active = false;
		}
    }
}
