using System.IO.Ports;
using System.Windows;

namespace SerialPower
{
	internal class SerialSender
	{
		public static SerialPort? serialPort;


		/// <summary>
		/// Connect to device
		/// </summary>
		public static void ConnectDevice()
		{
			if (ConfigHandler.currentConfig != null)
			{
				try
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
						serialPort.Open();
						Logger.PrintStatus("Connect device", Logger.StatusCode.OK);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		/// <summary>
		/// Diconnect device
		/// </summary>
		public static void DisconnectDevice()
		{
			if (serialPort != null)
			{
				if (serialPort.IsOpen)
				{
					try
					{
						serialPort.Close();
						Logger.PrintStatus("Disconnect device", Logger.StatusCode.OK);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
		}

		/// <summary>
		/// Send data to device
		/// </summary>
		/// <param name="data">Command</param>
		/// <param name="showLogging">Should the message logged</param>
		public static void SendData(string data, bool showLogging = true)
		{
			if (serialPort != null)
			{
				if (!serialPort.IsOpen)
				{
					ConnectDevice();
				}
				try
				{
					serialPort.Write(data + Environment.NewLine);

					if (showLogging)
						Logger.PrintStatus($"[{serialPort.PortName}] Sending data:\t" + data, Logger.StatusCode.OK);
				}
				catch (TimeoutException ex)
				{
					Logger.PrintStatus(ex.Message, Logger.StatusCode.FAILED);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		/// <summary>
		/// Send data and wait for response
		/// </summary>
		/// <param name="data">Command</param>
		/// <param name="showLogging">Should the message logged</param>
		/// <returns>Response of given data</returns>
		public static string SendDataAndRecv(string data, bool showLogging = true)
		{
			if (serialPort != null)
			{
				if (!serialPort.IsOpen)
				{
					ConnectDevice();
				}
				try
				{
					serialPort.Write(data + Environment.NewLine);

					if (showLogging)
						Logger.PrintStatus($"[{serialPort.PortName}] Sending data:\t" + data, Logger.StatusCode.OK);

					string response = serialPort.ReadLine().Trim();

					if (showLogging)
						Logger.PrintStatus($"[{serialPort.PortName}] Received data:\t" + response, Logger.StatusCode.OK);

					return response;
				}
				catch (TimeoutException ex)
				{
					Logger.PrintStatus(ex.Message, Logger.StatusCode.FAILED);
					return "Timeout";
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			return "SerialPort is null";
		}
	}
}
