using System;
using System.Data;
using System.Windows.Forms; // Provides classes for creating Windows-based applications
using MySql.Data.MySqlClient; // Provides classes for connecting to a MySQL database
using iTextSharp.text; // Provides classes for creating PDF documents
using iTextSharp.text.pdf; // Provides classes for writing PDF documents
using System.IO; // Provides classes for file operations

namespace GymManagementSystem.Views
{
    public partial class PaidUsersForm : Form
    {
        // Connection string for connecting to the MySQL database
        private string connectionString = "Server=localhost;Database=gym_management;User ID=root;Password=Asma@123;";

        public PaidUsersForm()
        {
            InitializeComponent(); // Initialize UI components
            LoadPaidUsers(); // Load paid users into the data grid view
        }

        // Method to load paid users from the database
        private void LoadPaidUsers()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open(); // Open the connection to the database

                // Query to select all members who have paid
                string query = "SELECT ID, Name, Address, PhoneNumber, MemberID, MembershipType, Paid FROM members WHERE Paid = 1";

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable(); // Create a data table to hold the data
                    adapter.Fill(dataTable); // Fill the data table with data from the database

                    // Bind the data table to the data grid view
                    paidUsersDataGridView.DataSource = dataTable;

                    // Optionally, hide the "Balance" column if it exists
                    if (dataTable.Columns.Contains("Balance"))
                    {
                        paidUsersDataGridView.Columns["Balance"].Visible = false;
                    }
                }
            }
        }

        // Event handler for generating a PDF report
        private void generatePdfButton_Click(object sender, EventArgs e)
        {
            if (paidUsersDataGridView.Rows.Count > 0) // Check if there is data to export
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf", // Filter for saving PDF files
                    FileName = "PaidUsersReport.pdf" // Default file name
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK) // Show save file dialog and check if user clicked OK
                {
                    using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (Document doc = new Document()) // Create a new PDF document
                        {
                            PdfWriter.GetInstance(doc, fs); // Associate the document with the file stream
                            doc.Open(); // Open the document for writing

                            // Add title to the PDF
                            doc.Add(new Paragraph("Paid Users Report"));

                            // Create a table with the same number of columns as the data grid view
                            PdfPTable table = new PdfPTable(paidUsersDataGridView.Columns.Count);

                            // Add headers to the table
                            foreach (DataGridViewColumn column in paidUsersDataGridView.Columns)
                            {
                                table.AddCell(new Phrase(column.HeaderText)); // Add header cell for each column
                            }

                            // Add rows to the table
                            foreach (DataGridViewRow row in paidUsersDataGridView.Rows)
                            {
                                if (row.IsNewRow) continue; // Skip the new row placeholder
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    table.AddCell(new Phrase(cell.Value?.ToString() ?? string.Empty)); // Add cell value
                                }
                            }

                            doc.Add(table); // Add the table to the document
                            doc.Close(); // Close the document
                        }
                    }

                    MessageBox.Show("PDF report generated successfully!"); // Notify the user that the report was generated
                }
            }
            else
            {
                MessageBox.Show("No data available to generate the report."); // Notify the user if there is no data
            }
        }
    }
}
