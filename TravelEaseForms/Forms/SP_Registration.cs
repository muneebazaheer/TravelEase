using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TravelEaseForms.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

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
                    //ctl.Font = new Font("Montserrat", 10, FontStyle.Regular);
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

        private bool checkNull(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                MessageBox.Show("Please enter some data", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            return false;
        }
        private bool validateEmail(string data)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(data);
                return addr.Address == data;
            }
            catch
            {
                return false;
            }

        }

        private bool validatePassword(string data)
        {
             if (data.Length < 8 ||
                !data.Any(char.IsUpper) ||
                !data.Any(char.IsLower) ||
                !data.Any(char.IsDigit) ||
                !Regex.IsMatch(data, @"[@$!%*?&!._^#]"))
            {
                return false;
            }
            return true;
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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            //Make sure none of the text boxes are empty
            if (checkNull(textBox1.Text) || checkNull(textBox2.Text) ||
                checkNull(textBox3.Text) || checkNull(textBox4.Text) ||
                checkNull(textBox5.Text))
            {
                return;
            }

            //email validation
            if (!validateEmail(textBox2.Text))
            {
                MessageBox.Show("Please enter a valid email address", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }

            // City Code validation - 2 uppercase letters only
            if (!Regex.IsMatch(textBox5.Text, @"^[A-Z]{2}$"))
            {
                MessageBox.Show("City code must be exactly 2 uppercase letters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox5.Focus();
                return;
            }

            // Contact validation - numbers only
            if (!Regex.IsMatch(textBox3.Text, @"^\d+$"))
            {
                MessageBox.Show("Contact must contain only numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return;
            }

            //Password validation
            if (!validatePassword(textBox6.Text))
            {
                MessageBox.Show("Password must be at least 8 characters and contain uppercase, lowercase, number, and special character.",
                 "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox6.Focus();
                return;
            }

            // Insert into database
            string connectionString = "Data Source=DESKTOP-7HGU5BP\\SQLEXPRESS;Initial Catalog=TravelEaseDataBase;Integrated Security=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string userId;

                            // Insert into Users table
                            string userInsert = @"INSERT INTO Users (CityCode, Email, Address, Name, ContactInfo) 
                                     VALUES (@CityCode, @Email, @Address, @Name, @ContactInfo);
                                     SELECT UserID FROM Users WHERE UserNumber = SCOPE_IDENTITY();";

                            using (SqlCommand command = new SqlCommand(userInsert, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@Name", textBox1.Text);
                                command.Parameters.AddWithValue("@Email", textBox2.Text);
                                command.Parameters.AddWithValue("@Address", textBox4.Text);
                                command.Parameters.AddWithValue("@CityCode", textBox5.Text.ToUpper());
                                command.Parameters.AddWithValue("@ContactInfo", textBox3.Text);

                                // Get the auto-generated UserID
                                userId = command.ExecuteScalar()?.ToString();
                            }

                            // Insert into ServiceProviders table
                            string spInsert = @"INSERT INTO ServiceProviders (UserID, Password) 
                                   VALUES (@UserID, @Password)";

                            using (SqlCommand command = new SqlCommand(spInsert, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@UserID", userId);
                                command.Parameters.AddWithValue("@Password", textBox6.Text); 

                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Both inserts successful, commit transaction
                                    transaction.Commit();

                                    MessageBox.Show($"Registration successful!\nUserID: {userId}\nProviderID: SP{userId}",
                                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    this.Hide();
                                    var dashboard = new SP_Main();
                                    dashboard.FormClosed += (s, args) => this.Close();
                                    dashboard.Show();
                                }
                                else
                                {
                                    transaction.Rollback();
                                    MessageBox.Show("Registration failed. Please try again.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SP_Registration_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();

            var dashboard = new SP_Login();

            dashboard.FormClosed += (s, args) => this.Close();

            dashboard.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void panelBody_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
