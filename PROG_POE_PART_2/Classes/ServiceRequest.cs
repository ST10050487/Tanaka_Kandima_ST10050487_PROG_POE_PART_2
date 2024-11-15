using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_POE_PART_2.Classes
{
    public class ServiceRequest
    {
        private int id;
        private string name;
        private string description;
        private string status;
        private DateTime createdAt;
        private DateTime? completedAt;

        public ServiceRequest(int id, string name, string description, string status, DateTime createdAt, DateTime? completedAt)
        {
            Id = id;
            this.Name = name;
            this.Description = description;
            this.Status = status;
            this.CreatedAt = createdAt;
            this.CompletedAt = completedAt;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Status { get => status; set => status = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public DateTime? CompletedAt { get => completedAt; set => completedAt = value; }

    }
}
