using System;

namespace Domain.Users
{
    public class UserDetail
    {
        // Added by EKT & DM
        public User User { get; set; }
        public byte Gender { get; set; }
        public DateTime Birthdate { get; set; }
    }
}