using System.Collections.Generic;

namespace Domain.Users
{
    public class Platform
    {
        // Added by NG
        // Modified by EKT & NVZ & XV
        public int Id { get; set; }
        public ICollection<PlatformOwner> Owners { get; set; }
        public ICollection<User> Users { get; set; }

        //Added by NG
        public Platform(int id)
        {
            Id = id;
        }

        // Added by EKT
        // Modified by NVZ
        // Methods
        #region
        public void AddOwner(PlatformOwner owner)
        {
            Owners.Add(owner);
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }
        #endregion
    }
}