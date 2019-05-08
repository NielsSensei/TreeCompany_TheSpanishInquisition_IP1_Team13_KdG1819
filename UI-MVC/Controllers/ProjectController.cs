using System;
using System.Collections;
using System.Collections.Generic;
using BL;
using DAL.Data_Transfer_Objects;
using Domain.Identity;
using Domain.Projects;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UIMVC.Models;

namespace UIMVC.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectManager _mgr;
        private PlatformManager _mgrPlatform;
        private readonly UserManager<UIMVCUser> _userManager;

        public ProjectController(UserManager<UIMVCUser> userManager)
        {
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
        public IActionResult AddProject(ProjectViewModel pvm, int platform)
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

        //[Authorize]
        [HttpGet]
        public IActionResult ChangeProject(int id)
        {
            Project project = _mgr.GetProject(id, false);

            if (project == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);
            }

            return View(project);
        }

        [HttpPost]
        //[Authorize]
        public IActionResult ChangeProject(Project project)
        {
            _mgr.EditProject(project);
            // TODO: make the redirect work
            return RedirectToAction("Index", "Platform", new {id = project.Platform.Id});
        }

        #endregion


        #region

        [HttpGet]
        public IActionResult DestroyProject(int id)
        {
            Project project = _mgr.GetProject(id, false);
            int platformId = project.Platform.Id;

            project.Phases = (List<Phase>) _mgr.GetAllPhases(project.Id);

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