namespace TravelEaseForms.Forms
{
    partial class Report
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabHotels = new System.Windows.Forms.TabPage();
            this.splitContainerHotels = new System.Windows.Forms.SplitContainer();
            this.dgvHotelOccupancy = new System.Windows.Forms.DataGridView();
            this.chartHotelOccupancy = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabGuides = new System.Windows.Forms.TabPage();
            this.splitContainerGuides = new System.Windows.Forms.SplitContainer();
            this.dgvGuideRatings = new System.Windows.Forms.DataGridView();
            this.chartGuideRatings = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabTransport = new System.Windows.Forms.TabPage();
            this.splitContainerTransport = new System.Windows.Forms.SplitContainer();
            this.dgvTransportPerformance = new System.Windows.Forms.DataGridView();
            this.chartTransportPerformance = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabSummary = new System.Windows.Forms.TabPage();
            this.txtSummary = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportPDF = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabHotels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHotels)).BeginInit();
            this.splitContainerHotels.Panel1.SuspendLayout();
            this.splitContainerHotels.Panel2.SuspendLayout();
            this.splitContainerHotels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHotelOccupancy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartHotelOccupancy)).BeginInit();
            this.tabGuides.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGuides)).BeginInit();
            this.splitContainerGuides.Panel1.SuspendLayout();
            this.splitContainerGuides.Panel2.SuspendLayout();
            this.splitContainerGuides.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGuideRatings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartGuideRatings)).BeginInit();
            this.tabTransport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTransport)).BeginInit();
            this.splitContainerTransport.Panel1.SuspendLayout();
            this.splitContainerTransport.Panel2.SuspendLayout();
            this.splitContainerTransport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransportPerformance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTransportPerformance)).BeginInit();
            this.tabSummary.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabHotels);
            this.tabControl1.Controls.Add(this.tabGuides);
            this.tabControl1.Controls.Add(this.tabTransport);
            this.tabControl1.Controls.Add(this.tabSummary);
            this.tabControl1.Location = new System.Drawing.Point(12, 58);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1060, 505);
            this.tabControl1.TabIndex = 0;
            // 
            // tabHotels
            // 
            this.tabHotels.Controls.Add(this.splitContainerHotels);
            this.tabHotels.Location = new System.Drawing.Point(4, 22);
            this.tabHotels.Name = "tabHotels";
            this.tabHotels.Padding = new System.Windows.Forms.Padding(3);
            this.tabHotels.Size = new System.Drawing.Size(1052, 479);
            this.tabHotels.TabIndex = 0;
            this.tabHotels.Text = "Hotel Occupancy";
            this.tabHotels.UseVisualStyleBackColor = true;
            // 
            // splitContainerHotels
            // 
            this.splitContainerHotels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerHotels.Location = new System.Drawing.Point(3, 3);
            this.splitContainerHotels.Name = "splitContainerHotels";
            this.splitContainerHotels.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerHotels.Panel1
            // 
            this.splitContainerHotels.Panel1.Controls.Add(this.dgvHotelOccupancy);
            // 
            // splitContainerHotels.Panel2
            // 
            this.splitContainerHotels.Panel2.Controls.Add(this.chartHotelOccupancy);
            this.splitContainerHotels.Size = new System.Drawing.Size(1046, 473);
            this.splitContainerHotels.SplitterDistance = 220;
            this.splitContainerHotels.TabIndex = 0;
            // 
            // dgvHotelOccupancy
            // 
            this.dgvHotelOccupancy.AllowUserToAddRows = false;
            this.dgvHotelOccupancy.AllowUserToDeleteRows = false;
            this.dgvHotelOccupancy.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHotelOccupancy.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvHotelOccupancy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHotelOccupancy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHotelOccupancy.Location = new System.Drawing.Point(0, 0);
            this.dgvHotelOccupancy.Name = "dgvHotelOccupancy";
            this.dgvHotelOccupancy.ReadOnly = true;
            this.dgvHotelOccupancy.Size = new System.Drawing.Size(1046, 220);
            this.dgvHotelOccupancy.TabIndex = 0;
            // 
            // chartHotelOccupancy
            // 
            chartArea1.Name = "ChartArea1";
            this.chartHotelOccupancy.ChartAreas.Add(chartArea1);
            this.chartHotelOccupancy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartHotelOccupancy.Location = new System.Drawing.Point(0, 0);
            this.chartHotelOccupancy.Name = "chartHotelOccupancy";
            this.chartHotelOccupancy.Size = new System.Drawing.Size(1046, 249);
            this.chartHotelOccupancy.TabIndex = 0;
            this.chartHotelOccupancy.Text = "Hotel Occupancy";
            // 
            // tabGuides
            // 
            this.tabGuides.Controls.Add(this.splitContainerGuides);
            this.tabGuides.Location = new System.Drawing.Point(4, 22);
            this.tabGuides.Name = "tabGuides";
            this.tabGuides.Padding = new System.Windows.Forms.Padding(3);
            this.tabGuides.Size = new System.Drawing.Size(1052, 479);
            this.tabGuides.TabIndex = 1;
            this.tabGuides.Text = "Guide Ratings";
            this.tabGuides.UseVisualStyleBackColor = true;
            // 
            // splitContainerGuides
            // 
            this.splitContainerGuides.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerGuides.Location = new System.Drawing.Point(3, 3);
            this.splitContainerGuides.Name = "splitContainerGuides";
            this.splitContainerGuides.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerGuides.Panel1
            // 
            this.splitContainerGuides.Panel1.Controls.Add(this.dgvGuideRatings);
            // 
            // splitContainerGuides.Panel2
            // 
            this.splitContainerGuides.Panel2.Controls.Add(this.chartGuideRatings);
            this.splitContainerGuides.Size = new System.Drawing.Size(1046, 473);
            this.splitContainerGuides.SplitterDistance = 220;
            this.splitContainerGuides.TabIndex = 1;
            // 
            // dgvGuideRatings
            // 
            this.dgvGuideRatings.AllowUserToAddRows = false;
            this.dgvGuideRatings.AllowUserToDeleteRows = false;
            this.dgvGuideRatings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGuideRatings.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvGuideRatings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGuideRatings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGuideRatings.Location = new System.Drawing.Point(0, 0);
            this.dgvGuideRatings.Name = "dgvGuideRatings";
            this.dgvGuideRatings.ReadOnly = true;
            this.dgvGuideRatings.Size = new System.Drawing.Size(1046, 220);
            this.dgvGuideRatings.TabIndex = 0;
            // 
            // chartGuideRatings
            // 
            chartArea2.Name = "ChartArea1";
            this.chartGuideRatings.ChartAreas.Add(chartArea2);
            this.chartGuideRatings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartGuideRatings.Location = new System.Drawing.Point(0, 0);
            this.chartGuideRatings.Name = "chartGuideRatings";
            this.chartGuideRatings.Size = new System.Drawing.Size(1046, 249);
            this.chartGuideRatings.TabIndex = 0;
            this.chartGuideRatings.Text = "Guide Ratings";
            // 
            // tabTransport
            // 
            this.tabTransport.Controls.Add(this.splitContainerTransport);
            this.tabTransport.Location = new System.Drawing.Point(4, 22);
            this.tabTransport.Name = "tabTransport";
            this.tabTransport.Padding = new System.Windows.Forms.Padding(3);
            this.tabTransport.Size = new System.Drawing.Size(1052, 479);
            this.tabTransport.TabIndex = 2;
            this.tabTransport.Text = "Transport Performance";
            this.tabTransport.UseVisualStyleBackColor = true;
            // 
            // splitContainerTransport
            // 
            this.splitContainerTransport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTransport.Location = new System.Drawing.Point(3, 3);
            this.splitContainerTransport.Name = "splitContainerTransport";
            this.splitContainerTransport.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTransport.Panel1
            // 
            this.splitContainerTransport.Panel1.Controls.Add(this.dgvTransportPerformance);
            // 
            // splitContainerTransport.Panel2
            // 
            this.splitContainerTransport.Panel2.Controls.Add(this.chartTransportPerformance);
            this.splitContainerTransport.Size = new System.Drawing.Size(1046, 473);
            this.splitContainerTransport.SplitterDistance = 220;
            this.splitContainerTransport.TabIndex = 1;
            // 
            // dgvTransportPerformance
            // 
            this.dgvTransportPerformance.AllowUserToAddRows = false;
            this.dgvTransportPerformance.AllowUserToDeleteRows = false;
            this.dgvTransportPerformance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransportPerformance.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvTransportPerformance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransportPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransportPerformance.Location = new System.Drawing.Point(0, 0);
            this.dgvTransportPerformance.Name = "dgvTransportPerformance";
            this.dgvTransportPerformance.ReadOnly = true;
            this.dgvTransportPerformance.Size = new System.Drawing.Size(1046, 220);
            this.dgvTransportPerformance.TabIndex = 0;
            // 
            // chartTransportPerformance
            // 
            chartArea3.Name = "ChartArea1";
            this.chartTransportPerformance.ChartAreas.Add(chartArea3);
            this.chartTransportPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartTransportPerformance.Location = new System.Drawing.Point(0, 0);
            this.chartTransportPerformance.Name = "chartTransportPerformance";
            this.chartTransportPerformance.Size = new System.Drawing.Size(1046, 249);
            this.chartTransportPerformance.TabIndex = 0;
            this.chartTransportPerformance.Text = "Transport Performance";
            // 
            // tabSummary
            // 
            this.tabSummary.Controls.Add(this.txtSummary);
            this.tabSummary.Location = new System.Drawing.Point(4, 22);
            this.tabSummary.Name = "tabSummary";
            this.tabSummary.Padding = new System.Windows.Forms.Padding(3);
            this.tabSummary.Size = new System.Drawing.Size(1052, 479);
            this.tabSummary.TabIndex = 3;
            this.tabSummary.Text = "Summary";
            this.tabSummary.UseVisualStyleBackColor = true;
            // 
            // txtSummary
            // 
            this.txtSummary.BackColor = System.Drawing.Color.White;
            this.txtSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSummary.Location = new System.Drawing.Point(3, 3);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.ReadOnly = true;
            this.txtSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSummary.Size = new System.Drawing.Size(1046, 473);
            this.txtSummary.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnExportExcel);
            this.panel1.Controls.Add(this.btnExportPDF);
            this.panel1.Location = new System.Drawing.Point(12, 569);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1060, 50);
            this.panel1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(982, 13);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(255, 13);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(134, 13);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(115, 23);
            this.btnExportExcel.TabIndex = 1;
            this.btnExportExcel.Text = "Export to Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnExportPDF
            // 
            this.btnExportPDF.Location = new System.Drawing.Point(13, 13);
            this.btnExportPDF.Name = "btnExportPDF";
            this.btnExportPDF.Size = new System.Drawing.Size(115, 23);
            this.btnExportPDF.TabIndex = 0;
            this.btnExportPDF.Text = "Export to PDF";
            this.btnExportPDF.UseVisualStyleBackColor = true;
            this.btnExportPDF.Click += new System.EventHandler(this.btnExportPDF_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(342, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Service Provider Efficiency Report";
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1084, 631);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Service Provider Efficiency Report";
            this.Load += new System.EventHandler(this.Report_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabHotels.ResumeLayout(false);
            this.splitContainerHotels.Panel1.ResumeLayout(false);
            this.splitContainerHotels.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerHotels)).EndInit();
            this.splitContainerHotels.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHotelOccupancy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartHotelOccupancy)).EndInit();
            this.tabGuides.ResumeLayout(false);
            this.splitContainerGuides.Panel1.ResumeLayout(false);
            this.splitContainerGuides.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGuides)).EndInit();
            this.splitContainerGuides.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGuideRatings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartGuideRatings)).EndInit();
            this.tabTransport.ResumeLayout(false);
            this.splitContainerTransport.Panel1.ResumeLayout(false);
            this.splitContainerTransport.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTransport)).EndInit();
            this.splitContainerTransport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransportPerformance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTransportPerformance)).EndInit();
            this.tabSummary.ResumeLayout(false);
            this.tabSummary.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabHotels;
        private System.Windows.Forms.TabPage tabGuides;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnExportPDF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabTransport;
        private System.Windows.Forms.TabPage tabSummary;
        private System.Windows.Forms.SplitContainer splitContainerHotels;
        private System.Windows.Forms.DataGridView dgvHotelOccupancy;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartHotelOccupancy;
        private System.Windows.Forms.SplitContainer splitContainerGuides;
        private System.Windows.Forms.DataGridView dgvGuideRatings;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartGuideRatings;
        private System.Windows.Forms.SplitContainer splitContainerTransport;
        private System.Windows.Forms.DataGridView dgvTransportPerformance;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTransportPerformance;
        private System.Windows.Forms.TextBox txtSummary;
    }
}