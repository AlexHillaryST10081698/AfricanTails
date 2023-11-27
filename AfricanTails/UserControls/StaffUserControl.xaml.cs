using AfricanTails.Classes;
using AfricanTails.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AfricanTails.UserControls
{
    /// <summary>
    /// Interaction logic for StaffUserControl.xaml
    /// </summary>
    public partial class StaffUserControl : UserControl
    {
        //---------------------------------------------------------------Variables-------------------------------------------------------------------------//
        private String StaffID;
        private String StaffName;
        private String StaffSurname;
        private String StaffPassword;
        private String StaffConfirmPassword;
        public StaffUserControl()
        {
            InitializeComponent();
            StaffDetComboBox.SelectionChanged += StaffDetComboBox_SelectionChanged;
            LoadStaffData();
        }

        private void StaffSavebtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get data from text fields
                StaffID = StaffIdNumTxt.Text;
                StaffName = SNameTxt.Text;
                StaffSurname = SSurnameTxt.Text;
                StaffPassword = ConvertToUnsecureString(SPasswordTxt.SecurePassword);
                StaffConfirmPassword = ConvertToUnsecureString(CPasswordTxt.SecurePassword);
                PasswordEncryption PE = new PasswordEncryption();
                string EncryptedPassword = PE.HashedPassword(StaffPassword);

                // Check if any of the required fields are empty
                if (string.IsNullOrEmpty(StaffID) || string.IsNullOrEmpty(StaffName) ||
                    string.IsNullOrEmpty(StaffSurname) || string.IsNullOrEmpty(StaffPassword) ||
                    string.IsNullOrEmpty(StaffConfirmPassword))
                {
                    // Show an error message or handle the case where fields are not filled
                    MessageBox.Show("Please fill in all fields before saving.", "Incomplete Information", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Check if the StaffID already exists
                DatabaseHandler DB = new DatabaseHandler();
                if (DB.StaffIDExists(StaffID))
                {
                    MessageBox.Show("StaffID already exists. Please choose a different StaffID.", "Duplicate StaffID", MessageBoxButton.OK, MessageBoxImage.Warning);
                    StaffIdNumTxt.Clear();
                    SNameTxt.Clear();
                    SSurnameTxt.Clear();
                    SPasswordTxt.Clear();
                    CPasswordTxt.Clear();
                    return;
                }

                // Check if the entered password matches the confirmation
                if (!StaffPassword.Equals(StaffConfirmPassword))
                {
                    MessageBox.Show("Your Passwords Do Not Match", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    SPasswordTxt.Clear();
                    CPasswordTxt.Clear();
                    return;
                }

                // All checks pass, proceed to save to the database
                DB.AddStaffToDatabase(StaffID, StaffName, StaffSurname, EncryptedPassword);

                // Optionally, you can clear input fields or perform other actions
                StaffIdNumTxt.Clear();
                SNameTxt.Clear();
                SSurnameTxt.Clear();
                SPasswordTxt.Clear();
                CPasswordTxt.Clear();

                // Optionally, you can show a success message or perform other actions here
                MessageBox.Show("Staff details saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Handle exceptions, you can log the exception or show an error message
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedPassword = IntPtr.Zero;
            try
            {
                unmanagedPassword = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedPassword);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPassword);
            }
        }

        private void StaffSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchText = StaffSearchField.Text;
            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Please enter a search term.", "Empty Search", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DatabaseHandler DB = new DatabaseHandler();
            List<Staff> searchResults = DB.SearchStaff(searchText);

            // Assuming StaffDataGrid is the name of your DataGrid control
            StaffDataGrid.ItemsSource = searchResults;
            if (searchResults.Count == 0)
            {
                MessageBox.Show("No matching staff records found in the database.", "No Records Found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected staff item
            // Get the selected staff item
            Staff selectedStaff = (Staff)StaffDataGrid.SelectedItem;

            if (selectedStaff != null)
            {
                // Open the edit window
                var editWindow = new EditStaffWindow();
                editWindow.ShowDialog();

                // Retrieve the new name and surname
                string newName = editWindow.NewName;
                string newSurname = editWindow.NewSurname;

                // Update the database with the edited values
                DatabaseHandler DBHandler = new DatabaseHandler();
                DBHandler.EditStaffInDatabase(selectedStaff.StaffID, newName, newSurname);

                // Refresh the data grid to reflect the changes
                LoadStaffData();
            }

        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a row is selected
            if (StaffDataGrid.SelectedItem != null)
            {
                // Get the selected staff member
                Staff selectedStaff = (Staff)StaffDataGrid.SelectedItem;

                // Show a confirmation dialog
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedStaff.StaffName} {selectedStaff.StaffSurname}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Call the method to delete the staff member from the database
                    DatabaseHandler dbhandler = new DatabaseHandler();
                    dbhandler.DeleteStaffFromDatabase(selectedStaff.StaffID);

                    // Refresh the data grid
                    LoadStaffData();

                    // Inform the user about the successful deletion
                    MessageBox.Show("Staff member deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a staff member to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadStaffData()
        {
            // Call the method to get the latest staff data from the database
            DatabaseHandler dbhandler = new DatabaseHandler();
            List<Staff> staffData = dbhandler.GetStaffData();

            // Set the data source of the data grid to the updated list of staff members
            StaffDataGrid.ItemsSource = staffData;
        }
        private void StaffDetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected item from the ComboBox
            ComboBoxItem selectedOption = (ComboBoxItem)StaffDetComboBox.SelectedItem;

            // Check which option is selected and perform the corresponding sorting
            switch (selectedOption.Content.ToString())
            {
                case "All":
                    // Show all fosters without sorting
                    //LoadFosterData();
                    break;

                case "Alphabetical order":
                    // Sort fosters by FullName in alphabetical order
                    SortStaffAlphabetically();
                    break;

                case "Newest":
                    // Sort fosters by date added (newest to oldest)
                    SortStaffByNewest();
                    break;

                default:
                    // Handle any additional options if needed
                    break;
            }
        }
        private void SortStaffAlphabetically()
        {
            // Call the method to get adoption data sorted alphabetically
            DatabaseHandler dbhandler = new DatabaseHandler();
            List<Staff> staffList = dbhandler.GetStaffData();

            // Sort the staff data alphabetically by name and surname
            List<Staff> sortedStaff = staffList.OrderBy(s => s.StaffName).ThenBy(s => s.StaffSurname).ToList();

            // Set the data source of the data grid to the sorted list of adoptions
            StaffDataGrid.ItemsSource = sortedStaff;
        }

        private void SortStaffByNewest()
        {
            // Call the method to get adoption data sorted by the newest entry
            DatabaseHandler dbhandler = new DatabaseHandler();
            List<Staff> sortedStaff = dbhandler.GetStaffData();

            // Sort the adoptions by DateAdded property in ascending order
            sortedStaff = sortedStaff.OrderBy(a => a.DateAdded).ToList();

            // Reverse the list to have the newest entry at the top
            sortedStaff.Reverse();

            // Set the data source of the data grid to the sorted list of adoptions
            StaffDataGrid.ItemsSource = sortedStaff;
        }

    }
}
