using System.Collections.Generic;
using System.Linq;
using BL;
using Domain.Identity;
using Domain.Projects;
using Domain.UserInput;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers
{
    
    public class PlatformController : Controller
    {
        private readonly PlatformManager _platformMgr;
        private readonly ProjectManager _projectMgr;
        private readonly IdeationQuestionManager _iqMgr;

        public PlatformController()
        {
            _platformMgr = new PlatformManager();
            _projectMgr = new ProjectManager();
            _iqMgr = new IdeationQuestionManager();
        }

        [Route("Platform/{id}")]
        public IActionResult Index(int id)
        {
            Platform platform = _platformMgr.GetPlatform(id);
            if (platform == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);
            }
            return View(platform);
        }

        #region Platform

        public IActionResult Search(string search)
        {
            ViewData["search"] = search;
            var platforms = _platformMgr.SearchPlatforms(search);
            return View(platforms);
        }

        #endregion

        #region Change
        
        [HttpGet]
        [Authorize(Roles = "ADMIN, SUPERADMIN")]
        public IActionResult ChangePlatform(int id)
        {
            Domain.Users.Platform platform = _platformMgr.GetPlatform(id);
            if (platform == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);
            }
            return View(platform);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, SUPERADMIN")]
        public IActionResult ChangePlatform(Platform platform)
        {
            _platformMgr.EditPlatform(platform);
            // TODO: make the redirect work
            return RedirectToAction("Index", new {id = platform.Id});
        }
        #endregion
        
        #region Project
        [HttpGet]
        public IActionResult CollectProject(int id)
        {
            Project project = _projectMgr.GetProject(id, false);

            if (project.Visible && project.Id != 0)
            {
                List<Phase> phases = (List<Phase>) _projectMgr.GetAllPhases(id);

                foreach (Phase phase in phases)
                {
                    if (phase.Id == project.CurrentPhase.Id)
                    {
                        project.CurrentPhase = phase;                       
                    }                
                }

                phases.Remove(project.CurrentPhase);
                ViewData["Phases"] = phases;
            
                return View(project); 
            }
            
            return RedirectToAction("HandleErrorCode", "Errors", 404);
        }
        #endregion
        
        #region Ideation
        public IActionResult CollectIdeation(int id)
        {
            Ideation ideation = _projectMgr.ModuleMan.GetIdeation(id);
            
            return View(ideation);            
        }

        public IActionResult CollectIdeationThread(int id, string message)
        {
            IdeationQuestion iq = _iqMgr.GetQuestion(id, false);

            ViewData["Message"] = message;
            ViewData["IdeationQuestion"] = iq;
            
            return View(iq);
        }
        
        #region Idea
        [Authorize]
        public IActionResult AddVote(int idea, string user, int thread)
        {
            if (_iqMgr.MakeVote(idea, user))
            {
                return RedirectToAction("CollectIdeationThread", "Platform", routeValues: new
                    { id = thread, message = "Stem gelukt, dankjewel!" }); 
            }
            
            return RedirectToAction("CollectIdeationThread", "Platform", routeValues: new
                { id = thread, message = "Al gestemd op dit idee!" });
        }

        [Authorize]
        public IActionResult AddReport(int idea, string flagger, int thread)
        {
            Idea ToReport = _iqMgr.GetIdea(idea);

            Report report = new Report()
            {
                Idea = ToReport,
                Flagger = new UimvcUser() {Id = flagger},
                Reportee = new UimvcUser() {Id = ToReport.User.Id},
                Status = ReportStatus.StatusNotViewed
            };
            
            if (!Request.Form["Reason"].ToString().Equals(""))
            {
                Report alreadyReport = _iqMgr.GetAllReportsByIdea(idea).FirstOrDefault(
                    r => r.Flagger.Id.Equals(flagger));

                if (alreadyReport == null)
                {
                    report.Reason = Request.Form["Reason"].ToString();
                    _iqMgr.MakeReport(report);

                    ToReport.Reported = true;
                    _iqMgr.EditIdea(ToReport);

                    return RedirectToAction("CollectIdeationThread", "Platform", routeValues: new
                        {id = thread, message = "Idee geraporteerd!"});
                }

                return RedirectToAction("CollectIdeationThread", "Platform", routeValues: new
                    {id = thread, message = "Je oude rapport is nog in de behandeling"});
            }
            
            return RedirectToAction("CollectIdeationThread", "Platform", routeValues: new
                               { id = thread, message = "Je hebt geen reden opgegeven voor je rapport!" });
        }
        #endregion
        #endregion
    }
}