﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SerialPower.UserControls
{
	/// <summary>
	/// Interaktionslogik für UC_Sequencer.xaml
	/// </summary>
	public partial class UC_Sequencer : UserControl
	{
		public UC_Sequencer()
		{
			InitializeComponent();
		}

		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (Convert.ToBoolean(e.NewValue.ToString()))
			{
				SerialSender.SetChannelState(SerialSender.State.OFF);
			}
		}
	}
}
