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
			Console.WriteLine("TEST MODULE STARTING...");
			Console.Beep(650, 2000);

			Thread.Sleep(1000);
			TestIO();

			Thread.Sleep(1000);
			TestInet();

			Thread.Sleep(1000);
			TestConfigHandler();

			Thread.Sleep(1000);
			TestCrypting();

			Thread.Sleep(1000);
			Logger.PrintHeader("Test Module Ended");
		}

		private static void TestIO()
		{
			Logger.PrintHeader("Test: IO");
			Logger.Info("Test IO: OK");
		}

		private static void TestInet()
		{
			Logger.PrintHeader("Test: Inet");
			try
			{
				using (Ping ping = new Ping())
				{
					PingReply reply = ping.Send("google.de");
					if (reply.Status == IPStatus.Success)
					{
						Logger.Info($"Test Inet Connection: {reply.Status}");
					}
				}
			}
			catch (Exception)
			{
				Logger.Error("Test Inet Connection: failed");
			}
		}

		private static void TestConfigHandler()
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
			Logger.Info("Test ConfigHandler: OK");
		}

		private static void TestCrypting()
		{
			try
			{
				Logger.PrintHeader("Test: Crypting");

				var rand = new Random();
				int count = 500;
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
					Logger.Info($"Run Test [{i + 1}/{count}]\torig. val: {buffer[i]}\tCrypted val: {xCrypted}\tEncrypted val: {xEncrypted}");

					if (buffer[i].ToString() != xEncrypted)
					{
						Logger.Error($"Crypt Overflow: Len Original val:{buffer[i].ToString().Length}\tLen Encrypted val: {xEncrypted.Length}");
						return;
					}
				}
				Logger.Info("Test Cryprting SHA512: OK");
			}
			catch (Exception ex)
			{
				Logger.Error($"ERROR {ex.Message}");
			}
		}
	}
}
