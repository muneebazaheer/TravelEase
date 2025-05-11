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
    public partial class SP_Login : Form
    {
        public SP_Login()
        {
            this.AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();
            ApplyColorPalette();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void ApplyColorPalette()
        {

            Color beige = ColorTranslator.FromHtml("#FAF1E6");
            Color offWhite = ColorTranslator.FromHtml("#FDFAF6");
            Color mint = ColorTranslator.FromHtml("#E4EFE7");
            Color sage = ColorTranslator.FromHtml("#99BC85");
            Color textCol = ColorTranslator.FromHtml("#333333");

            // 2. Form background
            this.BackColor = offWhite;

            // 3. Header bar — use the warm beige
            panelHeader.BackColor = mint;
            label1.ForeColor = textCol;
            label1.Font = new Font("Montserrat", 18, FontStyle.Bold);

            label2.ForeColor = textCol;
            label2.Font = new Font("Montserrat", 18, FontStyle.Bold);

            // 4. Body area — a soft mint background for the form fields
            panelBody.BackColor = beige;

            // 5. Labels — dark text on mint
            foreach (Control ctl in panelBody.Controls)
            {
                if (ctl is Label)
                {
                    ctl.ForeColor = textCol;
                    ctl.Font = new Font("Montserrat", 10, FontStyle.Regular);
                }
            }

            //6.TextBoxes —  off-white
            foreach (Control ctl in panelBody.Controls)
            {
                if (ctl is TextBox tb)
                {
                    tb.BackColor = offWhite;
                    tb.ForeColor = textCol;
                    tb.Font = new Font("Montserrat", 9);
                }
            }

            // 8. Login button — sage green with white text
            btnLogin.BackColor = sage;
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Font = new System.Drawing.Font("Montserrat", 8.0f, FontStyle.Bold);

            btnLogin.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(sage, 0.1f);
        }

        private bool checkNull(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                MessageBox.Show("Please enter some data", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            return false;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void SP_Login_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            this.Hide();
            var dashboard = new SP_Registration();
            dashboard.FormClosed += (s, args) => this.Close();
            dashboard.Show();
        }

        private void panelBody_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
