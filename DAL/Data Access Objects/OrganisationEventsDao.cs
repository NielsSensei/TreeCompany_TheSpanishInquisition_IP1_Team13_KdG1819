﻿using System;
using Domain.Users;

namespace DAL.Data_Access_Objects
{
    /*
     * @authors Sacha Buelens & Niels Van Zandbergen
     */
    public class OrganisationEventsDao
    {
        public int EventId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PlatformId { get; set; }
    }
}
