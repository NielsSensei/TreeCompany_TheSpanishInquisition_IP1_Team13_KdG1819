using System;

namespace Domain.Users
{
    public class Event
    {
        // Added BY NG
        // Modified by EKT & NVZ
        public int Id { get; set; }
        internal string Name { get; set; }
        internal string Description { get; set; }
        internal DateTime StartDate { get; set; }
        internal DateTime EndDate { get; set; }
        public int organiserID { get; set; }
    }
}