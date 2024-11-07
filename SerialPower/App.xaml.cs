using System.Diagnostics;
using System.IO;
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

		private static void BuildFilesystem()
		{
			Logger.PrintHeader("Build Filesystem");
			try
			{
				Directory.CreateDirectory(ConfigHandler.DIR_ROOT);
				Logger.PrintStatus("Checking " + ConfigHandler.DIR_ROOT, Logger.StatusCode.OK);

				Directory.CreateDirectory(ConfigHandler.DIR_CONFIGS);
				Logger.PrintStatus("Checking " + ConfigHandler.DIR_CONFIGS, Logger.StatusCode.OK);

				Directory.CreateDirectory(ConfigHandler.DIR_DATABASE);
				Logger.PrintStatus("Checking " + ConfigHandler.DIR_DATABASE, Logger.StatusCode.OK);

				Directory.CreateDirectory(ConfigHandler.DIR_TEMP);
				Logger.PrintStatus("Checking " + ConfigHandler.DIR_TEMP, Logger.StatusCode.OK);

				Logger.PrintStatus("Build filesystem", Logger.StatusCode.OK);
			}
			catch (Exception)
			{
				Logger.PrintStatus("Build filesystem", Logger.StatusCode.FAILED);
			}
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

			Logger.PrintHeader("Booting");
			Console.WriteLine("Loading program...\n");
			Console.WriteLine("Start Arguments:\t\t" + e.Args.Length);
			foreach (var arg in e.Args)
			{
				Console.WriteLine(arg);
			}
			Console.WriteLine("Config Handler:\t\t\tJSON");
			Console.WriteLine($"Current release version:\t{fileVersionInfo.FileVersion}");
			Console.WriteLine($"DotNet version:\t\t\t{Environment.Version}");
			Console.WriteLine($"CPUs:\t\t\t\t{Environment.ProcessorCount}");
			Console.WriteLine($"Machine name:\t\t\t{Environment.MachineName}");
			Console.WriteLine($"Admin override:\t\t\t{Environment.IsPrivilegedProcess}");
			Console.WriteLine($"OS:\t\t\t\t{Environment.OSVersion}");

			Logger.PrintHeader("Testing Logger System");
			Logger.PrintStatus("Info logging", Logger.StatusCode.INFO);
			Logger.PrintStatus("Ok logging", Logger.StatusCode.OK);
			Logger.PrintStatus("Failed logging", Logger.StatusCode.FAILED);

			BuildFilesystem();
			ConfigHandler.Init();
			string configData = File.ReadAllText(ConfigHandler.CONFIG_FILE);
			Logger.PrintStatus("Current config settings:" + Environment.NewLine + configData, Logger.StatusCode.INFO);

			Logger.PrintHeader("Booting finished");
			Thread.Sleep(2500);
		}
	}
}
