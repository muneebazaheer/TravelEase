using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TravelEaseForms
{
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

        /*CONSTRUCTOR*/
        public SP_Main()
        {
            InitializeComponent();
            InitializeLayout();
            ApplyColorPalette();
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
        private Panel CreateBookingCard(FlowLayoutPanel flp, (string BookingID, string CustomerName, string ServiceType,
                                      string Date, decimal Amount, string Status) booking)
        {
            var card = new Panel
            {
                Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                Height = 100,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = ColorTranslator.FromHtml("#FAF1E6"), // beige
                Padding = new Padding(10)
            };

            // Customer info
            var lblName = new Label
            {
                Text = $"{booking.CustomerName} - {booking.BookingID}",
                Font = new Font("Montserrat", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10)
            };
            card.Controls.Add(lblName);

            // Service info
            var lblService = new Label
            {
                Text = $"Service: {booking.ServiceType}",
                Font = new Font("Montserrat", 9),
                AutoSize = true,
                Location = new Point(10, 35)
            };
            card.Controls.Add(lblService);

            // Date info
            var lblDate = new Label
            {
                Text = $"Date: {booking.Date}",
                Font = new Font("Montserrat", 9),
                AutoSize = true,
                Location = new Point(10, 55)
            };
            card.Controls.Add(lblDate);

            // Amount info
            var lblAmount = new Label
            {
                Text = $"Amount: ${booking.Amount:F2}",
                Font = new Font("Montserrat", 9),
                AutoSize = true,
                Location = new Point(10, 75)
            };
            card.Controls.Add(lblAmount);

            return card;
        }

        // Helper method to add a booking card with standard options
        private void AddBookingCard(FlowLayoutPanel flp, (string BookingID, string CustomerName, string ServiceType,
                                   string Date, decimal Amount, string Status) booking, bool showAllOptions = false)
        {
            var card = CreateBookingCard(flp, booking);

            // Add status indicator
            var statusPanel = new Panel
            {
                Size = new Size(80, 25),
                Location = new Point(card.Width - 100, 10)
            };

            var statusLabel = new Label
            {
                Text = booking.Status,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Montserrat", 9, FontStyle.Bold)
            };

            // Set color based on status
            if (booking.Status == "Paid")
            {
                statusPanel.BackColor = ColorTranslator.FromHtml("#99BC85"); // green
                statusLabel.ForeColor = Color.White;
            }
            else if (booking.Status == "Confirmed")
            {
                statusPanel.BackColor = ColorTranslator.FromHtml("#FFD966"); // yellow
                statusLabel.ForeColor = ColorTranslator.FromHtml("#333333");
            }
            else
            {
                statusPanel.BackColor = ColorTranslator.FromHtml("#F8D7DA"); // light red
                statusLabel.ForeColor = ColorTranslator.FromHtml("#721C24");
            }

            statusPanel.Controls.Add(statusLabel);
            card.Controls.Add(statusPanel);

            flp.Controls.Add(card);
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
            btnApproveTours = CreateSidebarButton("Approve Tours");
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

            // 3) Dummy data – will later add query results here
            var tours = new[]
            {
                new { TourID="T001", TourName="Mountain Hike",   AssignedBy="Operator A" },
                new { TourID="T002", TourName="City Walk",       AssignedBy="Operator B" },
                new { TourID="T003", TourName="Desert Safari",   AssignedBy="Operator C" },
            };

            // 4) Build one “card” (row) per tour
            foreach (var t in tours)
            {
                var row = new Panel
                {
                    Height = 80,
                    Margin = new Padding(0, 0, 0, 10),
                    BackColor = ColorTranslator.FromHtml("#FAF1E6")  // beige
                };
                // initial width; will correct on resize below
                row.Width = flp.ClientSize.Width - flp.Padding.Horizontal;

                // b) Title label
                var lblName = new Label
                {
                    Text = $"{t.TourName} ({t.TourID})",
                    AutoSize = true,
                    Font = new Font("Montserrat", 11, FontStyle.Bold),
                    ForeColor = ColorTranslator.FromHtml("#333333"),
                    Location = new Point(10, 10)
                };
                row.Controls.Add(lblName);

                // c) Subtitle label
                var lblBy = new Label
                {
                    Text = $"Assigned by: {t.AssignedBy}",
                    AutoSize = true,
                    Font = new Font("Montserrat", 9, FontStyle.Regular),
                    ForeColor = ColorTranslator.FromHtml("#333333"),
                    Location = new Point(10, 35)
                };
                row.Controls.Add(lblBy);

                // d) Reject button (right-most)
                var btnReject = new Button
                {
                    Text = "Reject",
                    Size = new Size(80, 30),
                    BackColor = ColorTranslator.FromHtml("#D22B2B"), // light red
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    //Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnReject.FlatAppearance.BorderSize = 0;
                btnReject.Click += (s, e) =>
                {
                    MessageBox.Show($"Rejected tour {t.TourID}");
                    flp.Controls.Remove(row);
                };
                row.Controls.Add(btnReject);

                // e) Accept button (to the left of Reject)
                var btnAccept = new Button
                {
                    Text = "Accept",
                    Size = new Size(80, 30),
                    BackColor = ColorTranslator.FromHtml("#99BC85"), // sage
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    //Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnAccept.FlatAppearance.BorderSize = 0;
                btnAccept.Click += (s, e) =>
                {
                    MessageBox.Show($"Approved tour {t.TourID}");
                    flp.Controls.Remove(row);
                };
                row.Controls.Add(btnAccept);

                var btnPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Right,
                    Width = btnAccept.Width + btnReject.Width + 30,
                    FlowDirection = FlowDirection.RightToLeft,
                    Padding = new Padding(0, (row.Height - btnAccept.Height) / 2, 10, 0),
                    BackColor = Color.Transparent,  // so it “floats” on the card
                    AutoSize = false,
                    AutoSizeMode = AutoSizeMode.GrowOnly
                };
                btnPanel.Controls.Add(btnReject);
                btnPanel.Controls.Add(btnAccept);
                row.Controls.Add(btnPanel);

                // f) Position the buttons whenever the row resizes
                row.SizeChanged += (s, e) =>
                {
                    // 20px from the right edge
                    btnReject.Location = new Point(
                        row.ClientSize.Width - btnReject.Width - 20,
                        (row.ClientSize.Height - btnReject.Height) / 2
                    );
                    // 10px gap to the left of Reject
                    btnAccept.Location = new Point(
                        btnReject.Left - btnAccept.Width - 10,
                        (row.ClientSize.Height - btnAccept.Height) / 2
                    );
                };
                row.PerformLayout();
                flp.Controls.Add(row);
            }

            // 5) Make sure each card spans the full width whenever flp is resized
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

            // 1) Dummy data inside the method
            var services = new List<(string ServiceID, string Name, decimal Cost)> {
                ("S001", "Hotel Booking", 120.00m),
                ("S002", "City Tour",      75.00m),
                ("S003", "Transport",      40.00m),
                ("S004", "Guide Service",  50.00m),
                ("S005", "Meal Plan",      30.00m),
            };

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

            // 4) The FlowLayoutPanel that will hold your cards
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

                // --- VIEW ALL ---
                if (choice == "View all current services")
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

                        var cost = new Label
                        {
                            Text = $"Cost: ${svc.Cost:F2}",
                            AutoSize = true,
                            Font = new Font("Montserrat", 9),
                            Location = new Point(10, 35)
                        };
                        card.Controls.Add(cost);

                        flp.Controls.Add(card);
                    }
                }
                // --- REMOVE ---
                else if (choice == "Remove a service")
                {
                    foreach (var svc in services.ToList())
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

                        btn.Click += (_, __) => {
                            // Remove the service with the matching ID
                            services.RemoveAll(x => x.ServiceID == serviceIdToRemove);
                            MessageBox.Show($"Service {serviceIdToRemove} removed successfully!");

                            // Refresh the list by re-selecting the "Remove a service" option
                            cmb.SelectedIndex = -1; // Clear selection first
                            cmb.SelectedIndex = cmb.Items.IndexOf("Remove a service");
                        };

                        card.Controls.Add(btn);
                        btn.Location = new Point(
                            card.ClientSize.Width - btn.Width - 20,
                            (card.ClientSize.Height - btn.Height) / 2
                        );

                        flp.Controls.Add(card);
                    }
                }
                // --- ADD ---
                else if (choice == "Add a service")
                {
                    var form = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 160,
                        Padding = new Padding(20),
                        BackColor = ColorTranslator.FromHtml("#FAF1E6"),
                        Margin = new Padding(0, 0, 0, 10)
                    };

                    var lblN = new Label
                    {
                        Text = "Service Name:",
                        Font = new Font("Montserrat", 9),
                        Location = new Point(0, 0),
                        AutoSize = true
                    };
                    var txtN = new TextBox
                    {
                        Font = new Font("Montserrat", 9),
                        Location = new Point(120, 0),
                        Width = 200
                    };
                    form.Controls.AddRange(new Control[] { lblN, txtN });

                    var lblC = new Label
                    {
                        Text = "Cost:",
                        Font = new Font("Montserrat", 9),
                        Location = new Point(0, 40),
                        AutoSize = true
                    };
                    var txtC = new TextBox
                    {
                        Font = new Font("Montserrat", 9),
                        Location = new Point(120, 36),
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
                        Location = new Point(120, 80)
                    };
                    btnA.FlatAppearance.BorderSize = 0;
                    btnA.Click += (_, __) => {
                        if (string.IsNullOrWhiteSpace(txtN.Text) || !decimal.TryParse(txtC.Text, out var cost))
                        {
                            MessageBox.Show("Enter valid name & cost");
                            return;
                        }
                        var id = $"S{services.Count + 1:000}";
                        services.Add((id, txtN.Text.Trim(), cost));
                        MessageBox.Show($"Service {id} added!");
                        cmb.SelectedIndex = cmb.Items.IndexOf("View all current services");
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

            // 1) Dummy booking data
            var bookings = new List<(string BookingID, string CustomerName, string ServiceType,
                                   string Date, decimal Amount, string Status)>
    {
        ("B001", "John Smith", "Hotel Booking", "2025-05-10", 240.00m, "Pending"),
        ("B002", "Emma Johnson", "City Tour", "2025-05-12", 150.00m, "Confirmed"),
        ("B003", "Michael Brown", "Transport", "2025-05-15", 120.00m, "Pending"),
        ("B004", "Sarah Wilson", "Guide Service", "2025-05-20", 100.00m, "Pending"),
        ("B005", "Robert Davis", "Hotel Booking", "2025-05-22", 320.00m, "Paid"),
    };

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
        "Pending confirmations",
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

                // --- VIEW ALL BOOKINGS ---
                if (choice == "View all bookings")
                {
                    foreach (var booking in bookings)
                    {
                        AddBookingCard(flp, booking, showAllOptions: true);
                    }
                }
                // --- PENDING CONFIRMATIONS ---
                else if (choice == "Pending confirmations")
                {
                    var pendingBookings = bookings.Where(b => b.Status == "Pending").ToList();

                    if (pendingBookings.Count == 0)
                    {
                        AddInfoCard(flp, "No pending bookings to confirm");
                        return;
                    }

                    foreach (var booking in pendingBookings)
                    {
                        var card = CreateBookingCard(flp, booking);

                        // Add confirm button
                        var btnConfirm = new Button
                        {
                            Text = "Confirm",
                            Size = new Size(90, 30),
                            BackColor = ColorTranslator.FromHtml("#99BC85"), // sage green
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat
                        };
                        btnConfirm.FlatAppearance.BorderSize = 0;

                        string bookingId = booking.BookingID;
                        btnConfirm.Click += (_, __) => {
                            // Find and update the booking status
                            var index = bookings.FindIndex(b => b.BookingID == bookingId);
                            if (index >= 0)
                            {
                                var updatedBooking = bookings[index];
                                bookings[index] = (updatedBooking.BookingID, updatedBooking.CustomerName,
                                                  updatedBooking.ServiceType, updatedBooking.Date,
                                                  updatedBooking.Amount, "Confirmed");

                                MessageBox.Show($"Booking {bookingId} has been confirmed!");

                                // Refresh the view
                                cmb.SelectedIndex = -1;
                                cmb.SelectedIndex = cmb.Items.IndexOf("Pending confirmations");
                            }
                        };

                        // Add reject button
                        var btnReject = new Button
                        {
                            Text = "Reject",
                            Size = new Size(90, 30),
                            BackColor = ColorTranslator.FromHtml("#D22B2B"), // red
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat
                        };
                        btnReject.FlatAppearance.BorderSize = 0;

                        btnReject.Click += (_, __) => {
                            // Remove the booking
                            bookings.RemoveAll(b => b.BookingID == bookingId);
                            MessageBox.Show($"Booking {bookingId} has been rejected and removed.");

                            // Refresh the view
                            cmb.SelectedIndex = -1;
                            cmb.SelectedIndex = cmb.Items.IndexOf("Pending confirmations");
                        };

                        // Add buttons panel
                        var btnPanel = new FlowLayoutPanel
                        {
                            FlowDirection = FlowDirection.RightToLeft,
                            Size = new Size(200, 40),
                            Location = new Point(card.Width - 220, card.Height - 45),
                            BackColor = Color.Transparent
                        };
                        btnPanel.Controls.Add(btnReject);
                        btnPanel.Controls.Add(btnConfirm);
                        card.Controls.Add(btnPanel);

                        flp.Controls.Add(card);
                    }
                }
                // --- TRACK PAYMENTS ---
                else if (choice == "Track payments")
                {
                    // Add payment summary card
                    var summaryCard = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 100,
                        Margin = new Padding(0, 0, 0, 15),
                        BackColor = ColorTranslator.FromHtml("#E4EFE7"), // light mint
                        Padding = new Padding(15)
                    };

                    // Calculate payment statistics
                    decimal totalAmount = bookings.Sum(b => b.Amount);
                    decimal paidAmount = bookings.Where(b => b.Status == "Paid").Sum(b => b.Amount);
                    decimal pendingAmount = totalAmount - paidAmount;
                    int paidCount = bookings.Count(b => b.Status == "Paid");
                    int pendingCount = bookings.Count(b => b.Status != "Paid");

                    var lblSummary = new Label
                    {
                        Text = "Payment Summary",
                        Font = new Font("Montserrat", 12, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(10, 10)
                    };

                    var lblStats = new Label
                    {
                        Text = $"Total: ${totalAmount:F2} • Paid: ${paidAmount:F2} ({paidCount} bookings) • " +
                               $"Pending: ${pendingAmount:F2} ({pendingCount} bookings)",
                        Font = new Font("Montserrat", 10),
                        AutoSize = true,
                        Location = new Point(10, 40)
                    };

                    var lblNote = new Label
                    {
                        Text = "Click on a booking to update its payment status",
                        Font = new Font("Montserrat", 9, FontStyle.Italic),
                        ForeColor = ColorTranslator.FromHtml("#666666"),
                        AutoSize = true,
                        Location = new Point(10, 70)
                    };

                    summaryCard.Controls.AddRange(new Control[] { lblSummary, lblStats, lblNote });
                    flp.Controls.Add(summaryCard);

                    // Add booking cards with payment options
                    foreach (var booking in bookings)
                    {
                        var card = CreateBookingCard(flp, booking);

                        // Add status indicator
                        var statusPanel = new Panel
                        {
                            Size = new Size(80, 25),
                            Location = new Point(card.Width - 100, 10)
                        };

                        var statusLabel = new Label
                        {
                            Text = booking.Status,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Dock = DockStyle.Fill,
                            Font = new Font("Montserrat", 9, FontStyle.Bold)
                        };

                        // Set color based on status
                        if (booking.Status == "Paid")
                        {
                            statusPanel.BackColor = ColorTranslator.FromHtml("#99BC85"); // green
                            statusLabel.ForeColor = Color.White;
                        }
                        else if (booking.Status == "Confirmed")
                        {
                            statusPanel.BackColor = ColorTranslator.FromHtml("#FFD966"); // yellow
                            statusLabel.ForeColor = ColorTranslator.FromHtml("#333333");
                        }
                        else
                        {
                            statusPanel.BackColor = ColorTranslator.FromHtml("#F8D7DA"); // light red
                            statusLabel.ForeColor = ColorTranslator.FromHtml("#721C24");
                        }

                        statusPanel.Controls.Add(statusLabel);
                        card.Controls.Add(statusPanel);

                        // Add update payment button for non-paid bookings
                        if (booking.Status != "Paid")
                        {
                            var btnPayment = new Button
                            {
                                Text = "Mark as Paid",
                                Size = new Size(120, 30),
                                BackColor = ColorTranslator.FromHtml("#99BC85"),
                                ForeColor = Color.White,
                                FlatStyle = FlatStyle.Flat,
                                Location = new Point(card.Width - 140, card.Height - 40)
                            };
                            btnPayment.FlatAppearance.BorderSize = 0;

                            string bookingId = booking.BookingID;
                            btnPayment.Click += (_, __) => {
                                // Update payment status
                                var index = bookings.FindIndex(b => b.BookingID == bookingId);
                                if (index >= 0)
                                {
                                    var updatedBooking = bookings[index];
                                    bookings[index] = (updatedBooking.BookingID, updatedBooking.CustomerName,
                                                      updatedBooking.ServiceType, updatedBooking.Date,
                                                      updatedBooking.Amount, "Paid");

                                    MessageBox.Show($"Payment for booking {bookingId} has been recorded!");

                                    // Refresh the view
                                    cmb.SelectedIndex = -1;
                                    cmb.SelectedIndex = cmb.Items.IndexOf("Track payments");
                                }
                            };

                            card.Controls.Add(btnPayment);
                        }

                        flp.Controls.Add(card);
                    }
                }
                // --- UPDATE AVAILABILITY ---
                else if (choice == "Update availability")
                {
                    // Group bookings by service type for availability management
                    var serviceGroups = bookings
                        .GroupBy(b => b.ServiceType)
                        .Select(g => new {
                            ServiceType = g.Key,
                            BookingCount = g.Count(),
                            TotalRevenue = g.Sum(b => b.Amount)
                        })
                        .ToList();

                    // Add availability management form
                    var formPanel = new Panel
                    {
                        Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                        Height = 250,
                        Margin = new Padding(0, 0, 0, 15),
                        BackColor = ColorTranslator.FromHtml("#FAF1E6"),
                        Padding = new Padding(20)
                    };

                    var lblTitle = new Label
                    {
                        Text = "Update Service Availability",
                        Font = new Font("Montserrat", 12, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(0, 0)
                    };
                    formPanel.Controls.Add(lblTitle);

                    var lblService = new Label
                    {
                        Text = "Service Type:",
                        Font = new Font("Montserrat", 9),
                        AutoSize = true,
                        Location = new Point(0, 40)
                    };
                    formPanel.Controls.Add(lblService);

                    var cmbService = new ComboBox
                    {
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Font = new Font("Montserrat", 9),
                        Location = new Point(120, 36),
                        Width = 200
                    };
                    cmbService.Items.AddRange(new[] {
                "Hotel Booking",
                "City Tour",
                "Transport",
                "Guide Service",
                "Meal Plan"
            });
                    formPanel.Controls.Add(cmbService);

                    var lblDate = new Label
                    {
                        Text = "Date:",
                        Font = new Font("Montserrat", 9),
                        AutoSize = true,
                        Location = new Point(0, 70)
                    };
                    formPanel.Controls.Add(lblDate);

                    var dtpDate = new DateTimePicker
                    {
                        Font = new Font("Montserrat", 9),
                        Location = new Point(120, 66),
                        Width = 200,
                        Format = DateTimePickerFormat.Short
                    };
                    formPanel.Controls.Add(dtpDate);

                    var lblCapacity = new Label
                    {
                        Text = "Capacity:",
                        Font = new Font("Montserrat", 9),
                        AutoSize = true,
                        Location = new Point(0, 100)
                    };
                    formPanel.Controls.Add(lblCapacity);

                    var numCapacity = new NumericUpDown
                    {
                        Font = new Font("Montserrat", 9),
                        Location = new Point(120, 96),
                        Width = 100,
                        Minimum = 0,
                        Maximum = 100,
                        Value = 20
                    };
                    formPanel.Controls.Add(numCapacity);

                    var btnUpdate = new Button
                    {
                        Text = "Update Availability",
                        Size = new Size(150, 35),
                        BackColor = ColorTranslator.FromHtml("#99BC85"),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Location = new Point(120, 140)
                    };
                    btnUpdate.FlatAppearance.BorderSize = 0;

                    btnUpdate.Click += (_, __) => {
                        if (cmbService.SelectedItem == null)
                        {
                            MessageBox.Show("Please select a service type.");
                            return;
                        }

                        string serviceType = cmbService.SelectedItem.ToString();
                        string date = dtpDate.Value.ToString("yyyy-MM-dd");
                        int capacity = (int)numCapacity.Value;

                        MessageBox.Show($"Availability for {serviceType} on {date} updated to {capacity} slots.");
                    };
                    formPanel.Controls.Add(btnUpdate);

                    flp.Controls.Add(formPanel);

                    // Add service usage stats
                    var statsTitle = new Label
                    {
                        Text = "Current Service Usage",
                        Font = new Font("Montserrat", 12, FontStyle.Bold),
                        AutoSize = true,
                        Margin = new Padding(10, 0, 0, 10)
                    };
                    flp.Controls.Add(statsTitle);

                    foreach (var group in serviceGroups)
                    {
                        var statsCard = new Panel
                        {
                            Width = flp.ClientSize.Width - flp.Padding.Horizontal,
                            Height = 80,
                            Margin = new Padding(0, 0, 0, 10),
                            BackColor = ColorTranslator.FromHtml("#F8F9FA"), // light gray
                            Padding = new Padding(15)
                        };

                        var lblServiceName = new Label
                        {
                            Text = group.ServiceType,
                            Font = new Font("Montserrat", 11, FontStyle.Bold),
                            AutoSize = true,
                            Location = new Point(10, 10)
                        };

                        var lblBookings = new Label
                        {
                            Text = $"Total Bookings: {group.BookingCount} • Revenue: ${group.TotalRevenue:F2}",
                            Font = new Font("Montserrat", 9),
                            AutoSize = true,
                            Location = new Point(10, 40)
                        };

                        statsCard.Controls.AddRange(new Control[] { lblServiceName, lblBookings });
                        flp.Controls.Add(statsCard);
                    }
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
