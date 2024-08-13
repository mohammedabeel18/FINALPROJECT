using System;
using System.Windows.Forms; // Provides classes for creating Windows-based applications
using MySql.Data.MySqlClient; // Provides classes for connecting to a MySQL database

namespace GymManagementSystem.Views
{
    public partial class MembershipPaymentForm : Form
    {
        // Connection string for the MySQL database
        private string connectionString = "Server=localhost;Database=gym_management;User ID=root;Password=Asma@123;";

        public MembershipPaymentForm()
        {
            InitializeComponent(); // Initialize UI components
            CreateTablesIfNotExists(); // Ensure the members table exists
        }

        // Event handler for processing a payment
        private void processPaymentButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve input values from the form
                string memberID = memberIDTextBox.Text;
                decimal amount = decimal.Parse(amountTextBox.Text);

                // Validate inputs
                if (string.IsNullOrWhiteSpace(memberID) || amount <= 0)
                {
                    throw new ArgumentException("Member ID and a valid amount are required.");
                }

                // Process payment in the database
                ProcessPayment(memberID, amount);

                // Display success message
                MessageBox.Show($"Payment of {amount:C} for Member ID {memberID} processed successfully!");
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid amount."); // Handle invalid amount format
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Error: {ex.Message}"); // Handle validation errors
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}"); // Handle other errors
            }
        }

        // Method to process a payment in the database
        private void ProcessPayment(string memberID, decimal amount)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open(); // Open the connection to the database

                // Example query: Update the member's payment status and balance
                string query = "UPDATE members SET Balance = Balance - @Amount, Paid = 1 WHERE MemberID = @MemberID";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    // Set the parameter values
                    cmd.Parameters.AddWithValue("@Amount", amount);
                    cmd.Parameters.AddWithValue("@MemberID", memberID);

                    // Execute the update query
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        // Handle case where no member was updated
                        throw new Exception("Member ID not found or no changes made.");
                    }
                }
            }
        }

        // Method to create the members table if it does not exist
        private void CreateTablesIfNotExists()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open(); // Open the connection to the database

                // SQL query to create the 'members' table if it doesn't exist
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS members (
                        ID INT AUTO_INCREMENT PRIMARY KEY,
                        Name VARCHAR(100) NOT NULL,
                        Address VARCHAR(255) NOT NULL,
                        PhoneNumber VARCHAR(20),
                        MemberID VARCHAR(50) UNIQUE,
                        Balance DECIMAL(10, 2) DEFAULT 0.00,
                        Paid BOOLEAN DEFAULT FALSE
                    );";

                using (MySqlCommand cmd = new MySqlCommand(createTableQuery, connection))
                {
                    cmd.ExecuteNonQuery(); // Execute the create table query
                }
            }
        }

        // Event handler for showing the list of paid users
        private void showPaidUsersButton_Click(object sender, EventArgs e)
        {
            PaidUsersForm paidUsersForm = new PaidUsersForm(); // Create a new instance of the PaidUsersForm
            paidUsersForm.Show(); // Show the PaidUsersForm
        }
    }
}
