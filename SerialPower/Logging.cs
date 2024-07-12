using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPower
{
	internal class Logging
	{
		public static void Info(string message)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"[{GetCurrentTime()}] [INFO]:\t{message}");
			Console.ForegroundColor = ConsoleColor.White;
		}
		public static void Warn(string message)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"[{GetCurrentTime()}] [WARN]:\t{message}");
			Console.ForegroundColor = ConsoleColor.White;
		}
		public static void Error(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"[{GetCurrentTime()}] [ERROR]:\t{message}");
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void PrintHeader(string title)
		{

			short x = (short)Console.WindowWidth;
			short titleLenght = (short)title.Length;

			ConsoleColor currentForegroundColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Magenta;

			for (int i = 0; i < (x / 2) - (titleLenght / 2) - 1; i++)
			{
				Console.Write('=');
			}

			Console.Write($" {title} ");

			for (int i = 0; i < (x / 2) - (titleLenght / 2) - 1; i++)
			{
				Console.Write('=');
			}
			Console.WriteLine();
			Console.ForegroundColor = currentForegroundColor;
		}
		private static string GetCurrentTime()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}
	}
}
