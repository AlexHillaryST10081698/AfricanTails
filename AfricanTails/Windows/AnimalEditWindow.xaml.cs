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
using System.Windows.Shapes;

namespace AfricanTails.Windows
{
    /// <summary>
    /// Interaction logic for AnimalEditWindow.xaml
    /// </summary>
    public partial class AnimalEditWindow : Window
    {
        public string NewName { get; private set; }
        public string NewBreed { get; private set; }
        public string NewColour { get; private set; }
        public string NewSex { get; private set; }
        public long NewMicrochip { get; private set; }
        public string NewStatus { get; private set; }
        public DateTime NewDateofBirth { get; private set; }
        public DateTime NewDateAdopted { get; private set; }
        public DateTime NewDateFostered { get; private set; }

        public AnimalEditWindow()
        {
            InitializeComponent();
            // Center the window on startup
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void ClearText(object sender, RoutedEventArgs e)
        {
            // Clear the watermark text when the TextBox gets focus
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == textBox.Tag?.ToString())
            {
                textBox.Text = string.Empty;
            }
        }

        private void RestoreText(object sender, RoutedEventArgs e)
        {
            // Restore the watermark text when the TextBox loses focus and is empty
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag?.ToString();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the values from the TextBoxes
            NewName = EditNameTextBox.Text;
            NewBreed = EditBreedTextBox.Text;
            NewColour = EditColourTextBox.Text;
            NewSex = EditSexTextBox.Text;
            if (long.TryParse(EditMicrochipTextBox.Text, out long microchipValue))
            {
                NewMicrochip = microchipValue;
            }
            else
            {
                // Handle the case where the input is not a valid long
                MessageBox.Show("Invalid Microchip value. Please enter a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Abort the save operation
            }
            NewStatus = (EditStatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            NewDateofBirth = EditDateofBirthDatePicker.SelectedDate ?? DateTime.MinValue; 
            NewDateAdopted = EditDateAdoptionDatePicker.SelectedDate ?? DateTime.MinValue;
            NewDateFostered = EditDateFosteredDatePicker.SelectedDate ?? DateTime.MinValue;

            // Close the window
            Close();
        }
    }
}

