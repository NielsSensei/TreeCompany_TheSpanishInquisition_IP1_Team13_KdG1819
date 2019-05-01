using System.Collections.Generic;
using Domain.Common;
using Domain.Identity;

namespace Domain.Users
{
    public class Platform
    {
        // Added by NG
        // Modified by EKT & NVZ & XV & DM
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Image Image { get; set; }

        public List<UIMVCUser> Owners { get; set; }
        public List<UIMVCUser> Users { get; set; }

        // Added by EKT
        // Modified by NVZ
        // Methods

        #region

        public void AddOwner(UIMVCUser owner)
        {
            Owners.Add(owner);
        }

        public void AddUser(UIMVCUser user)
        {
            Users.Add(user);
        }

        #endregion
    }
}