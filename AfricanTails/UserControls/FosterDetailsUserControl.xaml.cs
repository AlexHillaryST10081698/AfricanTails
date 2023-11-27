using AfricanTails.Classes;
using AfricanTails.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for FosterDetailsUserControl.xaml
    /// </summary>
    public partial class FosterDetailsUserControl : UserControl
    {
        //variables
        private String FosterID;
        private String FosterFullname;
        private String FosterContactNumber;
        private String FosterEmail;
        private String FosterAddress;
        public FosterDetailsUserControl()
        {
            InitializeComponent();
            FosterDetComboBox.SelectionChanged += FosterDetComboBox_SelectionChanged;
            LoadFosterData();
        }

        private void FosterSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get data from text fields
                FosterID = FosterIDTxt.Text;
                FosterFullname = FosterFullnameTxt.Text;
                FosterContactNumber = FosterCellNumTxt.Text;
                FosterEmail = FosterEmailAddresTxt.Text;
                FosterAddress = FosterAddressTxt.Text;

                // Check if any of the required fields are empty
                if (string.IsNullOrEmpty(FosterID) || string.IsNullOrEmpty(FosterFullname) ||
                    string.IsNullOrEmpty(FosterContactNumber) || string.IsNullOrEmpty(FosterEmail) ||
                    string.IsNullOrEmpty(FosterAddress))
                {
                    // Show an error message or handle the case where fields are not filled
                    MessageBox.Show("Please fill in all fields before saving.", "Incomplete Information", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Check if the FosterID already exists
                DatabaseHandler DB = new DatabaseHandler();
                if (DB.FosterIDExists(FosterID))
                {
                    MessageBox.Show("FosterID already exists. Please choose a different FosterID.", "Duplicate FosterID", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // All checks pass, proceed to save to the database
                DB.AddFosterToDatabase(FosterID, FosterFullname, FosterContactNumber, FosterEmail, FosterAddress);

                // Clear input fields
                FosterIDTxt.Clear();
                FosterFullnameTxt.Clear();
                FosterCellNumTxt.Clear();
                FosterEmailAddresTxt.Clear();
                FosterAddressTxt.Clear();

                // Optionally, you can show a success message or perform other actions here
                MessageBox.Show("Foster details saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Handle exceptions, you can log the exception or show an error message
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void FosterSearchBtn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchText = FosterSearchBar.Text;

                // Check if the search text is empty
                if (string.IsNullOrEmpty(searchText))
                {
                    MessageBox.Show("Please enter a search term.", "Empty Search", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Search for fosters in the database
                DatabaseHandler DB = new DatabaseHandler();
                List<Foster> fosterList = DB.SearchFosters(searchText);

                // Update the data grid with the search results
                FosterDisplayDataGrid.ItemsSource = fosterList;

                // Optionally, you can show a message if no results are found
                if (fosterList.Count == 0)
                {
                    MessageBox.Show("No matching records found.", "No Results", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void FosterEditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected foster item
            Foster selectedFoster = (Foster)FosterDisplayDataGrid.SelectedItem;

            if (selectedFoster != null)
            {
                // Open the edit window
                var editWindow = new EditFosterWindow();
                editWindow.ShowDialog();

                // Retrieve the new name, contact number, email, and address
                string newFullName = editWindow.NewFullName;
                string newContactNumber = editWindow.NewContactNumber;
                string newEmail = editWindow.NewEmail;
                string newAddress = editWindow.NewAddress;

                // Update the database with the edited values
                DatabaseHandler DBHandler = new DatabaseHandler();
                DBHandler.EditFosterInDatabase(selectedFoster.FosterID, newFullName, newContactNumber, newEmail, newAddress);

                // Refresh the data grid to reflect the changes
                LoadFosterData();
            }
        }

        private void FosterDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the clicked button
                Button deleteButton = (Button)sender;

                // Get the corresponding data item (Foster) from the DataGrid
                Foster selectedFoster = (Foster)deleteButton.DataContext;

                // Ask the user for confirmation before deleting
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Perform the deletion in the database
                    DatabaseHandler dbHandler = new DatabaseHandler();
                    dbHandler.DeleteFosterFromDatabase(selectedFoster.FosterID);
                    List<Foster> FosterList = (List<Foster>)FosterDisplayDataGrid.ItemsSource;

                    // Remove the item from the underlying collection bound to the DataGrid
                    FosterList.Remove(selectedFoster);

                    // Refresh the DataGrid to reflect the changes
                    FosterDisplayDataGrid.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadFosterData()
        {
            // Call the method to get the latest foster data from the database
            DatabaseHandler dbHandler = new DatabaseHandler();
            List<Foster> fosterData = dbHandler.GetFosterData();

            // Set the data source of the data grid to the updated list of foster members
            FosterDisplayDataGrid.ItemsSource = fosterData;
        }
        private void FosterDetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected item from the ComboBox
            ComboBoxItem selectedOption = (ComboBoxItem)FosterDetComboBox.SelectedItem;

            // Check which option is selected and perform the corresponding sorting
            switch (selectedOption.Content.ToString())
            {
                case "All":
                    // Show all fosters without sorting
                    //LoadFosterData();
                    break;

                case "Alphabetical order":
                    // Sort fosters by FullName in alphabetical order
                    SortFostersAlphabetically();
                    break;

                case "Newest":
                    // Sort fosters by date added (newest to oldest)
                    SortFostersByNewest();
                    break;

                default:
                    // Handle any additional options if needed
                    break;
            }
        }
        private void SortFostersByNewest()
        {
            // Call the method to get foster data sorted by the date added
            DatabaseHandler dbHandler = new DatabaseHandler();
            List<Foster> sortedFosters = dbHandler.GetFosterData();
            // Sort fosters by FullName in alphabetical order
            sortedFosters = sortedFosters.OrderBy(f => f.FosterFullname).ToList();

            sortedFosters.Reverse();
            // Set the data source of the data grid to the sorted list of fosters
            FosterDisplayDataGrid.ItemsSource = sortedFosters;
        }
        private void SortFostersAlphabetically()
        {
            // Call the method to get foster data
            DatabaseHandler dbHandler = new DatabaseHandler();
            List<Foster> fosterData = dbHandler.GetFosterData();

            // Sort fosters by FullName in alphabetical order
            fosterData = fosterData.OrderBy(f => f.FosterFullname).ToList();

            // Set the data source of the data grid to the sorted list of fosters
            FosterDisplayDataGrid.ItemsSource = fosterData;
        }




    }
}
