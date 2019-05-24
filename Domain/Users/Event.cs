using System;

namespace Domain.Users
{
    /*
     * @authors David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
     */
    public class Event
    {
        public int Id { get; set; }
        public Organisation Organisation { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Platform Platform { get; set; }
    }
}
