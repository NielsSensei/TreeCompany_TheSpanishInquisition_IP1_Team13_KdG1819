using System.Linq;
using BL;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;

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
            return _usrMgr.Users.FirstOrDefault(user => user.Id == id.ToString())?.Name;
        }
    }
}