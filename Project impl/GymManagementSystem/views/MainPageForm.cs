using System;
using System.Windows.Forms; // Required for Windows Forms functionality

namespace GymManagementSystem.Views
{
    public partial class MainPageForm : Form
    {
        // Constructor for initializing the main page form
        public MainPageForm()
        {
            InitializeComponent(); // Initialize UI components
        }

        // Event handler for opening the Member Management form
        private void memberManagementButton_Click(object sender, EventArgs e)
        {
            // Create and show the MemberManagementForm
            MemberManagementForm memberForm = new MemberManagementForm();
            memberForm.Show();
        }

        // Event handler for opening the Payment Management form
        private void paymentManagementButton_Click(object sender, EventArgs e)
        {
            // Create and show the MembershipPaymentForm
            MembershipPaymentForm paymentForm = new MembershipPaymentForm();
            paymentForm.Show();
        }

        // Event handler for opening the Staff Management form
        private void staffManagementButton_Click(object sender, EventArgs e)
        {
            // Create and show the StaffManagementForm
            StaffManagementForm staffForm = new StaffManagementForm();
            staffForm.Show();
        }

        // Event handler for opening the Equipment Management form
        private void equipmentManagementButton_Click(object sender, EventArgs e)
        {
            // Create and show the EquipmentManagementForm
            EquipmentManagementForm equipmentForm = new EquipmentManagementForm();
            equipmentForm.Show();
        }

        // Event handler for opening the Fitness Class Management form
        private void fitnessClassManagementButton_Click(object sender, EventArgs e)
        {
            // Create and show the FitnessClassManagementForm
            FitnessClassManagementForm fitnessClassForm = new FitnessClassManagementForm();
            fitnessClassForm.Show();
        }

        // Event handler for exiting the application
        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Close the application
        }
    }
}
