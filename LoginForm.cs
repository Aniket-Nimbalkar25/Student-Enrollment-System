using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Enrollment_System
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            
            txtPassword.PasswordChar = '*'; 


        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Simple username and password validation (hardcoded)
            if (username == "admin" && password == "123")
            {
                MessageBox.Show("Login Successful!");
                this.Hide();  // Hide the login form
                AdminDashboardForm dashboard = new AdminDashboardForm();
                dashboard.Show();  // Show the Admin Dashboard form
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

    }
}
