using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_POE_PART_2.Classes
{
    public class Issue
    {
        // Constructor to initialize fields
        public Issue(string location, string category, string description, List<string> pictures)
        {
            Location = location;
            Category = category;
            Description = description;
            Pictures = pictures ?? new List<string>();
            Comments = new List<string>();
            Ratings = new Dictionary<string, int>();
        }

        public Issue()
        {
            Location = "";
            Category = "";
            Description = "";
            Pictures = new List<string>();
            Comments = new List<string>();
            Ratings = new Dictionary<string, int>();
        }

        // Properties
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<string> Pictures { get; set; }

        // List of comments
        public List<string> Comments { get; set; }

        // Dictionary to store ratings by user ID (or other key)
        public Dictionary<string, int> Ratings { get; set; }

        // Simplified properties to bind to UI
        public string Comment { get; set; }
        public int Rating { get; set; }

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
            if (Ratings.Count == 0) return 0;
            return (int)Ratings.Values.Average();
        }
    }
}
