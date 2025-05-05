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
    public partial class AdminDashBoard : Form
    {
        private System.ComponentModel.IContainer components = null;

        private MenuStrip menuStrip;
        private ToolStripMenuItem homeMenuItem;
        private ToolStripMenuItem userManagementMenuItem;
        private ToolStripMenuItem operatorManagementMenuItem;
        private ToolStripMenuItem tourCategoriesMenuItem;
        private ToolStripMenuItem reportsAnalyticsMenuItem;
        private ToolStripMenuItem reviewsManagementMenuItem;
        private ToolStripMenuItem logoutMenuItem;
        private Panel contentPanel;
        private Button viewUsersButton;
        private Button addUserButton;
        private Button viewOperatorsButton;
        private Button generateReportsButton;

        public AdminDashBoard()
        {
            InitializeComponent1();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose of the components container if it exists
                components?.Dispose();

                // Unsubscribe from any event handlers to prevent memory leaks
                viewUsersButton.Click -= ViewUsersButton_Click;
                addUserButton.Click -= AddUserButton_Click;
                viewOperatorsButton.Click -= ViewOperatorsButton_Click;
                generateReportsButton.Click -= GenerateReportsButton_Click;
            }

            // Call the base class Dispose method
            base.Dispose(disposing);
        }

        private void InitializeComponent1()
        {
            this.menuStrip = new MenuStrip();
            this.homeMenuItem = new ToolStripMenuItem();
            this.userManagementMenuItem = new ToolStripMenuItem();
            this.operatorManagementMenuItem = new ToolStripMenuItem();
            this.tourCategoriesMenuItem = new ToolStripMenuItem();
            this.reportsAnalyticsMenuItem = new ToolStripMenuItem();
            this.reviewsManagementMenuItem = new ToolStripMenuItem();
            this.logoutMenuItem = new ToolStripMenuItem();
            this.contentPanel = new Panel();
            this.viewUsersButton = new Button();
            this.addUserButton = new Button();
            this.viewOperatorsButton = new Button();
            this.generateReportsButton = new Button();

            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new ToolStripItem[] {
            this.homeMenuItem,
            this.userManagementMenuItem,
            this.operatorManagementMenuItem,
            this.tourCategoriesMenuItem,
            this.reportsAnalyticsMenuItem,
            this.reviewsManagementMenuItem,
            this.logoutMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(800, 28);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";

            // 
            // menuStrip Items
            // 
            this.homeMenuItem.Text = "Home";
            this.userManagementMenuItem.Text = "User Management";
            this.operatorManagementMenuItem.Text = "Operator Management";
            this.tourCategoriesMenuItem.Text = "Tour Categories";
            this.reportsAnalyticsMenuItem.Text = "Reports/Analytics";
            this.reviewsManagementMenuItem.Text = "Reviews Management";
            this.logoutMenuItem.Text = "Logout";

            this.userManagementMenuItem.Click += new EventHandler(this.UserManagementMenuItem_Click);
            this.operatorManagementMenuItem.Click += new EventHandler(this.OperatorManagementMenuItem_Click);

            // 
            // contentPanel
            // 
            this.contentPanel.Location = new System.Drawing.Point(12, 40);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(776, 400);
            this.contentPanel.TabIndex = 1;

            // 
            // Buttons
            // 
            this.viewUsersButton.Text = "View Users";
            this.addUserButton.Text = "Add User";
            this.viewOperatorsButton.Text = "View Operators";
            this.generateReportsButton.Text = "Generate Reports";

            this.viewUsersButton.Location = new System.Drawing.Point(20, 450);
            this.addUserButton.Location = new System.Drawing.Point(140, 450);
            this.viewOperatorsButton.Location = new System.Drawing.Point(260, 450);
            this.generateReportsButton.Location = new System.Drawing.Point(400, 450);

            // Add Click Event Handlers
            this.viewUsersButton.Click += new EventHandler(this.ViewUsersButton_Click);
            this.addUserButton.Click += new EventHandler(this.AddUserButton_Click);
            this.viewOperatorsButton.Click += new EventHandler(this.ViewOperatorsButton_Click);
            this.generateReportsButton.Click += new EventHandler(this.GenerateReportsButton_Click);

            // 
            // AdminDashBoard
            // 
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.generateReportsButton);
            this.Controls.Add(this.viewOperatorsButton);
            this.Controls.Add(this.addUserButton);
            this.Controls.Add(this.viewUsersButton);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "AdminDashBoard";
            this.Text = "Admin Dashboard";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // Event Handlers
        private void UserManagementMenuItem_Click(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(new Label { Text = "User Management", Location = new System.Drawing.Point(20, 20) });
        }

        private void OperatorManagementMenuItem_Click(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(new Label { Text = "Operator Management", Location = new System.Drawing.Point(20, 20) });
        }

        private void ViewUsersButton_Click(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(new Label { Text = "Displaying all users...", Location = new System.Drawing.Point(20, 20) });
        }

        private void AddUserButton_Click(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(new Label { Text = "Add a new user here", Location = new System.Drawing.Point(20, 20) });
        }

        private void ViewOperatorsButton_Click(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(new Label { Text = "Displaying all operators...", Location = new System.Drawing.Point(20, 20) });
        }

        private void GenerateReportsButton_Click(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(new Label { Text = "Generating reports...", Location = new System.Drawing.Point(20, 20) });
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AdminDashBoard
            // 
            this.ClientSize = new System.Drawing.Size(278, 244);
            this.Name = "AdminDashBoard";
            this.Load += new System.EventHandler(this.AdminDashBoard_Load);
            this.ResumeLayout(false);

        }

        private void AdminDashBoard_Load(object sender, EventArgs e)
        {

        }
    }
}
