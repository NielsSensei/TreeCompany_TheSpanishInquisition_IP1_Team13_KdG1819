using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace UIMVC.Services
{
    public class UserService
    {
        private readonly UserManager<UimvcUser> _usrMgr;


        public UserService(UserManager<UimvcUser> userManager)
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

        public async Task<int> GetUserPlatform(ClaimsPrincipal user)
        {
            var userFound = await _usrMgr.GetUserAsync(user);
            if (userFound == null) return 0;

            return userFound.PlatformDetails;
        }
    }
}
