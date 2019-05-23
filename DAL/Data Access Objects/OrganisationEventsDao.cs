using System;
using Domain.Users;

namespace DAL.Data_Access_Objects
{
    public class OrganisationEventsDao
    {
        public int EventId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Platform Platform { get; set; }
    }
}
