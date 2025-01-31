using System.Diagnostics;

namespace SerialPower
{
	internal class Logger
	{
		public static bool isDebugEnabled = false;
		/// <summary>
		/// Print status code of this running process
		/// </summary>
		public enum StatusCode
		{
			INFO,
			WARNING,
			ERROR,
			DEBUG
		}

		public static void Write(string text, StatusCode code)
		{
			if (code == StatusCode.DEBUG && !isDebugEnabled)
			{
				return;
			}

#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
			var methodInfo = new StackTrace().GetFrame(1).GetMethod();
			var classname = methodInfo.ReflectedType.Name;
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.
			Console.Write($"[{GetCurrentDate()}] {classname}: ");

			switch (code)
			{
				case StatusCode.INFO:
					Console.ForegroundColor = ConsoleColor.Green;
					break;
				case StatusCode.WARNING:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case StatusCode.ERROR:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case StatusCode.DEBUG:
					Console.ForegroundColor = ConsoleColor.Cyan;
					break;
				default:
					Console.ForegroundColor = ConsoleColor.White;
					break;
			}
			Console.WriteLine(text);
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void PrintHeader(string title)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine($"====================[ {title} ]====================");
		}

		private static string GetCurrentDate()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}
	}
}
