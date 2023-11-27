using AfricanTails.Classes;
using AfricanTails.Windows;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
    /// Interaction logic for AdoptionUserControl.xaml
    /// </summary>
    public partial class AdoptionUserControl : UserControl
    {
        //variables
        private String AdoptionID;
        private String AdoptionFullName;
        private String AdoptionEmail;
        private String AdoptionCellphoneNumber;
        private String AdoptionCurrentAddress;    
        public AdoptionUserControl()
        {
            InitializeComponent();
            AdoptionDetComboBox.SelectionChanged += AdoptionDetComboBox_SelectionChanged;
            LoadAdoptionData();
        }

        private void AdoptionSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve values from the input fields
            AdoptionID = AdoptionIDTxt.Text;
            AdoptionFullName = AdoptionFullnameTxt.Text;
            AdoptionEmail = AdoptionEmailAddresTxt.Text;
            AdoptionCellphoneNumber = AdoptionCellNumTxt.Text;
            AdoptionCurrentAddress = AdoptionAddressTxt.Text;

            // Check if any of the required fields is empty
            if (string.IsNullOrEmpty(AdoptionID) || string.IsNullOrEmpty(AdoptionFullName) ||
                string.IsNullOrEmpty(AdoptionEmail) || string.IsNullOrEmpty(AdoptionCellphoneNumber) ||
                string.IsNullOrEmpty(AdoptionCurrentAddress))
            {
                MessageBox.Show("Please fill in all the fields before saving.", "Incomplete Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if the AdoptionID already exists in the database
            DatabaseHandler DB = new DatabaseHandler();
            if (DB.AdoptionIDExists(AdoptionID))
            {
                MessageBox.Show("AdoptionID already exists. Please choose a different AdoptionID.", "Duplicate AdoptionID", MessageBoxButton.OK, MessageBoxImage.Warning);
                AdoptionIDTxt.Clear();
                AdoptionFullnameTxt.Clear();
                AdoptionEmailAddresTxt.Clear();
                AdoptionCellNumTxt.Clear();
                AdoptionAddressTxt.Clear();
                return;
            }

            // If all checks pass, proceed with saving to the database
            DB.AddAdoptionToDatabase(AdoptionID, AdoptionFullName, AdoptionCurrentAddress, AdoptionCellphoneNumber, AdoptionEmail);

            AdoptionIDTxt.Clear();
            AdoptionFullnameTxt.Clear();
            AdoptionEmailAddresTxt.Clear();
            AdoptionCellNumTxt.Clear();
            AdoptionAddressTxt.Clear();
        }



        private void AdoptionSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchText = AdoptionSearchField.Text;
            // Check if the search text is empty
            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Please enter a search term.", "Empty Search", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Perform the search using the searchText
            SearchAdoptions(searchText);
        }
        private void SearchAdoptions(string searchText)
        {
            // Call the DatabaseHandler method to get a list of adoptions based on the search criteria
            DatabaseHandler DB = new DatabaseHandler();
            List<Adoption> searchResults = DB.SearchAdoptions(searchText);

            // Populate the AdoptionDataGrid with the search results
            AdoptionDataGrid.ItemsSource = searchResults;
            if (searchResults.Count == 0)
            {
                MessageBox.Show("No matching staff records found in the database.", "No Records Found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        

        private void AdoptionEditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected adoption item
            Adoption selectedAdoption = (Adoption)AdoptionDataGrid.SelectedItem;

            if (selectedAdoption != null)
            {
                // Open the edit window
                var editWindow = new EditAdoptionWindow();
                editWindow.ShowDialog();

                // Retrieve the new values
                string newFullName = editWindow.NewFullName;
                string newContactNumber = editWindow.NewContactNumber;
                string newEmail = editWindow.NewEmail;
                string newAddress = editWindow.NewAddress;

                // Update the database with the edited values
                DatabaseHandler DBHandler = new DatabaseHandler();
                DBHandler.EditAdoptionInDatabase(selectedAdoption.AdoptionID, newFullName, newContactNumber, newEmail, newAddress);

                // Refresh the data grid to reflect the changes
                LoadAdoptionData();
            }
        }



        private void AdoptionDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the clicked button
            Button deleteButton = (Button)sender;

            // Get the corresponding data item (Adoption) from the DataGrid
            Adoption selectedAdoption = (Adoption)deleteButton.DataContext;

            // Ask the user for confirmation before deleting
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Perform the deletion in the database
                DatabaseHandler dbHandler = new DatabaseHandler();
                dbHandler.DeleteAdoption(selectedAdoption.AdoptionID);

                // Remove the item from the underlying collection bound to the DataGrid
                List<Adoption> adoptionList = (List<Adoption>)AdoptionDataGrid.ItemsSource;
                adoptionList.Remove(selectedAdoption);

                // Refresh the DataGrid to reflect the changes
                AdoptionDataGrid.Items.Refresh();
            }
        }
        // In your AdoptionUserControl class
        private void LoadAdoptionData()
        {
            // Call the method to get the latest adoption data from the database
            DatabaseHandler dbhandler = new DatabaseHandler();
            List<Adoption> adoptionData = dbhandler.GetAdoptionData();

            // Set the data source of the data grid to the updated list of adoptions
            AdoptionDataGrid.ItemsSource = adoptionData;
        }
        private void AdoptionDetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected item from the ComboBox
            ComboBoxItem selectedOption = (ComboBoxItem)AdoptionDetComboBox.SelectedItem;

            // Check which option is selected and perform the corresponding sorting
            switch (selectedOption.Content.ToString())
            {
                case "All":
                    // Show all adoptions without sorting
                    //LoadAdoptionData();
                    break;

                case "Alphabetical order":
                    // Sort adoptions by FullName in alphabetical order
                    SortAdoptionsAlphabetically();
                    break;

                case "Newest":
                    // Sort adoptions by some criteria for newest items
                    SortAdoptionsByNewest();
                    break;

                default:
                    // Handle any additional options if needed
                    break;
            }
        }

        private void SortAdoptionsAlphabetically()
        {
            // Call the method to get adoption data sorted alphabetically
            DatabaseHandler dbhandler = new DatabaseHandler();
            List<Adoption> sortedAdoptions = dbhandler.GetAdoptionDataSortedAlphabetically();

            // Set the data source of the data grid to the sorted list of adoptions
            AdoptionDataGrid.ItemsSource = sortedAdoptions;
        }

        private void SortAdoptionsByNewest()
        {
            // Call the method to get adoption data sorted by the newest entry
            DatabaseHandler dbhandler = new DatabaseHandler();
            List<Adoption> sortedAdoptions = dbhandler.GetAdoptionData();

            // Sort the adoptions by DateAdded property in ascending order
            sortedAdoptions = sortedAdoptions.OrderBy(a => a.DateAdded).ToList();

            // Reverse the list to have the newest entry at the top
            sortedAdoptions.Reverse();

            // Set the data source of the data grid to the sorted list of adoptions
            AdoptionDataGrid.ItemsSource = sortedAdoptions;
        }



    }
}
