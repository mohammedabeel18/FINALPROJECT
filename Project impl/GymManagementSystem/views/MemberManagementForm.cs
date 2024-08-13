using System;
using System.Windows.Forms; // For Windows Forms functionality
using MySql.Data.MySqlClient; // For MySQL database connectivity

namespace GymManagementSystem.Views
{
    public partial class MemberManagementForm : Form
    {
        // Connection string for the MySQL database
        private string connectionString = "Server=localhost;Database=gym_management;User ID=root;Password=Asma@123;";

        public MemberManagementForm()
        {
            InitializeComponent(); // Initialize UI components
            CreateMembersTableIfNotExists(); // Ensure the members table exists
        }

        // Event handler for adding a new member
        private void addMemberButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve input values from the form
                string name = nameTextBox.Text;
                string address = addressTextBox.Text;
                string phoneNumber = phoneTextBox.Text;
                string memberID = memberIDTextBox.Text;
                string membershipType = membershipTypeComboBox.SelectedItem?.ToString(); // Ensure there's a selected item

                // Check for required fields
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
                {
                    throw new ArgumentException("Name and Address are required fields.");
                }

                // Insert member into database
                AddMemberToDatabase(name, address, phoneNumber, memberID, membershipType);

                MessageBox.Show($"Member {name} added successfully!");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Method to add a member to the database
        private void AddMemberToDatabase(string name, string address, string phoneNumber, string memberID, string membershipType)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open the connection to the database
                    
                    // SQL query to insert a new member
                    string query = "INSERT INTO members (Name, Address, PhoneNumber, MemberID, MembershipType) VALUES (@Name, @Address, @PhoneNumber, @MemberID, @MembershipType)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Set the parameter values
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        cmd.Parameters.AddWithValue("@MembershipType", membershipType);
                        
                        cmd.ExecuteNonQuery(); // Execute the insert query
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}");
                }
            }
        }

        // Method to create the members table if it does not exist
        private void CreateMembersTableIfNotExists()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open the connection to the database
                    
                    // SQL query to create the members table if it doesn't exist
                    string query = @"
                        CREATE TABLE IF NOT EXISTS members (
                            ID INT AUTO_INCREMENT PRIMARY KEY,
                            Name VARCHAR(100) NOT NULL,
                            Address VARCHAR(255) NOT NULL,
                            PhoneNumber VARCHAR(20),
                            MemberID VARCHAR(50) UNIQUE,
                            MembershipType VARCHAR(50)
                        );";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery(); // Execute the create table query
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating table: {ex.Message}");
                }
            }
        }

        // Event handler for showing the list of members
        private void showMembersButton_Click(object sender, EventArgs e)
        {
            MemberListForm memberListForm = new MemberListForm(); // Create a new instance of the MemberListForm
            memberListForm.Show(); // Show the MemberListForm
        }
    }
}
