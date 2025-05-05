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
    public partial class TravellerRegistration : Form
    {
        public TravellerRegistration()
        {
            InitializeComponent();
            this.Load += TravellerRegistration_Load;
        }

        private void TravellerRegistration_Load(object sender, EventArgs e)
        {
            this.Text = "User Registration";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#FAF1E6");



            string[] labels = {
                "Full Name", "CNIC", "Date of Birth", "Nationality", "Email", "Password", "Confirm Password"
            };

            int y = 30;
            int labelWidth = 120;
            int textboxWidth = 250;

            for (int i = 0; i < labels.Length; i++)
            {
                // Label
                Label lbl = new Label();
                lbl.Text = labels[i];
                lbl.Location = new Point(50, y);
                lbl.Size = new Size(labelWidth, 25);
                lbl.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                this.Controls.Add(lbl);

                // TextBox or DateTimePicker
                Control input;
                if (labels[i] == "Date of Birth")
                {
                    DateTimePicker dobPicker = new DateTimePicker();
                    dobPicker.Location = new Point(180, y);
                    dobPicker.Size = new Size(textboxWidth, 30);
                    dobPicker.Format = DateTimePickerFormat.Short;
                    input = dobPicker;
                }
                else
                {
                    TextBox txt = new TextBox();
                    txt.Location = new Point(180, y);
                    txt.Size = new Size(textboxWidth, 30);
                    txt.Multiline = true;

                    if (labels[i].ToLower().Contains("password"))
                        txt.PasswordChar = '*';

                    input = txt;
                }

                this.Controls.Add(input);
                y += 50;
            }

            // Register Button
            Button registerBtn = new Button();
            registerBtn.Text = "Register";
            registerBtn.Size = new Size(150, 40);
            registerBtn.Location = new Point(170, y + 10);
            registerBtn.BackColor = ColorTranslator.FromHtml("#99BC85");
            registerBtn.ForeColor = Color.White;
            registerBtn.FlatStyle = FlatStyle.Flat;
            registerBtn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            registerBtn.Click += RegisterBtn_Click;

            this.Controls.Add(registerBtn);
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
