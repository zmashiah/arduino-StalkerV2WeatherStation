/* =====================================================================================================
 * Code to show graph of battery voltage of the Weather Station. Data is taken from daily summary as
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
    public partial class BSummaryGraph : Form
    {
        private DataGridView dgvWeatherAnalysis;

        public BSummaryGraph(DataGridView p)
        {
            int row;
            const int colBmin = 11;
            const int colBmax = 13;
            const string sBmin = "Bmin";
            const string sBmax = "Bmax";
            const string sChartArea = "ChartArea1";

            dgvWeatherAnalysis = p;
            InitializeComponent();

            BattChart.Series[sBmin].XValueType = ChartValueType.Date;
            BattChart.Series[sBmax].XValueType = ChartValueType.Date;
            BattChart.ChartAreas[sChartArea].AxisY.Minimum = 3.4;
            BattChart.ChartAreas[sChartArea].AxisY.Maximum = 4.5;

            row = 1;
            foreach (DataGridViewRow dgvr in dgvWeatherAnalysis.Rows)
            {
                if (dgvr.Cells[0].Value != null)
                {
                    DateTime x = DateParser.DateStringToDateTime((string)dgvr.Cells[0].Value);
                    BattChart.Series[sBmin].Points.AddXY(x.ToOADate(), dgvr.Cells[colBmin].Value);
                    BattChart.Series[sBmax].Points.AddXY(x.ToOADate(), dgvr.Cells[colBmax].Value);
                    row++;
                }
            }



            InitializeComponent();
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
