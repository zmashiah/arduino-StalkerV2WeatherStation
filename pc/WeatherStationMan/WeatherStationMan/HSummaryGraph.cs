/* =====================================================================================================
 * Code to show graph of humidity of the Weather Station. Data is taken from daily summary as
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
    public partial class HSummaryGraph : Form
    {
        private DataGridView dgvWeatherAnalysis;

        public HSummaryGraph(DataGridView p)
        {
            int row;
            const int colHmin = 6;
            const int colHavg = 8;
            const int colHmax = 9;
            const string sHmin = "Hmin";
            const string sHavg = "Havg";
            const string sHmax = "Hmax";

            dgvWeatherAnalysis = p;
            InitializeComponent();

            HSummary.Series[sHmin].XValueType = ChartValueType.Date;
            HSummary.Series[sHavg].XValueType = ChartValueType.Date;
            HSummary.Series[sHmax].XValueType = ChartValueType.Date;

            row = 1;
            foreach (DataGridViewRow dgvr in dgvWeatherAnalysis.Rows)
            {
                if (dgvr.Cells[0].Value != null)
                {
                    DateTime x = DateParser.DateStringToDateTime((string)dgvr.Cells[0].Value);
                    HSummary.Series[sHmin].Points.AddXY(x.ToOADate(), dgvr.Cells[colHmin].Value);
                    HSummary.Series[sHavg].Points.AddXY(x.ToOADate(), dgvr.Cells[colHavg].Value);
                    HSummary.Series[sHmax].Points.AddXY(x.ToOADate(), dgvr.Cells[colHmax].Value);
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
