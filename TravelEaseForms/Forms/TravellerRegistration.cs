using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        }

        private void TravellerRegistration_Load(object sender, EventArgs e)
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
            //Craete an instance of the LoginForm
            TravellerLogin loginForm = new TravellerLogin();
            loginForm.Show();
            this.Hide();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string name = nameTextbox.Text;
            string email = emailtextbox.Text;
            string password = passwordTextbox.Text;
            string contact = textBox4.Text;
            string address = textBox5.Text;
            string cityCode = textBox6.Text;
            DateTime dateTime = DateTime.Now;

            string connectionString = "Data Source=MAHAD\\SQLEXPRESS01;Initial Catalog=TravelEaseDatabase;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Insert into Users table
                string userInsertQuery = @"
                    INSERT INTO Users (CityCode, Email, Address, Name, ContactInfo, RegistrationDate)
                    VALUES (@CityCode, @Email, @Address, @Name, @ContactInfo, @RegistrationDate);";


                using (SqlCommand cmd = new SqlCommand(userInsertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@CityCode", cityCode);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@ContactInfo", contact);
                    cmd.Parameters.AddWithValue("@RegistrationDate", dateTime.Date);
                    cmd.ExecuteNonQuery();
                }

                string userIDQuery = "SELECT UserID FROM Users WHERE Email = @Email";
                string userID;
                using (SqlCommand cmd = new SqlCommand(userIDQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    userID = (string)cmd.ExecuteScalar();
                }

                string personInsertQuery = @"
                    INSERT INTO Persons (UserID, CNIC_SSN, Nationality, DOB)
                    VALUES (@UserID, @CNIC_SSN, @Nationality, @DOB);
                ";
                using (SqlCommand cmd = new SqlCommand(personInsertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@CNIC_SSN", "example_cnic");
                    cmd.Parameters.AddWithValue("@Nationality", "example_nationality");
                    cmd.Parameters.AddWithValue("@DOB", DateTime.Now.Date);
                    cmd.ExecuteNonQuery();
                }

                string travelerInsertQuery = @"
                    INSERT INTO Traveler (UserID, LoginPassword)
                    VALUES (@UserID, @LoginPassword);";

                using (SqlCommand cmd = new SqlCommand(travelerInsertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@LoginPassword", password);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Registration successful!");
            }

            this.Hide();
            TravellerLogin logintraveler = new TravellerLogin();
            logintraveler.Show();
        }
    }
}

