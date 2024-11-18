using PROG_POE_PART_2.Classes;
using System.Collections.Generic;
using System.Linq;

public class Issue
{
    // Constructor to initialize fields
    public Issue(string location, string category, string description, List<string> pictures)
    {
        Location = location;
        Category = category;
        Description = description;
        Pictures = pictures;
        Comments = new List<string>();
        Ratings = new Dictionary<string, int>();
        Documents = new List<DocumentItem>();
        Videos = new List<string>();
    }

    public Issue() { }

    // Properties
    public string Location { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public List<string> Pictures { get; set; }
    public int Priority { get; set; }

    // List of comments
    public List<string> Comments { get; set; }
    // Dictionary to store ratings by user ID (or other key)
    public Dictionary<string, int> Ratings { get; set; }
    // Simplified properties to bind to UI
    public string Comment { get; set; }
    public int Rating { get; set; }

    // List of documents
    public List<DocumentItem> Documents { get; set; }
    // List of videos
    public List<string> Videos { get; set; }

    // Method to add or update a comment
    public void AddComment(string comment)
    {
        Comments.Add(comment);
    }

    // Method to add or update a rating for a user
    public void AddOrUpdateRating(string userId, int rating)
    {
        Ratings[userId] = rating;
    }

    // Method to get the average rating
    public int GetAverageRating()
    {
        return Ratings.Values.Any() ? (int)Ratings.Values.Average() : 0;
    }
}
