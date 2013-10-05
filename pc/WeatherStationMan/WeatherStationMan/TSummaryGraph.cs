/* =====================================================================================================
 * Code to show graph of temperatures of the Weather Station. Data is taken from daily summary as
 * presented in the GridView
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
    public partial class TSummaryGraph : Form
    {
        private DataGridView dgvWeatherAnalysis;

        public TSummaryGraph(DataGridView p)
        {
            int row;
            const int colTmin = 1;
            const int colTavg = 3;
            const int colTmax = 4;
            const string sTmin = "Tmin";
            const string sTavg = "Tavg";
            const string sTmax = "Tmax";

            dgvWeatherAnalysis = p;
            InitializeComponent();

            SummaryChart.Series[sTmin].XValueType = ChartValueType.Date;
            SummaryChart.Series[sTavg].XValueType = ChartValueType.Date;
            SummaryChart.Series[sTmax].XValueType = ChartValueType.Date;

            row = 1;
            foreach (DataGridViewRow dgvr in dgvWeatherAnalysis.Rows)
            {
                if (dgvr.Cells[0].Value != null)
                {
                    DateTime x = DateParser.DateStringToDateTime((string)dgvr.Cells[0].Value);
                    SummaryChart.Series[sTmin].Points.AddXY(x.ToOADate(), dgvr.Cells[colTmin].Value);
                    SummaryChart.Series[sTavg].Points.AddXY(x.ToOADate(), dgvr.Cells[colTavg].Value);
                    SummaryChart.Series[sTmax].Points.AddXY(x.ToOADate(), dgvr.Cells[colTmax].Value);
                    row++;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                dgvWeatherAnalysis = null;
            }
            base.Dispose(disposing);
        }
    }
}
