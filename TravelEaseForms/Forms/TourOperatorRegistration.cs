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
    public partial class TourOperatorRegistration : Form
    {
        public TourOperatorRegistration()
        {
            InitializeComponent();
        }

        private void TravelerRegistrationLabel_Click(object sender, EventArgs e)
        {

        }

        private void TourOperatorRegistration_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#FAF1E6");
            RegisterButton.FlatStyle = FlatStyle.Flat;
            RegisterButton.BackColor = ColorTranslator.FromHtml("#99BC85");
            RegisterButton.ForeColor = Color.White;
            TravelerRegistrationLabel.ForeColor = ColorTranslator.FromHtml("#99BC85");
            LoginlinkLabel.LinkColor = ColorTranslator.FromHtml("#99BC85");
            LoginlinkLabel.LinkBehavior = LinkBehavior.NeverUnderline;
        }

        private void LoginlinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TourOperatorLogin loginForm = new TourOperatorLogin();
            loginForm.Show();
            this.Hide();
        }
    }
}
