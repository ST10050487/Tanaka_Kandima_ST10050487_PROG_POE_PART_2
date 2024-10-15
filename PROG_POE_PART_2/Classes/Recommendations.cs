using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_POE_PART_2.Classes
{
    public class Recommendations
    {
        private Hashtable searchHistory;
        private Hashtable eventPreferences;
        public List<Event> allEvents { get; set; }
        public List<Event> recommendedEvents { get; set; }
        //********************************************************************************************NAKA*********************************************************************************************//
        //Constructor
        public Recommendations()
        {
            searchHistory = new Hashtable();
            eventPreferences = new Hashtable();
            allEvents = new List<Event>();
            recommendedEvents = new List<Event>();
        }
        //A method to load default event recommendations
        public List <Event> LoadDefaultRecommendations()
        {
            allEvents.Add(new Event("Health Fair", new DateTime(2024, 11, 5), "Community Health Center", new DateTime(2024, 11, 5, 9, 0, 0), "Municipal event: Free health screenings and wellness seminars.", "Health", 3));
            allEvents.Add(new Event("Environmental Awareness Day", new DateTime(2024, 11, 15), "City Park", new DateTime(2024, 11, 15, 10, 0, 0), "Municipal event: Learn about eco-friendly practices.", "Environment", 4));
            allEvents.Add(new Event("Job Fair", new DateTime(2024, 11, 25), "Town Hall", new DateTime(2024, 11, 25, 8, 30, 0), "Municipal event: Connect with local employers and job opportunities.", "Employment", 2));
            return allEvents;
        }
        //********************************************************************************************NAKA*********************************************************************************************//
        //A method to log the search history
        public void LogSearchHistory(string search)
        {
            // Ignoring empty or very short searches
            if (string.IsNullOrWhiteSpace(search) || search.Length < 3) return;

            search = search.ToLower().Trim();

            if (searchHistory.ContainsKey(search))
            {
                searchHistory[search] = (int)searchHistory[search] + 1;
            }
            else
            {
                searchHistory.Add(search, 1);
            }
        }
        //********************************************************************************************NAKA*********************************************************************************************//
        //A method to track event clicks and searches separately
        public void LogEventClick(Event ev)
        {
            // Logging event selection/click
            if (eventPreferences.ContainsKey(ev))
            {
                // Incrementing priority based on clicks
                eventPreferences[ev] = (int)eventPreferences[ev] + 1;
            }
            else
            {
                // Start with a rating from 1
                eventPreferences.Add(ev, 1);
            }
        }
        //********************************************************************************************NAKA*********************************************************************************************//
        //A method to log the event preferences
        public void LogEventPreferences(Event ev, int rating)
        {
            if (eventPreferences.ContainsKey(ev))
            {
                // Ensure rating is between 1 and 5
                eventPreferences[ev] = Math.Min((int)eventPreferences[ev] + rating, 5);
            }
            else
            {
                eventPreferences.Add(ev, rating);
            }
        }

        //********************************************************************************************NAKA*********************************************************************************************//
        //A method to get the recommended events
        public List<Event> GetRecommendedEvents()
        {


            foreach (var eventEntry in allEvents)
            {
                Debug.WriteLine("Checking event: " + eventEntry.Name);

                // Add base recommendation
                recommendedEvents.Add(eventEntry);

                // Boost recommendation if the event name matches popular searches
                if (searchHistory.Keys.Cast<string>().Any(search =>
                    eventEntry.Name.ToLower().Contains(search) ||
                    eventEntry.Description.ToLower().Contains(search) ||
                    eventEntry.Category.ToLower().Contains(search)))
                {
                    recommendedEvents.Add(eventEntry);
                    Debug.WriteLine("Recommended based on search history: " + eventEntry.Name);
                }

                // Boost recommendation based on event preferences
                if (eventPreferences.ContainsKey(eventEntry))
                {
                    int rating = (int)eventPreferences[eventEntry];
                    // Consider adding the event multiple times based on its rating, for example
                    for (int i = 0; i < rating; i++)
                    {
                        recommendedEvents.Add(eventEntry);
                    }
                }
            }

            // Logging recommended events
            foreach (var eventEntry in recommendedEvents.Distinct()) // Avoid duplicates
            {
                Debug.WriteLine("Recorded Recommended Events: " + eventEntry.Name);
            }

            return recommendedEvents.Distinct().ToList(); // Return unique recommendations
        }

        //********************************************************************************************NAKA*********************************************************************************************//
        // A method to get the recorded search history
        public Hashtable GetSearchHistory()
        {
            return searchHistory;
        }
    }
}
