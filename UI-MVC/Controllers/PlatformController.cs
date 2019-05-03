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
        [Authorize]
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
        [Authorize]
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

            if (project.Visible && project != null)
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
            Ideation ideation = (Ideation) _projectMgr.ModuleMan.GetModule(id, false, false);
            
            return View(ideation);            
        }

        public IActionResult CollectIdeationThread(int id)
        {
            IdeationQuestion iq = _iqMgr.GetQuestion(id, false);
            
            return View(iq);
        }
        
        #region Idea
        [Authorize]
        public IActionResult AddVote(int idea, string user, int thread)
        {
            _iqMgr.MakeVote(idea,  user);
            
            return RedirectToAction("CollectIdeationThread", "Platform", routeValues: new
            { id = thread });
        }
        #endregion
        #endregion
    }
}