using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly UserManager<UIMVCUser> _userManager;

        public ModerationController(UserManager<UIMVCUser> userManager)
        {
            _ideaMgr = new IdeationQuestionManager();
            _platformMgr = new PlatformManager();
            _userManager = userManager;
        }

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

            foreach (Idea idea in ideas)
            {
                idea.User = _userManager.Users.FirstOrDefault(user => user.Id == idea.User.Id);
            }

            return View(ideas);
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
                Owners = new List<UIMVCUser>(),
                Users = new List<UIMVCUser>()
            };

            var newPlatform = _platformMgr.MakePlatform(platform);

            return RedirectToAction("Index", "Platform", new {Id = newPlatform.Id} );
        }
        #endregion

        #region Ideation
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










    }
}
