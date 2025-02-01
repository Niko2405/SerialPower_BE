using NLua;
using System;
using System.Collections.Generic;
using System.IO;
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
		public Lua state = new Lua();

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
				Title = "Select Sequence file",
				InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\" + ConfigHandler.DIR_SEQUENCES.Replace('/', '\\'),
			};
			if (openFileDialog.ShowDialog() == true)
			{
				TextBoxSelectedFile.Text = openFileDialog.FileName;
			}
		}

		private void ButtonReadFile_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(TextBoxSelectedFile.Text);
				TextBoxFileName.Text = fileInfo.Name;
				TextBoxLastEdit.Text = fileInfo.LastWriteTime.ToString();
				TextBoxLastAccess.Text = fileInfo.LastAccessTime.ToString();
				TextBoxLenght.Text = fileInfo.Length.ToString();
				TextBoxExists.Text = fileInfo.Exists.ToString();

				if (fileInfo.Exists)
				{
					ButtonRunProgram.IsEnabled = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Write(ex.Message, Logger.StatusCode.ERROR);
				MessageBox.Show(ex.Message, "FileInfo Error", MessageBoxButton.OK, MessageBoxImage.Error);
			} 
		}

		private void ButtonRunProgram_Click(object sender, RoutedEventArgs e)
		{
			state.DoFile(TextBoxSelectedFile.Text.Trim());
			double voltageCH1 = (double)state["voltageCH1"];
			double currentCH1 = (double)state["currentCH1"];
			double voltageCH2 = (double)state["voltageCH2"];
			double currentCH2 = (double)state["currentCH2"];
			MessageBox.Show($"Current Test\nVoltage Channel 1: {voltageCH1}\nCurrent Channel 1: {currentCH1}\nVoltage Channel 2: {voltageCH2}\nCurrent Channel 2: {currentCH2}\n", "Lua Sequencer (Alpha)", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}
