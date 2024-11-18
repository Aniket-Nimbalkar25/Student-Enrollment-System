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

namespace Student_Enrollment_System
{
    public partial class Payment_Form : Form
    {
        private string connectionString = "Server=ANIKET\\SQLEXPRESS;Initial Catalog=student_enrollment_system;Integrated Security=True;";
        public Payment_Form()
        {
            InitializeComponent();
        }
        private void LoadPayments()
        {
            string query = "SELECT PaymentId, StudentId, Amount, PaymentDate, PaymentMethod, Status FROM Payments";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridViewPayments.DataSource = dataTable;
            }
        }

        private void LoadAuditLogs()
        {
            string query = "SELECT LogId, ActionType, TableName, RecordId, UserName, ActionMessage, Timestamp FROM AuditLogs";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridViewAuditLogs.DataSource = dataTable;
            }
        }

        private void ProcessPayment(int studentId, decimal amount, string paymentMethod)
        {
            // Insert Payment record
            string paymentQuery = "INSERT INTO Payments (StudentId, Amount, PaymentDate, PaymentMethod, Status) " +
                                  "VALUES (@StudentId, @Amount, @PaymentDate, @PaymentMethod, @Status)";

            // Log action in AuditLogs table
            LogAuditAction("Insert", "Payments", studentId, "Admin", $"Processed payment of {amount} for StudentId: {studentId}");

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(paymentQuery, conn))
                    {
                        
                        cmd.Parameters.AddWithValue("@StudentId", studentId);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                        cmd.Parameters.AddWithValue("@Status", "Successful");

                        // Execute the query
                        cmd.ExecuteNonQuery();

                        lblMessage.Text = "Payment processed successfully!";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error processing payment: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        
        private void LogAuditAction(string actionType, string tableName, int recordId, string userName, string actionMessage)
        {
            string query = "INSERT INTO AuditLogs (ActionType, TableName, RecordId, UserName, ActionMessage, Timestamp) " +
                           "VALUES (@ActionType, @TableName, @RecordId, @UserName, @ActionMessage, @Timestamp)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ActionType", actionType);
                cmd.Parameters.AddWithValue("@TableName", tableName);
                cmd.Parameters.AddWithValue("@RecordId", recordId);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@ActionMessage", actionMessage);
                cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPayments();
            LoadAuditLogs();
        }

        private void btnProcessPayment_Click(object sender, EventArgs e)
        {
            int studentId = Convert.ToInt32(txtStudentId.Text.Trim());
            decimal amount = Convert.ToDecimal(txtAmount.Text.Trim());
            string paymentMethod = cmbMethod.Text.Trim();

            ProcessPayment(studentId, amount, paymentMethod);
        }


        private void Payment_Form_Load(object sender, EventArgs e)
        {
            cmbMethod.Items.Add("Cash");
            cmbMethod.Items.Add("Card");
            cmbMethod.Items.Add("UPI");
            cmbMethod.SelectedIndex = -1;

        }

        private void cmbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}
