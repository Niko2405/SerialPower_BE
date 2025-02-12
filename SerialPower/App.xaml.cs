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

		/// <summary>
		/// Check filesystem version of the program
		/// </summary>
		private static void CheckFilesystemVersion()
		{
			// Check version of filesystem
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

			try
			{
				if (File.Exists(ConfigHandler.DIR_DATABASE + "version.dat"))
				{
					Logger.Write("Version file found. Reading version...", Logger.StatusCode.INFO);
					string versionFile = File.ReadAllText(ConfigHandler.DIR_DATABASE + "version.dat");
					if (versionFile != fileVersionInfo.FileVersion)
					{
						Logger.Write("Version of filesystem is not supported", Logger.StatusCode.ERROR);
						MessageBox.Show($"Filesystem version: {versionFile}\nProgram version: {fileVersionInfo.FileVersion}\nCurrent filesystem is unsupported. Please delete filesystem: " + System.Environment.CurrentDirectory.Replace("\\", "/") + "/" + ConfigHandler.DIR_ROOT, "Unsupported filesystem detected", MessageBoxButton.OK, MessageBoxImage.Error);
						Environment.Exit(1);
					}
					else if (versionFile == fileVersionInfo.FileVersion)
					{
						Logger.Write("Version of filesystem is supported", Logger.StatusCode.INFO);
					}
					return;
				}
				else
				{
					File.WriteAllText(ConfigHandler.DIR_DATABASE + "version.dat", fileVersionInfo.FileVersion);
					Logger.Write("New version file created", Logger.StatusCode.INFO);
					return;
				}
			}
			catch (DirectoryNotFoundException)
			{
				// First start
				return;
			}
		}

		/// <summary>
		/// Create base filesystem
		/// </summary>
		private static void BuildFilesystem()
		{
			try
			{
				Directory.CreateDirectory(ConfigHandler.DIR_ROOT);
				Logger.Write("Checking " + ConfigHandler.DIR_ROOT, Logger.StatusCode.INFO);

				Directory.CreateDirectory(ConfigHandler.DIR_CONFIGS);
				Logger.Write("Checking " + ConfigHandler.DIR_CONFIGS, Logger.StatusCode.INFO);

				Directory.CreateDirectory(ConfigHandler.DIR_DATABASE);
				Logger.Write("Checking " + ConfigHandler.DIR_DATABASE, Logger.StatusCode.INFO);

				Directory.CreateDirectory(ConfigHandler.DIR_TEMP);
				Logger.Write("Checking " + ConfigHandler.DIR_TEMP, Logger.StatusCode.INFO);

				Logger.Write("Build filesystem", Logger.StatusCode.INFO);
			}
			catch (Exception)
			{
				Logger.Write("Build filesystem", Logger.StatusCode.ERROR);
			}
		}

		/// <summary>
		/// First entry-point
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			Console.WriteLine("Loading program...\n");
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

			// read args
			Console.WriteLine("Start Arguments length:\t\t" + e.Args.Length);
			foreach (var arg in e.Args)
			{
				Console.WriteLine(arg);
				if (arg == "--disablePortVerify")
				{
					SerialSender.DisablePortVerify = true;
				}
				if (arg == "--debug")
				{
					Logger.isDebugEnabled = true;
				}
				if (arg == "--help")
				{
					Console.WriteLine("usable commands:\n--disablePortVerify\tDisable port verify to scan for power supplies.");
					Environment.Exit(0);
				}
			}

			// Print infos
			Logger.PrintHeader("Systeminfo");
			Console.WriteLine("Config Handler:\t\t\tJSON");
			Console.WriteLine($"Current release version:\t{fileVersionInfo.FileVersion}");
			Console.WriteLine($"DotNet version:\t\t\t{Environment.Version}");
			Console.WriteLine($"CPUs:\t\t\t\t{Environment.ProcessorCount}");
			Console.WriteLine($"Machine name:\t\t\t{Environment.MachineName}");
			Console.WriteLine($"Admin override:\t\t\t{Environment.IsPrivilegedProcess}");
			Console.WriteLine($"Operating system:\t\t{Environment.OSVersion}");

			// Test logger
			Logger.PrintHeader("Logger");
			Logger.Write("Testing Logger System", Logger.StatusCode.INFO);
			Logger.Write("Logger Test: Info logging", Logger.StatusCode.INFO);
			Logger.Write("Logger Test: Warning logging", Logger.StatusCode.WARNING);
			Logger.Write("Logger Test: Failed logging", Logger.StatusCode.ERROR);
			Logger.Write("Logger Test: Debug logging", Logger.StatusCode.DEBUG);

			// Create filesystem
			Logger.PrintHeader("Filesystem check");
			BuildFilesystem();

			// Check Filesystem Version
			CheckFilesystemVersion();

			// Load primary config
			Logger.PrintHeader("Config system");
			ConfigHandler.Init();
			ConfigHandler.PrintConfig();

			Console.Title = $"SerialPower - v{fileVersionInfo.FileVersion}";
			Thread.Sleep(1000);
		}
	}
}
