namespace WeatherDataAnalyzer
{
    partial class WeatherDataReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

#if false
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
#endif
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeatherDataReport));
            this.dgvWeatherAnalysis = new System.Windows.Forms.DataGridView();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tmin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TminTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tavg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tmax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TmaxTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hmin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HminTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Havg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hmax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HmaxTima = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Vmin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VminTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Vmax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VmaxTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pmin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PminTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pavg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pmax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PmaxTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.Export = new System.Windows.Forms.Button();
            this.TempGraph = new System.Windows.Forms.Button();
            this.SaveProgressBar = new System.Windows.Forms.ProgressBar();
            this.HumidityGraph = new System.Windows.Forms.Button();
            this.BatGraph = new System.Windows.Forms.Button();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.fromDate = new System.Windows.Forms.Label();
            this.toDate = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.filterDatesRange = new System.Windows.Forms.RadioButton();
            this.filterAll = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.aboutButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pressureGraph = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeatherAnalysis)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvWeatherAnalysis
            // 
            this.dgvWeatherAnalysis.AllowUserToDeleteRows = false;
            this.dgvWeatherAnalysis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWeatherAnalysis.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Date,
            this.Tmin,
            this.TminTime,
            this.Tavg,
            this.Tmax,
            this.TmaxTime,
            this.Hmin,
            this.HminTime,
            this.Havg,
            this.Hmax,
            this.HmaxTima,
            this.Vmin,
            this.VminTime,
            this.Vmax,
            this.VmaxTime,
            this.Pmin,
            this.PminTime,
            this.Pavg,
            this.Pmax,
            this.PmaxTime});
            this.dgvWeatherAnalysis.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvWeatherAnalysis.Location = new System.Drawing.Point(9, 12);
            this.dgvWeatherAnalysis.Name = "dgvWeatherAnalysis";
            this.dgvWeatherAnalysis.ReadOnly = true;
            this.dgvWeatherAnalysis.Size = new System.Drawing.Size(1143, 694);
            this.dgvWeatherAnalysis.TabIndex = 0;
            this.dgvWeatherAnalysis.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Date
            // 
            this.Date.DividerWidth = 2;
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 70;
            // 
            // Tmin
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.Format = "##.00 \'°C\'";
            dataGridViewCellStyle1.NullValue = null;
            this.Tmin.DefaultCellStyle = dataGridViewCellStyle1;
            this.Tmin.HeaderText = "Min Temp";
            this.Tmin.Name = "Tmin";
            this.Tmin.ReadOnly = true;
            this.Tmin.Width = 60;
            // 
            // TminTime
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TminTime.DefaultCellStyle = dataGridViewCellStyle2;
            this.TminTime.HeaderText = "@Time";
            this.TminTime.Name = "TminTime";
            this.TminTime.ReadOnly = true;
            this.TminTime.Width = 50;
            // 
            // Tavg
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "##.00 \'°C\'";
            dataGridViewCellStyle3.NullValue = null;
            this.Tavg.DefaultCellStyle = dataGridViewCellStyle3;
            this.Tavg.HeaderText = "Avg Temp";
            this.Tavg.Name = "Tavg";
            this.Tavg.ReadOnly = true;
            this.Tavg.Width = 60;
            // 
            // Tmax
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle4.Format = "##.00 \'°C\'";
            this.Tmax.DefaultCellStyle = dataGridViewCellStyle4;
            this.Tmax.HeaderText = "Max Temp";
            this.Tmax.Name = "Tmax";
            this.Tmax.ReadOnly = true;
            this.Tmax.Width = 60;
            // 
            // TmaxTime
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TmaxTime.DefaultCellStyle = dataGridViewCellStyle5;
            this.TmaxTime.DividerWidth = 2;
            this.TmaxTime.HeaderText = "@Time";
            this.TmaxTime.Name = "TmaxTime";
            this.TmaxTime.ReadOnly = true;
            this.TmaxTime.Width = 50;
            // 
            // Hmin
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle6.Format = "##\\%";
            dataGridViewCellStyle6.NullValue = null;
            this.Hmin.DefaultCellStyle = dataGridViewCellStyle6;
            this.Hmin.HeaderText = "Min Humidity";
            this.Hmin.Name = "Hmin";
            this.Hmin.ReadOnly = true;
            this.Hmin.Width = 50;
            // 
            // HminTime
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.HminTime.DefaultCellStyle = dataGridViewCellStyle7;
            this.HminTime.HeaderText = "@Time";
            this.HminTime.Name = "HminTime";
            this.HminTime.ReadOnly = true;
            this.HminTime.Width = 50;
            // 
            // Havg
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "##\\%";
            this.Havg.DefaultCellStyle = dataGridViewCellStyle8;
            this.Havg.HeaderText = "Avg Humidity ";
            this.Havg.Name = "Havg";
            this.Havg.ReadOnly = true;
            this.Havg.Width = 50;
            // 
            // Hmax
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle9.Format = "##\\%";
            this.Hmax.DefaultCellStyle = dataGridViewCellStyle9;
            this.Hmax.HeaderText = "Max Humidty";
            this.Hmax.Name = "Hmax";
            this.Hmax.ReadOnly = true;
            this.Hmax.Width = 50;
            // 
            // HmaxTima
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.HmaxTima.DefaultCellStyle = dataGridViewCellStyle10;
            this.HmaxTima.DividerWidth = 2;
            this.HmaxTima.HeaderText = "@Time";
            this.HmaxTima.Name = "HmaxTima";
            this.HmaxTima.ReadOnly = true;
            this.HmaxTima.Width = 50;
            // 
            // Vmin
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle11.Format = "##.00\\V";
            this.Vmin.DefaultCellStyle = dataGridViewCellStyle11;
            this.Vmin.HeaderText = "Min Bat";
            this.Vmin.Name = "Vmin";
            this.Vmin.ReadOnly = true;
            this.Vmin.Width = 50;
            // 
            // VminTime
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.VminTime.DefaultCellStyle = dataGridViewCellStyle12;
            this.VminTime.HeaderText = "@Time";
            this.VminTime.Name = "VminTime";
            this.VminTime.ReadOnly = true;
            this.VminTime.Width = 50;
            // 
            // Vmax
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Green;
            dataGridViewCellStyle13.Format = "##.00\\V";
            this.Vmax.DefaultCellStyle = dataGridViewCellStyle13;
            this.Vmax.HeaderText = "Max Bat";
            this.Vmax.Name = "Vmax";
            this.Vmax.ReadOnly = true;
            this.Vmax.Width = 50;
            // 
            // VmaxTime
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.VmaxTime.DefaultCellStyle = dataGridViewCellStyle14;
            this.VmaxTime.DividerWidth = 2;
            this.VmaxTime.HeaderText = "@Time";
            this.VmaxTime.Name = "VmaxTime";
            this.VmaxTime.ReadOnly = true;
            this.VmaxTime.Width = 50;
            // 
            // Pmin
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle15.Format = "##,#Pa";
            this.Pmin.DefaultCellStyle = dataGridViewCellStyle15;
            this.Pmin.HeaderText = "Min Pressure";
            this.Pmin.Name = "Pmin";
            this.Pmin.ReadOnly = true;
            this.Pmin.Width = 60;
            // 
            // PminTime
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PminTime.DefaultCellStyle = dataGridViewCellStyle16;
            this.PminTime.HeaderText = "@Time";
            this.PminTime.Name = "PminTime";
            this.PminTime.ReadOnly = true;
            this.PminTime.Width = 50;
            // 
            // Pavg
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle17.Format = "##,#Pa";
            this.Pavg.DefaultCellStyle = dataGridViewCellStyle17;
            this.Pavg.HeaderText = "Avg Pressure";
            this.Pavg.Name = "Pavg";
            this.Pavg.ReadOnly = true;
            this.Pavg.Width = 60;
            // 
            // Pmax
            // 
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle18.Format = "##,#Pa";
            this.Pmax.DefaultCellStyle = dataGridViewCellStyle18;
            this.Pmax.HeaderText = "Max Pressure";
            this.Pmax.Name = "Pmax";
            this.Pmax.ReadOnly = true;
            this.Pmax.Width = 60;
            // 
            // PmaxTime
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PmaxTime.DefaultCellStyle = dataGridViewCellStyle19;
            this.PmaxTime.HeaderText = "@Time";
            this.PmaxTime.Name = "PmaxTime";
            this.PmaxTime.ReadOnly = true;
            this.PmaxTime.Width = 50;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.dgvWeatherAnalysis);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Location = new System.Drawing.Point(6, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1169, 728);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Weather Data Analysis Report";
            // 
            // CloseButton
            // 
            this.CloseButton.Image = global::WeatherStationMan.Properties.Resources.close;
            this.CloseButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CloseButton.Location = new System.Drawing.Point(1189, 13);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(153, 25);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // Export
            // 
            this.Export.Image = global::WeatherStationMan.Properties.Resources.Excel;
            this.Export.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Export.Location = new System.Drawing.Point(1189, 56);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(153, 25);
            this.Export.TabIndex = 3;
            this.Export.Text = "Export to Excel";
            this.Export.UseVisualStyleBackColor = true;
            this.Export.Click += new System.EventHandler(this.Export_Click);
            // 
            // TempGraph
            // 
            this.TempGraph.Image = global::WeatherStationMan.Properties.Resources.temperature;
            this.TempGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TempGraph.Location = new System.Drawing.Point(1189, 99);
            this.TempGraph.Name = "TempGraph";
            this.TempGraph.Size = new System.Drawing.Size(153, 25);
            this.TempGraph.TabIndex = 4;
            this.TempGraph.Text = "Temperatures Graph";
            this.TempGraph.UseVisualStyleBackColor = true;
            this.TempGraph.Click += new System.EventHandler(this.Graph_Click);
            // 
            // SaveProgressBar
            // 
            this.SaveProgressBar.Location = new System.Drawing.Point(12, 749);
            this.SaveProgressBar.Name = "SaveProgressBar";
            this.SaveProgressBar.Size = new System.Drawing.Size(548, 21);
            this.SaveProgressBar.TabIndex = 5;
            this.SaveProgressBar.Visible = false;
            // 
            // HumidityGraph
            // 
            this.HumidityGraph.Image = global::WeatherStationMan.Properties.Resources.humidity;
            this.HumidityGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.HumidityGraph.Location = new System.Drawing.Point(1189, 142);
            this.HumidityGraph.Name = "HumidityGraph";
            this.HumidityGraph.Size = new System.Drawing.Size(153, 25);
            this.HumidityGraph.TabIndex = 7;
            this.HumidityGraph.Text = "Humidity Graph";
            this.HumidityGraph.UseVisualStyleBackColor = true;
            this.HumidityGraph.Click += new System.EventHandler(this.HumidityGraph_Click);
            // 
            // BatGraph
            // 
            this.BatGraph.Image = global::WeatherStationMan.Properties.Resources.battery;
            this.BatGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BatGraph.Location = new System.Drawing.Point(1189, 185);
            this.BatGraph.Name = "BatGraph";
            this.BatGraph.Size = new System.Drawing.Size(153, 25);
            this.BatGraph.TabIndex = 8;
            this.BatGraph.Text = "Battery Graph";
            this.BatGraph.UseVisualStyleBackColor = true;
            this.BatGraph.Click += new System.EventHandler(this.BatGraph_Click);
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.AllowDrop = true;
            this.dateTimePickerFrom.Enabled = false;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(3, 100);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(170, 20);
            this.dateTimePickerFrom.TabIndex = 10;
            this.dateTimePickerFrom.ValueChanged += new System.EventHandler(this.dateTimePickerFrom_ValueChanged);
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.AllowDrop = true;
            this.dateTimePickerTo.Enabled = false;
            this.dateTimePickerTo.Location = new System.Drawing.Point(3, 139);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(170, 20);
            this.dateTimePickerTo.TabIndex = 11;
            this.dateTimePickerTo.ValueChanged += new System.EventHandler(this.dateTimePickerTo_ValueChanged);
            // 
            // fromDate
            // 
            this.fromDate.AutoSize = true;
            this.fromDate.Location = new System.Drawing.Point(0, 72);
            this.fromDate.Name = "fromDate";
            this.fromDate.Size = new System.Drawing.Size(56, 13);
            this.fromDate.TabIndex = 12;
            this.fromDate.Text = "From Date";
            // 
            // toDate
            // 
            this.toDate.AutoSize = true;
            this.toDate.Location = new System.Drawing.Point(0, 123);
            this.toDate.Name = "toDate";
            this.toDate.Size = new System.Drawing.Size(46, 13);
            this.toDate.TabIndex = 13;
            this.toDate.Text = "To Date";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.filterDatesRange);
            this.panel1.Controls.Add(this.filterAll);
            this.panel1.Location = new System.Drawing.Point(15, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(108, 53);
            this.panel1.TabIndex = 9;
            // 
            // filterDatesRange
            // 
            this.filterDatesRange.AutoSize = true;
            this.filterDatesRange.Location = new System.Drawing.Point(0, 26);
            this.filterDatesRange.Name = "filterDatesRange";
            this.filterDatesRange.Size = new System.Drawing.Size(88, 17);
            this.filterDatesRange.TabIndex = 1;
            this.filterDatesRange.TabStop = true;
            this.filterDatesRange.Text = "Dates Range";
            this.filterDatesRange.UseVisualStyleBackColor = true;
            this.filterDatesRange.CheckedChanged += new System.EventHandler(this.filterDatesRange_CheckedChanged);
            // 
            // filterAll
            // 
            this.filterAll.AutoSize = true;
            this.filterAll.Checked = true;
            this.filterAll.Location = new System.Drawing.Point(0, 3);
            this.filterAll.Name = "filterAll";
            this.filterAll.Size = new System.Drawing.Size(79, 17);
            this.filterAll.TabIndex = 0;
            this.filterAll.TabStop = true;
            this.filterAll.Text = "All Records";
            this.filterAll.UseVisualStyleBackColor = true;
            this.filterAll.CheckedChanged += new System.EventHandler(this.filterAll_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.toDate);
            this.groupBox2.Controls.Add(this.fromDate);
            this.groupBox2.Controls.Add(this.dateTimePickerTo);
            this.groupBox2.Controls.Add(this.dateTimePickerFrom);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Location = new System.Drawing.Point(1186, 377);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(196, 186);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter Data";
            // 
            // aboutButton
            // 
            this.aboutButton.Image = global::WeatherStationMan.Properties.Resources.about;
            this.aboutButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.aboutButton.Location = new System.Drawing.Point(1189, 695);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(153, 25);
            this.aboutButton.TabIndex = 15;
            this.aboutButton.Text = "About";
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xlsx";
            this.saveFileDialog1.FileName = "ws.xlsx";
            this.saveFileDialog1.Filter = "Office 2010 Excel Files|*.xlsx|Office 2003-2007 Excel Files|*.xls";
            this.saveFileDialog1.InitialDirectory = "C:\\Temp";
            this.saveFileDialog1.Title = "Export to Excel File";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // pressureGraph
            // 
            this.pressureGraph.Image = global::WeatherStationMan.Properties.Resources.pressure;
            this.pressureGraph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pressureGraph.Location = new System.Drawing.Point(1189, 228);
            this.pressureGraph.Name = "pressureGraph";
            this.pressureGraph.Size = new System.Drawing.Size(153, 25);
            this.pressureGraph.TabIndex = 16;
            this.pressureGraph.Text = "Pressure Graph";
            this.pressureGraph.UseVisualStyleBackColor = true;
            this.pressureGraph.Click += new System.EventHandler(this.pressureGraph_Click);
            // 
            // WeatherDataReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1389, 778);
            this.Controls.Add(this.pressureGraph);
            this.Controls.Add(this.aboutButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.BatGraph);
            this.Controls.Add(this.HumidityGraph);
            this.Controls.Add(this.SaveProgressBar);
            this.Controls.Add(this.TempGraph);
            this.Controls.Add(this.Export);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WeatherDataReport";
            this.Text = "Weather Data Report";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeatherAnalysis)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgvWeatherAnalysis;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button Export;
        private System.Windows.Forms.Button TempGraph;
        private System.Windows.Forms.ProgressBar SaveProgressBar;
        private System.Windows.Forms.Button HumidityGraph;
        private System.Windows.Forms.Button BatGraph;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.Label fromDate;
        private System.Windows.Forms.Label toDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton filterDatesRange;
        private System.Windows.Forms.RadioButton filterAll;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button aboutButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button pressureGraph;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tmin;
        private System.Windows.Forms.DataGridViewTextBoxColumn TminTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tavg;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tmax;
        private System.Windows.Forms.DataGridViewTextBoxColumn TmaxTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hmin;
        private System.Windows.Forms.DataGridViewTextBoxColumn HminTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Havg;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hmax;
        private System.Windows.Forms.DataGridViewTextBoxColumn HmaxTima;
        private System.Windows.Forms.DataGridViewTextBoxColumn Vmin;
        private System.Windows.Forms.DataGridViewTextBoxColumn VminTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Vmax;
        private System.Windows.Forms.DataGridViewTextBoxColumn VmaxTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pmin;
        private System.Windows.Forms.DataGridViewTextBoxColumn PminTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pavg;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pmax;
        private System.Windows.Forms.DataGridViewTextBoxColumn PmaxTime;
    }
}

