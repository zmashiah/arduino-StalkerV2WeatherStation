/* =====================================================================================================
 * Code to show graph of temperatures, humidity, pressure and battery voltage of the Weather Station
 * during a selected day. The graph is called when double click done in the GridView on a row and shows
 * the data of the date presented in that row.
 * =====================================================================================================
 * By: Zakie Mashiah
 * Copyright: You can use the code below, portions or all of it for any use. Credit the author will be
 * appreciated
 * =====================================================================================================
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;

namespace WeatherDataAnalyzer
{
    public partial class InDayAllCharts : Form
    {
        public InDayAllCharts(DataTable dtWeather, string date)
        {
            int row;
            bool found = false;
            const int colTemp1 = 5;
            const int colTemp2 = 6;
            const int colTemp3 = 7;
            const int colHumidity = 8;
            const int colPressure = 9;
            const int colBattery= 2;

            InitializeComponent();
            Text += " " + date; // Add the date we are showing to form window caption

            // Set X Axis as Time axis and Y scale for each graph
            this.temperatures.Series[0].XValueType = ChartValueType.Time;
            this.humidity.Series[0].XValueType = ChartValueType.Time;
            this.humidity.ChartAreas[0].AxisY.Minimum = 0;
            this.humidity.ChartAreas[0].AxisY.Maximum = 100;
            this.pressure.Series[0].XValueType = ChartValueType.Time;
            this.pressure.ChartAreas[0].AxisY.Minimum = 99000;
            this.pressure.ChartAreas[0].AxisY.Maximum = 104000;
            this.battery.Series[0].XValueType = ChartValueType.Time;
            this.battery.ChartAreas[0].AxisY.Minimum = 3.4;
            this.battery.ChartAreas[0].AxisY.Maximum = 4.5;

            row = 1;
            foreach (DataRow dr in dtWeather.Rows)
            {
                if (date.Equals((string)dr[0]))
                {
                    float humidity = (float)dr[colHumidity];
                    if (humidity != 0)  // Sometimes the DHT22 fails and we are getting 0's. There is no 0% humidity realistically, so drop these points
                    {
                        DateTime x = DateParser.DateAndTimeStringToDateTime((string)dr[0], (string)dr[1]);
                        found = true;
                        double temp = ((float)dr[colTemp1] + (float)dr[colTemp2] + (float)dr[colTemp3]) / 3;
                        this.temperatures.Series[0].Points.AddXY(x.ToShortTimeString(), temp);
                        this.humidity.Series[0].Points.AddXY(x.ToShortTimeString(), dr[colHumidity]);
                        this.pressure.Series[0].Points.AddXY(x.ToShortTimeString(), dr[colPressure]);
                        this.battery.Series[0].Points.AddXY(x.ToShortTimeString(), dr[colBattery]);

                        /*
                        SummaryChart.Series[sTmin].Points.AddXY(x.ToOADate(), dgvr.Cells[colTmin].Value);
                            SummaryChart.Series[sTavg].Points.AddXY(x.ToOADate(), dgvr.Cells[colTavg].Value);
                            SummaryChart.Series[sTmax].Points.AddXY(x.ToOADate(), dgvr.Cells[colTmax].Value);
                         */
                    }
                }
                else
                    if (found == true) // If not equal anymore and we did find the date, break the foreach loop
                        break;
                row++;
            }
        }
    }
}
