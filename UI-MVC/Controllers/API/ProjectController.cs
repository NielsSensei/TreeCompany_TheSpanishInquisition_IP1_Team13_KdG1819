using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Domain.Projects;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UIMVC.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private ProjectManager projMgr;
        private PlatformManager platMgr;

        public ProjectController()
        {
            projMgr = new ProjectManager();
            platMgr = new PlatformManager();
        }

        // GET api/<controller>/GetById?projectId=1
        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(int projectId)
        {
            var project = projMgr.GetProject(projectId, true);

            if (project == null)
                return NotFound();

            return Ok(project);
        }
        
        [HttpGet]
        [Route("SortedBy")]
        public IActionResult SortedBy(int quota, int platformId)
        {
            var projects = projMgr.GetPlatformProjects(platMgr.GetPlatform(platformId));
            var visibleProjects = new List<Project>();
            
            foreach (Project project in projects)
            {
                if (!project.Visible) continue;
                project.Phases = projMgr.GetAllPhases(project.Id).ToList();
                var curPhase = projMgr.GetPhase(project.CurrentPhase.Id);
                project.CurrentPhase = curPhase;
                visibleProjects.Add(project);
            }

            switch (quota)
            {
                case 1: return Ok(visibleProjects.OrderBy(m => m.Title));
                    break;
                case 2: return Ok(visibleProjects.OrderBy(m => m.Status));
                    break;
                case 3: return Ok(visibleProjects.OrderBy(m => m.LikeCount));
                    break;
                case 4: return Ok(visibleProjects.OrderBy(m => m.ReactionCount));
                    break;
                default: return Ok(visibleProjects);
            }
                
        }

        
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Project project)
        {
            projMgr.EditProject(project);

            return NoContent();


        }

       
    }
}
