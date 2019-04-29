using System.Collections.Generic;
using System.Linq;
using BL;
using Domain.Projects;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers
{
    
    public class PlatformController : Controller
    {
        private readonly PlatformManager _platformMgr;
        private readonly ProjectManager _projectMgr;

        public PlatformController()
        {
            _platformMgr = new PlatformManager();
            _projectMgr = new ProjectManager();
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
    }
}