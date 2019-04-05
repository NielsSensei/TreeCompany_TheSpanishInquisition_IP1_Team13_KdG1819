using System.Collections.Generic;
using Domain.Projects;

namespace Domain.Users
{
    public class Organisation : User
    {
        // Added by NG
        // Modified by XV & NVZ & DM & EKT
        public string OrgName { get; set; }
        public string OrgUrl { get; set; }
        public string Description { get; set; }
        
        public List<Event> organisationEvents { get; set; }
        public List<Ideation> Ideations { get; set; }
    }
}