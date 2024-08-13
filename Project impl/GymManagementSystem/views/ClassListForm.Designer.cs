namespace GymManagementSystem.Views
{
    partial class ClassListForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.classesDataGridView = new System.Windows.Forms.DataGridView();
            this.generatePdfButton = new System.Windows.Forms.Button();
            this.deleteClassButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.classesDataGridView)).BeginInit();
            this.SuspendLayout();
            
            // classesDataGridView
            this.classesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.classesDataGridView.Location = new System.Drawing.Point(20, 20);
            this.classesDataGridView.Name = "classesDataGridView";
            this.classesDataGridView.RowTemplate.Height = 24;
            this.classesDataGridView.Size = new System.Drawing.Size(760, 400);
            this.classesDataGridView.TabIndex = 0;
            
            // generatePdfButton
            this.generatePdfButton.Location = new System.Drawing.Point(20, 430);
            this.generatePdfButton.Name = "generatePdfButton";
            this.generatePdfButton.Size = new System.Drawing.Size(150, 30);
            this.generatePdfButton.TabIndex = 1;
            this.generatePdfButton.Text = "Generate PDF Report";
            this.generatePdfButton.UseVisualStyleBackColor = true;
            this.generatePdfButton.Click += new System.EventHandler(this.generatePdfButton_Click);

            // deleteClassButton
            this.deleteClassButton.Location = new System.Drawing.Point(200, 430);
            this.deleteClassButton.Name = "deleteClassButton";
            this.deleteClassButton.Size = new System.Drawing.Size(150, 30);
            this.deleteClassButton.TabIndex = 2;
            this.deleteClassButton.Text = "Delete Selected Class";
            this.deleteClassButton.UseVisualStyleBackColor = true;
            this.deleteClassButton.Click += new System.EventHandler(this.deleteClassButton_Click);

            // ClassListForm
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.deleteClassButton);
            this.Controls.Add(this.generatePdfButton);
            this.Controls.Add(this.classesDataGridView);
            this.Name = "ClassListForm";
            this.Text = "Class List";
            ((System.ComponentModel.ISupportInitialize)(this.classesDataGridView)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView classesDataGridView;
        private System.Windows.Forms.Button generatePdfButton;
        private System.Windows.Forms.Button deleteClassButton;
    }
}
