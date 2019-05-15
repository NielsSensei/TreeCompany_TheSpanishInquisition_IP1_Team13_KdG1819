using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BL;
using Domain.Projects;
using Domain.Identity;
using Domain.UserInput;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UIMVC.Models;
using UIMVC.Services;

namespace UIMVC.Controllers
{
    public class ModerationController : Controller
    {
        private readonly PlatformManager _platformMgr;
        private readonly IdeationQuestionManager _ideaMgr;
        private readonly ModuleManager _moduleMgr;
        private readonly ProjectManager _projMgr;
        private readonly UserManager<UimvcUser> _userManager;
        private readonly RoleService _roleService;

        public ModerationController(UserManager<UimvcUser> userManager, RoleService roleService)
        {
            _ideaMgr = new IdeationQuestionManager();
            _platformMgr = new PlatformManager();
            _moduleMgr = new ModuleManager();
            _projMgr = new ProjectManager();
            _userManager = userManager;
            _roleService = roleService;
        }

        #region AddPlatform
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddPlatform()
        {
            ViewData["platforms"] = _platformMgr.ReadAllPlatforms();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddPlatform(CreatePlatformModel cpm)
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

        #region Ideation
        //TODO sprint2 eens dat edwin klaar is met ze ding kunnen we ooit iets doen met events
        [Authorize(Roles = "SuperAdmin, Moderator, Admin")]
        [HttpGet]
        public IActionResult AddIdeation(int project)
        {
            List<Phase> allPhases = (List<Phase>) _projMgr.GetAllPhases(project);
            List<Phase> availablePhases = new List<Phase>();

            foreach (Phase phase in allPhases)
            {
                if (_moduleMgr.GetIdeation(phase.Id, project) == null)
                {
                    availablePhases.Add(phase);
                }
            }

            if (availablePhases.Count == 0)
            {
                return BadRequest("No available phases");
            }

            ViewData["Phases"] = availablePhases;
            ViewData["Project"] = project;

            return View();
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        public IActionResult AddIdeation(CreateIdeationModel cim, int project, string user)
        {
            if (cim == null)
            {
                return BadRequest("Ideation can't be null");
            }

            Ideation i = new Ideation()
            {
                Project = new Project() {Id = project},
                ParentPhase = new Phase() {Id = Int32.Parse(Request.Form["Parent"].ToString())},
                User = new UimvcUser(){Id = user},
                ModuleType = ModuleType.Ideation,
                Title = cim.Title,
                OnGoing = true
            };

            if (cim.ExtraInfo != null)
            {
                i.ExtraInfo = cim.ExtraInfo;
            }

            _moduleMgr.MakeIdeation(i);

            return RedirectToAction("CollectProject", "Platform", new {Id = project});
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult AddTag(int ideation)
        {
            string tag = Request.Form["GetMeATag"].ToString();

            if (tag == null)
            {
                return BadRequest("Tag can't be null");
            }

            _moduleMgr.MakeTag(tag, ideation, false);

            return RedirectToAction("CollectIdeation", "Platform",
                new {Id = ideation});
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public IActionResult AddCentralQuestion(int ideation)
        {
            ViewData["Ideation"] = ideation;
            return View();
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        public IActionResult AddCentralQuestion(CreateIdeationQuestionModel ciqm, int ideation)
        {
            if (ciqm == null)
            {
                return BadRequest("IdeationQuestion can't be null");
            }

            IdeationQuestion iq = new IdeationQuestion()
            {
               Description = ciqm.Description,
               SiteUrl = ciqm.SiteUrl,
               QuestionTitle = ciqm.QuestionTitle,
               Ideation = new Ideation(){ Id = ideation }
            };

            _ideaMgr.MakeQuestion(iq, ideation);

            return RedirectToAction("CollectIdeation", "Platform", new {Id = ideation});
        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public IActionResult ChangeIdeation(int id)
        {
            Ideation i = _moduleMgr.GetIdeation(id);

            ViewData["Project"] = i.Project.Id;

            List<Phase> allPhases = (List<Phase>) _projMgr.GetAllPhases(i.Project.Id);
            List<Phase> availablePhases = new List<Phase>();

            foreach (Phase phase in allPhases)
            {
                if (_moduleMgr.GetIdeation(phase.Id, i.Project.Id) == null)
                {
                    availablePhases.Add(phase);
                }
            }

            ViewData["Phases"] = availablePhases;
            ViewData["PhaseCount"] = availablePhases.Count;

            ViewData["Ideation"] = id;
            AlterIdeationModel aim = new AlterIdeationModel()
            {
                Title = i.Title,
                ExtraInfo = i.ExtraInfo,
                ParentPhase = _projMgr.GetPhase(i.ParentPhase.Id)
            };

            return View(aim);
        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        public IActionResult ConfirmChangeIdeation(int ideation)
        {
            Ideation i = new Ideation()
            {
                Id = ideation,
                Title = Request.Form["Title"].ToString(),
                ExtraInfo = Request.Form["ExtraInfo"].ToString()
            };

            if (!Request.Form["ParentPhase"].ToString().Equals(null))
            {
                try
                {
                    i.ParentPhase = _projMgr.GetPhase(Int32.Parse(Request.Form["ParentPhase"].ToString()));
                    _moduleMgr.EditIdeation(i);
                }
                catch (FormatException e)
                {
                    _moduleMgr.EditIdeation(i);
                }

            }

            return RedirectToAction("CollectIdeation", "Platform", new {Id = ideation});
        }


        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult DestroyIdeation(int id)
        {
            Ideation i = _moduleMgr.GetIdeation(id);

            List<IdeationQuestion> iqs = _ideaMgr.GetAllByModuleId(i.Id);
            foreach (IdeationQuestion iq in iqs)
            {
                List<Idea> ideas = _ideaMgr.GetIdeas(iq.Id);
                foreach (Idea idea in ideas)
                {
                    _ideaMgr.RemoveFields(idea.Id);
                    _ideaMgr.RemoveReports(idea.Id);
                    _ideaMgr.RemoveVotes(idea.Id);
                    _ideaMgr.RemoveIdea(idea.Id);
                }

                _ideaMgr.RemoveQuestion(iq.Id);
            }

            _moduleMgr.RemoveModule(id, false);

            return RedirectToAction("CollectProject", "Platform", new { Id = i.Project.Id });
        }
        #region Ideas
        [HttpGet]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public IActionResult CollectAllIdeas(string filter = "all")
        {
            List<Idea> ideas = new List<Idea>();

            switch (filter)
            {
                case "all": ideas = _ideaMgr.GetIdeas(); break;
                case "admin": ideas = _ideaMgr.GetIdeas().FindAll(i => i.ReviewByAdmin); break;
                case "report": ideas = _ideaMgr.GetIdeas().FindAll(i => !i.ReviewByAdmin && i.Reported); break;
            }

            return View(ideas);
        }

        [HttpGet]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public IActionResult CollectIdea(int id)
        {
            Idea idea = _ideaMgr.GetIdea(id);
            idea.User = _userManager.Users.FirstOrDefault(user => user.Id == idea.User.Id);
            if (idea.Visible)
            {
                ViewData["Reports"] = _ideaMgr.GetAllReportsByIdea(id);

                return View(idea);
            }

            return RedirectToAction(controllerName: "Errors", actionName: "HandleErrorCode", routeValues: id);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public IActionResult ReviewByAdmin(int idea, int  report)
        {
            Idea foundIdea = _ideaMgr.GetIdea(idea);
            Report foundReport = _ideaMgr.GetReport(report);

            foundIdea.ReviewByAdmin = true;
            foundReport.Status = ReportStatus.StatusNeedAdmin;

            _ideaMgr.EditIdea(foundIdea);
            _ideaMgr.EditReport(foundReport);

            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas", routeValues: "admin");
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult ApproveReport(int report)
        {
            Report foundReport = _ideaMgr.GetReport(report);

            foundReport.Status = ReportStatus.StatusApproved;

            _ideaMgr.EditReport(foundReport);

            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas", routeValues: "report");
        }

        [HttpPost]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public IActionResult DenyReport(int report, int idea)
        {
            Report foundReport = _ideaMgr.GetReport(report);
            foundReport.Status = ReportStatus.StatusDenied;
            _ideaMgr.EditReport(foundReport);

            HandleRemainingReports(idea);

            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas", routeValues: "report");
        }

        [HttpPost]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public IActionResult DestroyReport(int report, int idea)
        {
            _ideaMgr.RemoveReport(report);

            HandleRemainingReports(idea);

            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas" , routeValues: "report");
        }

        [HttpPost]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public IActionResult DestroyIdea(int idea, string from, int thread)
        {
            Idea toDelete = _ideaMgr.GetIdea(idea);
            toDelete.IsDeleted = true;

            _ideaMgr.EditIdea(toDelete);

            if (from.Equals("ModerationPanel"))
            {
                return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas");
            }


            if (from.Equals("IdeationThread") && thread > 0)
            {
                return RedirectToAction("CollectIdeationThread", "Platform",
                    new {Id = thread});
            }

            return RedirectToAction("HandleErrorCode", "Errors", 404);
        }


        private void HandleRemainingReports(int idea)
        {
            IEnumerable<Report> remainingReports = _ideaMgr.GetAllReportsByIdea(idea);
            if (!remainingReports.Any())
            {
                Idea foundIdea = _ideaMgr.GetIdea(idea);
                foundIdea.ReviewByAdmin = false;
                foundIdea.Reported = false;

                _ideaMgr.EditIdea(foundIdea);
            }
        }
        #endregion
        #endregion

        #region UIMVCUser
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
//                case "role":
//                    users = users.OrderBy(u => u.Role); break;
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
            // This part is still borked.
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> SetRole(AssignRoleModel arm, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            string roletext = Request.Form["Role"];
//            if (!roletext.Any()) return RedirectToAction("CollectAllUsers", "Moderation");
//            var role = (Role) Enum.Parse(typeof(Role), roletext);
            Object roleParse = null;
            if (!Enum.TryParse(typeof(Role), roletext, out roleParse)) return RedirectToAction("CollectAllUsers", "Moderation");
            var role = (Role) roleParse;

            // TODO Send a message to the user stating that the role could not be added
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
            return RedirectToAction("CollectAllUsers", "Moderation");
        }

        [HttpPost]
        public async Task<IActionResult> RequestVerification(int platformId)
        {
            
            
        }
        #endregion
    }
}
