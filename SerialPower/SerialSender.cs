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
		public static int SelectedParity = 0;
		public static int SelectedReadTimeout = 100;
		public static int SelectedWriteTimeout = 100;

		private static SerialPort? serialPort;

		/// <summary>
		/// Send Command to COM
		/// </summary>
		/// <param name="command">Command to send</param>
		/// <param name="wait">Should the func wait for feedback</param>
		/// <returns></returns>
		public static string SendCommand(string command, bool readLine = false)
		{
			Debug.WriteLine("Send command: " +  command);
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
					Parity = (Parity)SelectedParity,
					ReadTimeout = SelectedReadTimeout,
					WriteTimeout = SelectedWriteTimeout
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
							if (readLine)
							{
								response = serialPort.ReadLine().Trim();
							}
							serialPort.Close();

							// unlock function
							isLocked = false;
						}
					}
					catch (TimeoutException)
					{
						// reset connection
						if (serialPort.IsOpen)
						{
							serialPort.Close();
							isLocked = false;
						}
						//MessageBox.Show("WARN", "Timeout", MessageBoxButton.OK, MessageBoxImage.Error);
					}
					catch (Exception ex)
					{
						// reset connection
						if (serialPort.IsOpen)
						{
							serialPort.Close();
							isLocked = false;
						}

						MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						return ex.Message;
					}
				}
				return response;
			}
			return "N/A";
		}
	}
}
