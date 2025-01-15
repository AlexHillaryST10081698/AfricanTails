using AfricanTails.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// Interaction logic for DogMedicalUserControl.xaml
    /// </summary>
    public partial class DogMedicalUserControl : UserControl
    {
        //Variables 
        private String MedicalRecordID;
        private String AnimalID;
        public DateTime Sterilization;
        public DateTime FirstVaccination;
        public DateTime SecondVaccination;
        public DateTime ThirdVaccination;
        public String DewormedTest;
        public DateTime DewormedTestDate;
        public String RabiesTest;
        public DateTime RabiesTestDate;
        public String MedicalTreatmentTest;
        public DateTime MedicalTreatmentTestDate;
        public String FleaTreatmentTest;
        public DateTime FleaTreatmentTestDate;
        public string ParvoTest;
        public DateTime ParvoTestDate;
        public string DistemperTest;
        public DateTime DistemperTestDate;

        //--------------------------------------------------------------------------------------Constructor ----------------------------------------------------------------------------------------------------------------//
        public DogMedicalUserControl()
        {
            InitializeComponent();
        }

        //Event Handler For Dog Save button for Medical Records
        private void DogMedicalSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecordID = DogMedID.Text;
            AnimalID = DogAnimalID.Text;
            // Check if MedicalRecordID and AnimalID are not null or empty
            if (string.IsNullOrEmpty(MedicalRecordID) || string.IsNullOrEmpty(AnimalID))
            {
                MessageBox.Show("Medical Record ID and Animal ID cannot be null or empty.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Sterilization = DogSerilDate.SelectedDate ?? DateTime.MinValue;
            FirstVaccination = DogFirstVaccDate.SelectedDate ?? DateTime.MinValue;
            SecondVaccination = DogSecondVaccDate.SelectedDate ?? DateTime.MinValue;
            ThirdVaccination = DogThirdVaccDate.SelectedDate ?? DateTime.MinValue;
            DewormedTest = (DogDeworming.SelectedItem as ComboBoxItem)?.Content.ToString();
            DewormedTestDate = DogDewormingDate.SelectedDate ?? DateTime.MinValue;
            RabiesTest = (DogRabiesTest.SelectedItem as ComboBoxItem)?.Content.ToString();
            RabiesTestDate = DogRabiesDate.SelectedDate ?? DateTime.MinValue;
            MedicalTreatmentTest = DogtMedTreatmentDetail.Text;
            MedicalTreatmentTestDate = DogMedTreatDate.SelectedDate ?? DateTime.MinValue;
            FleaTreatmentTest = (DogFleaTreamentTest.SelectedItem as ComboBoxItem)?.Content.ToString();
            FleaTreatmentTestDate = DogFleaTreatmentDate.SelectedDate ?? DateTime.MinValue;
            ParvoTest = (DogParvoTestResult.SelectedItem as ComboBoxItem)?.Content.ToString();
            ParvoTestDate = DogParvoTestDate.SelectedDate ?? DateTime.MinValue;
            DistemperTest = (DogDisTestRes.SelectedItem as ComboBoxItem)?.Content.ToString();
            DistemperTestDate = DogDisTestDate.SelectedDate ?? DateTime.MinValue;
            DatabaseHandler DB = new DatabaseHandler();

            //Exception To Check if Animal Exists
            if (!DB.AnimalIDExists(AnimalID))
            {
                MessageBox.Show("Animal ID does not exist in the database.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Add Medical Record to the database
            DB.AddMedicalRecordToDatabase(MedicalRecordID, AnimalID);
            DB.AddMedicalRecordDetailToDatabase(MedicalRecordID, Sterilization, FirstVaccination, SecondVaccination, ThirdVaccination, DewormedTest, DewormedTestDate, RabiesTest, RabiesTestDate, MedicalTreatmentTest, MedicalTreatmentTestDate, FleaTreatmentTest, FleaTreatmentTestDate);
            DB.AddDogSpecificMedicalRecordToDatabase(MedicalRecordID, DistemperTest, DistemperTestDate, ParvoTest, ParvoTestDate);

            MessageBox.Show("Medical Record Saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
//---------------------------------------------------------------------------------------------End OF fIle ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
