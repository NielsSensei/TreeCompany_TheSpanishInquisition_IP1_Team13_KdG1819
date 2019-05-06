using System;
using System.Collections.Generic;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BL
{
    public class UserManager
    {
        /*
        public void VerifyUser(string userToVerify)
        {
            // How does UserManager (Identity) save objects?
            var alteredUser = GetUser(userToVerify, false);
            if (alteredUser.Role == Role.LOGGEDIN && alteredUser.Active)
            {
                alteredUser.Role = Role.LOGGEDINVERIFIED;
                UserRepo.Update(alteredUser);
            }
        }

        public void ToggleBanUser(string userId)
        {
            // Meow meow woof woof moooooooooo
            var alteredUser = GetUser(userId, false);
            if (alteredUser.Banned)
            {
                alteredUser.Banned = false;
                alteredUser.Active = true;
            }
            else
            {
                alteredUser.Banned = true;
                alteredUser.Active = false;
            }

            UserRepo.Update(alteredUser);
        }*/
    }
}