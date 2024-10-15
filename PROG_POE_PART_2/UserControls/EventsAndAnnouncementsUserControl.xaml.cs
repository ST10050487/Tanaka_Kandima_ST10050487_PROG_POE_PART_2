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

namespace PROG_POE_PART_2.UserControls
{
    /// <summary>
    /// Interaction logic for EventsAndAnnouncementsUserControl.xaml
    /// </summary>
    public partial class EventsAndAnnouncementsUserControl : UserControl
    {
        private Queue<Event> eventQueue = new Queue<Event>();
        private Stack<Event> undoStack = new Stack<Event>();
        private SortedDictionary<DateTime, List<Event>> sortedEvents = new SortedDictionary<DateTime, List<Event>>();
        private Dictionary<string, List<Event>> eventsByCategory = new Dictionary<string, List<Event>>();
        private HashSet<string> uniqueCategories = new HashSet<string>();
        public Stack<Announcement> AnnouncementsStack { get; set; }
        // Referencing to HomeScreenUserControl
        private HomeScreenUserControl homeScreenUserControl = new HomeScreenUserControl();
        private List<Event> recommendedEvents;

        //Creating a Instance of the Recommendations class
        Recommendations recommendations = new Recommendations();
        //************************************************************************************NAKA*********************************************************************************************//
        //Default Constructor
        public EventsAndAnnouncementsUserControl()
        {
            InitializeComponent();
            if (categoryCmbBox != null)
            {
                LoadCategories();
            }

            LoadEvents();
            LoadAnnouncements();
            announcementsList.ItemsSource = AnnouncementsStack.ToList();
            DataContext = this;
        }
        public EventsAndAnnouncementsUserControl(HomeScreenUserControl homeScreenControl)
        {
            InitializeComponent();
            homeScreenUserControl = homeScreenControl ?? throw new ArgumentNullException(nameof(homeScreenControl));

            if (categoryCmbBox != null)
            {
                LoadCategories();
            }

            LoadEvents();
            LoadAnnouncements();
            announcementsList.ItemsSource = AnnouncementsStack.ToList();
            DataContext = this;
        }
        //************************************************************************************NAKA*********************************************************************************************//
        //A method to load events
        private void LoadEvents()
        {
            List<Event> initialEvents = new List<Event>
            {
            new Event("Health Fair", new DateTime(2024, 11, 5), "Community Health Center", new DateTime(2024, 11, 5, 9, 0, 0), "Municipal event: Free health screenings and wellness seminars.", "Health", 3),
            new Event("Environmental Awareness Day", new DateTime(2024, 11, 15), "City Park", new DateTime(2024, 11, 15, 10, 0, 0), "Municipal event: Learn about eco-friendly practices.", "Environment", 4),
            new Event("Job Fair", new DateTime(2024, 11, 25), "Town Hall", new DateTime(2024, 11, 25, 8, 30, 0), "Municipal event: Connect with local employers and job opportunities.", "Employment", 2),
            new Event("Family Fun Day", new DateTime(2024, 12, 2), "Central Park", new DateTime(2024, 12, 2, 12, 0, 0), "Municipal event: Games, rides, and activities for the whole family.", "Recreation", 5),
            new Event("Business Networking Event", new DateTime(2024, 12, 12), "City Conference Center", new DateTime(2024, 12, 12, 17, 0, 0), "Municipal event: Build connections with local business leaders.", "Business", 1),
            new Event("Holiday Lighting Ceremony", new DateTime(2024, 12, 8), "City Square", new DateTime(2024, 12, 8, 18, 0, 0), "Municipal event: Annual lighting of the holiday tree and decorations.", "Community", 2),
            new Event("Blood Donation Drive", new DateTime(2024, 11, 10), "Community Hall", new DateTime(2024, 11, 10, 9, 0, 0), "Municipal event: Help save lives by donating blood.", "Health", 4),
            new Event("Senior Citizens Day", new DateTime(2024, 12, 18), "Senior Center", new DateTime(2024, 12, 18, 10, 0, 0), "Municipal event: A day dedicated to the senior citizens in the community.", "Community", 5),
            new Event("Cultural Festival", new DateTime(2024, 11, 22), "Cultural Center", new DateTime(2024, 11, 22, 13, 0, 0), "Municipal event: Celebrate the cultural diversity of the community.", "Culture", 3),
            new Event("Winter Sports Day", new DateTime(2024, 12, 30), "Snowy Peaks Park", new DateTime(2024, 12, 30, 9, 0, 0), "Municipal event: A day of snow sports and winter activities.", "Sports", 1),
            };

            foreach (var ev in initialEvents)
            {
                eventQueue.Enqueue(ev);
                AddEventToDataStructures(ev);
            }
             // Add loaded events to the recommendation system
    recommendations.allEvents.AddRange(initialEvents);

            eventsList.ItemsSource = sortedEvents.SelectMany(kvp => kvp.Value).ToList();
        }
        //************************************************************************************NAKA*********************************************************************************************//
        private void LoadAnnouncements()
        {
            AnnouncementsStack = new Stack<Announcement>();

            // Adding announcements to the Stack
            AnnouncementsStack.Push(new Announcement("Water Supply Disruption", "There will be water supply disruptions in the downtown area on October 20.", new DateTime(2024, 10, 20, 9, 30, 0), "Public Notice", 1));
            AnnouncementsStack.Push(new Announcement("Library Renovation", "The city library will be closed for renovations from November 1 to November 10.", new DateTime(2024, 11, 1, 8, 0, 0), "Community", 2));
            AnnouncementsStack.Push(new Announcement("Road Maintenance", "Road maintenance will occur on Main Street starting October 25.", new DateTime(2024, 10, 25, 7, 0, 0), "Infrastructure", 3));
            AnnouncementsStack.Push(new Announcement("Holiday Parade", "The annual holiday parade will take place on December 15 at 5:00 PM.", new DateTime(2024, 12, 15, 17, 0, 0), "Event", 4));
            AnnouncementsStack.Push(new Announcement("COVID-19 Vaccination Drive", "A COVID-19 vaccination drive will be held on October 22 at the community center.", new DateTime(2024, 10, 22, 10, 0, 0), "Health", 5));
        }
        //************************************************************************************NAKA*********************************************************************************************//
        // A method to add events to data structures
        private void AddEventToDataStructures(Event ev)
        {
            if (!sortedEvents.ContainsKey(ev.Date))
            {
                sortedEvents[ev.Date] = new List<Event>();
            }
            sortedEvents[ev.Date].Add(ev);

            if (!eventsByCategory.ContainsKey(ev.Category))
            {
                eventsByCategory[ev.Category] = new List<Event>();
            }
            eventsByCategory[ev.Category].Add(ev);
        }
        //************************************************************************************NAKA*********************************************************************************************//
        // A method to search for events
        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = searchBox.Text.ToLower();
            // Logging the search query
            recommendations.LogSearchHistory(searchQuery);
            // Calling the filter events method
            FilterEvents();
        }
        //************************************************************************************NAKA*********************************************************************************************//
        private void eventsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (eventsList.SelectedItem is Event selectedEvent)
            {
                // Logging event preference with an arbitrary rating of 1 (for an interaction like selection)
                recommendations.LogEventPreferences(selectedEvent, 4);

                // Get recommended events and display them in the HomeScreenUserControl
                List<Event> recommendedEvents = recommendations.GetRecommendedEvents();
                homeScreenUserControl.DisplayRecommendations(recommendedEvents);
            }
        }
        //************************************************************************************NAKA*********************************************************************************************//
        // A method to filter events by category
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            searchBox.Text = "";
            FilterEvents();
        }
        //************************************************************************************NAKA*********************************************************************************************//
        // A method to undo the last event added
        private void FilterEvents()
        {
            // Preventing filtering if the category combo box is not loaded or no category is selected
            if (categoryCmbBox == null || categoryCmbBox.SelectedItem == null)
            {
                return;
            }
            string searchQuery = searchBox.Text.ToLower();
            string selectedCategory = ((ComboBoxItem)categoryCmbBox.SelectedItem).Content.ToString();
            Debug.WriteLine("Selected Category: " + selectedCategory);

            var filteredEvents = sortedEvents.SelectMany(kvp => kvp.Value)
                                             .Where(ev => (selectedCategory == "All Categories" || ev.Category == selectedCategory)
                                                       && (ev.Name.ToLower().Contains(searchQuery)
                                                       || ev.Date.ToString().Contains(searchQuery)))
                                             // Sorting by date
                                             .OrderBy(ev => ev.Date)
                                             .ToList();
            // Clearing the list box and re-binding with the filtered events
            eventsList.ItemsSource = null;
            // Re-binding the list box with the filtered events
            eventsList.ItemsSource = filteredEvents;
            // Getting recommended events
             recommendedEvents =  recommendations.GetRecommendedEvents();
            //Checking if the recommended events are not null
            if (homeScreenUserControl != null)
            {
                // Displaying the recommended events in the HomeScreenUserControl
                homeScreenUserControl.DisplayRecommendations(recommendedEvents);
                Debug.WriteLine("Recommended Events: " + recommendedEvents.ToString());
            }
            else
            {
                Debug.WriteLine("homeScreenUserControl is null");
            }
        }
        //************************************************************************************NAKA*********************************************************************************************//
        // A method to filter announcements based on date range
        private void FilterByDate_Click(object sender, RoutedEventArgs e)
        {
            // Getting the start and end dates from the date pickers
            DateTime startDate = startDatePicker.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = endDatePicker.SelectedDate ?? DateTime.MaxValue;

            // Filtering announcements based on the date range
            var filteredAnnouncements = AnnouncementsStack.Where(announcement => announcement.Date >= startDate && announcement.Date <= endDate)
                                                           .OrderBy(announcement => announcement.Date)
                                                           .ToList();
            // Clearing the list box and re-binding with the filtered announcements
            announcementsList.ItemsSource = null;
            // Re-binding the list box with the filtered announcements
            announcementsList.ItemsSource = filteredAnnouncements;
        }
        //************************************************************************************NAKA*********************************************************************************************//
        // A method to populate the combo box with categories
        private void LoadCategories()
        {
            uniqueCategories.Add("All Categories");
            uniqueCategories.Add("Community");
            uniqueCategories.Add("Sports");
            uniqueCategories.Add("Health");
            uniqueCategories.Add("Culture");
            uniqueCategories.Add("Business");
            uniqueCategories.Add("Employment");
            uniqueCategories.Add("Environment");

            foreach (var category in uniqueCategories)
            {
                categoryCmbBox.Items.Add(new ComboBoxItem() { Content = category });
            }
            categoryCmbBox.SelectedIndex = 0;
        }
        //************************************************************************************NAKA*********************************************************************************************//
    }
}
