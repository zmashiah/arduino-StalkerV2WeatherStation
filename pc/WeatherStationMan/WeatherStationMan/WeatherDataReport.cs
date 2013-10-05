/* =====================================================================================================
 * Code to load Weather Station date from CSV file, display in GridView and launch the different
 * graphs. Code also uses the Excel JetCell library to export the data into an Excel file
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

using DTG.Spreadsheet;


namespace WeatherDataAnalyzer
{

    public partial class WeatherDataReport : Form
    {
        DataTable dtWeatherData;
        enum ExcelFileType { Office2010, Office2003 };

        DateTimeRange dtRange = new DateTimeRange();

        float progress;

        public WeatherDataReport()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                if (dtWeatherData != null)
                {
                    dtWeatherData.Dispose();
                }
                dtWeatherData = null;
            }
            base.Dispose(disposing);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string fileName = WeatherStationMan.WeatherStationManConstants.csvFileName;
            try
            {
                LoadDataTable(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadFile Failed: " + ex.Message,
                    "Error Loading " + fileName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
                return;
            }
        }



        private void LoadDataTable(string fileName)
        {
            Cursor c;

            c = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            dtWeatherData = new DataTable();

            try
            {
                WeatherStationReader r = new WeatherStationReader(fileName);
                dtWeatherData.Load(r);
            }
            catch (System.Data.ConstraintException)
            {
                foreach (DataRow e in dtWeatherData.GetErrors())
                {
                    //Trace.WriteLine(e.RowError);
                    foreach (DataColumn ce in e.GetColumnsInError())
                    {
                        //Trace.WriteLine(ce.ColumnName + ": " + e.GetColumnError(ce));
                    }
                }
                throw;
            }
            AnalyzeData(dtWeatherData);
            this.Cursor = c;
        }


        private void SetProgressBarParameters(int min, int max, int value)
        {
            this.SaveProgressBar.Minimum = min;
            this.SaveProgressBar.Maximum = max;
            this.SaveProgressBar.Value = value;
        }



        private void SaveToExcel(string fileName, ExcelFileType officeType, DataTable weatherData)
        {
            ExcelWorkbook eBook = new ExcelWorkbook();
            int worksheet = 0;
            int row_count, row;
            int col_count, col;

            // Make the progress bar on the form visible
            this.SaveProgressBar.Visible = true;

            // Dump the raw data to a worksheet 0
            eBook.Worksheets.Add("Weather Station Raw");
            // Start with column names
            col = 0;
            foreach (DataColumn c in weatherData.Columns)
            {
                eBook.Worksheets[worksheet].Cells[0, col].Value = c.ColumnName;
                col++;
            }

            // Count the rows
            row_count = 1;
            foreach (DataRow dr in weatherData.Rows)
                row_count++;

            // Show the progress bar
            SetProgressBarParameters(0, row_count, 1);

            // save the raw data to first worksheet in the excel file
            row = 1;
            foreach (DataRow dr in weatherData.Rows)
            {
                for (col = 0; col < weatherData.Columns.Count; col++)
                {
                    eBook.Worksheets[worksheet].Cells[row, col].Value = dr[col];
                }
                row++;
                this.SaveProgressBar.Value = row;
            }

            // Add the daily statistics to second worksheet in the excel file
            // Start by counting how many rows we have (for the progress bar)
            row_count = 1;
            foreach (DataGridViewRow dgvr in dgvWeatherAnalysis.Rows)
                row_count++;
            SetProgressBarParameters(1, row_count, 1);

            eBook.Worksheets.Add("Daily min, max and avg");
            worksheet++;
            // Put header columns in row 0 of th eworksheet
            col_count = 0;
            foreach (DataGridViewColumn c in dgvWeatherAnalysis.Columns)
            {
                eBook.Worksheets[worksheet].Cells[0, col].Value = c.HeaderText;
                col_count++;
            }
            // Now actually add the rows
            row = 1;
            foreach (DataGridViewRow dgvr in dgvWeatherAnalysis.Rows)
            {
                for (col = 0; col < col_count; col++)
                {
                    eBook.Worksheets[worksheet].Cells[row, col].Value = dgvr.Cells[col].Value;
                }
                row++;
                this.SaveProgressBar.Value = row;
            }

            // Set the progress bar parameters and call-back functions from eBook
            // The JetCell library is calling us with percent, so use 0..100 for progress bar
            SetProgressBarParameters(0, 100, 0);
            progress = 0;
            eBook.SavingProgress += new ProgressEventHandler(workbook_SavingProgress);

            // Save to File according to file type the user asked
            if (officeType == ExcelFileType.Office2010)
                eBook.WriteXLSX(fileName);
            else
                eBook.WriteXLS(fileName);

            this.SaveProgressBar.Visible = false; // Finally hide the progress bar
        }



        private void AnalyzeData(DataTable weatherData)
        {
            DateTime dt = new DateTime();

            AnalyzeData(weatherData, false, dt, dt);
        }



        private void AddTodayRow(string date, DataMeasure temperature, DataMeasure humidity, DataMeasure voltage, DataMeasure pressure)
        {
            // Build a data-grid row as summary of today values
            DataGridViewRow todayRow = new DataGridViewRow();
            int index = 0;

            todayRow.CreateCells(dgvWeatherAnalysis);
            todayRow.Cells[index++].Value = date;
            todayRow.Cells[index++].Value = temperature.getMin();
            todayRow.Cells[index++].Value = temperature.getMinTime();
            todayRow.Cells[index++].Value = temperature.getAvg();
            todayRow.Cells[index++].Value = temperature.getMax();
            todayRow.Cells[index++].Value = temperature.getMaxTime();

            todayRow.Cells[index++].Value = humidity.getMin();
            todayRow.Cells[index++].Value = humidity.getMinTime();
            todayRow.Cells[index++].Value = humidity.getAvg();
            todayRow.Cells[index++].Value = humidity.getMax();
            todayRow.Cells[index++].Value = humidity.getMaxTime();

            todayRow.Cells[index++].Value = voltage.getMin();
            todayRow.Cells[index++].Value = voltage.getMinTime();
            todayRow.Cells[index++].Value = voltage.getMax();
            todayRow.Cells[index++].Value = voltage.getMaxTime();

            todayRow.Cells[index++].Value = pressure.getMin();
            todayRow.Cells[index++].Value = pressure.getMinTime();
            todayRow.Cells[index++].Value = pressure.getAvg();
            todayRow.Cells[index++].Value = pressure.getMax();
            todayRow.Cells[index++].Value = pressure.getMaxTime();

            // add this row to the grid
            this.dgvWeatherAnalysis.Rows.Add(todayRow);
        }



        private void AnalyzeData(DataTable weatherData, bool filterSet, DateTime dtStart, DateTime dtEnd)
        {
            // Do some data analysis here
            DataRow oldDr = null;
            DataMeasure temperature = new DataMeasure(-273);    // Not likely to get this temperature ever
            DataMeasure humidity = new DataMeasure(0);          // Sometimes the DHT22 gives 0 reading. Simply ignore those
            DataMeasure voltage = new DataMeasure(6);           // We won't get 6V reading for sure
            DataMeasure pressure = new DataMeasure(0);
            bool row_avail = false;
            float humidityValue;

            foreach (DataRow dr in weatherData.Rows)
            {
                //DateTime dt = new DateTime((int)dr[4], (int)dr[5], (int)dr[6], (int)dr[7], (int)dr[8], (int)dr[9]);
                // The data and time of this reading as parsed from computer time
                DateTime dt = DateParser.DateAndTimeStringToDateTime((string)dr[0], (string)dr[1]);

                // check if filtering by dates and if so, if the record is in the range of the filter
                if ((filterSet == false) || ((filterSet == true) && (dt >= dtStart) && (dt <= dtEnd)))
                {
                    // loop through all the rows, and if this is not the first one
                    if (oldDr != null)
                    {
                        string s1 = (string)dr[0];
                        string s2 = (string)oldDr[0];
                        if (s1.Equals(s2) == false) // Did we change dates?
                        {
                            AddTodayRow((string)oldDr[0], temperature, humidity, voltage, pressure);

                            // And now reset the accumulators for next day reading
                            temperature.reset();
                            humidity.reset();
                            voltage.reset();
                            pressure.reset();
                            row_avail = false;
                        }
                    }

                    humidityValue = (float)dr[8];
                    if (humidityValue != 0) // Drop DHT22 reading if the humidity is 0.
                    {
                        temperature.add(((float)dr[5] + (float)dr[6] + (float)dr[7]) / 3, dt); // Average temperature from 3 temperatures sensor
                        humidity.add((float)dr[8], dt);
                    }
                    else
                        temperature.add(((float)dr[5] + (float)dr[7]) / 2, dt); // Average temperature from 2 temperatures sensor other than DHT22
                    voltage.add((float)dr[2], dt);
                    long l = (long)dr[9];
                    pressure.add(l, dt);

                    dtRange.add(dt);
                    row_avail = true;
                    oldDr = dr;
                }
            }
            // Check if last line needs to be added
            if (row_avail)
                AddTodayRow((string)oldDr[0], temperature, humidity, voltage, pressure);

            // Now update the controls of date filter with min and max dates
            bool firstTime = dtRange.firstTime();
            DateTime dtMin = dtRange.getMin();
            DateTime dtMax = dtRange.getMax();

            this.dateTimePickerFrom.MinDate = dtMin;
            this.dateTimePickerFrom.MaxDate = dtMax;
            if (firstTime == false)
              this.dateTimePickerFrom.Value = dtMin;
            this.dateTimePickerTo.MinDate = dtMin;
            this.dateTimePickerTo.MaxDate = dtMax;
            if (firstTime == false)
                this.dateTimePickerTo.Value = dtMax;
        }


        /* --------------------------------------------
         * Handlers for the different clicks and events
         * --------------------------------------------
         */
        private void workbook_SavingProgress(object sender, ProgressEventArgs e)
        {
            progress += e.Percent;
            if (progress > 100)
                progress = 100;
            SaveProgressBar.Value = (int)progress;
            this.SaveProgressBar.Update();
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ignore clicks on the header row that changes sorting
            {
                string today = (string)dgvWeatherAnalysis.Rows[e.RowIndex].Cells[0].Value;
                new InDayAllCharts(dtWeatherData, today).Show();
            }
        }


        // Close the grid view and null the table to free memory
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
            dtWeatherData = null;
            this.Dispose();
            GC.Collect();                       // Since the table is chewing lots of memory amd doing collect manually
            GC.WaitForPendingFinalizers();
        }



        private void Export_Click(object sender, EventArgs e)
        {
            Cursor c;

            saveFileDialog1.FileName = @"C:\Temp\ws.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Change cursor to wait cursor, export to file and return cursor to previous state
                c = this.Cursor;
                this.Cursor = Cursors.WaitCursor;

                // Based on extention filter index set the Excel file format
                if (saveFileDialog1.FilterIndex == 1)
                    SaveToExcel(saveFileDialog1.FileName, ExcelFileType.Office2010, dtWeatherData);
                else
                    SaveToExcel(saveFileDialog1.FileName, ExcelFileType.Office2003, dtWeatherData);

                // Restore cursor
                this.Cursor = c;
            }
        }


        // The buttons for different graphs
        private void Graph_Click(object sender, EventArgs e) { new TSummaryGraph(dgvWeatherAnalysis).Show(); }
        private void HumidityGraph_Click(object sender, EventArgs e) { new HSummaryGraph(dgvWeatherAnalysis).Show(); }
        private void BatGraph_Click(object sender, EventArgs e) { new BSummaryGraph(dgvWeatherAnalysis).Show(); }
        private void pressureGraph_Click(object sender, EventArgs e) { new PSummary(dgvWeatherAnalysis).Show(); }



        private void filterDatesRange_CheckedChanged(object sender, EventArgs e)
        {
            if (this.filterDatesRange.Checked)
            {
                this.dateTimePickerFrom.Enabled = true;
                this.dateTimePickerTo.Enabled = true;
                RebuildGridView();
            }
        }



        private void filterAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.filterAll.Checked)
            {
                this.dateTimePickerFrom.Enabled = false;
                this.dateTimePickerTo.Enabled = false;
                RebuildGridView();
            }
        }



        private void RebuildGridView()
        {
            // Clear the grid from current content
            this.dgvWeatherAnalysis.Rows.Clear();

            // Rebuild the grid view data using the filter
            AnalyzeData(dtWeatherData, this.filterDatesRange.Checked, this.dateTimePickerFrom.Value, this.dateTimePickerTo.Value);
        }



        private void dateTimePickerFrom_ValueChanged(object sender, EventArgs e) { RebuildGridView(); }
        private void dateTimePickerTo_ValueChanged(object sender, EventArgs e) { RebuildGridView(); }



        private void aboutButton_Click(object sender, EventArgs e)
        {
            // Couldn't care less, but the JetCell library is asking for ACK if using the free version, so here it is.
            MessageBox.Show(WeatherStationMan.WeatherStationManConstants.aboutMessage,"About", MessageBoxButtons.OK, MessageBoxIcon.Information); 
        }

        
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e) { }
    }



    // DataMeasure class used to calculate average, min and max of a given reading parameter
    class DataMeasure
    {
        private double sum;
        private int count;
        private double min;
        private double max;
        private double invalidData;
        private DateTime dtMin;
        private DateTime dtMax;

        public DataMeasure(double invalid_data) { reset(); invalidData = invalid_data; }

        public double getMin() { return min; }
        public string getMinTime() { return dtMin.ToShortTimeString(); }

        public double getMax() { return max; }
        public string getMaxTime() { return dtMax.ToShortTimeString(); }

        public double getAvg() { return sum / count; }
        public void reset() { sum = 0; count = 0; min = 200000; max = -273; }

        public void add(double v, DateTime dt)
        {
            if (v == invalidData)
                return;
            sum += v;
            count++;
            if (v > max)
            {
                max = v;
                dtMax = dt;
            }
            if (v < min)
            {
                min = v;
                dtMin = dt;
            }
        }
    } // DataMeasure



    class DateTimeRange
    {
        private DateTime min;
        private DateTime max;
        private bool isSet;

        public DateTimeRange()
        {
            min = new DateTime(2100, 12, 31);
            max = new DateTime(1753, 1, 1);
            isSet = false; // Will turn true once object is queried
        }



        public void add(DateTime dt)
        {
            if (isSet == false)
                if (dt < min)
                    min = dt;
                else
                    if (dt > max)
                        max = dt;
        }



        public DateTime getMax() { isSet = true; return max; }
        public DateTime getMin() { isSet = true; return min; }
        public bool firstTime() { return isSet; }
    } // DateTimeRange
}
