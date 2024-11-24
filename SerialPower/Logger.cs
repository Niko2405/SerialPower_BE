using System.Diagnostics;
using System.Reflection;

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
			WARNING,
			ERROR,
		}

		public static void Write(string text, StatusCode code)
		{
#pragma warning disable CS8602 // Dereferenzierung eines möglichen Nullverweises.
			var methodInfo = new StackTrace().GetFrame(1).GetMethod();
			var classname = methodInfo.ReflectedType.Name;
#pragma warning restore CS8602 // Dereferenzierung eines möglichen Nullverweises.
			Console.Write($"{GetCurrentDate()} {Environment.UserName} {classname}: ");

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
				default:
					Console.ForegroundColor = ConsoleColor.White;
					break;
			}
			Console.WriteLine(text);
			Console.ForegroundColor = ConsoleColor.White;
		}

		private static string GetCurrentDate()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}
	}
}
