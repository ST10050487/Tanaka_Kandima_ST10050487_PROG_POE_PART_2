using PROG_POE_PART_2.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace PROG_POE_PART_2.UserControls
{
    /// <summary>
    /// Interaction logic for HomeScreenUserControl.xaml
    /// </summary>
    public partial class HomeScreenUserControl : UserControl
    {
        Recommendations recommendations = new Recommendations();
        public HomeScreenUserControl()
        {
            InitializeComponent();
            this.DataContext = this;
            // Initialize current date and time
            InitializeDateTimeDisplay();
            // Load and display default recommended events directly in the constructor
            List<Event> defaultRecommendedEvents = LoadDefaultRecommendations();
            DisplayRecommendations(defaultRecommendedEvents);
            LoadIssues();
        }
        private void InitializeDateTimeDisplay()
        {
            // Set up a dispatcher timer to update the current date and time every second
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, args) =>
            {
                // Update the TextBlock with the current date and time
                currentDateTimeTextBlock.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy HH:mm:ss");
            };
            timer.Start();
        }
        public List<Event> LoadDefaultRecommendations()
        {
            List<Event> allEvents = new List<Event>(); // Ensure allEvents is initialized

            allEvents.Add(new Event("Health Fair", new DateTime(2024, 11, 5), "Community Health Center", new DateTime(2024, 11, 5, 9, 0, 0), "Municipal event: Free health screenings and wellness seminars.", "Health", 3));
            allEvents.Add(new Event("Environmental Awareness Day", new DateTime(2024, 11, 15), "City Park", new DateTime(2024, 11, 15, 10, 0, 0), "Municipal event: Learn about eco-friendly practices.", "Environment", 4));
            allEvents.Add(new Event("Job Fair", new DateTime(2024, 11, 25), "Town Hall", new DateTime(2024, 11, 25, 8, 30, 0), "Municipal event: Connect with local employers and job opportunities.", "Employment", 2));

            return allEvents; // Return the list of events
        }
        // Method to load and display recommended events
        private void LoadIssues()
        {
            // Fetching issues from the IssueManager and bind to ListView
            reportedIssuesListView.ItemsSource = IssueManager.Issues;

            // Check if there are any issues and update the visibility of the message
            if (IssueManager.Issues == null || !IssueManager.Issues.Any())
            {
                noIssuesMessage.Visibility = Visibility.Visible;
                reportedIssuesListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                noIssuesMessage.Visibility = Visibility.Collapsed;
                reportedIssuesListView.Visibility = Visibility.Visible;
            }
        }

        private void LoadRecommendedEvents()
        {
            // Load default recommendations first
            List<Event> defaultRecommendedEvents = recommendations.LoadDefaultRecommendations().ToList(); // Updated this line

            // Display default recommended events
            this.DisplayRecommendations(defaultRecommendedEvents);

            // Get additional recommended events based on user behavior
            List<Event> additionalRecommendedEvents = recommendations.GetRecommendedEvents();

            // Display additional recommended events, ensuring not to duplicate those already displayed
            var uniqueRecommendedEvents = additionalRecommendedEvents.Except(defaultRecommendedEvents).ToList();
            this.DisplayRecommendations(uniqueRecommendedEvents);
        }

        public void DisplayRecommendations(List<Event> recommendedEvents)
        {
            // Clear existing items in the ListView
            //recommendationListView.Items.Clear();

            // Set the ItemsSource for the ListView directly
            recommendationListView.ItemsSource = recommendedEvents;

            // Optionally log the displayed events
            foreach (var eventItem in recommendedEvents)
            {
                Debug.WriteLine("Recommended Events: " + eventItem.Name);
            }
        }
    }
}
