using System;
using System.Windows.Forms; // Provides classes for creating Windows-based applications
using MySql.Data.MySqlClient; // Provides classes for connecting to a MySQL database

namespace GymManagementSystem.Views
{
    public partial class StaffManagementForm : Form
    {
        // Connection string for connecting to the MySQL database
        private string connectionString = "Server=localhost;Database=gym_management;User ID=root;Password=Asma@123 ;";

        public StaffManagementForm()
        {
            InitializeComponent(); // Initialize UI components
            CreateStaffTableIfNotExists(); // Ensure the staff table exists
        }

        // Event handler for adding a staff member
        private void addStaffButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve data from the text boxes
                string name = nameTextBox.Text;
                string address = addressTextBox.Text;
                string phoneNumber = phoneTextBox.Text;
                string staffID = staffIDTextBox.Text;
                string role = roleTextBox.Text;

                // Check for required fields
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
                {
                    throw new ArgumentException("Name and Address are required fields.");
                }

                // Insert staff into database
                AddStaffToDatabase(name, address, phoneNumber, staffID, role);

                MessageBox.Show($"Staff {name} added successfully!"); // Notify user of success
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Error: {ex.Message}"); // Notify user of input errors
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}"); // Notify user of general errors
            }
        }

        // Method to insert staff into the database
        private void AddStaffToDatabase(string name, string address, string phoneNumber, string staffID, string role)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open the connection to the database
                    
                    // Query to insert a new staff record
                    string query = "INSERT INTO staff (Name, Address, PhoneNumber, StaffID, Role) VALUES (@Name, @Address, @PhoneNumber, @StaffID, @Role)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Add parameters to the command
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@StaffID", staffID);
                        cmd.Parameters.AddWithValue("@Role", role);
                        
                        cmd.ExecuteNonQuery(); // Execute the insert command
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}"); // Notify user of database errors
                }
            }
        }

        // Method to create the 'staff' table if it doesn't already exist
        private void CreateStaffTableIfNotExists()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open the connection to the database
                    
                    // Query to create the 'staff' table
                    string query = @"
                        CREATE TABLE IF NOT EXISTS staff (
                            ID INT AUTO_INCREMENT PRIMARY KEY,
                            Name VARCHAR(100) NOT NULL,
                            Address VARCHAR(255) NOT NULL,
                            PhoneNumber VARCHAR(20),
                            StaffID VARCHAR(50) UNIQUE,
                            Role VARCHAR(100)
                        );";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery(); // Execute the create table command
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating table: {ex.Message}"); // Notify user of table creation errors
                }
            }
        }

        // Event handler for showing the staff list
        private void showStaffButton_Click(object sender, EventArgs e)
        {
            DisplayStaffForm displayStaffForm = new DisplayStaffForm(); // Create an instance of DisplayStaffForm
            displayStaffForm.Show(); // Show the staff list form
        }
    }
}
