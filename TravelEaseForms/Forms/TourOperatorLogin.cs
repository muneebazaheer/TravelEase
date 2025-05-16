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
    public partial class TourOperatorLogin : Form
    {
        public TourOperatorLogin()
        {
            InitializeComponent();
        }

        private void TourOperatorLogin_Load(object sender, EventArgs e)
        {
            LoginButton.FlatStyle = FlatStyle.Flat;
            LoginButton.BackColor = ColorTranslator.FromHtml("#99BC85");
            LoginButton.ForeColor = Color.White;
            LoginButton.Font = new Font(LoginButton.Font.FontFamily, LoginButton.Font.Size, FontStyle.Bold);
            this.BackColor = ColorTranslator.FromHtml("#FAF1E6");
            RegisterLinkLabel.LinkColor = ColorTranslator.FromHtml("#99BC85");
            RegisterLinkLabel.LinkBehavior = LinkBehavior.NeverUnderline;
        }

        private void RegisterLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TourOperatorRegistration registrationForm = new TourOperatorRegistration();
            registrationForm.Show();
            this.Hide();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {

            string connectionString = "Data Source=MAHAD\\SQLEXPRESS01;Initial Catalog=TravelEaseDatabase;Integrated Security=True";
            //string email = emailtextbox.Text;
            //string password = passwordtextbox.Text;
            string email = "frank.wright2@example.com";
            string password = "Password1";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users U JOIN TourOperators T ON U.UserID = T.UserID WHERE U.Email = @email AND T.LoginPassword = @password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                con.Open();
                int result = (int)cmd.ExecuteScalar();

                if (result > 0)
                {
                    // Credentials are valid
                    SessionDetails.email = email;
                    SessionDetails.password = password;

                    // Fetch full user info
                    string userDetailsQuery = @"SELECT U.* 
                                                FROM Users U 
                                                JOIN TourOperators T ON U.UserID = T.UserID 
                                                WHERE U.Email = @Email AND T.LoginPassword = @Password";

                    SqlCommand userCmd = new SqlCommand(userDetailsQuery, con);
                    userCmd.Parameters.AddWithValue("@Email", email);
                    userCmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = userCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            SessionDetails.UserId = reader["UserID"].ToString();
                            SessionDetails.UserName = reader["Name"].ToString();
                            SessionDetails.Address = reader["Address"].ToString();

                        }
                    }
                    //MessageBox.Show(SessionDetails.UserId);
                    //MessageBox.Show(SessionDetails.UserName);
                    //MessageBox.Show(SessionDetails.Address);
                    TourOperatorDashboard dashboardForm = new TourOperatorDashboard();
                    dashboardForm.Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
        }
    }
}
