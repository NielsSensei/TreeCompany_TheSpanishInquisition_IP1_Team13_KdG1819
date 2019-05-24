using System.Collections.Generic;
using System.Linq;
using BL;
using Domain.Projects;
using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers.API
{

    //Webapi Controller: This is the controller to get projects from our database
    //Author: Sacha Buelens
    //Edited by: David Matei, Edwin Kai-Yin Tam


    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {

        //Importing managers to get projects and platforms
        //All data is being sent as a JSON like webapi should do

        private ProjectManager projMgr;
        private PlatformManager platMgr;

        public ProjectController()
        {
            projMgr = new ProjectManager();
            platMgr = new PlatformManager();
        }

        // GET api/<controller>/GetById?projectId=1

        //Method to get a project by its ID
        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(int projectId)
        {
            var project = projMgr.GetProject(projectId, true);

            if (project == null)
                return NotFound();

            return Ok(project);
        }
        
        //%ethod to get a sorted list of projects
        //Projects can be sorted by title, status, likecount and reactioncount
        //
        [HttpGet]
        [Route("SortedBy")]
        public IActionResult SortedBy(int quota, int platformId)
        {
            //Getting all projects to filter through
            var projects = projMgr.GetPlatformProjects(platMgr.GetPlatform(platformId, true));
            var visibleProjects = new List<Project>();


            //Getting al the visible projects and put them in a seperate list
            //Projects that aren't visible don't need to be requested
            foreach (Project project in projects)
            {
                if (!project.Visible) continue;
                project.Phases = projMgr.GetAllPhases(project.Id).ToList();
                var curPhase = projMgr.GetPhase(project.CurrentPhase.Id, true);
                project.CurrentPhase = curPhase;
                visibleProjects.Add(project);
            }

            //Quota is a number that is used to filter
            //Each number corresponds to a filtered list that you get through te API
            //So everytime the user filters it gets the corresponding projects

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
        //Currently unused method to make edit a project
        [HttpPut("{id}")]
        public IActionResult Put(int id, Project project)
        {
            projMgr.EditProject(project);

            return NoContent();


        }

       
    }
}
