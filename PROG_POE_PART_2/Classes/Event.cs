using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_POE_PART_2.Classes
{
    public class Event
    {
        private string name;
        private DateTime date;
        private string venue;
        private DateTime startTime;
        private string description;
        private string category;
        private int priority;

        public Event(string name, DateTime date, string venue, DateTime startTime, string description, string category, int priority)
        {
            this.Name = name;
            this.Date = date;
            this.Venue = venue;
            this.StartTime = startTime;
            this.Description = description;
            this.Category = category;
            this.Priority = priority;
        }

        public string Name { get => name; set => name = value; }
        public DateTime Date { get => date; set => date = value; }
        public string Venue { get => venue; set => venue = value; }
        public DateTime StartTime { get => startTime; set => startTime = value; }
        public string Description { get => description; set => description = value; }
        public string Category { get => category; set => category = value; }
        public int Priority { get => priority; set => priority = value; }
    }
}
