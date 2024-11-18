using System;
using System.Collections.Generic;

namespace PROG_POE_PART_2.Classes
{
    public class ServiceRequest
    {
        private static readonly string[] Statuses = { "Pending", "In Progress", "Resolved", "Closed" };

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<string> Images { get; set; }
        public List<DocumentItem> Documents { get; set; }
        public List<string> Videos { get; set; }

        public ServiceRequest(int id, string name, string description, string status, DateTime createdAt, DateTime? completedAt)
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
            CreatedAt = createdAt;
            CompletedAt = completedAt;
            Images = new List<string>();
            Documents = new List<DocumentItem>();
            Videos = new List<string>();
        }

        public void AssignRandomStatus()
        {
            Random random = new Random();
            Status = Statuses[random.Next(Statuses.Length)];
        }
    }

    public class DocumentItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
    }

}
