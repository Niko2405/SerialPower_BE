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
		}

		private void Button0V_Intern_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Button 0V - Intern");

			// Quelle 1 und 2 auf 0V setzen
			SerialSender.SendData("V1 0; V2 0");

			// Strombegrenzung setzen
			SerialSender.SendData("I1 0.15; I2 0.15");

			// Ausgänge einstellen
			SerialSender.SendData("OPALL 0");
		}

		private void Button6V_Intern_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Button 6V - Intern");

			SerialSender.SendData("V1 6; V2 0");
			SerialSender.SendData("I1 0.25; I2 0.25");
			SerialSender.SendData("OP1 1; OP2 0");
		}

		private void Button12V_Intern_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Button 12V - Intern");

			SerialSender.SendData("V1 12; V2 0");
			SerialSender.SendData("I1 0.15; I2 0.15");
			SerialSender.SendData("OP1 1; OP2 0");
		}

		private void Button24V_Intern_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Button 24V - Intern");

			SerialSender.SendData("V1 24; V2 0");
			SerialSender.SendData("I1 0.10; I2 0.10");
			SerialSender.SendData("OP1 1; OP2 0");
		}

		private void Button48V_Intern_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Button 48V - Intern");

			SerialSender.SendData("V1 48; V2 0");
			SerialSender.SendData("I1 0.10; I2 0.10");
			SerialSender.SendData("OP1 1; OP2 0");
		}

		private void Button0V_Extern_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Button 0V - Extern");

			SerialSender.SendData("V1 0; V2 0");
			SerialSender.SendData("I1 0.15; I2 0.15");
			SerialSender.SendData("OPALL 0");
		}

		private void Button6V_Extern_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Button 6V - Extern");

			SerialSender.SendData("V1 0; V2 6");
			SerialSender.SendData("I1 0.25; I2 0.25");
			SerialSender.SendData("OP1 0; OP2 1");
		}

		private void Button12V_Extern_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Button 12V - Extern");

			SerialSender.SendData("V1 0; V2 12");
			SerialSender.SendData("I1 0.15; I2 0.15");
			SerialSender.SendData("OP1 0; OP2 1");
		}

		private void Button24V_Extern_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Button 24V - Extern");

			SerialSender.SendData("V1 0; V2 24");
			SerialSender.SendData("I1 0.10; I2 0.10");
			SerialSender.SendData("OP1 0; OP2 1");
		}

		private void Button48V_Extern_Click(object sender, RoutedEventArgs e)
		{
			Logger.PrintHeader("Button 48V - Extern");

			SerialSender.SendData("V1 0; V2 48");
			SerialSender.SendData("I1 0.10; I2 0.10");
			SerialSender.SendData("OP1 0; OP2 1");
		}
	}
}
