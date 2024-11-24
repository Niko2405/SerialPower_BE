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
			Logger.Write("Button 0V - Intern", Logger.StatusCode.INFO);

			// Quelle 1 und 2 auf 0V setzen
			SerialSender.SendData("V1 0; V2 0");

			// Strombegrenzung setzen
			SerialSender.SendData("I1 0.15; I2 0.15");

			// Ausgänge einstellen
			SerialSender.SendData("OPALL 0");
		}

		private void Button6V_Intern_Click(object sender, RoutedEventArgs e)
		{
			Logger.Write("Button 6V - Intern", Logger.StatusCode.INFO);

			SerialSender.SendData("V1 6; V2 0");
			SerialSender.SendData("I1 0.25; I2 0.25");
			SerialSender.SendData("OP1 1; OP2 0");
		}

		private void Button12V_Intern_Click(object sender, RoutedEventArgs e)
		{
			Logger.Write("Button 12V - Intern", Logger.StatusCode.INFO);

			SerialSender.SendData("V1 12; V2 0");
			SerialSender.SendData("I1 0.15; I2 0.15");
			SerialSender.SendData("OP1 1; OP2 0");
		}

		private void Button24V_Intern_Click(object sender, RoutedEventArgs e)
		{
			Logger.Write("Button 24V - Intern", Logger.StatusCode.INFO);

			SerialSender.SendData("V1 24; V2 0");
			SerialSender.SendData("I1 0.10; I2 0.10");
			SerialSender.SendData("OP1 1; OP2 0");
		}

		private void Button48V_Intern_Click(object sender, RoutedEventArgs e)
		{
			Logger.Write("Button 48V - Intern", Logger.StatusCode.INFO);

			SerialSender.SendData("OP1 1; OP2 0");

			// increase power slowly
			SerialSender.SendData("V1 12; V2 0");
			SerialSender.SendData("V1 24; V2 0");
			SerialSender.SendData("V1 48; V2 0");

			SerialSender.SendData("I1 0.10; I2 0.10");
		}

		private void Button0V_Extern_Click(object sender, RoutedEventArgs e)
		{
			Logger.Write("Button 0V - Extern", Logger.StatusCode.INFO);

			SerialSender.SendData("V1 0; V2 0");
			SerialSender.SendData("I1 0.15; I2 0.15");
			SerialSender.SendData("OPALL 0");
		}

		private void Button6V_Extern_Click(object sender, RoutedEventArgs e)
		{
			Logger.Write("Button 6V - Extern", Logger.StatusCode.INFO);

			SerialSender.SendData("V1 0; V2 6");
			SerialSender.SendData("I1 0.25; I2 0.25");
			SerialSender.SendData("OP1 0; OP2 1");
		}

		private void Button12V_Extern_Click(object sender, RoutedEventArgs e)
		{
			Logger.Write("Button 12V - Extern", Logger.StatusCode.INFO);

			SerialSender.SendData("V1 0; V2 12");
			SerialSender.SendData("I1 0.15; I2 0.15");
			SerialSender.SendData("OP1 0; OP2 1");
		}

		private void Button24V_Extern_Click(object sender, RoutedEventArgs e)
		{
			Logger.Write("Button 24V - Extern", Logger.StatusCode.INFO);

			SerialSender.SendData("V1 0; V2 24");
			SerialSender.SendData("I1 0.10; I2 0.10");
			SerialSender.SendData("OP1 0; OP2 1");
		}

		private void Button48V_Extern_Click(object sender, RoutedEventArgs e)
		{
			Logger.Write("Button 48V - Extern", Logger.StatusCode.INFO);

			// increase power slowly
			SerialSender.SendData("V1 0; V2 12");
			SerialSender.SendData("V1 0; V2 24");
			SerialSender.SendData("V1 0; V2 48");

			SerialSender.SendData("I1 0.10; I2 0.10");
			SerialSender.SendData("OP1 0; OP2 1");
		}
	}
}
