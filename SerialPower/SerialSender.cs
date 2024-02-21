using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows;

namespace SerialPower
{
	internal class SerialSender
	{
		public static bool isLocked = false;
		public static string SelectedPortName = string.Empty;
		public static int SelectedBaudrate = 9600;
		public static int SelectedStopBits = 1;
		public static int SelectedDataBits = 8;

		private static SerialPort? serialPort;

		public static string[] GetPortNames()
		{
			return SerialPort.GetPortNames();
		}

		/// <summary>
		/// Send Command to COM
		/// </summary>
		/// <param name="command">Command to send</param>
		/// <param name="wait">Should the func wait for feedback</param>
		/// <returns></returns>
		public static string SendCommand(string command, bool wait = false)
		{
			// create empty string
			string response = string.Empty;

			// if func is not locked
			if (!isLocked)
			{
				serialPort = new SerialPort
				{
					PortName = SelectedPortName,
					BaudRate = SelectedBaudrate,
					StopBits = (StopBits)SelectedStopBits,
					DataBits = SelectedDataBits,
					Parity = 0,
					ReadTimeout = 500,
				};

				if (!serialPort.IsOpen)
				{
					// Lock function
					isLocked = true;
					try
					{
						// Open Port
						serialPort.Open();
						if (serialPort.IsOpen)
						{
							serialPort.WriteLine(command);
							// if wait, read next line and remove \r
							if (wait)
							{
								response = serialPort.ReadLine().Trim();
							}
							serialPort.Close();

							// unlock function
							isLocked = false;
						}
					}
					catch (TimeoutException ex)
					{
						if (serialPort.IsOpen)
						{
							serialPort.Close();
							isLocked = false;
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show("ERROR", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
						return ex.Message;
					}
				}
				return response;
			}
			return "N/A";
		}
	}
}
