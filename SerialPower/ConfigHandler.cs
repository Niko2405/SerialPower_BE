using System.IO;
using System.Text.Json;
using TartarosLogger;

namespace SerialPower
{
	class ConfigHandler
	{
		/// <summary>
		/// Global JsonOptions
		/// </summary>
		public static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };


		/// <summary>
		/// Config object of serialsender
		/// </summary>
		public static SerialSender? serialConfig;

		// Directories
		public static readonly string DIR_ROOT = "serialPower/";
		public static readonly string DIR_DATABASE = DIR_ROOT + "data/";
		public static readonly string DIR_CONFIGS = DIR_ROOT + "config/";
		public static readonly string DIR_TEMP = DIR_ROOT + "tmp/";

		// Files
		public static readonly string FILE_CONFIG_SERIAL = DIR_CONFIGS + "serial.json";
		public static readonly string VERSION_FILE = DIR_DATABASE + "version.dat";


		/// <summary>
		/// Create config files
		/// </summary>
		private static void CreateConfig()
		{
			try
			{
				File.WriteAllText(FILE_CONFIG_SERIAL, JsonSerializer.Serialize(new SerialSender(), JsonOptions));
			}
			catch (Exception e)
			{
				Logger.Error($"Failed to create config: {e.Message}");
			}
		}

		/// <summary>
		/// Save current object setting into json file
		/// </summary>
		public static void Save()
		{
			Logger.Info("Save current config into json file");
			try
			{
				// async!
				File.WriteAllTextAsync(FILE_CONFIG_SERIAL, JsonSerializer.Serialize(serialConfig, JsonOptions));
			}
			catch (Exception e)
			{
				Logger.Error("ConfigHandler: " + e.Message);
			}
		}

		/// <summary>
		/// Load data from json file into global objects
		/// </summary>
		public static void Load()
		{
			Logger.Info($"Load config from {FILE_CONFIG_SERIAL}");
			if (!File.Exists(FILE_CONFIG_SERIAL))
			{
				CreateConfig();
			}

			try
			{
				serialConfig = JsonSerializer.Deserialize<SerialSender>(File.ReadAllText(FILE_CONFIG_SERIAL), JsonOptions);
				Logger.Debug($"Curernt config: {FILE_CONFIG_SERIAL + System.Environment.NewLine + File.ReadAllText(FILE_CONFIG_SERIAL)}");
			}
			catch (Exception e)
			{
				Logger.Error("ConfigHandler: " + e.Message);
			}
		}
	}
}
