using System;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
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
        
        [PersonalData]
        public int PlatformDetails { get; set; }
        
        [PersonalData]
        public string OrgName { get; set; }
        
        [PersonalData]
        public string Description { get; set; }

        public bool Banned { get; set; }

        public bool Active { get; set; }
    }
}