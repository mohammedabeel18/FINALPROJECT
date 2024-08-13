using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // MySQL library for database operations
using iTextSharp.text; // iTextSharp library for PDF creation
using iTextSharp.text.pdf;
using System.IO; // For file operations

namespace GymManagementSystem.Views
{
    public partial class ClassListForm : Form
    {
        // Connection string for connecting to the MySQL database
        private string connectionString = "Server=localhost;Database=gym_management;User ID=root;Password=Asma@123;";

        public ClassListForm()
        {
            InitializeComponent(); // Initialize UI components
            CreateTableIfNotExists(); // Create table if it doesn't exist
            LoadClasses(); // Load classes into DataGridView
        }

        // Method to create the fitness_classes table if it doesn't already exist
        private void CreateTableIfNotExists()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open connection to the database
                    
                    // SQL query to create the table
                    string query = @"
                        CREATE TABLE IF NOT EXISTS fitness_classes (
                            ID INT AUTO_INCREMENT PRIMARY KEY,
                            ClassName VARCHAR(100) NOT NULL,
                            Instructor VARCHAR(100) NOT NULL,
                            StartTime TIME NOT NULL,
                            EndTime TIME NOT NULL
                        );";
                    
                    // Execute the query
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Show error message if table creation fails
                    MessageBox.Show($"Error creating table: {ex.Message}");
                }
            }
        }

        // Method to load classes from the database into the DataGridView
        private void LoadClasses()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open connection to the database

                    // SQL query to select all classes
                    string query = "SELECT * FROM fitness_classes";

                    // Use MySqlDataAdapter to fill DataTable with query results
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable); // Fill the DataTable
                        classesDataGridView.DataSource = dataTable; // Bind DataTable to DataGridView
                    }
                }
                catch (Exception ex)
                {
                    // Show error message if loading classes fails
                    MessageBox.Show($"Error loading classes: {ex.Message}");
                }
            }
        }

        // Event handler for generating a PDF report from the DataGridView
        private void generatePdfButton_Click(object sender, EventArgs e)
        {
            // Check if there are any rows to export
            if (classesDataGridView.Rows.Count > 0)
            {
                // Configure save file dialog for PDF file
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = "FitnessClassesReport.pdf"
                };

                // Show the save file dialog and process the result
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Create a new PDF file using FileStream
                        using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            using (Document doc = new Document())
                            {
                                // Get an instance of PdfWriter to write the document
                                PdfWriter.GetInstance(doc, fs);
                                doc.Open(); // Open the document for writing

                                // Add title to the document
                                doc.Add(new Paragraph("Fitness Classes Report"));

                                // Create a table with the same number of columns as DataGridView
                                PdfPTable table = new PdfPTable(classesDataGridView.Columns.Count);

                                // Add column headers to the table
                                foreach (DataGridViewColumn column in classesDataGridView.Columns)
                                {
                                    table.AddCell(new Phrase(column.HeaderText));
                                }

                                // Add rows to the table
                                foreach (DataGridViewRow row in classesDataGridView.Rows)
                                {
                                    if (row.IsNewRow) continue; // Skip the new row placeholder
                                    foreach (DataGridViewCell cell in row.Cells)
                                    {
                                        // Add cell value or empty string if null
                                        table.AddCell(new Phrase(cell.Value?.ToString() ?? string.Empty));
                                    }
                                }

                                doc.Add(table); // Add the table to the document
                                doc.Close(); // Close the document
                            }
                        }

                        // Show success message
                        MessageBox.Show("PDF report generated successfully!");
                    }
                    catch (Exception ex)
                    {
                        // Show error message if PDF generation fails
                        MessageBox.Show($"Error generating PDF: {ex.Message}");
                    }
                }
            }
            else
            {
                // Show message if no data is available to export
                MessageBox.Show("No data available to generate the report.");
            }
        }

        // Event handler for deleting selected classes from the DataGridView and database
        private void deleteClassButton_Click(object sender, EventArgs e)
        {
            // Check if any rows are selected
            if (classesDataGridView.SelectedRows.Count > 0)
            {
                // Loop through selected rows
                foreach (DataGridViewRow row in classesDataGridView.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        // Get the class ID from the selected row
                        int classId = Convert.ToInt32(row.Cells["ID"].Value);
                        DeleteClassFromDatabase(classId); // Delete the class from the database
                    }
                }
                LoadClasses(); // Reload classes after deletion
            }
            else
            {
                // Show message if no rows are selected
                MessageBox.Show("Please select at least one class to delete.");
            }
        }

        // Method to delete a class from the database using its ID
        private void DeleteClassFromDatabase(int classId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Open connection to the database

                    // SQL query to delete a class by ID
                    string query = "DELETE FROM fitness_classes WHERE ID = @ID";

                    // Execute the query with the specified class ID
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", classId);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Show error message if deletion fails
                    MessageBox.Show($"Error deleting class: {ex.Message}");
                }
            }
        }
    }
}
