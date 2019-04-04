using System;

namespace Domain.Users
{
    public class Event
    {
        // Added BY NG
        // Modified by EKT & NVZ & DM
        public int Id { get; set; }
        public Organisation Organisation { get; set; }
        internal string Name { get; set; }
        internal string Description { get; set; }
        internal DateTime StartDate { get; set; }
        internal DateTime EndDate { get; set; }
    }
}