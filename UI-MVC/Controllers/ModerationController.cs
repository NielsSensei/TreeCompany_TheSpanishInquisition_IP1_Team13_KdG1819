using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Domain.Projects;
using Domain.Identity;
using Domain.UserInput;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
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
        private readonly UserManager<UIMVCUser> _userManager;
        private readonly RoleService _roleService;

        public ModerationController(UserManager<UIMVCUser> userManager, RoleService roleService)
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
        [Authorize(Roles = "SUPERADMIN")]
        public IActionResult AddPlatform()
        {
            ViewData["platforms"] = _platformMgr.ReadAllPlatforms();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SUPERADMIN")]
        public IActionResult AddPlatform(CreatePlatformModel cpm)
        {
            if (cpm == null)
            {
                return BadRequest("Platform cannot be null");
            }
            Platform platform = new Platform()
            {
                Name = cpm.Name,
                Url = cpm.Url,
                Owners = new List<UIMVCUser>(),
                Users = new List<UIMVCUser>()
            };

            var newPlatform = _platformMgr.MakePlatform(platform);

            return RedirectToAction("Index", "Platform", new {Id = newPlatform.Id} );
        }
        #endregion

        #region Ideation
        //TODO sprint2 eens dat edwin klaar is met ze ding kunnen we ooit iets doen met events
        [Authorize(Roles = "SUPERADMIN, MODERATOR, ADMIN")]
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

        [Authorize(Roles = "ADMIN, SUPERADMIN")]
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
                User = new UIMVCUser(){Id = user},
                type = ModuleType.Ideation,
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
        
        [Authorize(Roles = "ADMIN, SUPERADMIN")]
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

        [Authorize(Roles = "ADMIN, SUPERADMIN")]
        [HttpGet]
        public IActionResult AddCentralQuestion(int ideation)
        {
            ViewData["Ideation"] = ideation;
            return View();
        }

        [Authorize(Roles = "ADMIN, SUPERADMIN")]
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
               SiteURL = ciqm.SiteURL,
               QuestionTitle = ciqm.QuestionTitle,
               Ideation = new Ideation(){ Id = ideation }
            };

            _ideaMgr.MakeQuestion(iq, ideation);

            return RedirectToAction("CollectIdeation", "Platform", new {Id = ideation});
        }


        [Authorize(Roles = "ADMIN, SUPERADMIN")]
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


        [Authorize(Roles = "ADMIN, SUPERADMIN")]
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


        [Authorize(Roles = "ADMIN, SUPERADMIN")]
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

            _moduleMgr.RemoveModule(id, i.Project.Id, false);

            return RedirectToAction("CollectProject", "Platform", new { Id = i.Project.Id });
        }
        #region Ideas
        [HttpGet]
        [Authorize(Roles = "MODERATOR, ADMIN, SUPERADMIN")]
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
        [Authorize(Roles = "MODERATOR, ADMIN, SUPERADMIN")]
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
        [Authorize(Roles = "MODERATOR, ADMIN, SUPERADMIN")]
        public IActionResult ReviewByAdmin(int idea, int  report)
        {
            Idea foundIdea = _ideaMgr.GetIdea(idea);
            Report foundReport = _ideaMgr.GetReport(report);

            foundIdea.ReviewByAdmin = true;
            foundReport.Status = ReportStatus.STATUS_NEEDADMIN;

            _ideaMgr.EditIdea(foundIdea);
            _ideaMgr.EditReport(foundReport);

            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas", routeValues: "admin");
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, SUPERADMIN")]
        public IActionResult ApproveReport(int report)
        {
            Report foundReport = _ideaMgr.GetReport(report);

            foundReport.Status = ReportStatus.STATUS_APPROVED;

            _ideaMgr.EditReport(foundReport);

            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas", routeValues: "report");
        }

        [HttpPost]
        [Authorize(Roles = "MODERATOR, ADMIN, SUPERADMIN")]
        public IActionResult DenyReport(int report, int idea)
        {
            Report foundReport = _ideaMgr.GetReport(report);
            foundReport.Status = ReportStatus.STATUS_DENIED;
            _ideaMgr.EditReport(foundReport);

            HandleRemainingReports(idea);

            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas", routeValues: "report");
        }

        [HttpPost]
        [Authorize(Roles = "MODERATOR, ADMIN, SUPERADMIN")]
        public IActionResult DestroyReport(int report, int idea)
        {
            _ideaMgr.RemoveReport(report);

            HandleRemainingReports(idea);

            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas" , routeValues: "report");
        }

        [HttpPost]
        [Authorize(Roles = "MODERATOR, ADMIN, SUPERADMIN")]
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
        [Authorize(Roles = "MODERATOR, ADMIN, SUPERADMIN")]
        public IActionResult CollectAllUsers(string sortOrder, string searchString)
        {

            ViewData["CurrentFilter"] = searchString;
            var users = (IEnumerable<UIMVCUser>)_userManager.Users;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Name.ToUpper().Contains(searchString.ToUpper()));
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
        [Authorize(Roles = "MODERATOR, ADMIN, SUPERADMIN")]
        public async Task<IActionResult> ToggleBanUser(string userId)
        {
            UIMVCUser userFound = await _userManager.FindByIdAsync(userId);

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

        [Authorize(Roles = "ADMIN, SUPERADMIN")]
        public async Task<IActionResult> SetRole(AssignRoleModel arm, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roletext = Request.Form["Role"];
//            if (!roletext.Any()) return RedirectToAction("CollectAllUsers", "Moderation");
//            var role = (Role) Enum.Parse(typeof(Role), roletext);
            Object roleParse = null;
            if (!Enum.TryParse(typeof(Role), roletext, out roleParse)) return RedirectToAction("CollectAllUsers", "Moderation");
            var role = (Role) roleParse;

            // TODO Send a message to the user stating that the role could not be added
            if (!await _roleService.IsSameRoleOrLower(User, role))
            {
                if (await _userManager.IsInRoleAsync(user, Enum.GetName(typeof(Role), role)))
                {
                    _userManager.RemoveFromRoleAsync(user, roletext);
                }
                else
                {
                    _roleService.AssignToRole(user, role);
                }
            }
            return RedirectToAction("CollectAllUsers", "Moderation");
        }
        #endregion
    }
}
