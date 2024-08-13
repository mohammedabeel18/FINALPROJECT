using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // MySQL library for database operations
using iTextSharp.text; // Library for PDF document generation
using iTextSharp.text.pdf; // Library for PDF document generation
using System.IO; // For file operations

namespace GymManagementSystem.Views
{
    public partial class FitnessClassManagementForm : Form
    {
        // Connection string for connecting to the MySQL database
        private string connectionString = "Server=localhost;Database=gym_management;User ID=root;Password=Asma@123;";

        public FitnessClassManagementForm()
        {
            InitializeComponent(); // Initialize UI components
        }

        // Event handler for adding a new fitness class when the add button is clicked
        private void addClassButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve class details from text boxes and date pickers
                string className = classNameTextBox.Text;
                string instructor = instructorTextBox.Text;
                DateTime startTime = startTimePicker.Value;
                DateTime endTime = endTimePicker.Value;

                // Check if required fields are empty
                if (string.IsNullOrWhiteSpace(className) || string.IsNullOrWhiteSpace(instructor))
                {
                    throw new ArgumentException("Class Name and Instructor are required fields.");
                }

                // Add the class to the database
                AddClassToDatabase(className, instructor, startTime, endTime);

                // Show success message
                MessageBox.Show($"Class {className} added successfully!");
            }
            catch (ArgumentException ex)
            {
                // Show error message if there is an issue with the input
                MessageBox.Show($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Show error message if any other exception occurs
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Method to add class details to the database
        private void AddClassToDatabase(string className, string instructor, DateTime startTime, DateTime endTime)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open connection to the database
                    
                    // SQL query to insert class details
                    string query = "INSERT INTO fitness_classes (ClassName, Instructor, StartTime, EndTime) VALUES (@ClassName, @Instructor, @StartTime, @EndTime)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Set parameter values for the query
                        cmd.Parameters.AddWithValue("@ClassName", className);
                        cmd.Parameters.AddWithValue("@Instructor", instructor);
                        cmd.Parameters.AddWithValue("@StartTime", startTime);
                        cmd.Parameters.AddWithValue("@EndTime", endTime);
                        
                        cmd.ExecuteNonQuery(); // Execute the query
                    }
                }
                catch (Exception ex)
                {
                    // Show error message if a database error occurs
                    MessageBox.Show($"Database error: {ex.Message}");
                }
            }
        }

        // Event handler to display the class list form when the show button is clicked
        private void showClassesButton_Click(object sender, EventArgs e)
        {
            // Create and show the ClassListForm
            ClassListForm classListForm = new ClassListForm();
            classListForm.Show();
        }
    }
}
