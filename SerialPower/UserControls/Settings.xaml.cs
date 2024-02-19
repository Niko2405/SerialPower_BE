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
	/// Interaktionslogik für Settings.xaml
	/// </summary>
	public partial class Settings : UserControl
	{
		public Settings()
		{
			InitializeComponent();
			string[] coms = System.IO.Ports.SerialPort.GetPortNames();
			foreach (string com in coms)
			{
				Debug.WriteLine($"{com} found. Add to list");
				ListBoxComPorts.Items.Add(com);
			}
		}

		private void ListBoxComPorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				var selectedItem = e.AddedItems[0];
				if (selectedItem != null)
				{
					MessageBox.Show($"COM Port is now set to {selectedItem}", "COM Port Set", MessageBoxButton.OK, MessageBoxImage.Warning);
					SerialSender.SelectedPortName = selectedItem.ToString();
				}
			}
		}
	}
}
