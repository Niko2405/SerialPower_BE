using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Windows;

namespace SerialPower
{
	internal class SerialSender
	{
		private static bool isLocked = false;
		private static SerialPort? serialPort;

		/// <summary>
		/// Send Command to COM
		/// </summary>
		/// <param name="command">Command to send</param>
		/// <param name="waitForResponse">Should the func wait for feedback</param>
		/// <param name="enableLogging">Should the data logged</param>
		/// <returns></returns>
		public static string SendCommand(string command, bool waitForResponse = false, bool enableLogging = true)
		{
			// create empty string
			string response = string.Empty;

			if (ConfigHandler.currentConfig != null)
			{
				// if func is not locked
				if (!isLocked)
				{
					serialPort = new SerialPort
					{
						PortName = ConfigHandler.currentConfig.SerialPortName,
						BaudRate = ConfigHandler.currentConfig.SerialPortBaudrate,
						StopBits = (StopBits)ConfigHandler.currentConfig.SerialPortStopBits,
						DataBits = ConfigHandler.currentConfig.SerialPortDataBits,
						Parity = (Parity)ConfigHandler.currentConfig.SerialPortParity,
						ReadTimeout = ConfigHandler.currentConfig.SerialPortReadTimeOut,
						WriteTimeout = ConfigHandler.currentConfig.SerialPortWriteTimeOut
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
								if (enableLogging)
									Logger.PrintStatus("Sending data:\t" + command, Logger.StatusCode.OK);

								// if wait, read next line and remove \r
								if (waitForResponse)
								{
									response = serialPort.ReadLine().Trim();
									if (enableLogging)
										Logger.PrintStatus("Received data:\t" + response, Logger.StatusCode.OK);
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
					// retry send data
					Thread.Sleep(1);
					return SendCommand(command, waitForResponse: false);
				}
				return string.Empty;
			}
			else
			{
				return "Config Handler is dead.";
			}
		}
	}
}
