using System.Collections.Generic;
using Domain.Projects;

namespace Domain.Users
{
    public class PlatformOwner
    {
        // Added by NG
        // Modified by EKT & NVZ & XV
        public int Id { get; set; }
        public string Name { get; set; }
        public int PostalCode { get; set; }
        public ICollection<User> Admins { get; set; }
        public ICollection<Project> Projects { get; set; }
        public int PlatformID { get; set; }
        
        //Added by NG
        public PlatformOwner(int id, string name)
        {
            Id = id;
            Name = name;
        }
        
        // Added by EKT
        // Modified by NVZ
        // Methods
        #region
        public PlatformOwner GetPlatformOwnerInfo()
        {
            PlatformOwner info = new PlatformOwner(this.Id, this.Name);
            info.PostalCode = this.PostalCode;
            return info;
        }

        public void addAdmin(User admin)
        {
            Admins.Add(admin);
        }

        public void addProject(Project project)
        {
            Projects.Add(project);
        }
        #endregion
    }
}