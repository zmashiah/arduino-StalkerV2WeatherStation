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
    public partial class PSummary : Form
    {
        private DataGridView dgvWeatherAnalysis;

        public PSummary(DataGridView p)
        {
            const string sPmin = "Pmin";
            const string sPavg = "Pavg";
            const string sPmax = "Pmax";
            const int colPmin = 15;
            const int colPavg = 17;
            const int colPmax = 18;
            int row;

            dgvWeatherAnalysis = p;
            InitializeComponent();

            this.pressureChart.Series[sPmin].XValueType = ChartValueType.Date;
            this.pressureChart.Series[sPavg].XValueType = ChartValueType.Date;
            this.pressureChart.Series[sPmax].XValueType = ChartValueType.Date;
            this.pressureChart.ChartAreas[0].AxisY.Minimum = 99000;
            this.pressureChart.ChartAreas[0].AxisY.Maximum = 104000;

            row = 1;
            foreach (DataGridViewRow dgvr in dgvWeatherAnalysis.Rows)
            {
                if (dgvr.Cells[0].Value != null)
                {
                    DateTime x = DateParser.DateStringToDateTime((string)dgvr.Cells[0].Value);
                    this.pressureChart.Series[sPmin].Points.AddXY(x.ToOADate(), dgvr.Cells[colPmin].Value);
                    this.pressureChart.Series[sPavg].Points.AddXY(x.ToOADate(), dgvr.Cells[colPavg].Value);
                    this.pressureChart.Series[sPmax].Points.AddXY(x.ToOADate(), dgvr.Cells[colPmax].Value);
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
