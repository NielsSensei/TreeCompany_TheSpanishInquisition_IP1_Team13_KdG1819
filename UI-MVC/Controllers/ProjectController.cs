using System.Collections;
using System.Collections.Generic;
using BL;
using DAL;
using Domain.Projects;
using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectManager _mgr;

        // GET /Project
        public IActionResult Index()
        {
            IEnumerable<Project> projects = _mgr.GetProjects();
            return View(projects);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Project createProject)
        {
            Project newProject = _mgr.MakeProject(createProject);

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