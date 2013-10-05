namespace WeatherDataAnalyzer
{
    partial class InDayAllCharts
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InDayAllCharts));
            this.temperatures = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.humidity = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pressure = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.battery = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.temperatures)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.humidity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.battery)).BeginInit();
            this.SuspendLayout();
            // 
            // temperatures
            // 
            this.temperatures.BackColor = System.Drawing.Color.Transparent;
            this.temperatures.BorderSkin.BackColor = System.Drawing.Color.LightSteelBlue;
            this.temperatures.BorderSkin.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.Center;
            this.temperatures.BorderSkin.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.temperatures.BorderSkin.PageColor = System.Drawing.Color.LightSteelBlue;
            this.temperatures.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            chartArea1.AxisX.Title = "Time";
            chartArea1.AxisY.Title = "°C";
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.temperatures.ChartAreas.Add(chartArea1);
            this.temperatures.Location = new System.Drawing.Point(16, 22);
            this.temperatures.Name = "temperatures";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Crimson;
            series1.Name = "temp";
            this.temperatures.Series.Add(series1);
            this.temperatures.Size = new System.Drawing.Size(510, 304);
            this.temperatures.TabIndex = 0;
            this.temperatures.Text = "Temperature";
            // 
            // humidity
            // 
            this.humidity.BackColor = System.Drawing.Color.Transparent;
            this.humidity.BorderSkin.BackColor = System.Drawing.Color.LightSteelBlue;
            this.humidity.BorderSkin.PageColor = System.Drawing.Color.LightSteelBlue;
            this.humidity.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            chartArea2.AxisX.Title = "Time";
            chartArea2.AxisY.Title = "%";
            chartArea2.BackColor = System.Drawing.Color.Transparent;
            chartArea2.Name = "ChartArea1";
            this.humidity.ChartAreas.Add(chartArea2);
            this.humidity.Location = new System.Drawing.Point(532, 22);
            this.humidity.Name = "humidity";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Aqua;
            series2.Name = "humidity";
            this.humidity.Series.Add(series2);
            this.humidity.Size = new System.Drawing.Size(516, 303);
            this.humidity.TabIndex = 1;
            this.humidity.Text = "chart2";
            // 
            // pressure
            // 
            this.pressure.BackColor = System.Drawing.Color.Transparent;
            this.pressure.BorderSkin.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pressure.BorderSkin.PageColor = System.Drawing.Color.LightSteelBlue;
            this.pressure.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            chartArea3.AxisX.Title = "Time";
            chartArea3.AxisY.Title = "Pa";
            chartArea3.BackColor = System.Drawing.Color.Transparent;
            chartArea3.Name = "ChartArea1";
            this.pressure.ChartAreas.Add(chartArea3);
            this.pressure.Location = new System.Drawing.Point(16, 352);
            this.pressure.Name = "pressure";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.White;
            series3.Name = "pressure";
            this.pressure.Series.Add(series3);
            this.pressure.Size = new System.Drawing.Size(510, 280);
            this.pressure.TabIndex = 2;
            this.pressure.Text = "Barometric Pressure";
            // 
            // battery
            // 
            this.battery.BackColor = System.Drawing.Color.Transparent;
            this.battery.BorderSkin.BackColor = System.Drawing.Color.LightSteelBlue;
            this.battery.BorderSkin.PageColor = System.Drawing.Color.LightSteelBlue;
            this.battery.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            chartArea4.AxisX.Title = "Time";
            chartArea4.AxisY.Title = "Volt";
            chartArea4.BackColor = System.Drawing.Color.Transparent;
            chartArea4.Name = "ChartArea1";
            this.battery.ChartAreas.Add(chartArea4);
            this.battery.Location = new System.Drawing.Point(532, 354);
            this.battery.Name = "battery";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.LawnGreen;
            series4.Name = "Battery Voltage";
            this.battery.Series.Add(series4);
            this.battery.Size = new System.Drawing.Size(516, 278);
            this.battery.TabIndex = 3;
            this.battery.Text = "Battery Voltage";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(88, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Temperature";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(611, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Humidity";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(611, 338);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Battery Voltage";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label4.Location = new System.Drawing.Point(88, 338);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Barometric Pressure";
            // 
            // InDayAllCharts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1065, 642);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.battery);
            this.Controls.Add(this.pressure);
            this.Controls.Add(this.humidity);
            this.Controls.Add(this.temperatures);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InDayAllCharts";
            this.Text = "In Day Weather Sensors Charts";
            ((System.ComponentModel.ISupportInitialize)(this.temperatures)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.humidity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.battery)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart temperatures;
        private System.Windows.Forms.DataVisualization.Charting.Chart humidity;
        private System.Windows.Forms.DataVisualization.Charting.Chart pressure;
        private System.Windows.Forms.DataVisualization.Charting.Chart battery;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}