using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPower
{
	internal class Logger
	{
		/// <summary>
		/// Print status code of this running process
		/// </summary>
		public enum StatusCode
		{
			INFO,
			OK,
			FAILED,
		}

		/// <summary>
		/// Print Status log of given process name
		/// </summary>
		/// <param name="message"></param>
		/// <param name="code"></param>
		public static void PrintStatus(string message, StatusCode code)
		{
			// [FAILED]
			// [ INFO ]
			// [  OK  ]
			switch (code)
			{
				case StatusCode.INFO:
					Console.Write("[");
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write(" INFO ");
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write("] ");
					Console.WriteLine(message);
					break;
				case StatusCode.OK:
					Console.Write("[");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write("  OK  ");
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write("] ");
					Console.WriteLine(message);
					break;
				case StatusCode.FAILED:
					Console.Write("[");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("FAILED");
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write("] ");
					Console.WriteLine(message);
					break;
				default:
					break;
			}
		}

		public static void PrintHeader(string title)
		{
			short x = (short)Console.WindowWidth;
			short titleLenght = (short)title.Length;
			short offset = 0;

			// if the title is odd, set offset to 1. To prevent '=' in newline
			if (titleLenght % 2 != 0)
			{
				offset = 1;
			}

			ConsoleColor currentForegroundColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkYellow;

			for (int i = 0; i < (x / 2) - (titleLenght / 2) - 1 - offset; i++)
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

		[Obsolete]
		public static void PrintHeaderOld(string title)
		{
			ConsoleColor currentForegroundColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkYellow;

			Console.Write($"==================[ {title} ]==================");
			Console.WriteLine();

			Console.ForegroundColor = currentForegroundColor;
		}
	}
}
