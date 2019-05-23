using System.Collections.Generic;
using Domain.Identity;
using Domain.Projects;

namespace Domain.Users
{
    /*
     * @authors Nathan David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
     */
    public class Organisation : UimvcUser
    {
        public string NewOrgName { get; set; }
        public string NewDescription { get; set; }
        public List<Event> OrganisationEvents { get; set; }
        public List<Ideation> Ideations { get; set; }
    }
}
