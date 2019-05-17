using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Domain.Projects;
using Domain.UserInput;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UIMVC.Controllers.API
{
    [Route("api/[controller]")]
    public class ModuleController : Controller
    {
        ProjectManager projMgr;
        ModuleManager modMgr;
        QuestionnaireQuestionManager qqMgr;
        IdeationQuestionManager iqMgr;
        

        public ModuleController()
        {
            projMgr = new ProjectManager();
            modMgr = new ModuleManager();
        }

        // GET: api/<controller>
        [HttpGet]
        [Route("GetModules")]
        public IActionResult GetModules(int projectId)
        {
            List<Module> modules = modMgr.GetAllModules(projectId).ToList();

            if (modules.Count == 0)
            {
                return NotFound("Geen modules gevonden voor project: " + projectId);
            }
            return Ok(modules);

            
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("GetQuestionnaire")]
        public IActionResult GetQuestionnaire(int projectId, int phaseId)
        {
            Questionnaire q = modMgr.GetQuestionnaire(phaseId, projectId);


            if(q == null)
            {
                return NotFound("Geen Questionnaire gevonden voor deze phase!");
            }

            if(q.OnGoing == false) return BadRequest("Deze vragenlijst is nog niet publiek geplaatst!");
           
            



            return Ok(q);
        }

        [HttpGet]
        [Route("GetIdeation")]
        public IActionResult GetIdeation(int projectId, int phaseId)
        {
            Ideation i = modMgr.GetIdeation(phaseId, projectId);

            if (i == null) return NotFound("Geen vragenlijst gevonden voor deze fase!");

            //if (i.OnGoing == false) return BadRequest("Deze ideation is nog niet publiek geplaatst!");

            return Ok(i);
        }

        [HttpGet]
        [Route("GetModuleForPhase")]
        public IActionResult GetModuleForPhase(int phaseId)
        {
            Phase phase = projMgr.GetPhase(phaseId);

            Module toReturn = null;

            List<Module> modules = modMgr.GetAllModules(phase.Project.Id).ToList();

            

            foreach (Module mod in modules)
            {
                if (mod.ParentPhase.Id == phase.Id) toReturn = mod;
            }

            /*if (toReturn.ModuleType == ModuleType.Questionnaire)
            {
                List<QuestionnaireQuestion> qQuestions = qqMgr.GetAllByModuleId(toReturn.Id);

                foreach (QuestionnaireQuestion qQ in qQuestions)
                {
                    if (qQ.QuestionType == QuestionType.Drop || qQ.QuestionType == QuestionType.Multi || qQ.QuestionType == QuestionType.Single)
                    {
                        
                    }
                }

            }*/


            if (toReturn == null)
            {
                return NotFound("Geen modules voor deze fase gevonden!");
            }

            return Ok(toReturn);

            
            

            

            

        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            return null;
        }

        // PUT api/<controller>/5
        [HttpPut]
        public IActionResult Put(int id, [FromBody]string value)
        {
            return null;
        }

        
    }
}
