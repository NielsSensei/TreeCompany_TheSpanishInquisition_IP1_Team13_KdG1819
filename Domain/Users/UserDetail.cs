using System;

namespace Domain.Users
{
    public class UserDetail
    {
        // Added by EKT & DM
        public User User { get; set; }
        public bool Gender { get; set; }
        public DateTime Birthdate { get; set; }
    }
}