using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für UC_Info.xaml
	/// </summary>
	public partial class UC_Info : UserControl
	{
		public UC_Info()
		{
			InitializeComponent();
		}

		private void InfoLoaded(object sender, RoutedEventArgs e)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
			LabelVersion.Content = $"Version: {fileVersionInfo.FileVersion}";
			LabelDotNetVersion.Content = $"DotNet Version: {Environment.Version}";
		}
	}
}
