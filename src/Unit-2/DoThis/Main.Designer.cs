namespace ChartApp
{
    partial class Main
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.sysChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.CpuBtn = new System.Windows.Forms.Button();
            this.MemoryBtn = new System.Windows.Forms.Button();
            this.DiskBtn = new System.Windows.Forms.Button();
            this.PauseBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sysChart)).BeginInit();
            this.SuspendLayout();
            // 
            // sysChart
            // 
            chartArea2.Name = "ChartArea1";
            this.sysChart.ChartAreas.Add(chartArea2);
            this.sysChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.sysChart.Legends.Add(legend2);
            this.sysChart.Location = new System.Drawing.Point(0, 0);
            this.sysChart.Name = "sysChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.sysChart.Series.Add(series2);
            this.sysChart.Size = new System.Drawing.Size(684, 446);
            this.sysChart.TabIndex = 0;
            this.sysChart.Text = "sysChart";
            // 
            // CpuBtn
            // 
            this.CpuBtn.Location = new System.Drawing.Point(584, 307);
            this.CpuBtn.Name = "CpuBtn";
            this.CpuBtn.Size = new System.Drawing.Size(88, 23);
            this.CpuBtn.TabIndex = 1;
            this.CpuBtn.Text = "CPU (On)";
            this.CpuBtn.UseVisualStyleBackColor = true;
            this.CpuBtn.Click += new System.EventHandler(this.CpuBtn_Click);
            // 
            // MemoryBtn
            // 
            this.MemoryBtn.Location = new System.Drawing.Point(584, 336);
            this.MemoryBtn.Name = "MemoryBtn";
            this.MemoryBtn.Size = new System.Drawing.Size(88, 23);
            this.MemoryBtn.TabIndex = 2;
            this.MemoryBtn.Text = "Memory (Off)";
            this.MemoryBtn.UseVisualStyleBackColor = true;
            this.MemoryBtn.Click += new System.EventHandler(this.MemoryBtn_Click);
            // 
            // DiskBtn
            // 
            this.DiskBtn.Location = new System.Drawing.Point(584, 365);
            this.DiskBtn.Name = "DiskBtn";
            this.DiskBtn.Size = new System.Drawing.Size(88, 23);
            this.DiskBtn.TabIndex = 3;
            this.DiskBtn.Text = "Disk (Off)";
            this.DiskBtn.UseVisualStyleBackColor = true;
            this.DiskBtn.Click += new System.EventHandler(this.DiskBtn_Click);
            // 
            // PauseBtn
            // 
            this.PauseBtn.Location = new System.Drawing.Point(584, 246);
            this.PauseBtn.Name = "PauseBtn";
            this.PauseBtn.Size = new System.Drawing.Size(88, 27);
            this.PauseBtn.TabIndex = 4;
            this.PauseBtn.Text = "Pause";
            this.PauseBtn.UseVisualStyleBackColor = true;
            this.PauseBtn.Click += new System.EventHandler(this.PauseBtn_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 446);
            this.Controls.Add(this.PauseBtn);
            this.Controls.Add(this.DiskBtn);
            this.Controls.Add(this.MemoryBtn);
            this.Controls.Add(this.CpuBtn);
            this.Controls.Add(this.sysChart);
            this.Name = "Main";
            this.Text = "System Metrics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sysChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart sysChart;
        private System.Windows.Forms.Button CpuBtn;
        private System.Windows.Forms.Button MemoryBtn;
        private System.Windows.Forms.Button DiskBtn;
        private System.Windows.Forms.Button PauseBtn;
    }
}

