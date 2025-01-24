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
    /// Interaktionslogik für UC_Terminal.xaml
    /// </summary>
    public partial class UC_Terminal : UserControl
    {
        public UC_Terminal()
        {
            InitializeComponent();
        }

		private void TextBoxCommandInput_KeyDown(object sender, KeyEventArgs e)
		{
            if (e.Key == Key.Enter)
            {
                string inputData = TextBoxCommandInput.Text;
                //string responseData = SerialSender.SendDataAndRecv(inputData);
                string responseData = string.Empty;
                if (responseData == "TIMEOUT")
                {
                    responseData = string.Empty;
                }
                TextBoxCommandOutput.Text += responseData + Environment.NewLine;
                TextBoxCommandInput.Text = string.Empty;
            }
		}
	}
}
