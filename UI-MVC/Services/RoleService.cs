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
        private readonly UserManager<UimvcUser> _userManager;
        private readonly IConfiguration _configuration;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<UimvcUser> userManager, IConfiguration configuration)
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
            if (await _userManager.FindByEmailAsync(_configuration["SuperAdmin:Email"]) == null)
            {
                UimvcUser user = new UimvcUser
                {
                    Name = _configuration["SuperAdmin:AccountName"],
                    Email = _configuration["SuperAdmin:Email"],
                    UserName = _configuration["SuperAdmin:Email"],
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(user, _configuration["SuperAdmin:Secret"]);

                var userFound = await _userManager.FindByEmailAsync(user.UserName);
                AssignToRole(userFound, Role.SuperAdmin);
            }
            if (await _userManager.FindByEmailAsync(_configuration["Admin:Email"]) == null)
            {
                UimvcUser user = new UimvcUser
                {
                    Name = _configuration["Admin:AccountName"],
                    Email = _configuration["Admin:Email"],
                    UserName = _configuration["Admin:Email"],
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(user, _configuration["Admin:Secret"]);

                var userFound = await _userManager.FindByEmailAsync(user.UserName);
                AssignToRole(userFound, Role.Admin);
            }
            if (await _userManager.FindByEmailAsync(_configuration["Moderator:Email"]) == null)
            {
                UimvcUser user = new UimvcUser
                {
                    Name = _configuration["Moderator:AccountName"],
                    Email = _configuration["Moderator:Email"],
                    UserName = _configuration["Moderator:Email"],
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(user, _configuration["Moderator:Secret"]);

                var userFound = await _userManager.FindByEmailAsync(user.UserName);
                AssignToRole(userFound, Role.Moderator);
            }
            if (await _userManager.FindByEmailAsync(_configuration["LoggedInOrg:Email"]) == null)
            {
                UimvcUser user = new UimvcUser
                {
                    Name = _configuration["LoggedInOrg:AccountName"],
                    Email = _configuration["LoggedInOrg:Email"],
                    UserName = _configuration["LoggedInOrg:Email"],
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(user, _configuration["LoggedInOrg:Secret"]);

                var userFound = await _userManager.FindByEmailAsync(user.UserName);
                AssignToRole(userFound, Role.LoggedInOrg);
            }
            if (await _userManager.FindByEmailAsync(_configuration["LoggedInVerified:Email"]) == null)
            {
                UimvcUser user = new UimvcUser
                {
                    Name = _configuration["LoggedInVerified:AccountName"],
                    Email = _configuration["LoggedInVerified:Email"],
                    UserName = _configuration["LoggedInVerified:Email"],
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(user, _configuration["LoggedInVerified:Secret"]);

                var userFound = await _userManager.FindByEmailAsync(user.UserName);
                AssignToRole(userFound, Role.LoggedInVerified);
            }
            if (await _userManager.FindByEmailAsync(_configuration["LoggedIn:Email"]) == null)
            {
                UimvcUser user = new UimvcUser
                {
                    Name = _configuration["LoggedIn:AccountName"],
                    Email = _configuration["LoggedIn:Email"],
                    UserName = _configuration["LoggedIn:Email"],
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(user, _configuration["LoggedIn:Secret"]);

                var userFound = await _userManager.FindByEmailAsync(user.UserName);
                AssignToRole(userFound, Role.LoggedIn);
            }
            if (await _userManager.FindByEmailAsync(_configuration["Anonymous:Email"]) == null)
            {
                UimvcUser user = new UimvcUser
                {
                    Name = _configuration["Anonymous:AccountName"],
                    Email = _configuration["Anonymous:Email"],
                    UserName = _configuration["Anonymous:Email"],
                    EmailConfirmed = true
                };

                _userManager.CreateAsync(user, _configuration["Anonymous:Secret"]);

                var userFound = await _userManager.FindByEmailAsync(user.UserName);
                AssignToRole(userFound, Role.Anonymous);
            }
        }

        #endregion

        #region CreateRoles
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

        public async void  AssignToRole(UimvcUser user, Role role)
        {
            if (!await _roleManager.RoleExistsAsync(role.ToString())) return;
            if (await _userManager.FindByIdAsync(user.Id) == null) return;

            _userManager.AddToRoleAsync(user, role.ToString());
        }

        #endregion

        #region GetRolesUser
        public async Task<IEnumerable<string>> GetRolesForUser(UimvcUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        #endregion

        #region Authorization
        public async Task<bool> IsVerified(ClaimsPrincipal user)
        {
            if (await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "LoggedInVerified") ||
                await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "Moderator") ||
                await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "Admin") ||
                await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "SuperAdmin"))
            {
                return true;
            }

            return false;

        }

        public async Task<bool> IsModerator(ClaimsPrincipal user)
        {
            if (await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "Moderator") ||
                await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "Admin") ||
                await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "SuperAdmin"))
            {
                return true;
            }

            return false;

        }

        public async Task<bool> IsAdmin(ClaimsPrincipal user)
        {
            if (await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "Admin") ||
                await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "SuperAdmin"))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsSuperAdmin(ClaimsPrincipal user)
        {
            if (await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(user), "SuperAdmin"))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsSameRoleOrHigher(ClaimsPrincipal appUserClaim, UimvcUser userCompare)
        {
            UimvcUser appUser = await _userManager.GetUserAsync(appUserClaim);
            IEnumerable<string> appUserRolesString = await _userManager.GetRolesAsync(appUser);
            List<Role> appUserRoles = new List<Role>();
            foreach (string roleString in appUserRolesString)
            {
                Role role = (Role) Enum.Parse(typeof(Role), roleString);
                appUserRoles.Add(role);
            }

            IEnumerable<string> userCompareRolesString = await _userManager.GetRolesAsync(userCompare);
            List<Role> userCompareRoles = new List<Role>();
            foreach (string roleString in userCompareRolesString)
            {
                Role role = (Role) Enum.Parse(typeof(Role), roleString);
                userCompareRoles.Add(role);
            }


            if (appUserRoles.Any() && userCompareRoles.Any())
            {
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
            UimvcUser user = await _userManager.GetUserAsync(userClaim);

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
        
        #region Users

        
        public async Task<IEnumerable<UimvcUser>> GetAllAdmins(int platformId)
        {
            List<UimvcUser> users = new List<UimvcUser>();
            foreach (UimvcUser user in _userManager.Users.Where(user => user.PlatformDetails == platformId))
            {
                if (await _userManager.IsInRoleAsync(user, "ADMIN"))
                {
                    users.Add(user);
                }
            }

            return users;
        }

        public async Task<IEnumerable<UimvcUser>> GetAllModerators(int platformId)
        {
            List<UimvcUser> users = new List<UimvcUser>();
            foreach (UimvcUser user in _userManager.Users.Where(user => user.PlatformDetails == platformId))
            {
                if (await _userManager.IsInRoleAsync(user, "MODERATOR"))
                {
                    users.Add(user);
                }
            }

            return users;
        }

        #endregion
    }
}

