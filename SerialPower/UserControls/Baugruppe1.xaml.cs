using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für Baugruppe1.xaml
	/// </summary>
	public partial class Baugruppe1 : UserControl
	{
		public Baugruppe1()
		{
			InitializeComponent();
		}

		private void Button0V_Intern_Click(object sender, RoutedEventArgs e)
		{
			// Quelle 1 und 2 auf 0V setzen
			SerialSender.SendCommand("V1 0; V2 0");

			// Strombegrenzung setzen
			SerialSender.SendCommand("I1 0.15; I2 0.15");

			// Ausgänge einstellen
			SerialSender.SendCommand("OPALL 0");
		}

		private void Button6V_Intern_Click(object sender, RoutedEventArgs e)
		{
			SerialSender.SendCommand("V1 6; V2 0");
			SerialSender.SendCommand("I1 0.25; I2 0.25");
			SerialSender.SendCommand("OP1 1; OP2 0");
		}

		private void Button12V_Intern_Click(object sender, RoutedEventArgs e)
		{
			SerialSender.SendCommand("V1 12; V2 0");
			SerialSender.SendCommand("I1 0.15; I2 0.15");
			SerialSender.SendCommand("OP1 1; OP2 0");
		}

		private void Button24V_Intern_Click(object sender, RoutedEventArgs e)
		{
			SerialSender.SendCommand("V1 24; V2 0");
			SerialSender.SendCommand("I1 0.10; I2 0.10");
			SerialSender.SendCommand("OP1 1; OP2 0");
		}

		private void Button48V_Intern_Click(object sender, RoutedEventArgs e)
		{
			SerialSender.SendCommand("V1 48; V2 0");
			SerialSender.SendCommand("I1 0.10; I2 0.10");
			SerialSender.SendCommand("OP1 1; OP2 0");
		}

		private void Button0V_Extern_Click(object sender, RoutedEventArgs e)
		{
			SerialSender.SendCommand("V1 0; V2 0");
			SerialSender.SendCommand("I1 0.15; I2 0.15");
			SerialSender.SendCommand("OPALL 0");
		}

		private void Button6V_Extern_Click(object sender, RoutedEventArgs e)
		{
			SerialSender.SendCommand("V1 0; V2 6");
			SerialSender.SendCommand("I1 0.25; I2 0.25");
			SerialSender.SendCommand("OP1 0; OP2 1");
		}

		private void Button12V_Extern_Click(object sender, RoutedEventArgs e)
		{
			SerialSender.SendCommand("V1 0; V2 12");
			SerialSender.SendCommand("I1 0.15; I2 0.15");
			SerialSender.SendCommand("OP1 0; OP2 1");
		}

		private void Button24V_Extern_Click(object sender, RoutedEventArgs e)
		{
			SerialSender.SendCommand("V1 0; V2 24");
			SerialSender.SendCommand("I1 0.10; I2 0.10");
			SerialSender.SendCommand("OP1 0; OP2 1");
		}

		private void Button48V_Extern_Click(object sender, RoutedEventArgs e)
		{
			SerialSender.SendCommand("V1 0; V2 48");
			SerialSender.SendCommand("I1 0.10; I2 0.10");
			SerialSender.SendCommand("OP1 0; OP2 1");
		}
	}
}
