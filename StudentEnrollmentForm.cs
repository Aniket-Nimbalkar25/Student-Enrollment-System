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
    public partial class StudentEnrollmentForm : Form
    {
        private int studentId; // 0 means adding new student, > 0 means editing an existing student
        private string connectionString = "Server=ANIKET\\SQLEXPRESS;Initial Catalog=student_enrollment_system;Integrated Security=True;";

        

        public StudentEnrollmentForm(int studentId = 0)
        {
            InitializeComponent();
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            cmbCourse.DisplayMember = "CourseName";
            cmbCourse.ValueMember = "CourseId";
      
            this.studentId = studentId;

            if (studentId > 0)
            {
                LoadStudentData(studentId); // Load existing student data for editing
            }
            LoadCourses();
        }

        private void LoadStudentData(int studentId)
        {
            string query = "SELECT * FROM Students WHERE StudentId = @StudentId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtFirstName.Text = reader["FirstName"].ToString();
                    txtLastName.Text = reader["LastName"].ToString();
                    dateTimePickerDOB.Value = Convert.ToDateTime(reader["DOB"]);
                    cmbGender.SelectedItem = reader["Gender"].ToString();
                    cmbCourse.SelectedValue = reader["CourseId"];
                    dateTimePickerEnrollmentDate.Value = Convert.ToDateTime(reader["EnrollmentDate"]);


                }
            }
          
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            DateTime dob = dateTimePickerDOB.Value;
            string gender = cmbGender.SelectedItem?.ToString();
            DateTime enrollmentDate = dateTimePickerEnrollmentDate.Value;
            
            cmbCourse.SelectedValue.ToString();
            int courseId = Convert.ToInt32(cmbCourse.SelectedValue);  
            if (cmbCourse.SelectedValue == null)
            {
                MessageBox.Show("Please select a course.");
                return;
            }
           
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(gender) || courseId == 0)
            {
                MessageBox.Show("Please fill in all the fields.");
                return; 
            }

            string query = "";
            if (studentId > 0) 
            {
                query = "UPDATE Students SET FirstName = @FirstName, LastName = @LastName, DOB = @DOB, Gender = @Gender, EnrollmentDate=@EnrollmentDate, " +  
                      "CourseId = @CourseId WHERE StudentId = @StudentId";
            }
            else 
            {
                query = "INSERT INTO Students (FirstName, LastName, DOB, Gender, CourseId,EnrollmentDate) VALUES (@FirstName, @LastName, @DOB, @Gender, @CourseId,@EnrollmentDate)";
            }

          
            MessageBox.Show($"Saving student: {firstName} {lastName}, Gender: {gender}, DOB: {dob.ToShortDateString()}, Course ID: {courseId}, EnrollmentDate:{enrollmentDate.ToShortDateString()}");

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@DOB", dob);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    cmd.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);

                    if (studentId > 0)
                    {
                        cmd.Parameters.AddWithValue("@StudentId", studentId); 
                    }

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Student saved successfully!");
                        this.Close(); 
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while saving the student.");
                    }
                    DateTime enrollmentdate = dateTimePickerEnrollmentDate.Value;
                }
            }
            catch (Exception ex)
            {
               
                MessageBox.Show("Error: " + ex.Message);
            }

        }


        public StudentEnrollmentForm()
        {
            InitializeComponent();
            LoadCourses();
        }

        private void StudentEnrollmentForm_Load(object sender, EventArgs e)
        {
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");
            cmbGender.Items.Add("Other");
            cmbGender.SelectedIndex = -1;

        }

        private void LoadCourses()
        {
            string query = "SELECT * FROM Courses"; // Select CourseId and CourseName from Courses table

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind the courses to the ComboBox
                    cmbCourse.DisplayMember= "CourseName";  // What will be displayed in the ComboBox
                   cmbCourse.ValueMember = "CourseId"; 
                    cmbCourse.DataSource = dt;
                    // The value that will be used for the course ID
                    cmbCourse.DataSource = dt;

                    cmbCourse.SelectedIndex = -1; // Default to no selection
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading courses: " + ex.Message);
                }
            }
        }
    }
}
