using System.Collections.Generic;
using BL;
using Domain.UserInput;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers
{
    public class ModerationController : Controller
    {
        private readonly IdeationQuestionManager _ideaMgr;
        private readonly UserManager _usrMgr;

        public ModerationController()
        {
            _ideaMgr = new IdeationQuestionManager();
            _usrMgr = new UserManager();
        }
        
        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpGet]
        [Authorize]
        public IActionResult CollectAllIdeas()
        {
            return View(_ideaMgr.GetIdeas());
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
        public IActionResult DeleteReport(int report, int idea)
        {
            _ideaMgr.DeleteReport(report);
            
            return RedirectToAction(controllerName: "Moderation", actionName: "CollectAllIdeas");
        }
    }
}