using System.IO.Ports;
using System.Windows;
using TartarosLogger;

namespace SerialPower
{
	internal class SerialSender
	{
		public static SerialPort? serialPort;

		public string SerialPortName { get; set; } = "COM1";
		public int Baudrate { get; set; } = 9600;
		public int DataBits { get; set; } = 8;
		public int StopBits { get; set; } = 1;
		public int WriteTimeout { get; set; } = 250;
		public int ReadTimeout { get; set; } = 250;
		public int Parity { get; set; } = 0;
		public short MeasureUpdateInterval { get; set; } = 1000;
		public bool ShortCircuitProtection { get; set; } = true;


		/// <summary>
		/// Disable port verify. Usefull for system there connected with rs232
		/// </summary>
		public static bool DisablePortVerify = false;

		/// <summary>
		/// Disable communication for com devices. (Dummy test)
		/// </summary>
		public static bool DisableCommunication = false;

		/// <summary>
		/// Current fault counter
		/// </summary>
		private static int faultCounter = 0;

		/// <summary>
		/// Fault limit
		/// </summary>
		private static readonly int faultLimit = 40;

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
			Logger.Info($"[{channel}] Set voltage to {voltage}V and current to {current}A");
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
				Logger.Warn($"Timeout. Increase fault counter: current[{faultCounter}] limit[{faultLimit}]");
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
				Logger.Warn($"Timeout. Increase fault counter: current[{faultCounter}] limit[{faultLimit}]");
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
			Logger.Info($"[{channel}] change state to {state}");
			SendData($"OP{(int)channel} {(int)state}");
		}

		/// <summary>
		/// Switch state of all channels
		/// </summary>
		/// <param name="state"></param>
		public static void SetChannelState(State state)
		{
			Logger.Info($"[CH1 and CH2] change state to {state}");
			SendData($"OPALL {(int)state}");
		}

		/// <summary>
		/// Connect to target COM Port
		/// </summary>
		public static void ConnectDevice()
		{
			if (DisableCommunication)
			{
				Logger.Warn("Communication is deactivated");
				return;
			}

			if (ConfigHandler.serialConfig != null)
			{
				try
				{
					serialPort = new SerialPort
					{
						PortName = ConfigHandler.serialConfig.SerialPortName,
						BaudRate = ConfigHandler.serialConfig.Baudrate,
						StopBits = (StopBits)ConfigHandler.serialConfig.StopBits,
						DataBits = ConfigHandler.serialConfig.DataBits,
						Parity = (Parity)ConfigHandler.serialConfig.Parity,
						ReadTimeout = ConfigHandler.serialConfig.ReadTimeout,
						WriteTimeout = ConfigHandler.serialConfig.WriteTimeout,
					};
					//serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
					if (!serialPort.IsOpen)
					{
						serialPort.Open();
						Logger.Info($"Connected to device [{serialPort.PortName}]");
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex.Message);
					MessageBox.Show(ex.ToString(), "Error at SerialSender", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		/*
		private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
		{
			// TODO: expand
		}
		*/

		/// <summary>
		/// Disconnect device
		/// </summary>
		public static void DisconnectDevice()
		{
			if (serialPort == null || !serialPort.IsOpen)
			{
				return;
			}
			try
			{
				Logger.Info("Disconnect device...");
				SetChannelState(State.OFF);
				serialPort.WriteLine("LOCAL");
				Thread.Sleep(500);
				serialPort.Close();
			}
			catch (TimeoutException)
			{
				Logger.Error(STATUSCODE.TIMEOUT.ToString());
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
				MessageBox.Show(ex.Message, "ERROR at SerialSender", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		/// <summary>
		/// Send data to device
		/// </summary>
		/// <param name="data">Command</param>
		private static void SendData(string data)
		{
			if (DisableCommunication || serialPort == null)
			{
				return;
			}
			
			// open connection
			if (!serialPort.IsOpen)
			{
				ConnectDevice();
			}
			try
			{
				serialPort.WriteLine(data);
				Logger.Debug($"[{serialPort.PortName}] Sending data: {data}");
			}
			catch (TimeoutException)
			{
				Logger.Error(STATUSCODE.TIMEOUT.ToString());
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
				MessageBox.Show(ex.Message, "ERROR at SerialSender", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		public static string SendDataAndRecv(string data)
		{
			if (DisableCommunication)
			{
				return "Disabled";
			}
			if (serialPort == null)
			{
				return "SerialPort is null";
			}
			
			// open connection
			if (!serialPort.IsOpen)
			{
				ConnectDevice();
			}
			
			try
			{
				serialPort.WriteLine(data);
				Logger.Debug($"[{serialPort.PortName}] Sending data: {data}");

				string response = serialPort.ReadLine().Trim();
				Logger.Debug($"[{serialPort.PortName}] Received data: {response}");

				return response;
			}
			catch (TimeoutException)
			{
				Logger.Error(STATUSCODE.TIMEOUT.ToString());
				return STATUSCODE.TIMEOUT.ToString();
			}
			catch (Exception ex)
			{
				if (MessageBox.Show("ERROR: " + ex.Message + "\nDo you want to continue?", "Critical error", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.No)
				{
					Environment.Exit(2);
				}
			}
			return "FATAL ERROR";
		}
	}
}
