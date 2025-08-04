using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TartarosLogger;

namespace SerialPower
{
	/// <summary>
	/// Just a test module for future ideas
	/// </summary>
	internal class TestModule
	{
		public required int ID { get; set; }
		public required string Name { get; set; }
		public required string Description { get; set; }


		public static void Start()
		{
			Thread.Sleep(1000);
			if (!TestIO())
			{
				Logger.Error("TEST.IO Failed");
				return;
			}
			Logger.Info("TEST.IO Success");

			Thread.Sleep(1000);
			if (!TestInet())
			{
				Logger.Error("TEST.INET Failed");
				return;
			}
			Logger.Info("TEST.INET Success");

			Thread.Sleep(1000);
			if (!TestConfigHandler())
			{
				Logger.Error("TEST.CONFIGHANDLER Failed");
				return;
			}
			Logger.Info("TEST.CONFIGHANDLER Success");

			Thread.Sleep(1000);
			if (!TestCrypting())
			{
				Logger.Error("TEST.CRYPTING Failed");
				return;
			}
			Logger.Info("TEST.CRYPTING Success");
			Thread.Sleep(1000);
		}

		private static bool TestIO()
		{
			Logger.PrintHeader("Test: IO");
			return true;
		}

		private static bool TestInet()
		{
			Logger.PrintHeader("Test: Inet");
			try
			{
				using (Ping ping = new Ping())
				{
					PingReply reply = ping.Send("google.de");
					return reply.Status == IPStatus.Success;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		private static bool TestConfigHandler()
		{
			Logger.PrintHeader("Test: ConfigHandler");
			var TestData = new TestModule()
			{
				ID = 1,
				Description = "Test",
				Name = "TestName",
			};
			string jsonString = JsonSerializer.Serialize(TestData, ConfigHandler.JsonOptions);

			// write new data
			File.WriteAllText(ConfigHandler.DIR_TEMP + "tmp.json", jsonString);
			Console.WriteLine(File.ReadAllText(ConfigHandler.DIR_TEMP + "tmp.json"));
			File.Delete(ConfigHandler.DIR_TEMP + "tmp.json");
			return true;
		}

		private static bool TestCrypting()
		{
			Logger.PrintHeader("Test: Crypting");

			var rand = new Random();
			int count = 100;
			long[] buffer = new long[count];

			Logger.Info($"Start filling the buffer: {count}");
			for (int i = 0; i < count; i++)
			{
				buffer[i] = rand.NextInt64(long.MaxValue / 2, long.MaxValue);
			}
			Logger.Info("Filling buffer finish");
			Logger.Info("Start encryp...");
			for (int i = 0; i < buffer.Length; i++)
			{
				string xCrypted = Crypt.Encrypt(buffer[i].ToString());
				string xEncrypted = Crypt.Decrypt(xCrypted);
				Logger.Info($"Run Test [{i + 1}/{count}]\tOriginal Value: {buffer[i]}\tCrypted Value: {xCrypted}\tEncrypted Value: {xEncrypted}");

				if (buffer[i].ToString() != xEncrypted)
				{
					Logger.Error($"Crypt Overflow: Len Original Value:{buffer[i].ToString().Length}\tLen Encrypted Value: {xEncrypted.Length}");
					return false;
				}
			}
			return true;
		}
	}
}
