using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TravelEaseForms
{

    //Class to capture the query output for tour requests
    public class ServiceCard
    {
        public string ServiceName { get; set; }
        public string OperatorName { get; set; }
        public decimal Rating { get; set; }
        public string OperatorUserID { get; set; }
        public string ServiceID { get; set; }

    }

    //Class to capture the query output for services
    public class ServiceInfo
    {
        public string ServiceID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Cost { get; set; }
    }

    //class to capture the query output for bookings
    public class BookingInfo
    {
        public int BookingID { get; set; }
        public DateTime BookingDate { get; set; }
        public int Quantity { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public string Status { get; set; } 
    }
    public class Hotel
    {
        public string HotelID { get; set; }
        public string HotelName { get; set; }
        public string Address { get; set; }
        public string ServiceID { get; set; }
    }

    public class AvailabilityInfo
    {
        public int TotalRooms { get; set; }
        public int BookedRooms { get; set; }
        public int AvailableRooms { get; set; }
    }

    public class Room
    {
        public string RoomID { get; set; }
        public int RoomNo { get; set; }
        public string Type { get; set; }
        public decimal Cost { get; set; }
    }

    public partial class SP_Main : Form
    {
        // navigation chrome (two parts in which the screen is divided
        private Panel panelSidebar;
        private Panel panelContent;

        // content panels (on our taskbar)
        private Panel panelApprove;
        private Panel panelServices;
        private Panel panelBookings;
        private Panel panelStats;

        // nav buttons
        private Button btnApproveTours;
        private Button btnManageServices;
        private Button btnManageBookings;
        private Button btnStatistics;

        string connectionString = "Data Source=DESKTOP-7HGU5BP\\SQLEXPRESS;Initial Catalog=TravelEaseDataBase;Integrated Security=True;";

        /*CONSTRUCTOR*/
        public SP_Main()
        {
            InitializeComponent();
            InitializeLayout();
            ApplyColorPalette();
        }

        //Queries for managing requests
        public List<ServiceCard> fetchOperatorRequests()
        {
            List<ServiceCard> services = new List<ServiceCard>();
            string currentProviderUserID = UserSession.CurrentUserID;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT u.Name AS OperatorName, s.Name AS ServiceName, op.Rating,
                          a.UserID AS OperatorUserID, a.ServiceID
                       FROM Assign AS a
                       JOIN TourOperators AS op ON a.UserID = op.UserID
                       JOIN Users AS u ON op.UserID = u.UserID
                       JOIN Services AS s ON a.ServiceID = s.ServiceID
                       WHERE s.UserID = @CurrentProviderUserID
                       AND (a.Approved = 0)"; 


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CurrentProviderUserID", currentProviderUserID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                services.Add(new ServiceCard
                                {
                                    ServiceName = reader["ServiceName"].ToString(),
                                    OperatorName = reader["OperatorName"].ToString(),
                                    Rating = reader["Rating"] != DBNull.Value ? Convert.ToDecimal(reader["Rating"]) : 0,
                                    OperatorUserID = reader["OperatorUserID"].ToString(),
                                    ServiceID = reader["ServiceID"].ToString()
                                });
                            }

                            if (services.Count == 0)
                            {
                                MessageBox.Show("No Requests to approve", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                return services;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return services;
        }
        private void ApproveService(string operatorUserID, string serviceID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 1. Update the Assign table to set Approved = true
                            string updateQuery = @"UPDATE Assign 
                                         SET Approved = 1 
                                         WHERE UserID = @OperatorID 
                                         AND ServiceID = @ServiceID";

                            using (SqlCommand command = new SqlCommand(updateQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@OperatorID", operatorUserID);
                                command.Parameters.AddWithValue("@ServiceID", serviceID);
                                command.ExecuteNonQuery();
                            }

                            //// 2. Get the TourID associated with this assignment
                            //string getTourQuery = @"SELECT TOP 1 t.TourID
                            //              FROM Tours t
                            //              JOIN TourOperators op ON t.UserID = op.UserID
                            //              WHERE op.UserID = @OperatorID";

                            //string tourID = null;
                            //using (SqlCommand command = new SqlCommand(getTourQuery, connection, transaction))
                            //{
                            //    command.Parameters.AddWithValue("@OperatorID", operatorUserID);
                            //    object result = command.ExecuteScalar();
                            //    if (result != null && result != DBNull.Value)
                            //    {
                            //        tourID = result.ToString();
                            //    }
                            //    else
                            //    {
                            //        throw new Exception("No Tour found for this operator");
                            //    }
                            //}

                            //// 3. Insert into Utilize table
                            //string insertQuery = @"INSERT INTO Utilize (TourID, ServiceID)
                            //             VALUES (@TourID, @ServiceID)";

                            //using (SqlCommand command = new SqlCommand(insertQuery, connection, transaction))
                            //{
                            //    command.Parameters.AddWithValue("@TourID", tourID);
                            //    command.Parameters.AddWithValue("@ServiceID", serviceID);
                            //    command.ExecuteNonQuery();
                            //}

                            // 4. Commit the transaction
                            transaction.Commit();
                            MessageBox.Show("Service has been approved and assigned to tour!",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction if any error occurs
                            transaction.Rollback();
                            throw new Exception("Transaction failed: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error approving service: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void RejectService(string operatorUserID, string serviceID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = @"DELETE FROM Assign 
                                 WHERE UserID = @OperatorID 
                                 AND ServiceID = @ServiceID";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@OperatorID", operatorUserID);
                        command.Parameters.AddWithValue("@ServiceID", serviceID);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Service request has been rejected.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to reject service.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error rejecting service: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Queries for viewing, adding, removing services
        private List<ServiceInfo> FetchServices()
        {
            List<ServiceInfo> services = new List<ServiceInfo>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT s.ServiceID, s.Name, s.Type, s.Cost 
                           FROM Services s
                           WHERE s.UserID = @CurrentUserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CurrentUserID", UserSession.CurrentUserID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                services.Add(new ServiceInfo
                                {
                                    ServiceID = reader["ServiceID"].ToString(),
                                    Name = reader["Name"].ToString(),
                                    Type = reader["Type"].ToString(),
                                    Cost = reader["Cost"] != DBNull.Value ? Convert.ToDecimal(reader["Cost"]) : 0
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching services: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return services;
        }
        private bool RemoveService(string serviceIdToRemove)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"DELETE FROM Services WHERE UserID = @currentUserID AND ServiceID = @serviceID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add both required parameters
                        command.Parameters.AddWithValue("@currentUserID", UserSession.CurrentUserID);
                        command.Parameters.AddWithValue("@serviceID", serviceIdToRemove);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing service: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void AddServiceToDatabase(string name, string type, decimal cost)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 1. Check whether this service already exists for the current user
                            string checkQuery = @"SELECT COUNT(*) FROM Services WHERE UserID = @userID AND Name = @name";

                            using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@userID", UserSession.CurrentUserID);
                                checkCmd.Parameters.AddWithValue("@name", name);
                                int existing = Convert.ToInt32(checkCmd.ExecuteScalar());

                                if (existing > 0)
                                {
                                    MessageBox.Show("A service with that name already exists.","Duplicate Service",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                                    transaction.Rollback();
                                    return;
                                }
                            }

                            // 2. Insert the new service
                            string insertQuery = @"INSERT INTO Services (Name, Type, Cost, UserID) VALUES (@name, @type, @cost, @userID);";
                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection, transaction))
                            {
                                insertCmd.Parameters.AddWithValue("@userID", UserSession.CurrentUserID);
                                insertCmd.Parameters.AddWithValue("@name", name);
                                insertCmd.Parameters.AddWithValue("@type", type);
                                insertCmd.Parameters.AddWithValue("@cost", cost);
                                insertCmd.ExecuteNonQuery();
                            }

                            
                            transaction.Commit();
                            MessageBox.Show("Service has been successfully added!","Success",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding service: {ex.Message}","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Queries for Bookings
        private List<BookingInfo> GetUserBookings()
        {
            List<BookingInfo> bookings = new List<BookingInfo>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT
                      b.BookingID,
                      b.BookingDate,
                      b.Quantity,
                      b.Status,
                      s.Name       AS ServiceName,
                      s.Type       AS ServiceType
                    FROM ServiceProviders AS sp
                      JOIN Services       AS s ON sp.UserID    = s.UserID
                      JOIN Bookings       AS b ON b.ServiceID  = s.ServiceID
                    WHERE sp.UserID = @currentUserID;
                    ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@currentUserID", UserSession.CurrentUserID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bookings.Add(new BookingInfo
                                {
                                    BookingID = reader.GetInt32(reader.GetOrdinal("BookingID")),
                                    BookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    ServiceName = reader.GetString(reader.GetOrdinal("ServiceName")),
                                    ServiceType = reader.GetString(reader.GetOrdinal("ServiceType")),
                                    Status = reader.GetString(reader.GetOrdinal("Status"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving bookings: {ex.Message}");
            }

            return bookings;
        }

        private List<BookingInfo> GetUnpaidServiceBookings()
        {
            var list = new List<BookingInfo>();
            const string sql = @"
                                SELECT
                                  b.BookingID,
                                  b.BookingDate,
                                  b.Quantity,
                                  s.Name         AS ServiceName,
                                  s.Type         AS ServiceType
                                FROM Services   AS s
                                INNER JOIN Bookings AS b
                                  ON s.ServiceID = b.ServiceID
                                LEFT JOIN Payments  AS p
                                  ON p.BookingID   = b.BookingPK
                                WHERE p.PaymentID IS NULL
                                  AND s.UserID     = @currentUserID;
                            ";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@currentUserID", UserSession.CurrentUserID);
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new BookingInfo
                        {
                            BookingID = Convert.ToInt32(rdr["BookingID"]),
                            BookingDate = (DateTime)rdr["BookingDate"],
                            Quantity = Convert.ToInt32(rdr["Quantity"]),
                            ServiceName = rdr["ServiceName"].ToString(),
                            ServiceType = rdr["ServiceType"].ToString(),
                            Status = "Pending"
                        });
                    }
                }
            }

            return list;
        }

        //bhai ye kia horaha hai

        // 1) Get all hotels managed by this provider
        private List<Hotel> GetHotelsForCurrentUser()
        {
            var list = new List<Hotel>();
            const string sql = @"
                SELECT DISTINCT
                  h.HotelID,
                  h.Name     AS HotelName,
                  h.Address,
                  h.ServiceID
                FROM ServiceProviders sp
                JOIN Services s  ON sp.UserID  = s.UserID
                JOIN Hotels h  ON s.ServiceID  = h.ServiceID
                WHERE sp.UserID = @currentUserID;
            ";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@currentUserID", UserSession.CurrentUserID);
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new Hotel
                        {
                            HotelID = rdr["HotelID"].ToString(),
                            HotelName = rdr["HotelName"].ToString(),
                            Address = rdr["Address"].ToString(),
                            ServiceID = rdr["ServiceID"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        // 2) Get availability counts for a hotel on a specific date
        private AvailabilityInfo GetRoomAvailability(string hotelId, DateTime date)
        {
            const string sql = @"
                    SELECT
                        COUNT(r.RoomID)                             AS TotalRooms,
                        SUM(CASE WHEN rr.RoomID IS NOT NULL THEN 1 ELSE 0 END) AS BookedRooms,
                        COUNT(r.RoomID)
                        - SUM(CASE WHEN rr.RoomID IS NOT NULL THEN 1 ELSE 0 END) AS AvailableRooms
                    FROM Hotels               AS h
                    JOIN Rooms                AS r  ON r.HotelID = h.HotelID
                    LEFT JOIN RoomReservations AS rr 
                        ON rr.RoomID     = r.RoomID
                        AND @date BETWEEN rr.ReservedFrom AND rr.ReservedTill
                    WHERE h.HotelID     = @hotelId
                    GROUP BY h.HotelID;
                ";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@hotelId", hotelId);
                cmd.Parameters.AddWithValue("@date", date.Date);
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        return new AvailabilityInfo
                        {
                            TotalRooms = Convert.ToInt32(rdr["TotalRooms"]),
                            BookedRooms = Convert.ToInt32(rdr["BookedRooms"]),
                            AvailableRooms = Convert.ToInt32(rdr["AvailableRooms"])
                        };
                    }
                }
            }
            return new AvailabilityInfo(); 
        }

        // 3) Get list of free rooms for that date
        private List<Room> GetAvailableRooms(string hotelId, DateTime date)
        {
            var list = new List<Room>();
            const string sql = @"
                SELECT
                  r.RoomID,
                  r.RoomNo,
                  r.Type,
                  r.Cost
                FROM Rooms                AS r
                LEFT JOIN RoomReservations AS rr
                  ON rr.RoomID     = r.RoomID
                 AND @date BETWEEN rr.ReservedFrom AND rr.ReservedTill
                WHERE r.HotelID     = @hotelId
                  AND rr.RoomID     IS NULL;
            ";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@hotelId", hotelId);
                cmd.Parameters.AddWithValue("@date", date.Date);
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new Room
                        {
                            RoomID = rdr["RoomID"].ToString(),
                            RoomNo = Convert.ToInt32(rdr["RoomNo"]),
                            Type = rdr["Type"].ToString(),
                            Cost = (decimal)rdr["Cost"]
                        });
                    }
                }
            }
            return list;
        }

        // 4) Book a room (insert a reservation)
        private void ReserveRoomWithTransaction(string hotelId,string serviceId,string roomId,DateTime from, DateTime to)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        // 1) Fetch the latest booking for this hotel-service
                        const string getBookingSql = @"
                            SELECT TOP 1
                              b.BookingPK
                            FROM Bookings AS b
                            JOIN Services AS s ON b.ServiceID = s.ServiceID
                            JOIN Hotels   AS h ON s.ServiceID = h.ServiceID
                            WHERE s.ServiceID = @serviceId
                              AND h.HotelID   = @hotelId
                            ORDER BY b.BookingDate DESC;
                        ";
                        string bookingId;
                        using (var cmd = new SqlCommand(getBookingSql, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@serviceId", serviceId);
                            cmd.Parameters.AddWithValue("@hotelId", hotelId);
                            var result = cmd.ExecuteScalar();
                            if (result == null || result is DBNull)
                            {
                                MessageBox.Show(
                                    "No existing booking found for this service.",
                                    "Cannot Reserve Room",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                                tx.Rollback();
                                return;
                            }
                            bookingId = result.ToString();
                        }

                        // 2) Insert the room reservation
                        const string insertSql = @"
                            INSERT INTO RoomReservations
                              (BookingID, RoomID, ReservedFrom, ReservedTill)
                            VALUES
                              (@bookingId, @roomId, @from, @to);
                        ";
                        using (var cmd = new SqlCommand(insertSql, conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@bookingId", bookingId);
                            cmd.Parameters.AddWithValue("@roomId", roomId);
                            cmd.Parameters.AddWithValue("@from", from.Date);
                            cmd.Parameters.AddWithValue("@to", to.Date);
                            cmd.ExecuteNonQuery();
                        }

                        // 3) Commit both steps together
                        tx.Commit();
                        MessageBox.Show(
                            $"Room {roomId} reserved under booking {bookingId}.",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Something went wrong: roll everything back
                        tx.Rollback();
                        MessageBox.Show(
                            $"Error reserving room: {ex.Message}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }




        /*HELPERS*/
        private void ShowPage(Panel p, Action init)
        {
            panelApprove.Visible =
            panelServices.Visible =
            panelBookings.Visible =
            panelStats.Visible = false;

            p.Visible = true;
            init();
        }

        private Button CreateSidebarButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Montserrat", 10, FontStyle.Regular),
                BackColor = ColorTranslator.FromHtml("#FDFAF6"),
                ForeColor = ColorTranslator.FromHtml("#333333")
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(btn.BackColor, 0.1f);
            btn.Click += SidebarButton_Click;
            return btn;
        }

        private void SidebarButton_Click(object sender, EventArgs e)
        {
            // hide all
            panelApprove.Visible =
            panelServices.Visible =
            panelBookings.Visible =
            panelStats.Visible = false;

            // show & init chosen
            switch (((Button)sender).Text)
            {
                case "Approve Tours":
                    panelApprove.Visible = true;
                    InitializeApproveToursView();
                    break;

                case "Manage Services":
                    panelServices.Visible = true;
                    InitializeManageServicesView();
                    break;

                case "Manage Bookings":
                    panelBookings.Visible = true;
                    InitializeManageBookingsView();
                    break;

                case "Statistics":
                    panelStats.Visible = true;
                    InitializeStatisticsView();
                    break;
            }
        }

        // Helper method to create a standard booking card
        private Panel CreateBookingCard(FlowLayoutPanel flp, BookingInfo booking)
        {
            var card = new Panel
            {
                Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                Height = 120, // Slightly taller to accommodate status
                Margin = new Padding(0, 0, 0, 10),
                BackColor = ColorTranslator.FromHtml("#FAF1E6"), // beige
                Padding = new Padding(10)
            };

            // Booking ID and Status
            var lblName = new Label
            {
                Text = $"Booking ID: {booking.BookingID} - {booking.Status}",
                Font = new Font("Montserrat", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            card.Controls.Add(lblName);

            // Service info
            var lblService = new Label
            {
                Text = $"Service: {booking.ServiceName} ({booking.ServiceType})",
                Font = new Font("Montserrat", 9),
                AutoSize = true,
                Location = new Point(10, 35)
            };
            card.Controls.Add(lblService);

            // Date info
            var lblDate = new Label
            {
                Text = $"Date: {booking.BookingDate:MM/dd/yyyy}",
                Font = new Font("Montserrat", 9),
                AutoSize = true,
                Location = new Point(10, 55)
            };
            card.Controls.Add(lblDate);

            // Quantity info
            var lblQuantity = new Label
            {
                Text = $"Quantity: {booking.Quantity}",
                Font = new Font("Montserrat", 9),
                AutoSize = true,
                Location = new Point(10, 75)
            };
            card.Controls.Add(lblQuantity);

            return card;
        }

        // Helper method to add an info message card
        private void AddInfoCard(FlowLayoutPanel flp, string message)
        {
            var infoCard = new Panel
            {
                Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                Height = 60,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = ColorTranslator.FromHtml("#E2E3E5"), // light gray
                Padding = new Padding(10)
            };

            var lblInfo = new Label
            {
                Text = message,
                Font = new Font("Montserrat", 10),
                ForeColor = ColorTranslator.FromHtml("#383D41"),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            infoCard.Controls.Add(lblInfo);
            flp.Controls.Add(infoCard);
        }
        private Label CreateBoldLabel(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Montserrat", 9, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
        }

        private void ApplyColorPalette()
        {
            var offWhite = ColorTranslator.FromHtml("#FDFAF6");
            var mint = ColorTranslator.FromHtml("#E4EFE7");
            var sage = ColorTranslator.FromHtml("#99BC85");
            var textCol = ColorTranslator.FromHtml("#333333");

            BackColor = offWhite;
            panelSidebar.BackColor = sage;
            panelContent.BackColor = mint;

            // restyle nav buttons
            foreach (var btn in panelSidebar.Controls.OfType<Button>())
            {
                btn.BackColor = offWhite;
                btn.ForeColor = textCol;
                btn.FlatAppearance.BorderSize = 0;
            }
        }

        /*LAYOUT AND VIEW INITIALIZATION*/
        private void InitializeLayout()
        {


            // 2) Content‐host
            panelContent = new Panel { Dock = DockStyle.Fill, BackColor = ColorTranslator.FromHtml("#E4EFE7") };
            Controls.Add(panelContent);

            // 1) Sidebar
            panelSidebar = new Panel { Dock = DockStyle.Left, Width = 200, BackColor = ColorTranslator.FromHtml("#99BC85") };
            Controls.Add(panelSidebar);

            // 3) Create the four content panels
            panelApprove = new Panel { Dock = DockStyle.Fill, Visible = false };
            panelServices = new Panel { Dock = DockStyle.Fill, Visible = false };
            panelBookings = new Panel { Dock = DockStyle.Fill, Visible = false };
            panelStats = new Panel { Dock = DockStyle.Fill, Visible = false };
            panelContent.Controls.AddRange(new[] { panelApprove, panelServices, panelBookings, panelStats });

            // 4) Instantiate each nav button into its field
            btnApproveTours = CreateSidebarButton("Tour Provider Requests");
            btnManageServices = CreateSidebarButton("Manage Services");
            btnManageBookings = CreateSidebarButton("Manage Bookings");
            btnStatistics = CreateSidebarButton("Statistics");

            // 5) Add them (in reverse order so “Approve Tours” sits at the top)
            panelSidebar.Controls.AddRange(new Control[]
            {
                btnStatistics,
                btnManageBookings,
                btnManageServices,
                btnApproveTours
            });

            // 6) Wire their clicks
            btnApproveTours.Click += (s, e) => ShowPage(panelApprove, InitializeApproveToursView);
            btnManageServices.Click += (s, e) => ShowPage(panelServices, InitializeManageServicesView);
            btnManageBookings.Click += (s, e) => ShowPage(panelBookings, InitializeManageBookingsView);
            btnStatistics.Click += (s, e) => ShowPage(panelStats, InitializeStatisticsView);
        }

        // ─── Feature 1 ────────────────────────────────────────────────────────────────

        private void InitializeApproveToursView()
        {
            // 1) Clear out any previous content
            panelApprove.Controls.Clear();

            // 2) Creating a flow layout panel
            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10),
                BackColor = panelApprove.BackColor
            };
            panelApprove.Controls.Add(flp);

            List<ServiceCard> services = fetchOperatorRequests();

            // 4) Build one “card” (row) per tour
            foreach (var service in services)
            {
                var row = new Panel
                {
                    Height = 125, 
                    Margin = new Padding(0, 0, 0, 10),
                    BackColor = ColorTranslator.FromHtml("#FAF1E6")  // beige
                };
                row.Width = flp.ClientSize.Width - flp.Padding.Horizontal;

                row.Tag = service;

                // Service Name as heading
                var lblServiceName = new Label
                {
                    Text = service.ServiceName,
                    AutoSize = true,
                    Font = new Font("Montserrat", 14, FontStyle.Bold),
                    ForeColor = ColorTranslator.FromHtml("#333333"),
                    Location = new Point(10, 10)
                };
                row.Controls.Add(lblServiceName);

                // Operator Name
                var lblOperator = new Label
                {
                    Text = $"Tour Operator: {service.OperatorName}",
                    AutoSize = true,
                    Font = new Font("Montserrat", 10, FontStyle.Regular),
                    ForeColor = ColorTranslator.FromHtml("#666666"),
                    Location = new Point(10, 40)
                };
                row.Controls.Add(lblOperator);

                // Rating
                var lblRating = new Label
                {
                    Text = $"Operator Rating: {service.Rating:F1}/5.0",
                    AutoSize = true,
                    Font = new Font("Montserrat", 10, FontStyle.Regular),
                    ForeColor = ColorTranslator.FromHtml("#666666"),
                    Location = new Point(10, 65)
                };
                row.Controls.Add(lblRating);

                // Button panel (keep existing button code)
                var btnPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Right,
                    Width = 180,  // Adjust as needed
                    FlowDirection = FlowDirection.RightToLeft,
                    Padding = new Padding(0, (row.Height - 30) / 2, 10, 0),
                    BackColor = Color.Transparent,
                    AutoSize = false,
                    AutoSizeMode = AutoSizeMode.GrowOnly
                };

                // Reject button
                var btnReject = new Button
                {
                    Text = "Reject",
                    Size = new Size(80, 30),
                    BackColor = ColorTranslator.FromHtml("#D22B2B"),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnReject.FlatAppearance.BorderSize = 0;
                btnReject.Click += (s, e) => {
                    var svc = (ServiceCard)row.Tag;
                    RejectService(svc.OperatorUserID, svc.ServiceID);
                    flp.Controls.Remove(row);

                    // Check if this was the last card
                    if (flp.Controls.Count == 0)
                    {
                        // Add "No requests" message (same code as above)
                        Panel noRequestsPanel = new Panel
                        {
                            Height = 80,
                            Margin = new Padding(0, 0, 0, 10),
                            BackColor = ColorTranslator.FromHtml("#FAF1E6"),
                            Width = flp.ClientSize.Width - flp.Padding.Horizontal
                        };

                        Label noRequestsLabel = new Label
                        {
                            Text = "No requests to approve",
                            AutoSize = true,
                            Font = new Font("Montserrat", 12, FontStyle.Bold),
                            ForeColor = ColorTranslator.FromHtml("#666666"),
                            Location = new Point(10, 30)
                        };

                        noRequestsPanel.Controls.Add(noRequestsLabel);
                        flp.Controls.Add(noRequestsPanel);
                    }
                };

                // Accept button
                var btnAccept = new Button
                {
                    Text = "Accept",
                    Size = new Size(80, 30),
                    BackColor = ColorTranslator.FromHtml("#99BC85"),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnAccept.FlatAppearance.BorderSize = 0;
                btnAccept.Click += (s, e) => {
                    var svc = (ServiceCard)row.Tag;
                    ApproveService(svc.OperatorUserID, svc.ServiceID);
                    flp.Controls.Remove(row);

                    // Check if this was the last card
                    if (flp.Controls.Count == 0)
                    {
                        // Add "No requests" message
                        Panel noRequestsPanel = new Panel
                        {
                            Height = 80,
                            Margin = new Padding(0, 0, 0, 10),
                            BackColor = ColorTranslator.FromHtml("#FAF1E6"),
                            Width = flp.ClientSize.Width - flp.Padding.Horizontal
                        };

                        Label noRequestsLabel = new Label
                        {
                            Text = "No requests to approve",
                            AutoSize = true,
                            Font = new Font("Montserrat", 12, FontStyle.Bold),
                            ForeColor = ColorTranslator.FromHtml("#666666"),
                            Location = new Point(10, 30)
                        };

                        noRequestsPanel.Controls.Add(noRequestsLabel);
                        flp.Controls.Add(noRequestsPanel);
                    }
                };

                btnPanel.Controls.Add(btnReject);
                btnPanel.Controls.Add(btnAccept);
                row.Controls.Add(btnPanel);

                flp.Controls.Add(row);
            }

            // Make sure each card spans the full width whenever flp is resized
            flp.Resize += (s, e) =>
            {
                foreach (Panel row in flp.Controls.OfType<Panel>())
                    row.Width = flp.ClientSize.Width - flp.Padding.Horizontal;
            };
            flp.PerformLayout();
        }

        // ─── Feature 2 ────────────────────────────────────────────────────────────────

        private void InitializeManageServicesView()
        {
            panelServices.Controls.Clear();

            // 2) Create a panel to hold both the combo box and the flow layout panel
            var containerPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            panelServices.Controls.Add(containerPanel);

            // 3) The ComboBox menu
            var cmb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Montserrat", 10),
                Dock = DockStyle.Top,
                Height = 35
            };

            cmb.Items.AddRange(new[] {
                "View all current services",
                "Add a service",
                "Remove a service"
            });

           //make flow layout panel to hold cards
            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10),
                BackColor = panelServices.BackColor
            };

            // 5) Add the ComboBox first, then the FlowLayoutPanel to maintain the correct docking order
            containerPanel.Controls.Add(flp);
            containerPanel.Controls.Add(cmb);

            // 6) Wire up the selection-changed **before** setting SelectedIndex
            cmb.SelectedIndexChanged += (s, e) =>
            {
                flp.Controls.Clear();
                var choice = cmb.SelectedItem as string;

                List<ServiceInfo> services;
                // --- VIEW ALL ---
                if (choice == "View all current services")
                {
                    //fetch from db
                    services = FetchServices();

                    if (services.Count == 0)
                    {
                        // No services found
                        var noServicePanel = new Panel
                        {
                            Height = 80,
                            Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                            Margin = new Padding(0, 0, 0, 10),
                            BackColor = ColorTranslator.FromHtml("#FAF1E6")
                        };

                        var noServiceLabel = new Label
                        {
                            Text = "No services found",
                            AutoSize = true,
                            Font = new Font("Montserrat", 11, FontStyle.Bold),
                            ForeColor = ColorTranslator.FromHtml("#666666"),
                            Location = new Point(10, 30)
                        };

                        noServicePanel.Controls.Add(noServiceLabel);
                        flp.Controls.Add(noServicePanel);
                    }
                    else
                    {
                        foreach (var svc in services)
                        {
                         
                            var card = new Panel
                            {
                                Height = 100, 
                                Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                                Margin = new Padding(0, 0, 0, 10),
                                BackColor = ColorTranslator.FromHtml("#FAF1E6")
                            };

                            // Service name
                            var lblName = new Label
                            {
                                Text = $"{svc.Name} ({svc.ServiceID})",
                                AutoSize = true,
                                Font = new Font("Montserrat", 11, FontStyle.Bold),
                                ForeColor = ColorTranslator.FromHtml("#333333"),
                                Location = new Point(10, 10)
                            };
                            card.Controls.Add(lblName);

                            // Type 
                            var lblType = new Label
                            {
                                Text = $"Type: {svc.Type}",
                                AutoSize = true,
                                Font = new Font("Montserrat", 9),
                                ForeColor = ColorTranslator.FromHtml("#555555"),
                                Location = new Point(10, 35) 
                            };
                            card.Controls.Add(lblType);

                            // Cost 
                            var lblCost = new Label
                            {
                                Text = $"Cost: ${svc.Cost:F2}",
                                AutoSize = true,
                                Font = new Font("Montserrat", 9),
                                ForeColor = ColorTranslator.FromHtml("#555555"),
                                Location = new Point(10, 60) 
                            };
                            card.Controls.Add(lblCost);

                            flp.Controls.Add(card);
                        }
                    }
                }

                // --- REMOVE ---
                else if (choice == "Remove a service")
                {
                    services = FetchServices();

                    if (services.Count == 0)
                    {
                        // No services found
                        var noServicePanel = new Panel
                        {
                            Height = 80,
                            Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                            Margin = new Padding(0, 0, 0, 10),
                            BackColor = ColorTranslator.FromHtml("#FAF1E6")
                        };

                        var noServiceLabel = new Label
                        {
                            Text = "No services found",
                            AutoSize = true,
                            Font = new Font("Montserrat", 11, FontStyle.Bold),
                            ForeColor = ColorTranslator.FromHtml("#666666"),
                            Location = new Point(10, 30)
                        };

                        noServicePanel.Controls.Add(noServiceLabel);
                        flp.Controls.Add(noServicePanel);
                    }
                    else
                    {
                        foreach (var svc in services)
                        {
                            var card = new Panel
                            {
                                Height = 80,
                                Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                                Margin = new Padding(0, 0, 0, 10),
                                BackColor = ColorTranslator.FromHtml("#FAF1E6")
                            };
                            var lbl = new Label
                            {
                                Text = $"{svc.Name} ({svc.ServiceID})",
                                AutoSize = true,
                                Font = new Font("Montserrat", 11, FontStyle.Bold),
                                Location = new Point(10, 10)
                            };
                            card.Controls.Add(lbl);

                            var btn = new Button
                            {
                                Text = "Remove",
                                Size = new Size(80, 30),
                                BackColor = ColorTranslator.FromHtml("#D22B2B"),
                                ForeColor = Color.White,
                                FlatStyle = FlatStyle.Flat
                            };
                            btn.FlatAppearance.BorderSize = 0;

                            // Store service ID for removal
                            string serviceIdToRemove = svc.ServiceID;

                            btn.Click += (_, __) =>
                            {
                                DialogResult result = MessageBox.Show($"Are you sure you want to remove this service?",
                                                                      "Confirm Removal",
                                                                      MessageBoxButtons.YesNo,
                                                                      MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    bool success = RemoveService(serviceIdToRemove);

                                    if (success)
                                    {
                                        MessageBox.Show($"Service removed successfully!");

                                        cmb.SelectedIndex = -1;
                                        cmb.SelectedIndex = cmb.Items.IndexOf("Remove a service");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to remove service. Please try again.");
                                    }
                                }
                            };

                            card.Controls.Add(btn);
                            btn.Location = new Point(
                                card.ClientSize.Width - btn.Width - 20,
                                (card.ClientSize.Height - btn.Height) / 2
                            );

                            flp.Controls.Add(card);
                        }
                    }
                   
                }
                // --- ADD ---
                else if (choice == "Add a service")
                {
                    var form = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 150,
                        Padding = new Padding(20),
                        BackColor = ColorTranslator.FromHtml("#FAF1E6"),
                        Margin = new Padding(0, 0, 0, 10)
                    };

                    var lblN = new Label
                    {
                        Text = "Service Name:",
                        Font = new Font("Montserrat", 9),
                        Location = new Point(0, 5),
                        AutoSize = true
                    };
                    var txtN = new TextBox
                    {
                        Font = new Font("Montserrat", 9),
                        Location = new Point(120, 0),
                        Width = 200
                    };
                    form.Controls.AddRange(new Control[] { lblN, txtN });

                    var lblT = new Label
                    {
                        Text = "Service Type:",
                        Font = new Font("Montserrat", 9),
                        Location = new Point(0, 45),
                        AutoSize = true
                    };
                    var txtT = new TextBox
                    {
                        Font = new Font("Montserrat", 9),
                        Location = new Point(120, 36),
                        Width = 200
                    };
                    form.Controls.AddRange(new Control[] { lblT, txtT });

                    var lblC = new Label
                    {
                        Text = "Cost:",
                        Font = new Font("Montserrat", 9),
                        Location = new Point(0, 85),
                        AutoSize = true
                    };
                    var txtC = new TextBox
                    {
                        Font = new Font("Montserrat", 9),
                        Location = new Point(120, 72),
                        Width = 100
                    };
                    form.Controls.AddRange(new Control[] { lblC, txtC });

                    var btnA = new Button
                    {
                        Text = "Add Service",
                        Size = new Size(120, 35),
                        BackColor = ColorTranslator.FromHtml("#99BC85"),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Location = new Point(120, 104)
                    };
                    btnA.FlatAppearance.BorderSize = 0;
                    btnA.Click += (_, __) =>
                    {
                        if (string.IsNullOrWhiteSpace(txtN.Text))
                        {
                            MessageBox.Show("Please enter a service name");
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(txtT.Text))
                        {
                            MessageBox.Show("Please enter a service type");
                            return;
                        }

                        if (!decimal.TryParse(txtC.Text, out decimal cost))
                        {
                            MessageBox.Show("Please enter a valid cost");
                            return;
                        }

                        AddServiceToDatabase(txtN.Text, txtT.Text, cost);
                    };
                    form.Controls.Add(btnA);

                    flp.Controls.Add(form);
                }
            };

            // 7) Make sure cards resize when flow panel resizes
            flp.Resize += (s, e) =>
            {
                foreach (Control ctl in flp.Controls)
                {
                    ctl.Width = flp.ClientSize.Width - flp.Padding.Horizontal;
                }
            };

            // 8) Trigger the initial load into "View all"
            cmb.SelectedIndex = 0;
        }

        // ─── Feature 3 ────────────────────────────────────────────────────────────────
        private void InitializeManageBookingsView()
        {
            panelBookings.Controls.Clear();

            // 2) Create a container panel
            var containerPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            panelBookings.Controls.Add(containerPanel);

            // 3) Create the dropdown menu
            var cmb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Montserrat", 10),
                Dock = DockStyle.Top,
                Height = 35
            };

            cmb.Items.AddRange(new[] {
                "View all bookings",
                "Track payments",
                "Update availability"
            });

            // 4) Create the FlowLayoutPanel for cards
            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10),
                BackColor = panelBookings.BackColor
            };

            // 5) Add panels to container in correct order
            containerPanel.Controls.Add(flp);
            containerPanel.Controls.Add(cmb);

            // 6) Wire up the dropdown menu selection change event
            cmb.SelectedIndexChanged += (s, e) =>
            {
                flp.Controls.Clear();
                var choice = cmb.SelectedItem as string;

                //---VIEW ALL BOOKINGS ---
                if (choice == "View all bookings")
                {
                    List<BookingInfo> bookings = GetUserBookings();

                    if (bookings.Count == 0)
                    {
                        // No Bookings found
                        var noServicePanel = new Panel
                        {
                            Height = 80,
                            Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                            Margin = new Padding(0, 0, 0, 10),
                            BackColor = ColorTranslator.FromHtml("#FAF1E6")
                        };

                        var noServiceLabel = new Label
                        {
                            Text = "No services found",
                            AutoSize = true,
                            Font = new Font("Montserrat", 11, FontStyle.Bold),
                            ForeColor = ColorTranslator.FromHtml("#666666"),
                            Location = new Point(10, 30)
                        };

                        noServicePanel.Controls.Add(noServiceLabel);
                        flp.Controls.Add(noServicePanel);
                    }
                    else
                    {
                        foreach (var booking in bookings)
                        {
                            var card = CreateBookingCard(flp, booking);
                            flp.Controls.Add(card);

                        }
                    }
                }

                // --- TRACK PAYMENTS ---//
                else if (choice == "Track payments")
                {
                    // fetch unpaid bookings
                    var bookings = GetUnpaidServiceBookings();

                    // summary card (all of these are pending)
                    decimal paidAmount = 0m;
                    int paidCount = 0;
                    int pendingCount = bookings.Count;

                    var summaryCard = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 100,
                        Margin = new Padding(0, 0, 0, 15),
                        BackColor = ColorTranslator.FromHtml("#E4EFE7"),
                        Padding = new Padding(15)
                    };
                    summaryCard.Controls.Add(new Label
                    {
                        Text = "Payment Summary",
                        Font = new Font("Montserrat", 12, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(10, 10)
                    });
                    summaryCard.Controls.Add(new Label
                    {
                        //Text = $"Total: ${totalAmount:F2} • Paid: ${paidAmount:F2} ({paidCount}) • " +
                        //           $"Pending: ${pendingAmount:F2} ({pendingCount})",
                        Text = $" • Paid: ${paidAmount:F2} ({paidCount}) •",
                        Font = new Font("Montserrat", 10),
                        AutoSize = true,
                        Location = new Point(10, 40)
                    });
                    summaryCard.Controls.Add(new Label
                    {
                        Text = "Click on a booking to mark it paid",
                        Font = new Font("Montserrat", 9, FontStyle.Italic),
                        ForeColor = ColorTranslator.FromHtml("#666666"),
                        AutoSize = true,
                        Location = new Point(10, 70)
                    });
                    flp.Controls.Add(summaryCard);

                    // one card per pending booking
                    foreach (var booking in bookings)
                    {
                        var card = CreateBookingCard(flp, booking);

                        // status badge
                        var statusPanel = new Panel
                        {
                            Size = new Size(80, 25),
                            Location = new Point(card.Width - 100, 10),
                            BackColor = ColorTranslator.FromHtml("#F8D7DA")
                        };
                        var statusLabel = new Label
                        {
                            Text = booking.Status,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Dock = DockStyle.Fill,
                            Font = new Font("Montserrat", 9, FontStyle.Bold),
                            ForeColor = ColorTranslator.FromHtml("#721C24")
                        };
                        statusPanel.Controls.Add(statusLabel);
                        card.Controls.Add(statusPanel);

                        // “Mark as Paid” button
                        //var btnPayment = new Button
                        //{
                        //    Text = "Mark as Paid",
                        //    Size = new Size(120, 30),
                        //    BackColor = ColorTranslator.FromHtml("#99BC85"),
                        //    ForeColor = Color.White,
                        //    FlatStyle = FlatStyle.Flat,
                        //    Location = new Point(card.Width - 140, card.Height - 40)
                        //};
                        //btnPayment.FlatAppearance.BorderSize = 0;
                        //btnPayment.Click += (_, __) =>
                        //{
                        //    MessageBox.Show($"Payment for booking {booking.BookingID} has been recorded!",
                        //                    "Paid", MessageBoxButtons.OK, MessageBoxIcon.Information);

                         
                        //};
                        //card.Controls.Add(btnPayment);

                        flp.Controls.Add(card);
                    }
                }

                //    // --- UPDATE AVAILABILITY ---
                else if (choice == "Update availability")
                {
                    flp.Controls.Clear();

                    // 1) Hotel selector
                    var hotels = GetHotelsForCurrentUser();
                    var cmbHotel = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
                    cmbHotel.Items.AddRange(hotels.ToArray());
                    cmbHotel.DisplayMember = "HotelName";
                    cmbHotel.ValueMember = "HotelID";
                    flp.Controls.Add(cmbHotel);

                    // 2) Date picker
                    var dtp = new DateTimePicker { Format = DateTimePickerFormat.Short };
                    flp.Controls.Add(dtp);

                    // 3) Availability labels
                    var lblAvail = new Label { Location = new Point(0, 60), AutoSize = true };
                    flp.Controls.Add(lblAvail);

                    // 4) Room list
                    var lstRooms = new ListBox { Location = new Point(0, 90), Width = 300, Height = 150 };
                    flp.Controls.Add(lstRooms);

                    // 5) Book button
                    var btnBook = new Button
                    {
                        Text = "Book Selected Room",
                        Location = new Point(0, 250),
                        Enabled = false
                    };
                    flp.Controls.Add(btnBook);

                    // When hotel or date changes, refresh availability & room list
                    void refresh()
                    {
                        if (cmbHotel.SelectedItem is Hotel sel && dtp.Value != null)
                        {
                            var info = GetRoomAvailability(sel.HotelID, dtp.Value);
                            lblAvail.Text = $"Total {info.TotalRooms}, Booked {info.BookedRooms}, Free {info.AvailableRooms}";

                            var freeRooms = GetAvailableRooms(sel.HotelID, dtp.Value);
                            lstRooms.DataSource = freeRooms;
                            lstRooms.DisplayMember = "RoomNo";
                            lstRooms.ValueMember = "RoomID";
                            btnBook.Enabled = freeRooms.Any();
                        }
                    }

                    cmbHotel.SelectedIndexChanged += (_, __) => refresh();
                    dtp.ValueChanged += (_, __) => refresh();

                    // Handle booking
                    btnBook.Click += (_, __) =>
                    {
                        if (cmbHotel.SelectedItem is Hotel sel &&
                            lstRooms.SelectedItem is Room room)
                        {
                            ReserveRoomWithTransaction(
                                hotelId: sel.HotelID,
                                serviceId: sel.ServiceID,
                                roomId: room.RoomID,
                                from: dtp.Value,
                                to: dtp.Value
                            );
                            // then refresh your UI:
                            refresh();
                        }
                    };

                }
            };

            // 7) Make sure cards resize when flow panel resizes
            flp.Resize += (s, e) =>
            {
                foreach (Control ctl in flp.Controls)
                {
                    if (ctl is Panel panel)
                    {
                        panel.Width = flp.ClientSize.Width - flp.Padding.Horizontal;
                    }
                }
            };

            // 8) Set initial view
            cmb.SelectedIndex = 0;
        }

      
        // ─── Feature 4 ────────────────────────────────────────────────────────────────
        private void InitializeStatisticsView()
        {
            panelStats.Controls.Clear();

            // Use the same color scheme from your existing code
            var offWhite = ColorTranslator.FromHtml("#FDFAF6");
            var mint = ColorTranslator.FromHtml("#E4EFE7");
            var sage = ColorTranslator.FromHtml("#99BC85");
            var beige = ColorTranslator.FromHtml("#FAF1E6");
            var textCol = ColorTranslator.FromHtml("#333333");

            // Create a dropdown similar to your other views
            var cmb = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Montserrat", 10),
                Dock = DockStyle.Top,
                Height = 35
            };

            cmb.Items.AddRange(new[] {
        "Occupancy Rates",
        "Traveler Feedback",
        "Revenue Analysis"
    });

            // Create the FlowLayoutPanel for content (matches your pattern in other views)
            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10),
                BackColor = panelStats.BackColor
            };

            // Add panels to container in correct order (same as your other implementations)
            panelStats.Controls.Add(flp);
            panelStats.Controls.Add(cmb);

            // Wire up the dropdown menu change event (following your pattern)
            cmb.SelectedIndexChanged += (s, e) =>
            {
                flp.Controls.Clear();
                var choice = cmb.SelectedItem as string;

                // --- OCCUPANCY RATES VIEW ---
                if (choice == "Occupancy Rates")
                {
                    // Title card
                    var titleCard = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 60,
                        Margin = new Padding(0, 0, 0, 15),
                        BackColor = sage,
                        Padding = new Padding(15)
                    };

                    var lblTitle = new Label
                    {
                        Text = "Occupancy Rates Report",
                        Font = new Font("Montserrat", 12, FontStyle.Bold),
                        ForeColor = offWhite,
                        AutoSize = true,
                        Location = new Point(10, 15)
                    };
                    titleCard.Controls.Add(lblTitle);
                    flp.Controls.Add(titleCard);

                    // Monthly data
                    var ratesCard = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 280,
                        Margin = new Padding(0, 0, 0, 15),
                        BackColor = beige,
                        Padding = new Padding(15)
                    };

                    // Create and populate rate data
                    var tblRates = new TableLayoutPanel
                    {
                        ColumnCount = 4,
                        RowCount = 6,
                        Dock = DockStyle.Fill,
                        CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                        BackColor = offWhite
                    };

                    // Add headers
                    tblRates.Controls.Add(CreateBoldLabel("Month"), 0, 0);
                    tblRates.Controls.Add(CreateBoldLabel("Hotel (%)"), 1, 0);
                    tblRates.Controls.Add(CreateBoldLabel("Tours (%)"), 2, 0);
                    tblRates.Controls.Add(CreateBoldLabel("Transport (%)"), 3, 0);

                    // Set column widths
                    tblRates.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                    tblRates.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                    tblRates.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                    tblRates.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

                    // Add data rows
                    var months = new[] { "January", "February", "March", "April", "May" };
                    var hotelData = new[] { 75, 80, 85, 90, 95 };
                    var tourData = new[] { 60, 65, 75, 80, 85 };
                    var transportData = new[] { 55, 60, 70, 75, 80 };

                    for (int i = 0; i < 5; i++)
                    {
                        tblRates.Controls.Add(new Label { Text = months[i], TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(5) }, 0, i + 1);
                        tblRates.Controls.Add(new Label { Text = hotelData[i].ToString(), TextAlign = ContentAlignment.MiddleCenter }, 1, i + 1);
                        tblRates.Controls.Add(new Label { Text = tourData[i].ToString(), TextAlign = ContentAlignment.MiddleCenter }, 2, i + 1);
                        tblRates.Controls.Add(new Label { Text = transportData[i].ToString(), TextAlign = ContentAlignment.MiddleCenter }, 3, i + 1);
                    }

                    ratesCard.Controls.Add(tblRates);
                    flp.Controls.Add(ratesCard);

                    // Summary card
                    var summaryCard = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 100,
                        Margin = new Padding(0, 0, 0, 15),
                        BackColor = mint,
                        Padding = new Padding(15)
                    };

                    var lblSummary = new Label
                    {
                        Text = "Occupancy Summary",
                        Font = new Font("Montserrat", 11, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(10, 10)
                    };

                    var lblDetails = new Label
                    {
                        Text = "• Average Hotel Occupancy: 85%\n• Average Tour Capacity: 73%\n• Average Transport Utilization: 68%",
                        Font = new Font("Montserrat", 9),
                        AutoSize = true,
                        Location = new Point(10, 35)
                    };

                    summaryCard.Controls.AddRange(new Control[] { lblSummary, lblDetails });
                    flp.Controls.Add(summaryCard);
                }

                // --- TRAVELER FEEDBACK VIEW ---
                else if (choice == "Traveler Feedback")
                {
                    // Title card
                    var titleCard = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 60,
                        Margin = new Padding(0, 0, 0, 15),
                        BackColor = sage,
                        Padding = new Padding(15)
                    };

                    var lblTitle = new Label
                    {
                        Text = "Traveler Feedback Analysis",
                        Font = new Font("Montserrat", 12, FontStyle.Bold),
                        ForeColor = offWhite,
                        AutoSize = true,
                        Location = new Point(10, 15)
                    };
                    titleCard.Controls.Add(lblTitle);
                    flp.Controls.Add(titleCard);

                    // Ratings Card
                    var ratingsCard = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 230,
                        Margin = new Padding(0, 0, 0, 15),
                        BackColor = beige,
                        Padding = new Padding(15)
                    };

                    var lblRatingsTitle = new Label
                    {
                        Text = "Service Ratings (out of 5)",
                        Font = new Font("Montserrat", 11, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(10, 10)
                    };
                    ratingsCard.Controls.Add(lblRatingsTitle);

                    // Rating bars
                    var categories = new[] { "Customer Service", "Tour Guides", "Hotel Quality", "Transport", "Value for Money" };
                    var ratings = new[] { 4.7, 4.5, 4.3, 4.0, 4.2 };

                    for (int i = 0; i < categories.Length; i++)
                    {
                        var y = 45 + (i * 35);

                        var lblCategory = new Label
                        {
                            Text = categories[i],
                            Font = new Font("Montserrat", 9),
                            AutoSize = true,
                            Location = new Point(10, y + 5)
                        };
                        ratingsCard.Controls.Add(lblCategory);

                        var pnlBar = new Panel
                        {
                            Location = new Point(150, y),
                            Size = new Size(200, 25),
                            BackColor = ColorTranslator.FromHtml("#E0E0E0")
                        };

                        var pnlFill = new Panel
                        {
                            Location = new Point(0, 0),
                            Size = new Size((int)(pnlBar.Width * (ratings[i] / 5.0)), 25),
                            BackColor = sage
                        };
                        pnlBar.Controls.Add(pnlFill);

                        var lblRating = new Label
                        {
                            Text = $"{ratings[i]:F1}",
                            Font = new Font("Montserrat", 9, FontStyle.Bold),
                            AutoSize = true,
                            Location = new Point(360, y + 5)
                        };

                        ratingsCard.Controls.AddRange(new Control[] { pnlBar, lblRating });
                    }
                    flp.Controls.Add(ratingsCard);

                    // Comments Card
                    var commentsCard = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 250,
                        Margin = new Padding(0, 0, 0, 15),
                        BackColor = beige,
                        Padding = new Padding(15)
                    };

                    var lblCommentsTitle = new Label
                    {
                        Text = "Recent Traveler Comments",
                        Font = new Font("Montserrat", 11, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(10, 10)
                    };
                    commentsCard.Controls.Add(lblCommentsTitle);

                    // Comment entries
                    var comments = new[] {
                new { Name = "John S.", Date = "Apr 28, 2025", Text = "The city tour was excellent, our guide was very knowledgeable." },
                new { Name = "Emma J.", Date = "Apr 25, 2025", Text = "Hotel room was clean but the check-in process took too long." },
                new { Name = "Michael B.", Date = "Apr 22, 2025", Text = "Great value for money on the desert safari package!" }
            };

                    for (int i = 0; i < comments.Length; i++)
                    {
                        var y = 45 + (i * 60);

                        var pnlComment = new Panel
                        {
                            Location = new Point(10, y),
                            Size = new Size(commentsCard.Width - 50, 50),
                            BackColor = offWhite
                        };

                        var lblName = new Label
                        {
                            Text = $"{comments[i].Name} - {comments[i].Date}",
                            Font = new Font("Montserrat", 9, FontStyle.Bold),
                            AutoSize = true,
                            Location = new Point(10, 5)
                        };

                        var lblComment = new Label
                        {
                            Text = comments[i].Text,
                            Font = new Font("Montserrat", 9),
                            AutoSize = true,
                            Location = new Point(10, 25)
                        };

                        pnlComment.Controls.AddRange(new Control[] { lblName, lblComment });
                        commentsCard.Controls.Add(pnlComment);
                    }
                    flp.Controls.Add(commentsCard);
                }

                // --- REVENUE ANALYSIS VIEW ---
                else if (choice == "Revenue Analysis")
                {
                    // Title card
                    var titleCard = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 60,
                        Margin = new Padding(0, 0, 0, 15),
                        BackColor = sage,
                        Padding = new Padding(15)
                    };

                    var lblTitle = new Label
                    {
                        Text = "Revenue Analysis Report",
                        Font = new Font("Montserrat", 12, FontStyle.Bold),
                        ForeColor = offWhite,
                        AutoSize = true,
                        Location = new Point(10, 15)
                    };
                    titleCard.Controls.Add(lblTitle);
                    flp.Controls.Add(titleCard);

                    // Revenue data card
                    var revenueCard = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 230,
                        Margin = new Padding(0, 0, 0, 15),
                        BackColor = beige,
                        Padding = new Padding(15)
                    };

                    var lblRevenueTitle = new Label
                    {
                        Text = "Monthly Revenue by Service Type (2025)",
                        Font = new Font("Montserrat", 11, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(10, 10)
                    };
                    revenueCard.Controls.Add(lblRevenueTitle);

                    // Revenue table
                    var tblRevenue = new TableLayoutPanel
                    {
                        ColumnCount = 5,
                        RowCount = 6,
                        Location = new Point(10, 40),
                        Size = new Size(revenueCard.Width - 50, 170),
                        CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                        BackColor = offWhite
                    };

                    // Headers
                    tblRevenue.Controls.Add(CreateBoldLabel("Month"), 0, 0);
                    tblRevenue.Controls.Add(CreateBoldLabel("Hotel ($)"), 1, 0);
                    tblRevenue.Controls.Add(CreateBoldLabel("Tours ($)"), 2, 0);
                    tblRevenue.Controls.Add(CreateBoldLabel("Transport ($)"), 3, 0);
                    tblRevenue.Controls.Add(CreateBoldLabel("Total ($)"), 4, 0);

                    // Column styles
                    tblRevenue.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
                    tblRevenue.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
                    tblRevenue.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
                    tblRevenue.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
                    tblRevenue.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

                    // Data
                    var months = new[] { "January", "February", "March", "April", "May" };
                    var hotelRevenue = new[] { 15400, 16200, 17500, 20100, 22400 };
                    var tourRevenue = new[] { 8500, 9100, 12400, 14500, 16200 };
                    var transportRevenue = new[] { 3200, 3500, 4100, 4800, 5300 };

                    for (int i = 0; i < 5; i++)
                    {
                        int total = hotelRevenue[i] + tourRevenue[i] + transportRevenue[i];

                        tblRevenue.Controls.Add(new Label { Text = months[i], TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(5) }, 0, i + 1);
                        tblRevenue.Controls.Add(new Label { Text = $"{hotelRevenue[i]:N0}", TextAlign = ContentAlignment.MiddleRight, Padding = new Padding(5) }, 1, i + 1);
                        tblRevenue.Controls.Add(new Label { Text = $"{tourRevenue[i]:N0}", TextAlign = ContentAlignment.MiddleRight, Padding = new Padding(5) }, 2, i + 1);
                        tblRevenue.Controls.Add(new Label { Text = $"{transportRevenue[i]:N0}", TextAlign = ContentAlignment.MiddleRight, Padding = new Padding(5) }, 3, i + 1);
                        tblRevenue.Controls.Add(new Label { Text = $"{total:N0}", TextAlign = ContentAlignment.MiddleRight, Padding = new Padding(5), Font = new Font("Montserrat", 8, FontStyle.Bold) }, 4, i + 1);
                    }

                    revenueCard.Controls.Add(tblRevenue);
                    flp.Controls.Add(revenueCard);

                    // KPI Card
                    var kpiCard = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 140,
                        Margin = new Padding(0, 0, 0, 15),
                        BackColor = mint,
                        Padding = new Padding(15)
                    };

                    var lblKpiTitle = new Label
                    {
                        Text = "Key Performance Indicators",
                        Font = new Font("Montserrat", 11, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(10, 10)
                    };
                    kpiCard.Controls.Add(lblKpiTitle);

                    // Calculate KPI values
                    int totalRevenue = hotelRevenue.Sum() + tourRevenue.Sum() + transportRevenue.Sum();
                    double monthlyAvg = totalRevenue / 5.0;
                    double growthRate = ((double)(hotelRevenue[4] + tourRevenue[4] + transportRevenue[4]) /
                                       (hotelRevenue[0] + tourRevenue[0] + transportRevenue[0]) - 1) * 100;

                    var lblTotal = new Label
                    {
                        Text = $"Total Revenue: ${totalRevenue:N0}",
                        Font = new Font("Montserrat", 10),
                        AutoSize = true,
                        Location = new Point(30, 40)
                    };

                    var lblMonthly = new Label
                    {
                        Text = $"Average Monthly Revenue: ${monthlyAvg:N0}",
                        Font = new Font("Montserrat", 10),
                        AutoSize = true,
                        Location = new Point(30, 70)
                    };

                    var lblGrowth = new Label
                    {
                        Text = $"Revenue Growth (Jan-May): {growthRate:F1}%",
                        Font = new Font("Montserrat", 10),
                        AutoSize = true,
                        Location = new Point(30, 100)
                    };

                    kpiCard.Controls.AddRange(new Control[] { lblTotal, lblMonthly, lblGrowth });

                    // Export button (matching your style)
                    var btnExport = new Button
                    {
                        Text = "Export Report",
                        Size = new Size(120, 35),
                        BackColor = sage,
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Location = new Point(kpiCard.Width - 150, 80)
                    };
                    btnExport.FlatAppearance.BorderSize = 0;
                    btnExport.Click += (_, __) => MessageBox.Show("Report exported successfully!");
                    kpiCard.Controls.Add(btnExport);

                    flp.Controls.Add(kpiCard);
                }
            };

            // Make sure cards resize when flow panel resizes (just like in your other views)
            flp.Resize += (s, e) =>
            {
                foreach (Control ctl in flp.Controls)
                {
                    if (ctl is Panel panel)
                    {
                        panel.Width = flp.ClientSize.Width - flp.Padding.Horizontal;
                    }
                }
            };

            // Set initial view
            cmb.SelectedIndex = 0;
        }

        private void SP_Main_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }


    }
}
