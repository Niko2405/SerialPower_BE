using System.IO.Ports;
using System.Windows;

namespace SerialPower
{
	internal class SerialSender
	{
		public static SerialPort? serialPort;

		/// <summary>
		/// Disable port verify. Usefull for system there connected with rs232
		/// </summary>
		public static bool DisablePortVerify = false;

		/// <summary>
		/// Disable communication for com devices. (Dummy)
		/// </summary>
		public static bool DisableCommunication = false;

		/// <summary>
		/// Current fault counter
		/// </summary>
		private static int faultCounter = 0;

		/// <summary>
		/// Fault limit
		/// </summary>
		private readonly static int faultLimit = 10 * 4;

		/// <summary>
		/// Channels of the power supply
		/// </summary>
		public enum Channel
		{
			CH1 = 1,
			CH2 = 2
		}

		/// <summary>
		/// State of channels. On or off
		/// </summary>
		public enum State
		{
			OFF = 0,
			ON = 1,
		}

		/// <summary>
		/// Voltage or current
		/// </summary>
		public enum TargetType
		{
			V,
			I
		}

		public enum STATUSCODE
		{
			OK = 0,
			TIMEOUT = 1,
		}

		/// <summary>
		/// Send new values to the power supply
		/// </summary>
		/// <param name="voltage"></param>
		/// <param name="current"></param>
		/// <param name="channel"></param>
		public static void SetPowerSupplyValues(float voltage, float current, Channel channel)
		{
			string command = $"V{(int)channel} {voltage}; I{(int)channel} {current}".Replace(",", ".");
			SendData(command);
		}

		/// <summary>
		/// Retrieve nominal (current) target value from the power supply
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="targetType"></param>
		/// <returns></returns>
		public static string GetPowerSupplyNominalValue(Channel channel, TargetType targetType)
		{
			// Example output: V1 24.00
			if (faultCounter >= faultLimit)
			{
				return STATUSCODE.TIMEOUT.ToString();
			}

			string value = SendDataAndRecv($"{targetType}{(int)channel}?");

			if (value.Equals(STATUSCODE.TIMEOUT.ToString()))
			{
				faultCounter++;
				Logger.Write($"Timeout. Increase fault counter: current[{faultCounter}] limit[{faultLimit}]", Logger.StatusCode.WARNING);
			}
			if (value.StartsWith("V1") || value.StartsWith("V2") || value.StartsWith("I1") || value.StartsWith("I2"))
			{
				value = value.Remove(0, 3); // remove V1, I2 or something
			}
			return value;
		}

		/// <summary>
		/// Retrieve actual value from the power supply
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="targetType"></param>
		/// <returns></returns>
		public static string GetPowerSupplyActualValue(Channel channel, TargetType targetType)
		{
			// Example output: 24.00V
			if (faultCounter >= faultLimit)
			{
				return STATUSCODE.TIMEOUT.ToString();
			}

			string value = SendDataAndRecv($"{targetType}{(int)channel}O?");

			if (value.Equals(STATUSCODE.TIMEOUT.ToString()))
			{
				faultCounter++;
				Logger.Write($"Timeout. Increase fault counter: current[{faultCounter}] limit[{faultLimit}]", Logger.StatusCode.WARNING);
			}
			return value;
		}

		/// <summary>
		/// Switch on or off the channels
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="state"></param>
		public static void SetChannelState(Channel channel, State state)
		{
			Logger.Write($"Change channel {channel} state to {state}", Logger.StatusCode.INFO);
			SendData($"OP{(int)channel} {(int)state}");
		}

		/// <summary>
		/// Switch state of all channels
		/// </summary>
		/// <param name="state"></param>
		public static void SetChannelState(State state)
		{
			Logger.Write($"Change both channels state to {state}", Logger.StatusCode.INFO);
			SendData($"OPALL {(int)state}");
		}

		/// <summary>
		/// Connect to target COM Port
		/// </summary>
		public static void ConnectDevice()
		{
			if (DisableCommunication)
			{
				Logger.Write("Communication is deactivated", Logger.StatusCode.WARNING);
				return;
			}

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
					Logger.Write(ex.ToString(), Logger.StatusCode.ERROR);
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
						serialPort.WriteLine("LOCAL");
						serialPort.Close();
						Logger.Write("Disconnect device", Logger.StatusCode.INFO);
					}
					catch (TimeoutException)
					{
						Logger.Write(STATUSCODE.TIMEOUT.ToString(), Logger.StatusCode.ERROR);
					}
					catch (Exception ex)
					{
						Logger.Write(ex.ToString(), Logger.StatusCode.ERROR);
						MessageBox.Show(ex.ToString(), "ERROR at SerialSender", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}
		}

		/// <summary>
		/// Send data to device
		/// </summary>
		/// <param name="data">Command</param>
		/// <param name="showLogging">Should the message logged</param>
		private static void SendData(string data)
		{
			if (DisableCommunication)
			{
				Logger.Write("Communication is deactivated", Logger.StatusCode.WARNING);
				return;
			}
			if (serialPort != null)
			{
				if (!serialPort.IsOpen)
				{
					ConnectDevice();
				}
				try
				{
					serialPort.WriteLine(data);

					if (Logger.isDebugEnabled)
						Logger.Write($"[{serialPort.PortName}] Sending data: " + data, Logger.StatusCode.DEBUG);
				}
				catch (TimeoutException)
				{
					Logger.Write(STATUSCODE.TIMEOUT.ToString(), Logger.StatusCode.ERROR);
				}
				catch (Exception ex)
				{
					Logger.Write(ex.ToString(), Logger.StatusCode.ERROR);
					MessageBox.Show(ex.ToString(), "ERROR at SerialSender", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		public static string SendDataAndRecv(string data)
		{
			if (DisableCommunication)
			{
				Logger.Write("Communication is deactivated", Logger.StatusCode.WARNING);
				return "DEACTIVATED";
			}
			if (serialPort != null)
			{
				if (!serialPort.IsOpen)
				{
					ConnectDevice();
				}
				try
				{
					serialPort.WriteLine(data);

					if (Logger.isDebugEnabled)
						Logger.Write($"[{serialPort.PortName}] Sending data: " + data, Logger.StatusCode.DEBUG);

					string response = serialPort.ReadLine().Trim();
					//string response = "return";

					if (Logger.isDebugEnabled)
						Logger.Write($"[{serialPort.PortName}] Received data: " + response, Logger.StatusCode.DEBUG);

					return response;
				}
				catch (TimeoutException)
				{
					Logger.Write(STATUSCODE.TIMEOUT.ToString(), Logger.StatusCode.ERROR);
					return STATUSCODE.TIMEOUT.ToString();
				}
				catch (Exception ex)
				{
					if (MessageBox.Show("ERROR: " + ex.Message + "\nDo you want to continue?", "Critical error", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.No)
					{
						Environment.Exit(2);
					}
				}
			}
			return "SerialPort is null";
		}
	}
}
