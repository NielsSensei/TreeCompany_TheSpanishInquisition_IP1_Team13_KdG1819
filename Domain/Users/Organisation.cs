using System.Collections.Generic;
using Domain.Projects;

namespace Domain.Users
{
    public class Organisation : User
    {
        // Added by NG
        // Modified by XV
        public string OrgName { get; set; }
        public string WebsiteUrl { get; set; }
        public string Description { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<Ideation> Ideations { get; set; }
        
        // Added by EKT
        // Modified by NVZ
        // Methods
        #region
        //NOTE ABOUT EVENT: This is a keyword within .NET Core, thus we can't use it as a parameter name
        // so I changed it to orgEvent - EKT
        public void AddEvent(Event orgEvent)
        {
            Events.Add(orgEvent);
        }

        public void AddIdeation(Ideation ideation)
        {
            Ideations.Add(ideation);
        }
        #endregion
    }
}