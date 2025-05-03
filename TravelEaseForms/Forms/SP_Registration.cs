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
    public partial class SP_Registration : Form
    {
        public SP_Registration()
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

            // 8. Register button — sage green with white text
            btnRegister.BackColor = sage;
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Font = new System.Drawing.Font("Montserrat", 8.0f, FontStyle.Bold);

            btnRegister.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(sage, 0.1f);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
