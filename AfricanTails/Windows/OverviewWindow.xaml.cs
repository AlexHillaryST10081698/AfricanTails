using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AfricanTails.Windows
{
    /// <summary>
    /// Interaction logic for OverviewWindow.xaml
    /// </summary>
    public partial class OverviewWindow : Window
    {
        private Form activeForm;
        public OverviewWindow()
        {
            InitializeComponent();
        }
       

        private void DogsButton_Checked(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = FindResource("AddDogTemplate") as DataTemplate;
            UIElement userControl = dataTemplate.LoadContent() as UIElement;
            OverviewContent.Content = userControl;
        }

        private void CatsButton_Checked(object sender, RoutedEventArgs e)
        {
            // Get the DataTemplate for the AddCatUserControl
            DataTemplate dataTemplate = FindResource("AddCatTemplate") as DataTemplate;

            // Create an instance of the user control from the DataTemplate
            UIElement userControl = dataTemplate.LoadContent() as UIElement;

            // Set the user control as the content of the ContentControl
            OverviewContent.Content = userControl;
        }

        private void FosterButton_Checked(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = FindResource("AddFosterTemplate") as DataTemplate;
            UIElement userControl = dataTemplate.LoadContent() as UIElement;
            OverviewContent.Content = userControl;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = FindResource("AddReminderTemplate") as DataTemplate;
            UIElement userControl = dataTemplate.LoadContent() as UIElement;
            OverviewContent.Content = userControl;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void DashboardButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void StaffBtn_Checked(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = FindResource("AddStaffTemplate") as DataTemplate;
            UIElement userControl = dataTemplate.LoadContent() as UIElement;
            OverviewContent.Content = userControl;
        }

        private void AdoptionButton_Checked(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = FindResource("AddAdoptionTemplate") as DataTemplate;
            UIElement userControl = dataTemplate.LoadContent() as UIElement;
            OverviewContent.Content = userControl;
        }

        private void DogMedButton_Checked(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = FindResource("AddDogMedicalRTemplate") as DataTemplate;
            UIElement userControl = dataTemplate.LoadContent() as UIElement;
            OverviewContent.Content = userControl;
        }

        private void CatMedButton_Checked(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = FindResource("AddCatMedicalRTemplate") as DataTemplate;
            UIElement userControl = dataTemplate.LoadContent() as UIElement;
            OverviewContent.Content = userControl;
        }

        private void DisplayButton_Checked(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = FindResource("AddDashboardTemplate") as DataTemplate;
            UIElement userControl = dataTemplate.LoadContent() as UIElement;
            OverviewContent.Content = userControl;
        }
    }
}
