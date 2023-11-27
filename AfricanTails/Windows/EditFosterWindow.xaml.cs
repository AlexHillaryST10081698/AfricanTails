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
    /// Interaction logic for EditFosterWindow.xaml
    /// </summary>
    public partial class EditFosterWindow : Window
    {
        public string NewFullName { get; private set; }
        public string NewContactNumber { get; private set; }
        public string NewEmail { get; private set; }
        public string NewAddress { get; private set; }

        public EditFosterWindow()
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
            NewFullName = EditFullNameTextBox.Text;
            NewContactNumber = EditContactNumberTextBox.Text;
            NewEmail = EditEmailTextBox.Text;
            NewAddress = EditAddressTextBox.Text;

            // Close the window
            Close();
        }
    }

}
