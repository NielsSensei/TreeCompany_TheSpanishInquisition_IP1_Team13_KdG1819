using System;

namespace Domain.Users
{
    public class Event
    {
        public int Id { get; set; }
        public Organisation Organisation { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}