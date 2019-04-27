using System.Collections.Generic;
using System.Linq;
using BL;
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
        private readonly UserManager _usrMgr;

        public ModerationController()
        {
            _ideaMgr = new IdeationQuestionManager();
            _platformMgr = new PlatformManager();
            _usrMgr = new UserManager();

        }
        
        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpGet]
        [Authorize]
        public IActionResult CollectAllIdeas()
        {
            return View(_ideaMgr.GetIdeas());
        }

        public JsonResult CollectIdeas(string filter)
        {
            List<Idea> ideas = new List<Idea>();
            
            switch (filter)
            {
                case "all": ideas = _ideaMgr.GetIdeas(); break; 
                case "admin": ideas = _ideaMgr.GetIdeas().FindAll(i => i.ReviewByAdmin); break;
                case "report": ideas = _ideaMgr.GetIdeas().FindAll(i => !i.ReviewByAdmin && i.Reported); break;
            }

            return Json(ideas);
        } 

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

        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpGet]
        [Authorize]
        public IActionResult CollectIdea(int id)
        {
            Idea idea = _ideaMgr.GetIdea(id);
            if (idea.Visible)
            {
                idea.User  = _usrMgr.GetUser(idea.User.Id, false);
                IEnumerable<Report> reportWithoutFlagger = _ideaMgr.GetAllReportsByIdea(id);
                foreach (Report r in reportWithoutFlagger)
                {
                    r.Flagger = _usrMgr.GetUser(r.Flagger.Id, false);
                }
                ViewData["Reports"] = reportWithoutFlagger;
            
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
            
            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas");
        }

        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpPost]
        [Authorize]
        public IActionResult ApproveReport(int report, int idea)
        {
            Report foundReport = _ideaMgr.GetReport(report);

            foundReport.Status = ReportStatus.STATUS_APPROVED;
            
            _ideaMgr.EditReport(foundReport);
            
            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas");
        }

        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpPost]
        [Authorize]
        public IActionResult DenyReport(int report, int idea)
        {
            Report foundReport = _ideaMgr.GetReport(report);

            foundReport.Status = ReportStatus.STATUS_DENIED;
            
            _ideaMgr.EditReport(foundReport);
            
            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas");
        }
        
        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpPost]
        [Authorize]
        public IActionResult DestroyReport(int report, int idea)
        {
            _ideaMgr.RemoveReport(report);
            
            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas");
        }

        [HttpPost]
        [Authorize]
        public IActionResult DestroyIdea(int idea)
        {
            _ideaMgr.RemoveIdea(idea);
            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas");
        }
    }
}