using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using BL;
using Domain.Identity;
using Domain.Projects;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Query.Internal;
using UIMVC.Models;

namespace UIMVC.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectManager _projManager;
        private ModuleManager _modManager;
        private readonly UserManager<UimvcUser> _userManager;


        public ProjectController(UserManager<UimvcUser> userManager)
        {
            _modManager = new ModuleManager();
            _projManager = new ProjectManager();
            _userManager = userManager;
        }


        #region Project

        #region Add

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult AddProject(int platform)
        {
            ViewData["platform"] = platform;
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult AddProject(CreateProjectModel pvm, int platform)
        {
            UimvcUser projectUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (pvm == null)
            {
                return BadRequest("Project cannot be null");
            }

            Project pr = new Project()
            {
                User = projectUser,
                CurrentPhase = pvm.CurrentPhase,
                /*EndDate = pvm.EndDate,
                StartDate = pvm.StartDate,*/
                Title = pvm.Title,
                Platform = new Platform() {Id = platform},
                Status = pvm.Status.ToUpper(),
                LikeVisibility = 1,
                Goal = pvm.Goal,
                Visible = pvm.Visible
            };

            pr.Phases.Add(pr.CurrentPhase);

            Project newProject = _projManager.MakeProject(pr);

            return RedirectToAction("Index", "Platform", new {id = newProject.Platform.Id});
        }

        #endregion


        #region ChangeProject

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult ChangeProject(int id)
        {
            Project project = _projManager.GetProject(id, false);

            if (project == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);
            }

            ViewData["Project"] = project;
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        public ActionResult ChangeProject(EditProjectModel epm, int id)
        {
            Project updateProj = _projManager.GetProject(id, false);

            updateProj.Title = epm.Title;
            updateProj.Goal = epm.Goal;
            /*updateProj.StartDate = epm.StartDate;
            updateProj.EndDate = epm.EndDate;*/
            updateProj.Visible = epm.Visible;
            updateProj.Status = epm.Status.ToUpper();


            _projManager.EditProject(updateProj);
            return RedirectToAction("CollectProject", "Platform", new {id = updateProj.Id});
        }

        #endregion


        #region DeleteProject

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult DestroyProject(int id)
        {
            Project project = _projManager.GetProject(id, false);
            int platformId = project.Platform.Id;
            project.Phases = (List<Phase>) _projManager.GetAllPhases(project.Id);

            project.Modules = new List<Module>();

            if (project.Modules != null && project.Modules.Count != 0)
            {
                foreach (var module in project.Modules)
                {
                    if (module.ModuleType == ModuleType.Questionnaire)
                    {
                        _modManager.RemoveModule(module.Id, true);
                    }

                    _modManager.RemoveModule(module.Id, false);
                }
            }

            if (project.Phases.Count != 0)
            {
                foreach (var phase in project.Phases)
                {
                    _projManager.RemovePhase(project.Id, phase.Id);
                }
            }

            _projManager.RemoveProject(id);


            return RedirectToAction("Index", "Platform", new {id = platformId});
        }

        #endregion

        #endregion

        #region phase

        #region AddPhase

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult AddPhase(int projectId)
        {
            ViewData["project"] = projectId;

            return View();
        }


        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult AddPhase(PhaseModel pm, int projectId)
        {
            if (pm == null)
            {
                return BadRequest("Phase cannot be null");
            }

            Phase p = new Phase()
            {
                Project = _projManager.GetProject(projectId, false),
                Description = pm.Description,
                StartDate = pm.StartDate,
                EndDate = pm.EndDate
            };

            _projManager.MakePhase(p, projectId);

            return RedirectToAction("CollectProject", "Platform", new {id = projectId});
        }

        #endregion


        #region ChangePhase

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult ChangePhase(int phaseId)
        {
            Phase phase = _projManager.GetPhase(phaseId);


            if (phase == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);
            }

            ViewData["Phase"] = phase;
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult ChangePhase(PhaseModel pm, int phaseId)
        {
            Phase updatePhase = _projManager.GetPhase(phaseId);

            updatePhase.Description = pm.Description;
            updatePhase.StartDate = pm.StartDate;
            updatePhase.EndDate = pm.EndDate;

            _projManager.EditPhase(updatePhase);
            return RedirectToAction("CollectProject", "Platform", new {id = updatePhase.Project.Id});
        }

        #endregion


        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult SetCurrentPhase(int projectId, int phaseId)
        {
            Project p = _projManager.GetProject(projectId, true);

            Phase ph = _projManager.GetPhase(phaseId);

            p.CurrentPhase = ph;

            _projManager.EditProject(p);

            return RedirectToAction("CollectProject", "Platform", new {id = projectId});
        }

        #region DestroyPhase

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult DestroyPhase(int phaseId, int projectId)
        {
            _projManager.RemovePhase(projectId, phaseId);

            return RedirectToAction("CollectProject", "Platform", new {id = projectId});
        }

        #endregion

        #endregion
    }
}