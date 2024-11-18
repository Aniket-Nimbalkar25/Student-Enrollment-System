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
    public partial class StudentSearchForm : Form
    {
        private string connectionString = "Server=ANIKET\\SQLEXPRESS;Initial Catalog=student_enrollment_system;Integrated Security=True;";
        public StudentSearchForm()
        {
            InitializeComponent();

        }

        private void StudentSearchForm_Load(object sender, EventArgs e)
        {

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            string query = "SELECT * FROM Students WHERE FirstName LIKE @Search OR LastName LIKE @Search";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@Search", "%" + searchQuery + "%");
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridViewResults.DataSource = dataTable;
            }
        }
    }
}
