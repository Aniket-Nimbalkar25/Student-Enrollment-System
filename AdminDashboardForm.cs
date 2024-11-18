using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Enrollment_System
{
    public partial class AdminDashboardForm : Form
    {
        private string connectionString = "Server=ANIKET\\SQLEXPRESS;Initial Catalog=student_enrollment_system;Integrated Security=True;";

        public AdminDashboardForm()
        {
            InitializeComponent();
            LoadStudentData();
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            this.btnAddNewStudent.Click += new System.EventHandler(this.btnAddNewStudent_Click);

            this.btnDeleteStudent.Click += new System.EventHandler(this.btnDeleteStudent_Click);
            this.btnEditStudent.Click += new System.EventHandler(this.btnEditStudent_Click);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);




        }
        private void LoadStudentData()
        {
            string query = "SELECT * FROM Students";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridViewStudents.DataSource = dataTable;
            }
        }
        private void btnAddNewStudent_Click(object sender, EventArgs e)
        {
            StudentEnrollmentForm enrollmentForm = new StudentEnrollmentForm();
            enrollmentForm.ShowDialog();
            LoadStudentData(); 
            // Refresh after adding a new student
        }
        private void btnEditStudent_Click(object sender, EventArgs e)
        {
            if (dataGridViewStudents.SelectedRows.Count > 0)
            {
                int studentId = Convert.ToInt32(dataGridViewStudents.SelectedRows[0].Cells["StudentId"].Value);
                StudentEnrollmentForm enrollmentForm = new StudentEnrollmentForm(studentId);
                enrollmentForm.ShowDialog();
                LoadStudentData();
                // Refresh after editing a student
            }
            else
            {
                MessageBox.Show("Please select a student to edit.");
            }
        }
        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            if (dataGridViewStudents.SelectedRows.Count > 0)
            {
                int studentId = Convert.ToInt32(dataGridViewStudents.SelectedRows[0].Cells["StudentId"].Value);

                // Step 1: Delete student-course relationship from StudentCourses table
                string deleteStudentCoursesQuery = "DELETE FROM StudentCourses WHERE StudentId = @StudentId";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(deleteStudentCoursesQuery, conn);
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                int StudentId = Convert.ToInt32(dataGridViewStudents.SelectedRows[0].Cells["StudentId"].Value);
                string query = "DELETE FROM Students WHERE StudentId = @StudentId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                LogAuditAction("Delete", "Students", studentId, "Admin");

                LoadStudentData(); // Refresh after deleting
            }
            else
            {
                MessageBox.Show("Please select a student to delete.");
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            string query = "SELECT * FROM Students WHERE STUDENTID LIKE @Search OR FIRSTName LIKE @Search";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@Search", "%" + searchQuery + "%");
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridViewStudents.DataSource = dataTable;
            }
        }
        // Event handler for 'Add Payment' button
        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            // Create an instance of the PaymentForm
            Payment_Form paymentForm = new Payment_Form();

            // Show the PaymentForm as a modal (blocks interaction with the Admin Dashboard until closed)
        
           paymentForm.ShowDialog();  // Use Show() if you want it to open non-modally (without blocking the Admin Dashboard)
           
        }


        private void LogAuditAction(string actionType, string tableName, int recordId, string userName)
        {
            string query = "INSERT INTO AuditLogs (ActionType, TableName, RecordId, UserName, Timestamp) " +
                           "VALUES (@ActionType, @TableName, @RecordId, @UserName, @Timestamp)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ActionType", actionType);
                cmd.Parameters.AddWithValue("@TableName", tableName);
                cmd.Parameters.AddWithValue("@RecordId", recordId);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();  // Hide the Admin Dashboard form
            LoginForm loginForm = new LoginForm();
            loginForm.Show();  // Show the Login form again
        }







        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {
           

        }
    }
}

