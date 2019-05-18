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

        
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Project project)
        {
            projMgr.EditProject(project);

            return NoContent();


        }

       
    }
}
