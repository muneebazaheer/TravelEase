using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TravelEaseForms.Forms
{
    public partial class TourOperatorDashboard : Form
    {
        public TourOperatorDashboard()
        {
            InitializeComponent();
            this.Load += TourOperatorDashboard_Load;

        }

        private void TourOperatorDashboard_Load(object sender, EventArgs e)
        {
            Greenbgrect.BackColor = ColorTranslator.FromHtml("#99BC85");
            profilePanel.BackColor = ColorTranslator.FromHtml("#E4EFE7");
            ReviewRatingButton.UseCompatibleTextRendering = true;
            TripSearchButtton.UseCompatibleTextRendering = true;
            ProfileButton.BackColor = ColorTranslator.FromHtml("#FAF1E6");
            ReviewRatingButton.BackColor = ColorTranslator.FromHtml("#FAF1E6");
            travelpassButton.BackColor = ColorTranslator.FromHtml("#FAF1E6");
            TripSearchButtton.BackColor = ColorTranslator.FromHtml("#FAF1E6");
            PAButton.BackColor = ColorTranslator.FromHtml("#FAF1E6");
            PAButton.UseCompatibleTextRendering = true;
            NamedisplayLabel.Text = SessionDetails.UserName;
            label1.Text = SessionDetails.email;
            label2.Text = SessionDetails.Address;
            panel1.BackColor = ColorTranslator.FromHtml("#E4EFE7");
            panel2.BackColor = ColorTranslator.FromHtml("#E4EFE7");
        }
        private void profilePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TripSearchButtton_Click(object sender, EventArgs e)
        {
            profilePanel.Hide();
            panel1.Show();
            panel2.Hide();  
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ProfileButton_Click(object sender, EventArgs e)
        {
            profilePanel.Show();
            panel1.Hide();
            panel2.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void travelpassButton_Click(object sender, EventArgs e)
        {
            panel2.Show();
            panel1.Hide();
            profilePanel.Hide();
        }
    }
}
