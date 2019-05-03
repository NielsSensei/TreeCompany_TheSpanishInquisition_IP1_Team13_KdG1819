using System.Collections.Generic;
using Domain.Identity;
using Domain.Projects;

namespace Domain.Users
{
    public class Organisation : UIMVCUser
    {
        // Added by NG
        // Modified by XV & NVZ & DM & EKT
        public string OrgName { get; set; }
        public string Description { get; set; }
        
        public List<Event> organisationEvents { get; set; }
        public List<Ideation> Ideations { get; set; }
    }
}