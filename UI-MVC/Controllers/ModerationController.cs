using System.Collections.Generic;
using System.Linq;
using BL;
using Domain.Projects;
using Domain.UserInput;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
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

        public ModerationController()
        {
            _ideaMgr = new IdeationQuestionManager();
            _platformMgr = new PlatformManager();
            _moduleMgr = new ModuleManager();
            _projMgr = new ProjectManager();
        }
        
        #region Platform
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
                Owners = new List<User>(),
                Users = new List<User>()
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
        public IActionResult AddIdeation(CreateIdeationModel cim, int project)
        {
            if (cim == null)
            {
                return BadRequest("Ideation can't be null");
            }
                                               
            return RedirectToAction("CollectProject", "Platform", new {Id = project});
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
        
        
        
        

        

        

        
    }
}