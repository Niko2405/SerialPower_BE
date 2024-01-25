using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialControl
{
	internal class SerialConnection
	{
		public static string SelectedPortName = string.Empty;
		public static int SelectedBaudrate = 9600;
		public static int SelectedStopBits = 1;
		public static int SelectedDataBits = 8;

		public static string[] GetPortNames()
		{
			return SerialPort.GetPortNames();
		}
	}
}
