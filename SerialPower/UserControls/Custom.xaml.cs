using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	/// Interaktionslogik für Custom.xaml
	/// </summary>
	public partial class Custom : UserControl
	{
		public Custom()
		{
			InitializeComponent();
		}
		// remove chars like V and empty. Replace , to .
		private static string ClearInputData(string rawData)
		{
			return rawData.Replace(",", ".").Replace("V", "").Replace(" ", "");
		}
		private void CheckBoxCH1_Checked(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("CH1 Online");
			SerialSender.SendCommand("OP1 1");
		}

		private void CheckBoxCH2_Checked(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("CH2 Online");
			SerialSender.SendCommand("OP2 1");
		}

		private void CheckBoxCH2_Unchecked(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("CH1 Offline");
			SerialSender.SendCommand("OP2 0");
		}

		private void CheckBoxCH1_Unchecked(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("CH1 Offline");
			SerialSender.SendCommand("OP1 0");
		}

		private void ButtonCH1_Click(object sender, RoutedEventArgs e)
		{
			// current setter
			string current = TextBox_CH1Current.Text;
			current = ClearInputData(current);
			SerialSender.SendCommand($"I1 {current}");

			// voltage setter
			string voltage = TextBox_CH1Voltage.Text;
			voltage = ClearInputData(voltage);
			SerialSender.SendCommand($"V1 {voltage}");
		}

		private void ButtonCH2_Click(object sender, RoutedEventArgs e)
		{
			// current setter
			string current = TextBox_CH2Current.Text;
			current = ClearInputData(current);
			SerialSender.SendCommand($"I2 {current}");

			// voltage setter
			string voltage = TextBox_CH2Voltage.Text;
			voltage = ClearInputData(voltage);
			SerialSender.SendCommand($"V2 {voltage}");
		}
    }
}
