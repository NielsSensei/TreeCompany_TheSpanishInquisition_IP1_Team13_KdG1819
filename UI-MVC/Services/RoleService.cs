using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Identity;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace UIMVC.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UIMVCUser> _userManager;
        private readonly IConfiguration _configuration;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<UIMVCUser> userManager, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            
            CreateRoles();
            CreateTestUsers();
        }

        #region TestUser

        private async void CreateTestUsers()
        {
            if (_userManager.FindByEmailAsync(_configuration["SuperAdmin:Email"]) != null)
            {
                UIMVCUser user = new UIMVCUser
                {
                    Name = _configuration["SuperAdmin:AccountName"],
                    Email = _configuration["SuperAdmin:Email"],
                    UserName = _configuration["SuperAdmin:Email"],
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(user, _configuration["SuperAdmin:Secret"]);
                
                var userFound = await _userManager.FindByEmailAsync(user.UserName);
                AssignToRole(userFound, Role.SUPERADMIN);
            }
            if (_userManager.FindByEmailAsync(_configuration["Admin:Email"]) != null)
            {
                UIMVCUser user = new UIMVCUser
                {
                    Name = _configuration["Admin:AccountName"],
                    Email = _configuration["Admin:Email"],
                    UserName = _configuration["Admin:Email"],
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(user, _configuration["Admin:Secret"]);
                
                var userFound = await _userManager.FindByEmailAsync(user.UserName);
                AssignToRole(userFound, Role.ADMIN);
            }
            if (_userManager.FindByEmailAsync(_configuration["Moderator:Email"]) != null)
            {
                UIMVCUser user = new UIMVCUser
                {
                    Name = _configuration["Moderator:AccountName"],
                    Email = _configuration["Moderator:Email"],
                    UserName = _configuration["Moderator:Email"],
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(user, _configuration["SuperAdmin:Secret"]);
                
                var userFound = await _userManager.FindByEmailAsync(user.UserName);
                AssignToRole(userFound, Role.MODERATOR);
            }
        }

        #endregion

        #region CreateRoles

        // Check if the roles exist, if not, create them
        private async void CreateRoles()
        {
            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    _roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }
        }

        #endregion

        #region AddRole

        public async void  AssignToRole(UIMVCUser user, Role role)
        {
            if (!await _roleManager.RoleExistsAsync(role.ToString())) return;
            if (await _userManager.FindByIdAsync(user.Id) == null) return;

            _userManager.AddToRoleAsync(user, role.ToString());
        }

        #endregion

        #region GetRolesUser

        public async Task<IEnumerable<string>> GetRolesForUser(UIMVCUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }
 
        #endregion

        #region AuthorizationHTML
        // Check if it's this type of user OR higher
        public async Task<bool> IsModerator(ClaimsPrincipal user)
        {
            if (await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "MODERATOR") ||
                await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "ADMIN") ||
                await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "SUPERADMIN"))
            {
                return true;
            }

            return false;
            
        }
        
        // XV
        public async Task<bool> IsAdmin(ClaimsPrincipal user)
        {
            if (await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "ADMIN") ||
                await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "SUPERADMIN"))
            {
                return true;
            }

            return false;
        }
        
        // XV
        public async Task<bool> IsSuperAdmin(ClaimsPrincipal user)
        {
            if (await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "SUPERADMIN"))
            {
                return true;
            }

            return false;
        }

        // XV
        public async Task<bool> IsSameRoleOrHigher(ClaimsPrincipal appUserClaim, UIMVCUser userCompare)
        {
            // Find the user using its claim
            UIMVCUser appUser = await _userManager.GetUserAsync(appUserClaim);
            // Get all of the user's roles
            IEnumerable<string> appUserRolesString = await _userManager.GetRolesAsync(appUser);
            // Create a new list
            List<Role> appUserRoles = new List<Role>();
            // Populate the list with the transformed roles (string -> Domain.Users.Role)
            //appUserRolesString.ToList().ForEach(s => appUserRoles.Append((Role) Enum.Parse(typeof(Role), s)));
            foreach (string roleString in appUserRolesString)
            {
                Role role = (Role) Enum.Parse(typeof(Role), roleString);
                appUserRoles.Add(role);
            }
            
            IEnumerable<string> userCompareRolesString = await _userManager.GetRolesAsync(userCompare);
            List<Role> userCompareRoles = new List<Role>();
            //userCompareRolesString.ToList().ForEach(s => userCompareRoles.Add((Role) Enum.Parse(typeof(Role), s)));
            foreach (string roleString in userCompareRolesString)
            {
                Role role = (Role) Enum.Parse(typeof(Role), roleString);
                userCompareRoles.Add(role);
            }
            
            
            // Check if both lists have roles
            if (appUserRoles.Any() && userCompareRoles.Any())
            {
                // return a boolean based on the highest role (based on the Enum Typecode)
                int appUserRoleHighest = (int) appUserRoles.Max(role => role);
                int userCompareRoleHighest = (int) userCompareRoles.Max(role => role);
                return appUserRoleHighest <= userCompareRoleHighest;
            }

            if (appUserRoles.Any() && userCompareRoles.Any() == false)
            {
                return false;
            }
            
            if (appUserRoles.Any() == false && userCompareRoles.Any())
            {
                return false;
            }
            
            if (appUserRoles.Any() == false && userCompareRoles.Any() == false)
            {
                return true;
            }
            
            throw new Exception("You shouldn't be able to get here");
        }

        #endregion

        #region Auhorization

        public async Task<bool> IsSameRoleOrLower(ClaimsPrincipal userClaim, Role roleCheck)
        {
            UIMVCUser user = await _userManager.GetUserAsync(userClaim);
            
            IEnumerable<string> userCompareRolesString = await _userManager.GetRolesAsync(user);
            List<Role> userCompareRoles = new List<Role>();
            
            foreach (string roleString in userCompareRolesString)
            {
                Role roleUser = (Role) Enum.Parse(typeof(Role), roleString);
                userCompareRoles.Add(roleUser);
            }

            if (userCompareRoles.Any())
            {
                int userCompareRoleHighest = (int) userCompareRoles.Max(role => role);
                int roleCheckNumber = (int) roleCheck;
                return userCompareRoleHighest < roleCheckNumber;
            }

            return false;
        }

        #endregion
    }
}