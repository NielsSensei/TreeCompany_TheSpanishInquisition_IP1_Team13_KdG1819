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

namespace UIMVC.Controllers
{
    public class ModerationController : Controller
    {
        private readonly PlatformManager _platformMgr;
        private readonly IdeationQuestionManager _ideaMgr;
        private readonly ModuleManager _moduleMgr;
        private readonly ProjectManager _projMgr;
        private readonly UserManager<UIMVCUser> _userManager;

        public ModerationController(UserManager<UIMVCUser> userManager)
        {
            _ideaMgr = new IdeationQuestionManager();
            _platformMgr = new PlatformManager();
            _moduleMgr = new ModuleManager();
            _projMgr = new ProjectManager();
            _userManager = userManager;
        }

        #region AddPlatform
        
        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpGet]
        [Authorize]
        public IActionResult AddPlatform()
        {
            ViewData["platforms"] = _platformMgr.ReadAllPlatforms();
            return View();
        }

        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpPost]
        [Authorize]
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
        //TODO add rolecheck hero we need to be admin yeet *@
        //TODO sprint2 eens dat edwin klaar is met ze ding kunnen we ooit iets doen met events
        [Authorize]
        [HttpGet]
        public IActionResult AddIdeation(int project)
        {
            List<Phase> allPhases = (List<Phase>) _projMgr.GetAllPhases(project);
            List<Phase> availablePhases = new List<Phase>();

            foreach (Phase phase in allPhases)
            {
                if (_moduleMgr.GetModule(phase.Id, project) == null)
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
        
        //TODO add rolecheck hero we need to be admin yeet *@
        [Authorize]
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
        
        //TODO add rolecheck hero we need to be admin yeet *@
        [Authorize]
        [HttpGet]
        public IActionResult AddTag(int ideation)
        {
            ViewData["Ideation"] = ideation;

            return View();
        }
        
        //TODO add rolecheck hero we need to be admin yeet *@
        [Authorize]
        [HttpPost]
        public IActionResult AddTag(string tag, int ideation)
        {
            if (tag == null)
            {
                return BadRequest("Tag can't be null");
            }
            
            _moduleMgr.MakeTag(tag, ideation, false);
            
            return RedirectToAction("CollectIdeation", "Platform", new {Id = ideation});
        }
        
        //TODO add rolecheck hero we need to be admin yeet *@
        [Authorize]
        [HttpGet]
        public IActionResult AddCentralQuestion(int ideation)
        {
            ViewData["Ideation"] = ideation;
            return View();
        }

        //TODO add rolecheck hero we need to be admin yeet *@
        [Authorize]
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

        #region Ideas
        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpGet]
        [Authorize]
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

        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpGet]
        [Authorize]
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

        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpPost]
        [Authorize]
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

        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpPost]
        [Authorize]
        public IActionResult ApproveReport(int report)
        {
            Report foundReport = _ideaMgr.GetReport(report);

            foundReport.Status = ReportStatus.STATUS_APPROVED;

            _ideaMgr.EditReport(foundReport);

            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas", routeValues: "report");
        }

        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpPost]
        [Authorize]
        public IActionResult DenyReport(int report, int idea)
        {
            Report foundReport = _ideaMgr.GetReport(report);
            foundReport.Status = ReportStatus.STATUS_DENIED;
            _ideaMgr.EditReport(foundReport);

            HandleRemainingReports(idea);

            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas", routeValues: "report");
        }

        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpPost]
        [Authorize]
        public IActionResult DestroyReport(int report, int idea)
        {
            _ideaMgr.RemoveReport(report);

            HandleRemainingReports(idea);

            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas" , routeValues: "report");
        }

        [HttpPost]
        [Authorize]
        public IActionResult DestroyIdea(int idea)
        {
            _ideaMgr.RemoveIdea(idea);
            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas");
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> ToggleBanUser(string userId)
        {
            UIMVCUser userFound = await _userManager.FindByIdAsync(userId);
            
            if (userFound == null) return RedirectToAction("CollectAllUsers");
            
            userFound.Banned = !userFound.Banned;
            var result = await _userManager.UpdateAsync(userFound);
                
            return RedirectToAction("CollectAllUsers");
            // This part is still borked.
        }
        /*
        [HttpPost]
        [Authorize]
        public IActionResult VerifyUser(string userId)
        {
            _userManager.VerifyUser(userId);
            // Borked as well.
            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllUsers");
        }*/
        #endregion
    }
}
