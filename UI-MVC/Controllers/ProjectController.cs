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


        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Test(ProjectViewModel viewmodel)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProjectViewModel projectViewModel)
        {
            Project project = projectViewModel.Project;

            project.Platform = new Platform {Id = 1};
            project.User = new User() {Id = 1};
            project.CurrentPhase = new Phase() {Id = 1};

            project.Status = project.Status.ToUpper();
            project.Visible = true;
            project.LikeVisibility = 1;

            Project newProject = _mgr.MakeProject(project);

            Phase newPhase = projectViewModel.Phase;
            
            
            newPhase.Project = newProject;
            _mgr.MakePhase(newPhase, project.Id);

           

            return RedirectToAction("Details", new {id = newProject.Id});
        }


        public IActionResult CreatePhase()
        {
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