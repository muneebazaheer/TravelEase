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
    public partial class TravelerDashboard : Form
    {
        public TravelerDashboard()
        {
            InitializeComponent();
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {

        }

        private void TravelerDashboard_Load(object sender, EventArgs e)
        {
            Greenbgrect.BackColor = ColorTranslator.FromHtml("#99BC85");
            profilePanel.BackColor = ColorTranslator.FromHtml("#E4EFE7");
            ReviewRatingButton.UseCompatibleTextRendering = true;
            ProfileButton.BackColor = ColorTranslator.FromHtml("#FAF1E6");
            ReviewRatingButton.BackColor = ColorTranslator.FromHtml("#FAF1E6");
            travelpassButton.BackColor = ColorTranslator.FromHtml("#FAF1E6");
            TripSearchButtton.BackColor = ColorTranslator.FromHtml("#FAF1E6");
            NamedisplayLabel.Text = SessionDetails.UserName;
            label1.Text = SessionDetails.email;
            label2.Text = SessionDetails.Address;
            TripsPanel.BackColor = ColorTranslator.FromHtml("#E4EFE7");
            passPanel.BackColor = ColorTranslator.FromHtml("#E4EFE7");

        }
        private void ReviewRatingButton_Click(object sender, EventArgs e)
        {
            passPanel.Hide();
            profilePanel.Hide();
            TripsPanel.Hide();
            panel1.Show();
        }

        private void TripSearchButtton_Click(object sender, EventArgs e)
        {
           profilePanel.Hide();
           TripsPanel.Show();
            passPanel.Hide();
            panel1.Hide();
        }

        private void profilePanel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

        }

        private string BuildSortedToursQuery(string selectedField, string selectedOrder)
        {
            string order = selectedOrder.ToUpper(); // ASC or DESC
            string query = "";

            if (selectedField == "Departure Date")
            {
                query = "SELECT * from Tours";
            }
            else if (selectedField == "Departure Place")
            {
                query = "SELECT * from Tours";
            }
            else if (selectedField == "Destination")
            {
                query = "SELECT * from Tours";
            }
            else if (selectedField == "Rating")
            {
                query = "SELECT * from Tours";
            }
            else if (selectedField == "Price")
            {
                query = "SELECT * from Tours";
            }
            else if (selectedField == "Tour Duration")
            {
                query = "SELECT * from Tours";
            }
            else
            {
                MessageBox.Show("Invalid selection");
            }

            return query;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string connectionString = "Data Source=MAHAD\\SQLEXPRESS01;Initial Catalog=TravelEaseDatabase;Integrated Security=True";
            string combo1sel= comboBox1.SelectedItem != null ? comboBox1.SelectedItem.ToString() : "";
            string combo2sel = comboBox2.SelectedItem != null ? comboBox2.SelectedItem.ToString() : "";
            string query = BuildSortedToursQuery(combo1sel, combo2sel);

            if (query != "")
            {
                List<TripInfoClass> triplist = new List<TripInfoClass>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                             TripInfoClass t = new TripInfoClass();
                            {
                                t.type = reader["Type"].ToString();
                                t.desc = reader["Description"].ToString();
                                t.EndDate = Convert.ToDateTime(reader["EndDate"]);
                                t.rating =Convert.ToDouble( reader["Rating"]);
                                t.cost = Convert.ToInt32(reader["Cost"]);
                                t.StartDate = Convert.ToDateTime(reader["StartDate"]);
                                t.departurePlace = reader["DeparturePlace"].ToString();
                                t.Destination = reader["Destination"].ToString();

                            }
                            ;
                            triplist.Add(t);
                        }
                        dataGridView1.DataSource = triplist;
                        dataGridView2.DataSource = triplist;

                    }
                }

            }
            else
            {
                MessageBox.Show("Please select a valid option.");
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void ProfileButton_Click(object sender, EventArgs e)
        {
            profilePanel.Show();
            TripsPanel.Hide();
            passPanel.Hide();
            panel1.Hide();
        }

        private void travelpassButton_Click(object sender, EventArgs e)
        {
            profilePanel.Hide();
            TripsPanel.Hide();
            passPanel.Show();
            panel1.Hide();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
