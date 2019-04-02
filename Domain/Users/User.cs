using System;
using System.Collections.Generic;
using Domain.UserInput;

namespace Domain.Users
{
    public class User
    {
        // Added by NG
        // Modified by XV & NVZ
        public int Id { get; set; }
        public Platform Platform { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ZipCode { get; set; }
        public Role Role { get; set; }
        public bool Banned { get; set; }
        public bool? Gender { get; set; }
        public bool Active { get; set; }
        public DateTime? BirthDate { get; set; }

        public List<Interaction> Interactions{ get; set; }
            // Added by EKT
        // Modified by NVZ
        // Methods

        #region

        public User GetUserInfo()
        {
            User info = new User()
            {
                Name = this.Name,
                ZipCode = this.ZipCode,
                Role = this.Role,
                Banned = this.Banned,
                Gender = this.Gender,
                BirthDate = this.BirthDate
            };
            return info;
        }

        public void SetActive(bool active)
        {
            Active = active;
        }

        public void SetBanned(bool banned)
        {
            Banned = banned;
        }

        public void UpdateRole(Role role)
        {
            Role = role;
        }
        #endregion
    }
}