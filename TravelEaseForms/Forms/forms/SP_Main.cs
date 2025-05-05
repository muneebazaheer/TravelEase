using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TravelEaseForms
{
    public partial class SP_Main : Form
    {
        private Panel panelSidebar;
        private Panel panelContent;

        private Panel panelApprove;
        private Panel panelServices;
        private Panel panelBookings;
        private Panel panelStats;

        private Button btnApproveTours;
        private Button btnManageServices;
        private Button btnManageBookings;
        private Button btnStatistics;

        public SP_Main()
        {
            this.AutoScaleMode = AutoScaleMode.None;
            Text = "TravelEase – Service Provider";
            WindowState = FormWindowState.Maximized;

            InitializeLayout();
            ApplyColorPalette();
        }

        private void InitializeLayout()
        {
            // 1) Sidebar
            panelSidebar = new Panel
            {
                Dock  = DockStyle.Left,
                Width = 200
            };
            Controls.Add(panelSidebar);

            // 2) Content area
            panelContent = new Panel
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(panelContent);

            // 3) Content panels
            panelApprove  = new Panel { Dock = DockStyle.Fill, Visible = true  };
            panelServices = new Panel { Dock = DockStyle.Fill, Visible = false };
            panelBookings = new Panel { Dock = DockStyle.Fill, Visible = false };
            panelStats    = new Panel { Dock = DockStyle.Fill, Visible = false };

            panelContent.Controls.AddRange(new Control[]
            {
                panelApprove,
                panelServices,
                panelBookings,
                panelStats
            });

            // 4) Sidebar buttons (text-only)
            btnApproveTours   = CreateSidebarButton("Approve Tours");
            btnManageServices = CreateSidebarButton("Manage Services");
            btnManageBookings = CreateSidebarButton("Manage Bookings");
            btnStatistics     = CreateSidebarButton("Statistics");

            // add in reverse so the first appears at the top
            panelSidebar.Controls.AddRange(new Control[]
            {
                btnStatistics,
                btnManageBookings,
                btnManageServices,
                btnApproveTours
            });
        }

        private Button CreateSidebarButton(string text)
        {
            var btn = new Button
            {
                Text      = text,
                Height    = 50,
                Dock      = DockStyle.Top,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleCenter,
                Font      = new Font("Montserrat", 10, FontStyle.Regular)
            };
            btn.FlatAppearance.BorderSize         = 0;
            btn.Click += SidebarButton_Click;
            return btn;
        }

        private void SidebarButton_Click(object sender, EventArgs e)
        {
            // hide all panels
            panelApprove.Visible  =
            panelServices.Visible =
            panelBookings.Visible =
            panelStats.Visible    = false;

            // show matching panel
            switch (((Button)sender).Text)
            {
                case "Approve Tours":     panelApprove.Visible  = true; break;
                case "Manage Services":   panelServices.Visible = true; break;
                case "Manage Bookings":   panelBookings.Visible = true; break;
                case "Statistics":        panelStats.Visible    = true; break;
            }
        }

        private void ApplyColorPalette()
        {
            // your color palette
            var beige    = ColorTranslator.FromHtml("#FAF1E6");
            var offWhite = ColorTranslator.FromHtml("#FDFAF6");
            var mint     = ColorTranslator.FromHtml("#E4EFE7");
            var sage     = ColorTranslator.FromHtml("#99BC85");
            var textCol  = ColorTranslator.FromHtml("#333333");

            // form background
            BackColor = offWhite;

            // sidebar styling
            panelSidebar.BackColor = sage;
            foreach (Button btn in panelSidebar.Controls.OfType<Button>())
            {
                btn.ForeColor      = textCol;
                btn.BackColor      = offWhite;
                btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(sage, 0.1f);
            }

            // content panels background
            panelContent.BackColor =
            panelApprove .BackColor =
            panelServices.BackColor =
            panelBookings.BackColor =
            panelStats   .BackColor = mint;
        }

        private void SP_Main_Load(object sender, EventArgs e)
        {

        }
    }
}
