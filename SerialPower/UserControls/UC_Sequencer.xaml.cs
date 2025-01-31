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
    /// Interaktionslogik für UC_Sequencer.xaml
    /// </summary>
    public partial class UC_Sequencer : UserControl
    {
        public UC_Sequencer()
        {
            InitializeComponent();
        }

		private void ButtonOpenFile_Click(object sender, RoutedEventArgs e)
		{
            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                CheckPathExists = true,
                Filter = "Lua scripts (*.lua)|*.lua",
                Title = "Select Sequence",
                InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\" + ConfigHandler.DIR_SEQUENCES.Replace('/', '\\'),
            };
            if(openFileDialog.ShowDialog() == true)
            {
                TextBoxSelectedFile.Text = openFileDialog.FileName;
            }
		}
    }
}
