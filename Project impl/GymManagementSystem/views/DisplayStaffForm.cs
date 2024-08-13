using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // MySQL library for database operations

namespace GymManagementSystem.Views
{
    public partial class DisplayStaffForm : Form
    {
        // Connection string for connecting to the MySQL database
        private string connectionString = "Server=localhost;Database=gym_management;User ID=cprg250;Password=password;";

        public DisplayStaffForm()
        {
            InitializeComponent(); // Initialize UI components
            LoadStaffData(); // Load staff data into DataGridView
        }

        // Method to load staff data from the database into the DataGridView
        private void LoadStaffData()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open connection to the database
                    
                    // SQL query to select staff details
                    string query = "SELECT ID, Name, Address, PhoneNumber, StaffID, Role FROM staff";
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        DataTable staffTable = new DataTable(); // Initialize DataTable
                        adapter.Fill(staffTable); // Fill the DataTable with query results
                        staffDataGridView.DataSource = staffTable; // Bind DataTable to DataGridView
                    }
                }
                catch (Exception ex)
                {
                    // Show error message if loading data fails
                    MessageBox.Show($"Database error: {ex.Message}");
                }
            }
        }

        // Event handler for deleting selected staff members from the DataGridView and database
        private void deleteButton_Click(object sender, EventArgs e)
        {
            // Check if any rows are selected
            if (staffDataGridView.SelectedRows.Count > 0)
            {
                try
                {
                    // Get the index of the selected row and extract the staff ID
                    int selectedRowIndex = staffDataGridView.SelectedCells[0].RowIndex;
                    int staffID = Convert.ToInt32(staffDataGridView.Rows[selectedRowIndex].Cells["ID"].Value);

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open(); // Open connection to the database
                        
                        // SQL query to delete a staff member by ID
                        string query = "DELETE FROM staff WHERE ID = @ID";
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            // Set the parameter value for the query
                            cmd.Parameters.AddWithValue("@ID", staffID);
                            cmd.ExecuteNonQuery(); // Execute the query
                        }
                    }

                    // Refresh the data grid view after deletion
                    LoadStaffData();
                    // Show success message
                    MessageBox.Show("Staff member deleted successfully!");
                }
                catch (Exception ex)
                {
                    // Show error message if deletion fails
                    MessageBox.Show($"Error deleting staff member: {ex.Message}");
                }
            }
            else
            {
                // Show message if no rows are selected
                MessageBox.Show("Please select a staff member to delete.");
            }
        }
    }
}
