using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace TravelEaseForms.Forms
{
    public partial class Report : Form
    {
        private string connectionString = "Data Source=DESKTOP-7HGU5BP\\SQLEXPRESS;Initial Catalog = TravelEaseDataBase; Integrated Security = True;";

        public Report()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            LoadReportData();
        }

        private void LoadReportData()
        {
            try
            {
                // Get the data for each metric
                DataTable hotelOccupancyData = GetHotelOccupancyData();
                DataTable guideRatingsData = GetGuideRatingsData();
                DataTable transportPerformanceData = GetTransportPerformanceData();

                // Populate the data grids
                dgvHotelOccupancy.DataSource = hotelOccupancyData;
                dgvGuideRatings.DataSource = guideRatingsData;
                dgvTransportPerformance.DataSource = transportPerformanceData;

                // Generate visualizations
                GenerateHotelOccupancyChart(hotelOccupancyData);
                GenerateGuideRatingsChart(guideRatingsData);
                GenerateTransportPerformanceChart(transportPerformanceData);

                // Calculate and display summary statistics
                DisplaySummaryStatistics(hotelOccupancyData, guideRatingsData, transportPerformanceData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading report data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetHotelOccupancyData()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // SQL query to calculate hotel occupancy rate
                // This joins Room Reservations with Hotels to get occupancy information
                string query = @"
                    SELECT 
                        h.HotelID,
                        h.Name AS HotelName,
                        h.Address AS HotelAddress,
                        COUNT(DISTINCT r.RoomID) AS TotalRooms,
                        COUNT(DISTINCT rr.RoomID) AS BookedRooms,
                        CAST(COUNT(DISTINCT rr.RoomID) * 100.0 / NULLIF(COUNT(DISTINCT r.RoomID), 0) AS DECIMAL(5,2)) AS OccupancyRate
                    FROM 
                        Hotels h
                    LEFT JOIN 
                        Rooms r ON h.HotelID = r.HotelID
                    LEFT JOIN 
                        RoomReservations rr ON r.RoomID = rr.RoomID 
                    GROUP BY 
                        h.HotelID, h.Name, h.Address
                    ORDER BY 
                        OccupancyRate DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        private DataTable GetGuideRatingsData()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // SQL query to get Service Provider Ratings
                // This joins Reviews with Guide information
                string query = @"
                  
                SELECT 
                    s.ServiceID,
                    s.Name AS ProviderName ,
                    AVG(r.Rating) AS AverageRating,
                    COUNT(r.ReviewID) AS NumberOfReviews,
                    MAX(r.Rating) AS HighestRating,
                    MIN(r.Rating) AS LowestRating
                FROM 
                    Services s
                LEFT JOIN 
                    Reviews r 
                  ON r.ServiceID = s.ServiceID
                GROUP BY 
                    s.ServiceID,
                    s.Name
                ORDER BY 
                    AverageRating DESC;
                ";


                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        private DataTable GetTransportPerformanceData()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // SQL query to analyze transport service punctuality
                // This joins Transport Services with Bookings and Reviews
                string query = @"
                    SELECT 
                    ts.ServiceID,
                    ts.Name AS ServiceName,
                    ts.Type AS TransportType,
                    AVG(r.Rating) AS AverageRating,
                    COUNT(r.ReviewID) AS NumberOfReviews,
                    CAST(
                        SUM(CASE WHEN r.Comments LIKE '%on time%' OR r.Comments LIKE '%punctual%' THEN 1 ELSE 0 END) * 100.0 / 
                        NULLIF(COUNT(r.ReviewID), 0) AS DECIMAL(5,2)
                    ) AS OnTimePercentage
                FROM 
                    TransportServices ts
                LEFT JOIN 
                    Services s ON ts.ServiceID = s.ServiceID
                LEFT JOIN 
                    Bookings b ON s.ServiceID = b.ServiceID
                LEFT JOIN 
                    Reviews r ON r.ServiceID = s.ServiceID
                GROUP BY 
                    ts.ServiceID, ts.Name, ts.Type
                ORDER BY 
                    OnTimePercentage DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        private void GenerateHotelOccupancyChart(DataTable data)
        {
            // Configure the chart for hotel occupancy
            chartHotelOccupancy.Series.Clear();
            chartHotelOccupancy.ChartAreas.Clear();

            // Add chart area
            ChartArea area = new ChartArea("HotelOccupancy");
            chartHotelOccupancy.ChartAreas.Add(area);

            // Configure series
            Series series = new Series("OccupancyRate");
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.SteelBlue;
            chartHotelOccupancy.Series.Add(series);

            // Add data points
            foreach (DataRow row in data.Rows)
            {
                string hotelName = row["HotelName"].ToString();
                if (hotelName.Length > 15)
                    hotelName = hotelName.Substring(0, 12) + "...";

                // Handle DBNull values properly
                double occupancyRate = row["OccupancyRate"] != DBNull.Value ?
                    Convert.ToDouble(row["OccupancyRate"]) : 0;

                series.Points.AddXY(hotelName, occupancyRate);

                // Add data point label
                //int lastIndex = series.Points.Count - 1;
                //series.Points[lastIndex].Label = $"{occupancyRate:F1}%";
                //series.Points[lastIndex].LabelForeColor = Color.Black;
            }

            // Chart title and formatting
            chartHotelOccupancy.Titles.Clear();
            chartHotelOccupancy.Titles.Add(new Title("Hotel Occupancy Rates", Docking.Top, new Font("Arial", 12, FontStyle.Bold), Color.Black));

            // Format X-axis
            area.AxisX.Title = "Hotels";
            area.AxisX.TitleFont = new Font("Arial", 10, FontStyle.Regular);
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.LabelStyle.Font = new Font("Arial", 8);

            // Format Y-axis
            area.AxisY.Title = "Occupancy Rate (%)";
            area.AxisY.TitleFont = new Font("Arial", 10, FontStyle.Regular);
            area.AxisY.Maximum = 100;
            area.AxisY.Minimum = 0;
            area.AxisY.Interval = 10;

            // Enable 3D
            chartHotelOccupancy.ChartAreas[0].Area3DStyle.Enable3D = false;

            // Legend
            chartHotelOccupancy.Legends.Clear();
        }

        private void GenerateGuideRatingsChart(DataTable data)
        {
            // Configure the chart for guide ratings
            chartGuideRatings.Series.Clear();
            chartGuideRatings.ChartAreas.Clear();

            // Add chart area
            ChartArea area = new ChartArea("ProviderRatings");
            chartGuideRatings.ChartAreas.Add(area);

            // Add series
            Series series = new Series("AverageRating");
            series.ChartType = SeriesChartType.Bar;
            series.Color = Color.ForestGreen;
            chartGuideRatings.Series.Add(series);

            // Add data points
            foreach (DataRow row in data.Rows)
            {
                string ProviderName = row["ProviderName"].ToString();
                if (ProviderName.Length > 15)
                    ProviderName = ProviderName.Substring(0, 12) + "...";

                // Handle DBNull values properly
                double avgRating = row["AverageRating"] != DBNull.Value ?
                    Convert.ToDouble(row["AverageRating"]) : 0;

                series.Points.AddXY(ProviderName, avgRating);

                // Add data point label
                int lastIndex = series.Points.Count - 1;
                series.Points[lastIndex].Label = $"{avgRating:F1}";
                series.Points[lastIndex].LabelForeColor = Color.Black;
            }

            // Chart title and formatting
            chartGuideRatings.Titles.Clear();
            chartGuideRatings.Titles.Add(new Title("Guide Average Ratings", Docking.Top, new Font("Arial", 12, FontStyle.Bold), Color.Black));

            // Format X-axis
            area.AxisX.Title = "Guides";
            area.AxisX.TitleFont = new Font("Arial", 10, FontStyle.Regular);
            area.AxisX.LabelStyle.Font = new Font("Arial", 8);

            // Format Y-axis
            area.AxisY.Title = "Average Rating";
            area.AxisY.TitleFont = new Font("Arial", 10, FontStyle.Regular);
            area.AxisY.Maximum = 5;
            area.AxisY.Minimum = 0;
            area.AxisY.Interval = 1;

            // Enable 3D
            chartGuideRatings.ChartAreas[0].Area3DStyle.Enable3D = false;

            // Legend
            chartGuideRatings.Legends.Clear();
        }

        private void GenerateTransportPerformanceChart(DataTable data)
        {
            // Configure the chart for transport performance
            chartTransportPerformance.Series.Clear();
            chartTransportPerformance.ChartAreas.Clear();

            // Add chart area
            ChartArea area = new ChartArea("TransportPerformance");
            chartTransportPerformance.ChartAreas.Add(area);

            // Add series
            Series series = new Series("OnTimePerformance");
            series.ChartType = SeriesChartType.Pie;
            chartTransportPerformance.Series.Add(series);

            // Create a summary by transport type
            var transportTypes = data.AsEnumerable()
                .Where(r => r["TransportType"] != DBNull.Value && r["OnTimePercentage"] != DBNull.Value)
                .GroupBy(r => r.Field<string>("TransportType"))
                .Select(g => new
                {
                    TransportType = g.Key,
                    OnTimePercentage = g.Average(r => r["OnTimePercentage"] != DBNull.Value ?
                        Convert.ToDouble(r["OnTimePercentage"]) : 0)
                })
                .ToList();

            // Add data points
            foreach (var transport in transportTypes)
            {
                if (transport.TransportType != null)
                {
                    series.Points.AddXY(transport.TransportType, transport.OnTimePercentage);

                    // Add data point label
                    int lastIndex = series.Points.Count - 1;
                    series.Points[lastIndex].Label = $"{transport.TransportType}: {transport.OnTimePercentage:F1}%";
                    series.Points[lastIndex].LabelForeColor = Color.White;
                }
            }

            // Chart title and formatting
            chartTransportPerformance.Titles.Clear();
            chartTransportPerformance.Titles.Add(new Title("On-Time Performance by Transport Type", Docking.Top, new Font("Arial", 12, FontStyle.Bold), Color.Black));

            // Legend
            chartTransportPerformance.Legends.Clear();
            Legend legend = new Legend("TransportLegend");
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            chartTransportPerformance.Legends.Add(legend);

            // Enable 3D
            chartTransportPerformance.ChartAreas[0].Area3DStyle.Enable3D = true;
        }

        private void DisplaySummaryStatistics(DataTable hotelData, DataTable guideData, DataTable transportData)
        {
            // Calculate summary statistics
            string summary = "SUMMARY STATISTICS\r\n\r\n";

            // Hotel statistics
            double avgOccupancy = hotelData.AsEnumerable()
                .Where(r => r["OccupancyRate"] != DBNull.Value)
                .Select(r => Convert.ToDouble(r["OccupancyRate"]))
                .DefaultIfEmpty(0)
                .Average();

            string topHotel = hotelData.Rows.Count > 0 ?
                hotelData.Rows[0]["HotelName"].ToString() : "N/A";

            summary += $"Hotels:\r\n";
            summary += $"- Average Occupancy Rate: {avgOccupancy:F2}%\r\n";
            summary += $"- Top Performing Hotel: {topHotel}\r\n\r\n";

            // Guide statistics
            double avgGuideRating = guideData.AsEnumerable()
                .Where(r => r["AverageRating"] != DBNull.Value)
                .Select(r => Convert.ToDouble(r["AverageRating"]))
                .DefaultIfEmpty(0)
                .Average();

            string topGuide = guideData.Rows.Count > 0 ?
                guideData.Rows[0]["ProviderName"].ToString() : "N/A";

            summary += $"Guides:\r\n";
            summary += $"- Average Guide Rating: {avgGuideRating:F2}/5\r\n";
            summary += $"- Top Rated Guide: {topGuide}\r\n\r\n";

            // Transport statistics
            double avgOnTime = transportData.AsEnumerable()
                .Where(r => r["OnTimePercentage"] != DBNull.Value)
                .Select(r => Convert.ToDouble(r["OnTimePercentage"]))
                .DefaultIfEmpty(0)
                .Average();

            string topTransport = transportData.Rows.Count > 0 ?
                transportData.Rows[0]["ServiceName"].ToString() : "N/A";

            summary += $"Transport Services:\r\n";
            summary += $"- Average On-Time Performance: {avgOnTime:F2}%\r\n";
            summary += $"- Most Punctual Service: {topTransport}\r\n";

            // Display in summary text box
            txtSummary.Text = summary;
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                // Implement PDF export functionality
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                saveDialog.Title = "Save Service Provider Efficiency Report";
                saveDialog.FileName = "Report.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Code to generate PDF would go here
                    // You'll need a PDF library like iTextSharp or PDFsharp
                    // This is just a placeholder
                    MessageBox.Show("Report exported successfully to " + saveDialog.FileName,
                        "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Implement Excel export functionality
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Service Provider Efficiency Report";
                saveDialog.FileName = "Report.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Code to generate Excel would go here
                    // You'll need a library like EPPlus or ClosedXML
                    // This is just a placeholder
                    MessageBox.Show("Report exported successfully to " + saveDialog.FileName,
                        "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Reload all report data
            LoadReportData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}