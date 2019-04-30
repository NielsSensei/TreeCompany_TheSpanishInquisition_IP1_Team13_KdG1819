using System.Collections;
using System.Collections.Generic;
using BL;
using Domain.Projects;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;
using UIMVC.Models;

namespace UIMVC.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectManager _mgr;

        public ProjectController()
        {
            _mgr = new ProjectManager();
        }

        // GET /Project
        [Route("Project/Index/{id}")]
        public IActionResult Index(int id)
        {
            Project project = _mgr.GetProject(id, false);
            if (project == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);
            }

            return View(project);
        }


        [HttpPost]
        public ActionResult Index(Project emp, List<Phase> dept)
        {
            emp.Phases = dept;
            return View(emp);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProjectViewModel projectViewModel /*Project project*/)
        {
            Project project = projectViewModel.Project;

            project.Platform = new Platform {Id = 1};
            project.User = new User() {Id = 1};
            project.CurrentPhase = new Phase() {Id = 1};

            project.Status = project.Status.ToUpper();
            project.LikeVisibility = 1;

            Project newProject = _mgr.MakeProject(project);

            
            

            for (int i = 0; i < 4; i++)
            {
                Phase newPhase = projectViewModel.Phases[i];
                newProject.Phases.Add(newPhase);
                newPhase.Project = newProject;
                _mgr.MakePhase(newPhase, project.Id);
            }
            return RedirectToAction("Details", new {id = newProject.Id});
        }


        public IActionResult CreatePhase(int projectId, Phase phase)
        {
            _mgr.MakePhase(phase, projectId);

            return PartialView();
        }

        //GET: /Project/Detail/<project_id>
        public IActionResult Details(int id)
        {
            Project project = _mgr.GetProject(id, false);

            return View(project);
        }
    }
}