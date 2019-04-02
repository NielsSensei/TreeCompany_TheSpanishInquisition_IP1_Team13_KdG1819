using System;

namespace Domain.Users
{
    public class UserDetail
    {
        public User User { get; set; }
        public bool Gender { get; set; }
        public DateTime Birthdate { get; set; }
    }
}