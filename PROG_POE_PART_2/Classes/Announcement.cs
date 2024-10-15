using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_POE_PART_2.Classes
{
    public class Announcement
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public int Priority { get; set; }

        public Announcement(string title, string description, DateTime date, string category, int priority)
        {
            Title = title;
            Description = description;
            Date = date;
            Category = category;
            Priority = priority;
        }
        //Formating the date to a string
        public string Time => Date.ToString("hh:mm tt"); 
    }
}
