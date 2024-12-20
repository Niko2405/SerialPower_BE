using System.IO.Ports;
using System.Windows;

namespace SerialPower
{
	internal class SerialSender
	{
		public static SerialPort? serialPort;
		public static bool DisablePortVerify = false;

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
						WriteTimeout = ConfigHandler.currentConfig.SerialPortWriteTimeOut,
						DtrEnable = true,
						RtsEnable = true,

					};
					serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
					if (!serialPort.IsOpen)
					{
						serialPort.Open();
						Logger.Write($"Connected to device [{serialPort.PortName}]", Logger.StatusCode.INFO);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString(), "Error at SerialSender", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
		{
			// TODO: Erweitern
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
						Logger.Write("Disconnect device", Logger.StatusCode.INFO);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString(), "Error at SerialSender", MessageBoxButton.OK, MessageBoxImage.Error);
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
						Logger.Write($"[{serialPort.PortName}] Sending data: " + data, Logger.StatusCode.INFO);
				}
				catch (TimeoutException ex)
				{
					Logger.Write(ex.Message, Logger.StatusCode.ERROR);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString(), "Error at SerialSender", MessageBoxButton.OK, MessageBoxImage.Error);
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
						Logger.Write($"[{serialPort.PortName}] Sending data: " + data, Logger.StatusCode.INFO);

					string response = serialPort.ReadLine().Trim();
					//string response = "return";

					if (showLogging)
						Logger.Write($"[{serialPort.PortName}] Received data: " + response, Logger.StatusCode.INFO);

					return response;
				}
				catch (TimeoutException ex)
				{
					Logger.Write(ex.Message, Logger.StatusCode.ERROR);
					return "TIMEOUT";
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
