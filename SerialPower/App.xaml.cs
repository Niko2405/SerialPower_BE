using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using TLogger;

namespace SerialPower
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		[DllImport("user32.dll")]
		private static extern long SetCursorPos(int x, int y);

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
				if (File.Exists(ConfigHandler.VERSION_FILE))
				{
					Logger.Info("Version file found. Reading version...");
					string versionFile = File.ReadAllText(ConfigHandler.VERSION_FILE);
					if (versionFile != fileVersionInfo.FileVersion)
					{
						Logger.Error("Version of filesystem is not supported");
						MessageBox.Show($"Filesystem version: {versionFile}\nProgram version: {fileVersionInfo.FileVersion}\nCurrent filesystem is unsupported. Please delete filesystem: " + System.Environment.CurrentDirectory.Replace("\\", "/") + "/" + ConfigHandler.DIR_ROOT, "Unsupported filesystem detected", MessageBoxButton.OK, MessageBoxImage.Error);
						Environment.Exit(1);
					}
					else if (versionFile == fileVersionInfo.FileVersion)
					{
						Logger.Info("Version of filesystem is supported");
					}
					return;
				}
				else
				{
					File.WriteAllText(ConfigHandler.VERSION_FILE, fileVersionInfo.FileVersion);
					Logger.Info("New version file created");
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
				Logger.Info("Checking " + ConfigHandler.DIR_ROOT);

				Directory.CreateDirectory(ConfigHandler.DIR_CONFIGS);
				Logger.Info("Checking " + ConfigHandler.DIR_CONFIGS);

				Directory.CreateDirectory(ConfigHandler.DIR_DATABASE);
				Logger.Info("Checking " + ConfigHandler.DIR_DATABASE);

				Directory.CreateDirectory(ConfigHandler.DIR_TEMP);
				Logger.Info("Checking " + ConfigHandler.DIR_TEMP);

				Logger.Info("Build filesystem");
			}
			catch (Exception)
			{
				Logger.Error("Build filesystem");
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
			SetCursorPos(0, 0);
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
					Logger.DebugEnabled = true;
				}
				if (arg == "--disableCommunication")
				{
					SerialSender.DisableCommunication = true;
					MessageBox.Show("You're communication was disabled", "COMMUNICATION IS OFFLINE", MessageBoxButton.OK, MessageBoxImage.Warning);
				}
				if (arg == "--help")
				{
					Console.WriteLine("Available commands:\n--disablePortVerify\tDisable port verify to scan for power supplies.\n--debug\tEnable Debug\n--disableCommunication\tDisable communication with COM. DUMMY MODE");
					Environment.Exit(0);
				}
			}

			// Print infos
			Logger.PrintHeader("SYSTEMINFO");
			Console.WriteLine("Config Handler:\t\t\tJSON");
			Console.WriteLine($"Current release version:\t{fileVersionInfo.FileVersion}");
			Console.WriteLine($"DotNet version:\t\t\t{Environment.Version}");
			Console.WriteLine($"CPUs:\t\t\t\t{Environment.ProcessorCount}");
			Console.WriteLine($"Machine name:\t\t\t{Environment.MachineName}");
			Console.WriteLine($"Admin override:\t\t\t{Environment.IsPrivilegedProcess}");
			Console.WriteLine($"Operating system:\t\t{Environment.OSVersion}");

			// Test logger
			Logger.PrintHeader("Logger");
			Logger.Info("TLogger Version: " + Logger.Version);
			Logger.Info("Info Message");
			Logger.Warn("Warning Message");
			Logger.Error("Error Message");
			Logger.Debug("Debug Message");

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
#if DEBUG
			Console.Title = $"SerialPower - v{fileVersionInfo.FileVersion} - DEBUG";
#endif
			Thread.Sleep(2000);
		}
	}
}
