using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SerialPower
{
	class ConfigHandler
	{
		/// <summary>
		/// Global JsonOptions
		/// </summary>
		public static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

		// Directories
		public static readonly string DIR_ROOT = "serialPower/";
		public static readonly string DIR_DATABASE = DIR_ROOT + "data/";
		public static readonly string DIR_CONFIGS = DIR_ROOT + "config/";
		public static readonly string DIR_TEMP = DIR_ROOT + "tmp/";

		// Files
		public static readonly string CONFIG_FILE = DIR_CONFIGS + "config.json";

		public static ConfigObject? currentConfig;

		/// <summary>
		/// Load config from file
		/// </summary>
		public static void Init()
		{
			if (!File.Exists(CONFIG_FILE))
			{
				try
				{
					File.WriteAllText(CONFIG_FILE, JsonSerializer.Serialize(new ConfigObject(), JsonOptions));
					Logger.PrintStatus("Create config at " + CONFIG_FILE, Logger.StatusCode.OK);
				}
				catch (UnauthorizedAccessException)
				{
					Logger.PrintStatus("No permission to create config file at " + CONFIG_FILE, Logger.StatusCode.FAILED);
					Environment.Exit(1);
				}
			}
			currentConfig = JsonSerializer.Deserialize<ConfigObject>(File.ReadAllText(CONFIG_FILE), JsonOptions);
			if (currentConfig == null)
			{
				Logger.PrintStatus("Init config", Logger.StatusCode.FAILED);
				return;
			}
			Logger.PrintStatus("Init config", Logger.StatusCode.OK);
		}

		/// <summary>
		/// Save current config
		/// </summary>
		public static void SaveConfig()
		{
			try
			{
				File.WriteAllText(CONFIG_FILE, JsonSerializer.Serialize(currentConfig, JsonOptions));
				Logger.PrintStatus("Config file saved at " + CONFIG_FILE, Logger.StatusCode.OK);
			}
			catch (UnauthorizedAccessException)
			{
				Logger.PrintStatus("No permission to create config file at " + CONFIG_FILE, Logger.StatusCode.FAILED);
				Environment.Exit(1);
			}
		}

		/// <summary>
		/// Primary Config Interface
		/// </summary>
		public class ConfigObject
		{
			// default settings
			public string SerialPortName { get; set; } = "COM1";
			public int SerialPortBaudrate { get; set; } = 9600;
			public int SerialPortParity { get; set; } = 0;
			public int SerialPortStopBits { get; set; } = 1;
			public int SerialPortDataBits { get; set; } = 8;
			public int SerialPortReadTimeOut { get; set; } = 50;
			public int SerialPortWriteTimeOut { get; set; } = 50;
			public int CurrentMonitorRate { get; set; } = 1000;
		}
	}
}
