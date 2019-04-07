using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UIMVC.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the UIMVCUser class
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
