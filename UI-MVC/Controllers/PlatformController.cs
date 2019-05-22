using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Domain.Identity;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UIMVC.Models;
using UIMVC.Services;

namespace UIMVC.Controllers
{
    public class PlatformController : Controller
    {
        private readonly PlatformManager _platformMgr;
        private readonly UserManager<UimvcUser> _userManager;
        private readonly RoleService _roleService;

        public PlatformController(UserManager<UimvcUser> userManager, RoleService service)
        {
            _platformMgr = new PlatformManager();
            _userManager = userManager;
            _roleService = service;
        }

        /**
         * @Author Xander Veldeman, Niels Van Zandbergen
         *
         * Shows the platform
         * @Param message: simple status messages
         */
        [Route("Platform/{id}")]
        public IActionResult Index(int id, string message)
        {
            Platform platform = _platformMgr.GetPlatform(id, true);
            if (platform == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors",
                    new { statuscode = 404, path="/Platform/" + id  });
            }

            if (message != null)
            {
                ViewData["StatusMessage"] = message;
            }

            return View(platform);
        }

        /**
         * @Author Xander Veldeman
         */
        #region Platform

        /**
         * @Author Xander Veldeman
         *
         * Search for a platform based on it's name and URL
         */
        public IActionResult Search(string search)
        {
            ViewData["search"] = search;
            var platforms = _platformMgr.SearchPlatforms(search);
            return View(platforms);
        }

        #endregion

        /**
         * @Author Xander Veldeman
         */
        #region Change

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> ChangePlatform(int id)
        {
            if (User.IsInRole(Role.Admin.ToString("G")) &&
                (await _userManager.GetUserAsync(User)).PlatformDetails != id)
                return BadRequest("You are no admin of this platform");

            Platform platform = _platformMgr.GetPlatform(id, false);
            if (platform == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors",
                    new { statuscode = 404, path="/Platform/ChangePlatform/" + id });
            }

            ViewData["platform"] = platform;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> ChangePlatform(AddPlatformModel platformEdit, int platformId)
        {
            Platform platform = new Platform()
            {
                Id = platformId,
                Name = platformEdit.Name,
                Url = platformEdit.Url
            };

            if (platformEdit.IconImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await platformEdit.IconImage.CopyToAsync(memoryStream);
                    platform.IconImage = memoryStream.ToArray();
                }
            }

            if (platformEdit.CarouselImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await platformEdit.CarouselImage.CopyToAsync(memoryStream);
                    platform.CarouselImage = memoryStream.ToArray();
                }
            }

            if (platformEdit.FrontPageImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await platformEdit.FrontPageImage.CopyToAsync(memoryStream);
                    platform.FrontPageImage = memoryStream.ToArray();
                }
            }

            _platformMgr.EditPlatform(platform);
            return RedirectToAction("ChangePlatform", new {id = platform.Id});
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> AssignAdmin(string usermail, int platformId)
        {
            UimvcUser user = await _userManager.FindByEmailAsync(usermail);
            Platform platform = _platformMgr.GetPlatform(platformId, true);
            if (user == null) return BadRequest("Can't find user");
            if (platform == null) return BadRequest("Can't find platform");

            user.PlatformDetails = platformId;
            await _userManager.UpdateAsync(user);

            return Ok(user);
        }

        #endregion

        /**
         * @Author Xander Veldeman, Niels Van Zandbergen
         */
        #region AddPlatform
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddPlatform()
        {
            ViewData["platforms"] = _platformMgr.ReadAllPlatforms();
            return View();
        }

        /**
         * @Documentation Xander Veldeman
         *
         * Images get taken from the AddPlatformModel as IFormFile and are converted to a byte array for storage
         */
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddPlatform(AddPlatformModel cpm)
        {
            if (cpm == null)
            {
                return BadRequest("Platform cannot be null");
            }

            Platform platform = new Platform()
            {
                Name = cpm.Name,
                Url = cpm.Url,
                Owners = new List<UimvcUser>(),
                Users = new List<UimvcUser>()
            };

            using (var memoryStream = new MemoryStream())
            {
                await cpm.IconImage.CopyToAsync(memoryStream);
                platform.IconImage = memoryStream.ToArray();
            }

            using (var memoryStream = new MemoryStream())
            {
                await cpm.CarouselImage.CopyToAsync(memoryStream);
                platform.CarouselImage = memoryStream.ToArray();
            }

            using (var memoryStream = new MemoryStream())
            {
                await cpm.FrontPageImage.CopyToAsync(memoryStream);
                platform.FrontPageImage = memoryStream.ToArray();
            }

            var newPlatform = _platformMgr.MakePlatform(platform);

            return RedirectToAction("Index", "Platform", new {Id = newPlatform.Id} );
        }

        /**
         * @Author Xander Veldeman
         * @Documentation Xander Veldeman
         *
         * A superadmin can assign any user to a platform.
         * An admin can only assign users of his own platform.
         */
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> AssignUserToPlatform(AssignUserModel aum)
        {
            if (aum == null) return BadRequest("Cannot be null");
            if (User.IsInRole(Role.Admin.ToString("G")) &&
                (await _userManager.GetUserAsync(User)).PlatformDetails != aum.PlatformId)
                return BadRequest("You are no admin of this platform");

            UimvcUser user = await _userManager.FindByEmailAsync(aum.UserMail);
            if (user == null) return BadRequest("Wrong user mail");
            user.PlatformDetails = aum.PlatformId;

            if (aum.Role == 0) aum.Role = AssignUserRole.MODERATOR;
            _userManager.AddToRoleAsync(user, Enum.GetName(typeof(AssignUserRole), aum.Role));

            await _userManager.UpdateAsync(user);

            return RedirectToAction("ChangePlatform", "Platform", new {Id = aum.PlatformId} );
        }

        /**
         * @Author Xander Veldeman
         */
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> RemoveUserFromPlatform(AssignUserModel aum)
        {
            if (aum == null)
            {
                return BadRequest("Cannot be null");
            }


            UimvcUser user = await _userManager.FindByEmailAsync(aum.UserMail);
            if (user == null) return BadRequest("Wrong user mail");
            user.PlatformDetails = 0;
            if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Moderator"))
            {
                _userManager.RemoveFromRolesAsync(user, new[] {"Moderator", "Admin"});
            }

            await _userManager.UpdateAsync(user);

            return RedirectToAction("ChangePlatform", "Platform", new {Id = aum.PlatformId} );
        }

        #endregion

        /**
         * @Author Xander Veldeman, Edwin Kai Yin Tam
         */
        #region UIMVCUser
        
        /**
         * @Documentation Xander Veldeman
         *
         * If the user is an admin or moderator, this will return the users of their platform.
         * If the user is a superadmin, it will display ALL users.
         */
        [HttpGet]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public async Task<IActionResult> CollectAllUsers(string sortOrder, string searchString)
        {

            ViewData["CurrentFilter"] = searchString;
            var users = (IEnumerable<UimvcUser>)_userManager.Users;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            if (!User.IsInRole(Role.SuperAdmin.ToString("G")))
            {
                UimvcUser user = await _userManager.GetUserAsync(User);
                users = users.Where(u => u.PlatformDetails == user.PlatformDetails);
            }

            switch (sortOrder)
            {
                case "platform":
                    users = users.OrderBy(u => u.PlatformDetails); break;
                case "name":
                    users = users.OrderBy(u => u.Name); break;
                case "birthday":
                    users = users.OrderBy(u => u.DateOfBirth); break;
                default:
                    users = users.OrderBy(u => u.Id); break;
            }
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public async Task<IActionResult> ToggleBanUser(string userId)
        {
            UimvcUser userFound = await _userManager.FindByIdAsync(userId);

            if (userFound == null) return RedirectToAction("CollectAllUsers");
            if (await _roleService.IsSameRoleOrHigher(HttpContext.User, userFound)) return RedirectToAction("CollectAllUsers");

            userFound.Banned = !userFound.Banned;
            _userManager.SetLockoutEnabledAsync(userFound, userFound.Banned);
            if (userFound.Banned)
            {
                _userManager.SetLockoutEndDateAsync(userFound, DateTime.MaxValue);
            }
            var result = await _userManager.UpdateAsync(userFound);

            return RedirectToAction("CollectAllUsers");
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> SetRole(AssignRoleModel arm, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            string roletext = Request.Form["Role"];
            Object roleParse = null;
            if (!Enum.TryParse(typeof(Role), roletext, out roleParse)) return RedirectToAction("CollectAllUsers",
                "Platform");
            var role = (Role) roleParse;

            if (!await _roleService.IsSameRoleOrLower(User, role))
            {
                if (await _userManager.IsInRoleAsync(user, roletext))
                {
                    await _userManager.RemoveFromRoleAsync(user, roletext);
                }
                else
                {
                    _roleService.AssignToRole(user, role);
                }
            }
            return RedirectToAction("CollectAllUsers", "Platform");
        }
        #endregion
    }
}
