using AfricanTails.Classes;
using AfricanTails.Windows;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AfricanTails.UserControls
{
    /// <summary>
    /// Interaction logic for AddCatUserControl.xaml
    /// </summary>
    public partial class AddCatUserControl : UserControl
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
        public AddCatUserControl()
        {
            InitializeComponent();
        }

        private void CatSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get data from text fields
                AnimalID = CatIDTxt.Text;
                Name = CatNameTxt.Text;
                Breed = CatBreedTxt.Text;

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
                    CatIDTxt.Clear();
                    CatNameTxt.Clear();
                    CatBreedTxt.Clear();
                    CatColourTxt.Clear();
                    CatGenTxt.Clear();
                    CatMicroTxt.Clear();
                    CatComboBox.SelectedIndex = -1;
                    Catdateofbirth.SelectedDate = null;
                    Catdateofadoption.SelectedDate = null;
                    Catdateofintake.SelectedDate = null;
                    return;
                }

                // Continue with the rest of your code if validation passes
                Colour = CatColourTxt.Text;
                Sex = CatGenTxt.Text;

                // Validate and convert Microchip to long
                if (!string.IsNullOrEmpty(CatMicroTxt.Text))
                {
                    if (long.TryParse(CatMicroTxt.Text, out long tempMicrochip))
                    {
                        Microchip = tempMicrochip;
                    }
                    else
                    {
                        MessageBox.Show("Microchip must be a valid integer.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; // Exit the method if validation fails
                    }
                }
                else
                {
                    // Set a default value for Microchip when it's not provided by the user
                    Microchip = 0; // You can choose a default value here
                }

                // Cast the selected item to ComboBoxItem and access its Content property
                Status = (CatComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                DateofBirth = Catdateofbirth.SelectedDate ?? DateTime.MinValue;
                DateAdopted = Catdateofadoption.SelectedDate ?? DateTime.MinValue;
                DateFostered = Catdateofintake.SelectedDate ?? DateTime.MinValue;

                // Set the time component to midnight to store only the date
                DateofBirth = DateofBirth.Date;
                DateAdopted = DateAdopted.Date;
                DateFostered = DateFostered.Date;

                // All checks pass, proceed to save to the database
                DB.AddAnimalToDatabase(AnimalID, Name, Breed);
                DB.AddAnimalAttributeToDatabase(AnimalID, Colour, Sex, Microchip, Status, DateofBirth, DateAdopted, DateFostered);

                // Optionally, you can clear input fields or perform other actions
                CatIDTxt.Clear();
                CatNameTxt.Clear();
                CatBreedTxt.Clear();
                CatColourTxt.Clear();
                CatGenTxt.Clear();
                CatMicroTxt.Clear();
                CatComboBox.SelectedIndex = -1;
                Catdateofbirth.SelectedDate = null;
                Catdateofadoption.SelectedDate = null;
                Catdateofintake.SelectedDate = null;

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
