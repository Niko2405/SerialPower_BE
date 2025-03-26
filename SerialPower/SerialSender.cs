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
		/// Get values from the power supply
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="targetType"></param>
		/// <returns></returns>
		public static string GetPowerSupplyValue(Channel channel, TargetType targetType)
		{
			return SendDataAndRecv($"{targetType}{(int)channel}?");
		}

		/// <summary>
		/// Get all datas from power supply like voltage and current
		/// </summary>
		/// <returns>Tuple with V1, I1, V2, I2</returns>
		public static Tuple<string, string, string, string> GetPowerSupplyValues()
		{
			if (faultCounter >= faultLimit)
			{
				Logger.Write($"GetPowerSupplyValues: Fail counter limit reached", Logger.StatusCode.ERROR);
				return Tuple.Create("FAULT", "LIMIT", "FAULT", "LIMIT");
			}

			short size = 4; //V1 I1 V2 I2
			string voltageChannel1;
			string voltageChannel2;
			string currentChannel1;
			string currentChannel2;

			string[] values = new string[size];

			try
			{
				if (serialPort != null)
				{
					values[0] = SendDataAndRecv("V1O?");
					values[1] = SendDataAndRecv("V2O?");
					values[2] = SendDataAndRecv("I1O?");
					values[3] = SendDataAndRecv("I2O?");

					if (values.Contains("TIMEOUT"))
					{
						faultCounter++;
						Logger.Write($"Timeout. Current fail counter: [{faultCounter}] limit: [{faultLimit}]", Logger.StatusCode.WARNING);
						return Tuple.Create("TIMEOUT", "TIMEOUT", "TIMEOUT", "TIMEOUT");
					}

					Array.Sort(values);
					/*
					 * 0. I1
					 * 1. I2
					 * 2. V1
					 * 3. V2
					 */
					currentChannel1 = values[0].Replace("I1", string.Empty).Replace(" ", string.Empty) + "A";
					currentChannel2 = values[1].Replace("I2", string.Empty).Replace(" ", string.Empty) + "A";
					voltageChannel1 = values[2].Replace("V1", string.Empty).Replace(" ", string.Empty) + "V";
					voltageChannel2 = values[3].Replace("V2", string.Empty).Replace(" ", string.Empty) + "V";

					return Tuple.Create(voltageChannel1, currentChannel1, voltageChannel2, currentChannel2);
				}
				Logger.Write($"SerialPort is null", Logger.StatusCode.ERROR);
				return Tuple.Create("ERROR", "ERROR", "ERROR", "ERROR");
			}
			catch (Exception ex)
			{
				Logger.Write(ex.Message, Logger.StatusCode.ERROR);
				return Tuple.Create("ERROR", "ERROR", "ERROR", "ERROR");
			}
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
						Logger.Write("Timeout", Logger.StatusCode.ERROR);
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
					Logger.Write("Timeout", Logger.StatusCode.ERROR);
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
					Logger.Write("Timeout", Logger.StatusCode.ERROR);
					return "TIMEOUT";
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
