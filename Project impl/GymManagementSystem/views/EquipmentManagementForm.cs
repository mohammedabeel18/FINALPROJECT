using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // MySQL library for database operations

namespace GymManagementSystem.Views
{
    public partial class EquipmentManagementForm : Form
    {
        // Connection string for connecting to the MySQL database
        private string connectionString = "Server=localhost;Database=gym_management;User ID=root;Password= Asma@123;";

        public EquipmentManagementForm()
        {
            InitializeComponent(); // Initialize UI components
            CreateEquipmentTableIfNotExists(); // Ensure the equipment table exists in the database
        }

        // Event handler for adding new equipment when the add button is clicked
        private void addEquipmentButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve equipment details from text boxes
                string equipmentName = equipmentNameTextBox.Text;
                string equipmentID = equipmentIDTextBox.Text;
                string description = descriptionTextBox.Text;

                // Check if required fields are empty
                if (string.IsNullOrWhiteSpace(equipmentName) || string.IsNullOrWhiteSpace(equipmentID))
                {
                    throw new ArgumentException("Equipment Name and Equipment ID are required fields.");
                }

                // Insert equipment into the database
                AddEquipmentToDatabase(equipmentName, equipmentID, description);

                // Show success message
                MessageBox.Show($"Equipment {equipmentName} added successfully!");
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

        // Method to add equipment details to the database
        private void AddEquipmentToDatabase(string equipmentName, string equipmentID, string description)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open connection to the database

                    // SQL query to insert equipment details
                    string query = "INSERT INTO equipment (EquipmentName, EquipmentID, Description) VALUES (@EquipmentName, @EquipmentID, @Description)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Set parameter values for the query
                        cmd.Parameters.AddWithValue("@EquipmentName", equipmentName);
                        cmd.Parameters.AddWithValue("@EquipmentID", equipmentID);
                        cmd.Parameters.AddWithValue("@Description", description);

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

        // Method to create the equipment table in the database if it does not exist
        private void CreateEquipmentTableIfNotExists()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open connection to the database

                    // SQL query to create equipment table
                    string query = @"
                        CREATE TABLE IF NOT EXISTS equipment (
                            ID INT AUTO_INCREMENT PRIMARY KEY,
                            EquipmentName VARCHAR(100) NOT NULL,
                            EquipmentID VARCHAR(50) UNIQUE,
                            Description TEXT
                        );";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery(); // Execute the query to create the table
                    }
                }
                catch (Exception ex)
                {
                    // Show error message if table creation fails
                    MessageBox.Show($"Error creating table: {ex.Message}");
                }
            }
        }

        // Event handler to display the equipment form when the show button is clicked
        private void showEquipmentButton_Click(object sender, EventArgs e)
        {
            // Create and show the DisplayEquipmentForm
            DisplayEquipmentForm displayEquipmentForm = new DisplayEquipmentForm();
            displayEquipmentForm.Show();
        }
    }
}
