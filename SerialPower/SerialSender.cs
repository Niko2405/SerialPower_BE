﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SerialPower
{
	internal class SerialSender
	{
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
			bool locked = false;
			string response = string.Empty;

			serialPort = new SerialPort
			{
				PortName = SelectedPortName,
				BaudRate = SelectedBaudrate,
				StopBits = (StopBits)SelectedStopBits,
				DataBits = SelectedDataBits,
				Parity = 0,
			};
			while (locked)
			{
				Thread.Sleep(10);
				Debug.WriteLine("Waiting...");
			}
			if (!serialPort.IsOpen)
			{
				try
				{
					locked = true;
					serialPort.Open();
					serialPort.WriteLine(command);
					if (wait)
					{
						response = serialPort.ReadLine();
					}
					serialPort.Close();
					locked = false;
				}
				catch (Exception ex)
				{
					MessageBox.Show("ERROR", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
					return ex.Message;
				}
			}
			return response;
		}
	}
}