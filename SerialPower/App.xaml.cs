using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace SerialPower
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.ToString(), "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
			e.Handled = true;
			Environment.Exit(1);
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

			Console.WriteLine("Loading program...\n");
			Console.WriteLine($"Current release version:\t{fileVersionInfo.FileVersion}");
			Console.WriteLine($"DotNet version:\t\t\t{Environment.Version}");
			Console.WriteLine($"CPUs:\t\t\t\t{Environment.ProcessorCount}");
			Console.WriteLine($"Machine name:\t\t\t{Environment.MachineName}");
			Console.WriteLine($"Admin override:\t\t\t{Environment.IsPrivilegedProcess}");
			Console.WriteLine($"OS:\t\t\t\t{Environment.OSVersion}");

			Logger.PrintHeader("Test Logger System");
			Logger.PrintStatus("Info logging", Logger.StatusCode.INFO);
			Logger.PrintStatus("OK logging", Logger.StatusCode.OK);
			Logger.PrintStatus("FAILED logging", Logger.StatusCode.FAILED);

			Logger.PrintHeader("Starting application");
		}
	}
}
