using System.Windows.Controls;
using System.Windows.Input;

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
				string? inputData = TextBoxCommandInput.Text;
				string responseData = string.Empty;

				if (inputData != null)
				{
					//responseData = SerialSender.SendDataAndRecv(inputData);
					if (responseData == SerialSender.STATUSCODE.TIMEOUT.ToString())
					{
						responseData = string.Empty;
					}
					TextBoxCommandOutput.Text += responseData + Environment.NewLine;
					TextBoxCommandInput.Text = string.Empty;
				}
			}
		}
	}
}
