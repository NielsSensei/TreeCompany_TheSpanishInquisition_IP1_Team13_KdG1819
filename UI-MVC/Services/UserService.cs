using System.Linq;
using BL;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace UIMVC.Services
{
    public class UserService
    {
        private readonly UserManager<UIMVCUser> _usrMgr;
        

        public UserService(UserManager<UIMVCUser> userManager)
        {
            _usrMgr = userManager;
        }

        public string CollectUserName(string id)
        {
            var foundUser = _usrMgr.Users.FirstOrDefault(user => user.Id == id);
            if (foundUser == null)
            {
                return "NOTFOUND";
            }

            return foundUser.Name;
        }
    }
}