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


        private void InitializeManageBookingsView()
        {
            panelBookings.Controls.Clear();
            var lbl = new Label
            {
                Text = "Manage Bookings",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Montserrat", 14, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#333333")
            };
            panelBookings.Controls.Add(lbl);
        }

        private void InitializeStatisticsView()
        {
            panelStats.Controls.Clear();
            var lbl = new Label
            {
                Text = "Statistics Dashboard",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Montserrat", 14, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#333333")
            };
            panelStats.Controls.Add(lbl);
        }


        private void SP_Main_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }


    }
}
