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
    public partial class TravellerLogin : Form
    {
        public TravellerLogin()
        {
            InitializeComponent();

            // Optional: Move to Form Load to avoid Designer overwrites
            this.Load += TravellerLogin_Load;
        }

        private void TravellerLogin_Load(object sender, EventArgs e)
        {
            LoginButton.FlatStyle = FlatStyle.Flat;
            LoginButton.BackColor = ColorTranslator.FromHtml("#99BC85");
            LoginButton.ForeColor = Color.White;
            LoginButton.Font = new Font(LoginButton.Font.FontFamily, LoginButton.Font.Size, FontStyle.Bold);
            this.BackColor = ColorTranslator.FromHtml("#FAF1E6");
            RegisterLinkLabel.LinkColor = ColorTranslator.FromHtml("#99BC85");
            RegisterLinkLabel.LinkBehavior = LinkBehavior.NeverUnderline;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LoginButton_Click(object sender, EventArgs e)
        {

        }

        private void TravellerLogin_Load_1(object sender, EventArgs e)
        {

        }

        private void RegisterLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
