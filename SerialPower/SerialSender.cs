using System;
using System.Diagnostics;
using System.IO;
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
		public static int SelectedReadTimeout = 50;
		public static int SelectedWriteTimeout = 50;

		private static SerialPort? serialPort;

		/// <summary>
		/// Send Command to COM
		/// </summary>
		/// <param name="command">Command to send</param>
		/// <param name="waitForResponse">Should the func wait for feedback</param>
		/// <returns></returns>
		public static string SendCommand(string command, bool waitForResponse = false)
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
							Logger.PrintStatus("Sending command: " + command, Logger.StatusCode.INFO);

							// if wait, read next line and remove \r
							if (waitForResponse)
							{
								response = serialPort.ReadLine().Trim();
								Logger.PrintStatus("Received data: " + response, Logger.StatusCode.INFO);
							}
							serialPort.Close();

							// unlock function
							isLocked = false;
						}
					}
					catch (TimeoutException ex)
					{
						// reset connection
						if (serialPort.IsOpen)
						{
							serialPort.Close();
							isLocked = false;
						}
						
						Logger.PrintStatus(ex.Message, Logger.StatusCode.FAILED);
						return "Timeout";
					}
					catch (Exception ex)
					{
						// reset connection
						if (serialPort.IsOpen)
						{
							Logger.PrintStatus(ex.Message, Logger.StatusCode.FAILED);
							serialPort.Close();
							isLocked = false;
						}

						MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						return ex.Message;
					}
				}
				return response;
			}
			else if (isLocked)
			{
				// retry send command
				Thread.Sleep(1);
				return SendCommand(command, waitForResponse: false);
			}
			return string.Empty;
		}
	}
}
