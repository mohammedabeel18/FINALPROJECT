using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // MySQL library for database operations

namespace GymManagementSystem.Views
{
    public partial class DisplayEquipmentForm : Form
    {
        // Connection string for connecting to the MySQL database
        private string connectionString = "Server=localhost;Database=gym_management;User ID=root;Password=Asma@123;";
        private DataTable equipmentTable; // DataTable to hold equipment data

        public DisplayEquipmentForm()
        {
            InitializeComponent(); // Initialize UI components
            LoadEquipmentData(); // Load equipment data into DataGridView
        }

        // Method to load equipment data from the database into the DataGridView
        private void LoadEquipmentData()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open connection to the database

                    // SQL query to select all equipment
                    string query = "SELECT * FROM equipment";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);

                    equipmentTable = new DataTable(); // Initialize DataTable
                    adapter.Fill(equipmentTable); // Fill the DataTable with query results

                    equipmentDataGridView.DataSource = equipmentTable; // Bind DataTable to DataGridView
                }
                catch (Exception ex)
                {
                    // Show error message if loading data fails
                    MessageBox.Show($"Error loading data: {ex.Message}");
                }
            }
        }

        // Event handler for deleting selected equipment from the DataGridView and database
        private void deleteEquipmentButton_Click(object sender, EventArgs e)
        {
            // Check if any rows are selected
            if (equipmentDataGridView.SelectedRows.Count > 0)
            {
                // Loop through selected rows
                foreach (DataGridViewRow row in equipmentDataGridView.SelectedRows)
                {
                    // Get the equipment ID from the selected row
                    int equipmentId = Convert.ToInt32(row.Cells["ID"].Value);
                    DeleteEquipment(equipmentId); // Delete the equipment from the database
                }
                LoadEquipmentData(); // Refresh data after deletion
            }
            else
            {
                // Show message if no rows are selected
                MessageBox.Show("Please select an equipment to delete.");
            }
        }

        // Method to delete equipment from the database using its ID
        private void DeleteEquipment(int equipmentId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open connection to the database

                    // SQL query to delete equipment by ID
                    string query = "DELETE FROM equipment WHERE ID = @ID";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Set the parameter value for the query
                        cmd.Parameters.AddWithValue("@ID", equipmentId);
                        cmd.ExecuteNonQuery(); // Execute the query
                    }
                }
                catch (Exception ex)
                {
                    // Show error message if deletion fails
                    MessageBox.Show($"Error deleting equipment: {ex.Message}");
                }
            }
        }
    }
}
