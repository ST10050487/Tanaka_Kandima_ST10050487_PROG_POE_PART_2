using PROG_POE_PART_2.Classes;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using System.Diagnostics;

public class Recommendations
{
    private Hashtable searchHistory = new Hashtable();
    private Hashtable eventPreferences = new Hashtable();
    public List<Event> allEvents { get; set; } = new List<Event>();
    public List<Event> recommendedEvents  = new List<Event>();
    private EventGraph eventGraph = new EventGraph();
    private EventHeap eventHeap = new EventHeap();
    List<Event> recommended = new List<Event>();

    public Recommendations()
    {
        // Initialize with some default events
        LoadDefaultRecommendations();
    }

    public List<Event> LoadDefaultRecommendations()
    {
        List<Event> defaultEvents = new List<Event>
        {
            new Event("Health Fair", new DateTime(2024, 11, 5), "Community Health Center", new DateTime(2024, 11, 5, 9, 0, 0), "Municipal event: Free health screenings and wellness seminars.", "Health", 3),
            new Event("Environmental Awareness Day", new DateTime(2024, 11, 15), "City Park", new DateTime(2024, 11, 15, 10, 0, 0), "Municipal event: Learn about eco-friendly practices.", "Environment", 4),
            new Event("Job Fair", new DateTime(2024, 11, 25), "Town Hall", new DateTime(2024, 11, 25, 8, 30, 0), "Municipal event: Connect with local employers and job opportunities.", "Employment", 2)
        };

        allEvents.AddRange(defaultEvents);
        return defaultEvents;
    }

    public void LogSearchHistory(string search)
    {
        if (searchHistory.ContainsKey(search))
            searchHistory[search] = (int)searchHistory[search] + 1;
        else
            searchHistory.Add(search, 1);
    }

    public void LogEventPreferences(Event ev, int rating)
    {
        if (eventPreferences.ContainsKey(ev))
            eventPreferences[ev] = (int)eventPreferences[ev] + rating;
        else
            eventPreferences.Add(ev, rating);

        eventHeap.Insert(ev);
    }

    public List<Event> GetRecommendedEvents()
    {
        //recommended.Clear();

        // Use the heap to get top priority events
        while (eventHeap.Count > 0)
        {
            recommended.Add(eventHeap.ExtractMax());
        }

        // Use graph traversal to find related events
        foreach (var ev in recommended)
        {
            recommended.AddRange(eventGraph.BFS(ev));
        }

        recommendedEvents = recommended.Distinct().ToList();
        return recommendedEvents;
    }

    public Hashtable GetSearchHistory()
    {
        return searchHistory;
    }

    // A method to display the recommendations
    public void DisplayRecommendations(List<Event> recommendedEvents)
    {
        GetRecommendedEvents();
        foreach (var eventItem in recommendedEvents)
        {
            Debug.WriteLine("Events Recorded <--> " + eventItem.Name);
        }
    }
}
