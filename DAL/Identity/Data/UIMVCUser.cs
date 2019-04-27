using System;
using Microsoft.AspNetCore.Identity;

namespace DAL.Identity.Data
{
    public class UIMVCUser : IdentityUser
    {

        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        public string Zipcode { get; set; }

        [PersonalData]
        public bool Gender { get; set; }

        [PersonalData]
        public DateTime DateOfBirth { get; set; }

        public bool Banned { get; set; }

        public bool Active { get; set; }
    }
}