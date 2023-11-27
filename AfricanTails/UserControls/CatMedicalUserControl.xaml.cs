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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AfricanTails.UserControls
{
    /// <summary>
    /// Interaction logic for CatMedicalUserControl.xaml
    /// </summary>
    public partial class CatMedicalUserControl : UserControl
    {
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
        public string FivStatus;
        public DateTime FIVDATE;
        public CatMedicalUserControl()
        {
            InitializeComponent();
        }

        private void CatMedicalRSaceBtn_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecordID = CatMedID.Text;
            AnimalID = CatAnimalID.Text;

            // Check if MedicalRecordID and AnimalID are not null or empty
            if (string.IsNullOrEmpty(MedicalRecordID) || string.IsNullOrEmpty(AnimalID))
            {
                MessageBox.Show("Medical Record ID and Animal ID cannot be null or empty.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Sterilization = CatSerilDate.SelectedDate ?? DateTime.MinValue;
            FirstVaccination = CatFirstVaccDate.SelectedDate ?? DateTime.MinValue;
            SecondVaccination = CatSecondVaccDate.SelectedDate ?? DateTime.MinValue;
            ThirdVaccination = CatThirdVaccDate.SelectedDate ?? DateTime.MinValue;
            DewormedTest = (CatDeworming.SelectedItem as ComboBoxItem)?.Content.ToString();
            DewormedTestDate = CatdDewormingDate.SelectedDate ?? DateTime.MinValue;
            RabiesTest = (CatRabiesTest.SelectedItem as ComboBoxItem)?.Content.ToString();
            RabiesTestDate = CatRabiesTestDate.SelectedDate ?? DateTime.MinValue;
            MedicalTreatmentTest = CatMedTreatmentDetail.Text;
            MedicalTreatmentTestDate = CatdMedTreatDate.SelectedDate ?? DateTime.MinValue;
            FleaTreatmentTest = (CatFleaTreatment.SelectedItem as ComboBoxItem)?.Content.ToString();
            FleaTreatmentTestDate = CatdFleaTreatmentDate.SelectedDate ?? DateTime.MinValue;
            FivStatus = (CatFevlFivStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            FIVDATE = CatdFleaTreatmentDate.SelectedDate ?? DateTime.MinValue;

            // Check if AnimalID exists in the database
            DatabaseHandler DB = new DatabaseHandler();
            if (!DB.AnimalIDExists(AnimalID))
            {
                MessageBox.Show("Animal ID does not exist in the database.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Add Medical Record to the database
            DB.AddMedicalRecordToDatabase(MedicalRecordID, AnimalID);
            DB.AddMedicalRecordDetailToDatabase(MedicalRecordID, Sterilization, FirstVaccination, SecondVaccination, ThirdVaccination, DewormedTest, DewormedTestDate, RabiesTest, RabiesTestDate, MedicalTreatmentTest, MedicalTreatmentTestDate, FleaTreatmentTest, FleaTreatmentTestDate);
            DB.AddCatSpecificMedicalRecordToDatabase(MedicalRecordID, FivStatus, FIVDATE);

            MessageBox.Show("Medical Record Saved", "", MessageBoxButton.OK, MessageBoxImage.Information);

        }

    }
}
