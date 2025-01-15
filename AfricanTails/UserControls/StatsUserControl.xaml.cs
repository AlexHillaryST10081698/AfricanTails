using AfricanTails.Classes;
using LiveCharts.Wpf;
using LiveCharts;
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
using System.Collections.ObjectModel;

namespace AfricanTails.UserControls
{

    /// <summary>
    /// Interaction logic for StatsUserControl.xaml
    /// </summary>
    public partial class StatsUserControl : UserControl
    {
        public ObservableCollection<string> SexLabels { get; set; } = new ObservableCollection<string>();
        public ChartValues<int> SexCounts { get; set; } = new ChartValues<int>();


        public ChartValues<int> MaleCount { get; set; }
        public ChartValues<int> FemaleCount { get; set; }


        public int Breed1Count { get; set; }
        public int Breed2Count { get; set; }
        public int Breed3Count { get; set; }

        public StatsUserControl()
        {
            InitializeComponent();
            DisplayTotalAnimalCount();
            DisplayCommonBreeds();
            DisplayTotalStaffCount();
            DisplayAnimalSexCounts();

            // Initialize with placeholder data
            //MaleCount = new ChartValues<int> { 4 };
            //FemaleCount = new ChartValues<int> { 6 };

            PopulateBreedPieChart();

            DataContext = this;  // Set the DataContext so XAML can access the SexLabels property

        }
        private void DisplayTotalAnimalCount()
        {
            // Create an instance of the database handler
            DatabaseHandler dbHandler = new DatabaseHandler();

            // Get the total animal count
            int totalAnimals = dbHandler.GetTotalAnimalCount();

            // Assuming you have a Label control on your dashboard to display the total animal count
            lblTotalAnimals.Text =  totalAnimals.ToString();
        }
        
        private void DisplayCommonBreeds()
        {
            var dbHandler = new DatabaseHandler();  // Create an instance of DatabaseHandler
            int uniqueBreedCount = dbHandler.GetUniqueBreedCount();  // Get the count of unique breed
            // Display the count in the TextBlock
            if (uniqueBreedCount > 0)
            {
                lblMostCommonBreed.Text = $" {uniqueBreedCount}";
            }
            else
            {
                lblMostCommonBreed.Text = "No breed data available."; // If no breeds exist
            }

        }
        private void DisplayTotalStaffCount()
        {
            // Create an instance of the database handler
            DatabaseHandler dbHandler = new DatabaseHandler();

            // Get the total animal count
            int totalStaff = dbHandler.GetTotalStaffCount();

            // Assuming you have a Label control on your dashboard to display the total animal count
            lblTotalStaff.Text = totalStaff.ToString();
        }

        private void DisplayAnimalSexCounts()
        {
            try
            {
                
                DatabaseHandler dbHandler = new DatabaseHandler();
                var sexCounts = dbHandler.GetAnimalCountBySex();

                // Check if the data is null or empty
                if (sexCounts == null || !sexCounts.Any())
                {
                    throw new Exception("No data returned from GetAnimalCountBySex.");
                }

                // Clear existing data to avoid duplication
                SexLabels.Clear();
                SexCounts.Clear();

                // Populate SexLabels and SexCounts with the data
                foreach (var kvp in sexCounts)
                {
                    SexLabels.Add(kvp.Key);   // Add the sex label (e.g., "Female", "Male")
                    SexCounts.Add(kvp.Value); // Add the corresponding count (e.g., 2, 3)
                }

                // Force the chart to refresh after updating data
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    sexDistributionChart.Update(true, true); // Force chart to redraw
                }));
            }
            catch (Exception ex)
            {
                // Log or display any error that occurs
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void PopulateBreedPieChart()
        {
            // Call the GetCommonBreeds() method
            DatabaseHandler dbHandler = new DatabaseHandler();
            List<KeyValuePair<string, int>> breedCounts = dbHandler.GetCommonBreeds();

            // Debugging output
            Console.WriteLine($"Breed Count: {breedCounts?.Count ?? 0}");

            // If breedCounts is null or empty, show a message
            if (breedCounts == null || !breedCounts.Any())
            {
                MessageBox.Show("No breed data available.");
                return; // Exit the method early if no data
            }

            // Prepare PieSeries for each breed
            SeriesCollection breedSeries = new SeriesCollection();

            // Add PieSeries for each breed
            foreach (var breed in breedCounts)
            {
                breedSeries.Add(new PieSeries
                {
                    Title = breed.Key, // Breed name (e.g., "Border Collie")
                    Values = new ChartValues<int> { breed.Value }, // Count of the breed
                    DataLabels = true, // Show the data labels on the chart
                    LabelPoint = chartPoint => $"{chartPoint.Y} ({chartPoint.Participation:P})" // Show count and percentage
                });
            }

            // Clear existing series before adding new ones (optional but recommended)
            sexProportionsChart.Series.Clear();

            // Bind data to the pie chart
            sexProportionsChart.Series = breedSeries;

            // Force the chart to refresh after updating data
            Dispatcher.BeginInvoke(new Action(() =>
            {
                sexProportionsChart.Update(true, true); // Force chart to redraw
                sexProportionsChart.InvalidateVisual();
            }));
        }








    }
}

