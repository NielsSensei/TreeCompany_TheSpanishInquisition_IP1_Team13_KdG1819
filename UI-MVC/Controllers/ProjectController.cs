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

            // pvm.Phases[0].IsCurrent = true;

            Phase currentPhase = pvm.Phases.Find(phase => phase.IsCurrent);

            Project pr = new Project()
            {
                User = projectUser,
                CurrentPhase = currentPhase,
                EndDate = pvm.Project.EndDate,
                StartDate = pvm.Project.StartDate,
                Title = pvm.Project.Title,
                Platform = new Platform() {Id = platform},
                Status = pvm.Project.Status.ToUpper(),
                LikeVisibility = 1,
                Goal = pvm.Project.Goal,
                Visible = pvm.Project.Visible,
                Phases = pvm.Phases
            };
            Project newProject = _mgr.MakeProject(pr);

            return RedirectToAction("Index", "Platform", new {id = newProject.Platform.Id});
        }

        #endregion


        #region ChangeProject

        //[Authorize]
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
        //[Authorize]
        public ActionResult ChangeProject(EditProjectModel epm, int id)
        {
            Project updateProj = _mgr.GetProject(id, false);

            updateProj.Title = epm.Title;
            updateProj.Goal = epm.Goal;
            updateProj.StartDate = epm.StartDate;
            updateProj.EndDate = epm.EndDate;

            _mgr.EditProject(updateProj);
            // TODO: make the redirect work
            return RedirectToAction("Index", "Platform", new {id = updateProj.Platform.Id});
        }

        #endregion


        #region

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
    }
}