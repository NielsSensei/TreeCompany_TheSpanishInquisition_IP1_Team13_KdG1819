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

        // GET: api/<controller>
        [HttpGet]
        [Route("GetAllByPlatform")]
        public IActionResult GetAllByPlatform(int platformId)
        {
            var projects = projMgr.GetPlatformProjects(platMgr.GetPlatform(platformId));

            foreach (Project project in projects)
            {
                project.Phases = projMgr.GetAllPhases(project.Id).ToList();
                Phase curPhase = projMgr.GetPhase(project.CurrentPhase.Id);
                project.CurrentPhase = curPhase;
            }

            if (projects == null)
                return NotFound();

            return Ok(projects);
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(int projectId)
        {
            var project = projMgr.GetProject(projectId, true);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        public IActionResult SortedBy(string quota, int platformId)
        {
            List<Project> projects = projMgr.GetPlatformProjects(platMgr.GetPlatform(platformId)).ToList();

            switch (quota)
            {
                case "Naam": return Ok(projects.OrderBy(m => m.Title));
                    break;
                case "Status": return Ok(projects.OrderBy(m => m.Status));
                    break;
                case "Likes": return Ok(projects.OrderBy(m => m.LikeCount));
                    break;
                case "Reacties": return Ok(projects.OrderBy(m => m.ReactionCount));
                    break;
                default: return Ok(projects);
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
