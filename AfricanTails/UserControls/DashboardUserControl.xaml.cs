using AfricanTails.Classes;
using AfricanTails.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AfricanTails.UserControls
{
    public partial class DashboardUserControl : UserControl
    {
        public DashboardUserControl()
        {
            InitializeComponent();
            CatsAndDodDataComboBox.SelectionChanged += AnimalDetComboBox_SelectionChanged;
            LoadAndDisplayAnimalData();

        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void LoadAndDisplayAnimalData()
        {
            // Call the method to get all animal data
            DatabaseHandler dbhandler = new DatabaseHandler();
            List<Animal> animalList = dbhandler.GetAnimalData();

            // Set the data source of the data grid to the list of animals
            AnimalDatagrid.ItemsSource = animalList;
        }

        private void AnimalSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadAndDisplayAnimalData();
        }
        // Assuming you have a method to get the selected animal from the DataGrid
        private Animal GetSelectedAnimal()
        {
            return (Animal)AnimalDatagrid.SelectedItem;
        }

        private void AnimalDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the clicked button
            Button deleteButton = (Button)sender;

            // Get the corresponding data item (Animal) from the DataGrid
            Animal selectedAnimal = (Animal)deleteButton.DataContext;

            // Ask the user for confirmation before deleting
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this animal?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Perform the deletion in the database
                DatabaseHandler dbHandler = new DatabaseHandler();
                dbHandler.DeleteAnimal(selectedAnimal.AnimalID);

                // Remove the item from the underlying collection bound to the DataGrid
                List<Animal> animalList = (List<Animal>)AnimalDatagrid.ItemsSource;
                animalList.Remove(selectedAnimal);

                // Refresh the DataGrid to reflect the changes
                AnimalDatagrid.Items.Refresh();
            }
        }

        private void AnimalEditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected animal item
            Animal selectedAnimal = (Animal)AnimalDatagrid.SelectedItem;

            if (selectedAnimal != null)
            {
                // Open the edit window
                var editWindow = new AnimalEditWindow();
                editWindow.ShowDialog();

                // Retrieve the new values
                string newName = editWindow.NewName;
                string newBreed = editWindow.NewBreed;
                string newColour = editWindow.NewColour;
                string newSex = editWindow.NewSex;
                long newMicrochip = editWindow.NewMicrochip;
                string newStatus = editWindow.NewStatus;
                DateTime newDateOfBirth = editWindow. NewDateofBirth;
                DateTime newDateAdopted = editWindow.NewDateAdopted;
                DateTime newDateFostered = editWindow.NewDateFostered;

                // Update the database with the edited values
                DatabaseHandler DBHandler = new DatabaseHandler();
                DBHandler.EditAnimalInDatabase(selectedAnimal.AnimalID, newName, newBreed, newColour, newSex, newMicrochip, newStatus, newDateOfBirth, newDateAdopted, newDateFostered);

                // Refresh the data grid to reflect the changes
                LoadAndDisplayAnimalData();
            }
        }
        private void AnimalDetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected item from the ComboBox
            ComboBoxItem selectedOption = (ComboBoxItem)CatsAndDodDataComboBox.SelectedItem;

            // Check which option is selected and perform the corresponding sorting
            switch (selectedOption.Content.ToString())
            {
                case "All":
                    // Show all adoptions without sorting
                    //LoadAdoptionData();
                    break;

                case "Alphabetical order":
                    // Sort adoptions by FullName in alphabetical order
                    SortAnimalAlphabetically();
                    break;

                case "Newest":
                    // Sort adoptions by some criteria for newest items
                    SortAnimalByNewest();
                    break;

                default:
                    // Handle any additional options if needed
                    break;
            }
        }
        private void SortAnimalAlphabetically()
        {
            // Call the method to get adoption data sorted alphabetically
            DatabaseHandler dbhandler = new DatabaseHandler();
            List<Animal> sortedAnimal = dbhandler.GetAnimalData();
            sortedAnimal = sortedAnimal.OrderBy(a => a.Name).ToList();

            // Set the data source of the data grid to the sorted list of adoptions
            AnimalDatagrid.ItemsSource = sortedAnimal;
        }

        private void SortAnimalByNewest()
        {
            // Call the method to get adoption data sorted by the newest entry
            DatabaseHandler dbhandler = new DatabaseHandler();
            List<Animal> sortedAnimal = dbhandler.GetAnimalData();

            // Sort the adoptions by DateAdded property in ascending order
            sortedAnimal = sortedAnimal.OrderBy(a => a.DateAdded).ToList();

            // Reverse the list to have the newest entry at the top
            sortedAnimal.Reverse();

            // Set the data source of the data grid to the sorted list of adoptions
            AnimalDatagrid.ItemsSource = sortedAnimal;
        }

        // Method to refresh the Animal DataGrid
        private void RefreshAnimalDataGrid()
        {
            // Call the method to get the updated animal data
            DatabaseHandler dbhandler = new DatabaseHandler();
            List<Animal> updatedAnimals = dbhandler.GetAnimalData();

            // Set the data source of the DataGrid to the updated list of animals
            AnimalDatagrid.ItemsSource = updatedAnimals;
        }

    }
}
