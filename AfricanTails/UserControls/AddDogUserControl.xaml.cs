using AfricanTails.Classes;
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
    /// Interaction logic for AddDogUserControl.xaml
    /// </summary>
    public partial class AddDogUserControl : UserControl
    {
        //Variables 
        private String AnimalID;
        private String Name;
        private String Breed;
        private String Colour;
        private String Sex;
        private long Microchip;
        private String Status;
        public DateTime DateofBirth;
        public DateTime DateAdopted;
        public DateTime DateFostered;
        public AddDogUserControl()
        {
            InitializeComponent();
        }

        private void ProceedToDogMedicalBtn_Click(object sender, RoutedEventArgs e)
        {

        }


        private void SaveDogBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get data from text fields
                AnimalID = DogIDTxt.Text;
                Name = NameTxt.Text;
                Breed = DogBreedTxt.Text;

                // Validate that required fields are not null or empty
                if (string.IsNullOrEmpty(AnimalID) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Breed))
                {
                    MessageBox.Show("AnimalID, Name, and Breed are required fields and cannot be empty.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // Exit the method if validation fails
                }

                // Check if the AnimalID already exists
                DatabaseHandler DB = new DatabaseHandler();
                if (DB.AnimalIDExists(AnimalID))
                {
                    MessageBox.Show("AnimalID already exists. Please choose a different AnimalID.", "Duplicate AnimalID", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Continue with the rest of your code if validation passes
                Colour = DogColourTxt.Text;
                Sex = DogGenTxt.Text;

                // Validate and convert Microchip to long
                if (!string.IsNullOrEmpty(DogMicroTxt.Text))
                {
                    if (long.TryParse(DogMicroTxt.Text, out long tempMicrochip))
                    {
                        Microchip = tempMicrochip;
                    }
                    else
                    {
                        MessageBox.Show("Microchip must be a valid number.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; // Exit the method if validation fails
                    }
                }
                else
                {
                    // Set a default value for Microchip when it's not provided by the user
                    Microchip = 0; // You can choose a default value here
                }

                // Cast the selected item to ComboBoxItem and access its Content property
                Status = (DogstatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                DateofBirth = Dogdateofbirth.SelectedDate ?? DateTime.MinValue;
                DateAdopted = DogdateofAdoption.SelectedDate ?? DateTime.MinValue;
                DateFostered = Dogdateofintake.SelectedDate ?? DateTime.MinValue;

                // Set the time component to midnight to store only the date
                DateofBirth = DateofBirth.Date;
                DateAdopted = DateAdopted.Date;
                DateFostered = DateFostered.Date;

                // All checks pass, proceed to save to the database
                DB.AddAnimalToDatabase(AnimalID, Name, Breed);
                DB.AddAnimalAttributeToDatabase(AnimalID, Colour, Sex, Microchip, Status, DateofBirth, DateAdopted, DateFostered);

                // Optionally, you can clear input fields or perform other actions
                DogIDTxt.Clear();
                NameTxt.Clear();
                DogBreedTxt.Clear();
                DogColourTxt.Clear();
                DogGenTxt.Clear();
                DogMicroTxt.Clear();
                DogstatusComboBox.SelectedIndex = -1;
                Dogdateofbirth.SelectedDate = null;
                DogdateofAdoption.SelectedDate = null;
                Dogdateofintake.SelectedDate = null;

                // Optionally, you can show a success message or perform other actions here
                MessageBox.Show("Animal Saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Handle exceptions, you can log the exception or show an error message
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




    }
}
