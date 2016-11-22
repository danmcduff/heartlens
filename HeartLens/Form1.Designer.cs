namespace HeartLens
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.timerFrameUpdate = new System.Windows.Forms.Timer(this.components);
            this.textBoxHeartRate = new System.Windows.Forms.TextBox();
            this.DeviceComboBox = new System.Windows.Forms.ComboBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.pictureBoxFace = new System.Windows.Forms.PictureBox();
            this.groupBoxVideo = new System.Windows.Forms.GroupBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.SuspendLayout();
            // 
            // timerFrameUpdate
            // 
            this.timerFrameUpdate.Enabled = true;
            this.timerFrameUpdate.Interval = 1000;
            this.timerFrameUpdate.Tick += new System.EventHandler(this.timerFrameUpdate_Tick);
            // 
            // textBoxHeartRate
            // 
            this.textBoxHeartRate.Location = new System.Drawing.Point(12, 12);
            this.textBoxHeartRate.Name = "textBoxHeartRate";
            this.textBoxHeartRate.Size = new System.Drawing.Size(100, 20);
            this.textBoxHeartRate.TabIndex = 0;
            // 
            // DeviceComboBox
            // 
            this.DeviceComboBox.FormattingEnabled = true;
            this.DeviceComboBox.Location = new System.Drawing.Point(164, 12);
            this.DeviceComboBox.Name = "DeviceComboBox";
            this.DeviceComboBox.Size = new System.Drawing.Size(121, 21);
            this.DeviceComboBox.TabIndex = 3;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(305, 7);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(54, 28);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // pictureBoxFace
            // 
            this.pictureBoxFace.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBoxFace.Location = new System.Drawing.Point(12, 39);
            this.pictureBoxFace.Name = "pictureBoxFace";
            this.pictureBoxFace.Size = new System.Drawing.Size(292, 304);
            this.pictureBoxFace.TabIndex = 5;
            this.pictureBoxFace.TabStop = false;
            // 
            // groupBoxVideo
            // 
            this.groupBoxVideo.Location = new System.Drawing.Point(327, 41);
            this.groupBoxVideo.Name = "groupBoxVideo";
            this.groupBoxVideo.Size = new System.Drawing.Size(322, 302);
            this.groupBoxVideo.TabIndex = 6;
            this.groupBoxVideo.TabStop = false;
            this.groupBoxVideo.Text = "groupBox1";
            this.groupBoxVideo.Enter += new System.EventHandler(this.groupBoxVideo_Enter);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(694, 43);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(490, 300);
            this.chart1.TabIndex = 7;
            this.chart1.Text = "chart1";
            // 
            // chart2
            // 
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(694, 349);
            this.chart2.Name = "chart2";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart2.Series.Add(series2);
            this.chart2.Size = new System.Drawing.Size(490, 267);
            this.chart2.TabIndex = 8;
            this.chart2.Text = "chart2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 638);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.groupBoxVideo);
            this.Controls.Add(this.pictureBoxFace);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.DeviceComboBox);
            this.Controls.Add(this.textBoxHeartRate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerFrameUpdate;
        private System.Windows.Forms.TextBox textBoxHeartRate;
        private System.Windows.Forms.ComboBox DeviceComboBox;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.PictureBox pictureBoxFace;
        private System.Windows.Forms.GroupBox groupBoxVideo;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
    }
}

