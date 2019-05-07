using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Identity;
using Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace UIMVC.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UIMVCUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<UIMVCUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            
            CreateRoles();
        }

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
        public async Task<bool> IsModerator(UIMVCUser user)
        {
            if (await _userManager.IsInRoleAsync(user, "MODERATOR") ||
                await _userManager.IsInRoleAsync(user, "ADMIN") ||
                await _userManager.IsInRoleAsync(user, "SUPERADMIN"))
            {
                return true;
            }

            return false;
            
        }
        
        public async Task<bool> IsAdmin(UIMVCUser user)
        {
            if (await _userManager.IsInRoleAsync(user, "ADMIN") ||
                await _userManager.IsInRoleAsync(user, "SUPERADMIN"))
            {
                return true;
            }

            return false;
        }
        
        public async Task<bool> IsSuperAdmin(UIMVCUser user)
        {
            if (await _userManager.IsInRoleAsync(user, "SUPERADMIN"))
            {
                return true;
            }

            return false;
        }

        #endregion
        
    }
}