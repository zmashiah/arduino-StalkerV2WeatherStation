using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WeatherStationMan
{
	static class Program
	{
        /// <summary>
		/// The main entry point for the application. Take command line argument of serial port name or no argument
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            string serialPortName;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 1)
            {
                serialPortName = args[0];
            }
            else
                serialPortName = null;

			Application.Run(new mainWindow(serialPortName));
		}
	}
}
