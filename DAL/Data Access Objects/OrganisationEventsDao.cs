using System;

namespace DAL.Data_Access_Objects
{
    /*
     * @authors Nathan Gijselings & Niels Van Zandbergen
     */
    public class OrganisationEventsDao
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
