using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using BL;
using DAL.Data_Transfer_Objects;
using Domain.Identity;
using Domain.Projects;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using UIMVC.Models;

namespace UIMVC.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectManager _mgr;
        private ModuleManager _modMan;
        private PlatformManager _mgrPlatform;
        private readonly UserManager<UIMVCUser> _userManager;

        public ProjectController(UserManager<UIMVCUser> userManager)
        {
            _modMan = new ModuleManager();
            _mgr = new ProjectManager();
            _mgrPlatform = new PlatformManager();
            _userManager = userManager;
        }

        
        #region Project
        
        #region Add

        [Authorize]
        [HttpGet]
        public IActionResult AddProject(int platform)
        {
            ViewData["platform"] = platform;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddProject(CreateProjectModel pvm, int platform)
        {
            UIMVCUser projectUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (pvm == null)
            {
                return BadRequest("Project cannot be null");
            }

            Project pr = new Project()
            {
                User = projectUser,
                CurrentPhase = pvm.CurrentPhase,
                EndDate = pvm.EndDate,
                StartDate = pvm.StartDate,
                Title = pvm.Title,
                Platform = new Platform() {Id = platform},
                Status = pvm.Status.ToUpper(),
                LikeVisibility = 1,
                Goal = pvm.Goal,
                Visible = true,
            };

            pr.Phases.Add(pr.CurrentPhase);

            Project newProject = _mgr.MakeProject(pr);

            return RedirectToAction("Index", "Platform", new {id = newProject.Platform.Id});
        }

        #endregion


        #region ChangeProject

        [Authorize]
        [HttpGet]
        public IActionResult ChangeProject(int id)
        {
            Project project = _mgr.GetProject(id, false);

            if (project == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);
            }

            ViewData["Project"] = project;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangeProject(EditProjectModel epm, int id)
        {
            Project updateProj = _mgr.GetProject(id, false);

            updateProj.Title = epm.Title;
            updateProj.Goal = epm.Goal;
            updateProj.StartDate = epm.StartDate;
            updateProj.EndDate = epm.EndDate;

            _mgr.EditProject(updateProj);
            return RedirectToAction("CollectProject", "Platform", new {id = updateProj.Id});
        }

        #endregion


        #region DeleteProject

        [HttpGet]
        public IActionResult DestroyProject(int id)
        {
            var modController = new ModerationController(_userManager);

            Project project = _mgr.GetProject(id, false);
            int platformId = project.Platform.Id;

            project.Phases = (List<Phase>) _mgr.GetAllPhases(project.Id);

            /*if (project.Modules.Count != 0)
            {
                foreach (var module in project.Modules)
                {
                    modController.DestroyIdeation(module.Id);
                }
            }*/

            foreach (var phase in project.Phases)
            {
                _mgr.RemovePhase(project.Id, phase.Id);
            }

            _mgr.RemoveProject(id);

            return RedirectToAction("Index", "Platform", new {id = platformId});
        }

        #endregion

        #endregion
        
        #region phase

        #region AddPhase

        

        
        [Authorize]
        [HttpGet]
        public IActionResult AddPhase(int projectId)
        {
            ViewData["project"] = projectId;

            return View();
        }


        [Authorize]
        [HttpPost]
        public IActionResult AddPhase(PhaseModel pm, int projectId)
        {
            if (pm == null)
            {
                return BadRequest("Phase cannot be null");
            }

            Phase p = new Phase()
            {
                Project = _mgr.GetProject(projectId, false),
                Description = pm.Description,
                StartDate = pm.StartDate,
                EndDate = pm.EndDate
            };

            _mgr.MakePhase(p, projectId);

            return RedirectToAction("CollectProject", "Platform", new {id = projectId});
        }
        #endregion

        
        #region ChangePhase

        

        
        [Authorize]
        [HttpGet]
        public IActionResult ChangePhase(int phaseId)
        {
            Phase phase = _mgr.GetPhase(phaseId);



            if (phase == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);

            }
            
            ViewData["Phase"] = phase;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePhase(PhaseModel pm, int phaseId)
        {

            Phase updatePhase = _mgr.GetPhase(phaseId);

            updatePhase.Description = pm.Description;
            updatePhase.StartDate = pm.StartDate;
            updatePhase.EndDate = pm.EndDate;
            
            _mgr.EditPhase(updatePhase);
            return RedirectToAction("CollectProject", "Platform", new {id = updatePhase.Project.Id});
        }
        
        #endregion


        
        [Authorize]
        [HttpGet]
        public IActionResult SetCurrentPhase(int projectId, int phaseId)
        {
            Project p = _mgr.GetProject(projectId, true);

            Phase ph = _mgr.GetPhase(phaseId);

            p.CurrentPhase = ph;

            _mgr.EditProject(p);

            return RedirectToAction("CollectProject", "Platform", new {id = projectId});
        }

        #endregion


        public IActionResult DestroyPhase()
        {
            throw new NotImplementedException();
        }
    }
}