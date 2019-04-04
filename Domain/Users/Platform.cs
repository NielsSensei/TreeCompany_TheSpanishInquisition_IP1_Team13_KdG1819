using System.Collections.Generic;
using Domain.Common;

namespace Domain.Users
{
    public class Platform
    {
        // Added by NG
        // Modified by EKT & NVZ & XV
        public List<User> Owners { get; set; }
        public List<User> Users { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Image Image { get; set; }

        //Added by NG
        public Platform(int id)
        {
            Id = id;
        }

        // Added by EKT
        // Modified by NVZ
        // Methods

        #region

        public void AddOwner(User owner)
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