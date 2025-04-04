﻿using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;

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
		public static readonly string VERSION_FILE = DIR_DATABASE + "version.dat";

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
					Logger.Write("Create config at " + CONFIG_FILE, Logger.StatusCode.INFO);
				}
				catch (UnauthorizedAccessException)
				{
					Logger.Write("No permission to create config file at " + CONFIG_FILE, Logger.StatusCode.ERROR);
					Environment.Exit(1);
				}
			}
			currentConfig = JsonSerializer.Deserialize<ConfigObject>(File.ReadAllText(CONFIG_FILE), JsonOptions);
			if (currentConfig == null)
			{
				Logger.Write("Init config", Logger.StatusCode.ERROR);
				return;
			}
			Logger.Write("Init config", Logger.StatusCode.INFO);
		}

		/// <summary>
		/// Save current config
		/// </summary>
		public static void SaveConfig()
		{
			try
			{
				File.WriteAllText(CONFIG_FILE, JsonSerializer.Serialize(currentConfig, JsonOptions));
				Logger.Write("Config file saved at " + CONFIG_FILE, Logger.StatusCode.INFO);
			}
			catch (UnauthorizedAccessException)
			{
				Logger.Write("No permission to create config file at " + CONFIG_FILE, Logger.StatusCode.ERROR);
				Environment.Exit(1);
			}
		}

		/// <summary>
		/// Print current data in config file
		/// </summary>
		public static void PrintConfig()
		{
			string configData = File.ReadAllText(CONFIG_FILE);
			Logger.Write("Current config settings:" + Environment.NewLine + configData, Logger.StatusCode.INFO);
		}

		/// <summary>
		/// Primary Config Interface
		/// </summary>
		public class ConfigObject
		{
			// default settings
			public string SerialPortName { get; set; } = "COM1";
			public int SerialPortBaudrate { get; set; } = 115200;
			public int SerialPortParity { get; set; } = 0;
			public int SerialPortStopBits { get; set; } = 1;
			public int SerialPortDataBits { get; set; } = 8;
			public int SerialPortReadTimeOut { get; set; } = 100;
			public int SerialPortWriteTimeOut { get; set; } = 100;
			public short MeasureUpdateInterval { get; set; } = 1000;
		}
	}
}
