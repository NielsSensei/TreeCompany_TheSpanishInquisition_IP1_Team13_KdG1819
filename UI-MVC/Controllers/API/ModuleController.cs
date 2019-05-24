using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Domain.Projects;
using Domain.UserInput;
using Microsoft.AspNetCore.Mvc;


namespace UIMVC.Controllers.API
{
    //Controller API for everything related to modules
    //Author: Sacha Buelens
    //Edited by: Edwin Kai-Yin Tam, David Matei

    [Route("api/[controller]")]
    public class ModuleController : Controller
    {
        //Getting managers to get modules from our database
        //Required managers are : project, module, questionnairequestion and ideationquestion
        //to get all the info we need from our database

        private readonly ProjectManager _projMgr;
        private readonly ModuleManager _modMgr;
        private readonly QuestionnaireQuestionManager _qqMgr;
        private readonly IdeationQuestionManager _iqMgr;

        public ModuleController()
        {
            _projMgr = new ProjectManager();
            _modMgr = new ModuleManager();
            _qqMgr = new QuestionnaireQuestionManager();
            _iqMgr = new IdeationQuestionManager();
        }


        //Currently unused method to get all modules for a project, by its Id
        // GET: api/<controller>/getModules?projectId=1
        /*[HttpGet]
        [Route("GetModules")]
        public IActionResult GetModules(int projectId)
        {
      
            List<Module> modules = modMgr.GetAllModules(projectId).ToList();

            if (modules.Count == 0)
            {
                return NotFound("Geen modules gevonden voor project: " + projectId);
            }

            return Ok(modules);
        }*/

        //GET api/<controller>/GetIdeas?ideationQuestionId=1

        //Method to get all ideas for an ideationquestion, filtered by its Id
        [HttpGet]
        [Route("GetIdeas")]
        public IActionResult GetIdeas(int ideationQuestionId)
        {
            List<Idea> ideas = _iqMgr.GetIdeas(ideationQuestionId);
            if (ideas == null)
            {
                return NotFound("Geen idee gevonden!");
            }

            return Ok(ideas);
        }


        // GET api/<controller>/5

         //Method to get a questionnaire for a project and a specific phase of that project (projectid, phaseid)
        [HttpGet]
        [Route("GetQuestionnaire")]
        public IActionResult GetQuestionnaire(int projectId, int phaseId)
        {
            Questionnaire q = _modMgr.GetQuestionnaire(phaseId, projectId);


            if (q == null)

                if (q.OnGoing == false)
                    return BadRequest("Deze vragenlijst is nog niet publiek geplaatst!");

            return Ok(q);
        }


        //Method to get all questionnaires for a project, filtered by its ID

        [HttpGet]
        [Route("GetQuestionnaires")]
        public IActionResult GetQuestionnaires(int projectId)
        {
            List<Questionnaire> i = _modMgr.GetQuestionnaires(projectId) as List<Questionnaire>;
            if (i == null) return NotFound("Geen vragenlijst gevonden!");
            return Ok(i);
        }

        // GET api/<controller>/GetIdeation?projectId=1&phaseId=1
        //Method to get an ideation of a specific phase of a project, filtered by their ID's
        [
            HttpGet]
        [Route("GetIdeation")]
        public IActionResult GetIdeation(int projectId, int phaseId)
        {
            Ideation i = _modMgr.GetIdeation(phaseId, projectId);

            if (i == null) return NotFound("Geen vragenlijst gevonden voor deze fase!");

            //if (i.OnGoing == false) return BadRequest("Deze ideation is nog niet publiek geplaatst!");

            return Ok(i);
        }

        //Method to get all ideationquestions for an ideation

        [HttpGet]
        [Route("GetIdeationQuestions")]
        public IActionResult GetIdeationQuestions(int moduleId)
        {
            List<IdeationQuestion> questions = _iqMgr.GetAllByModuleId(moduleId);
            if (questions == null) return NotFound("Geen Centrale vragen");

            return Ok(questions);
        }

        //method to get aan idea, by its ID
        [HttpGet]
        [Route("GetIdea")]
        public IActionResult GetIdea(int ideaId)
        {
            Idea i = _iqMgr.GetIdea(ideaId, false);
            if (i == null) return NotFound("Geen vragenlijst gevonden voor deze fase!");

            return Ok(i);
        }

        //Method to get an ideation by its Id
        [HttpGet]
        [Route("GetIdeation")]
        public IActionResult GetIdeation(int moduleId)
        {
            Ideation i = _modMgr.GetIdeation(moduleId, false);
            if (i == null) return NotFound("Geen vragenlijst gevonden voor deze fase!");

            return Ok(i);
        }

        // Method to get an ideation for a specific phase of a project
        [HttpGet]
        [Route("GetIdeation")]
        public IActionResult GetIdeationForPhase(int projectId, int phaseId)
        {
            Ideation i = _modMgr.GetIdeation(phaseId, projectId);
            if (i == null) return NotFound("Geen vragenlijst gevonden voor deze fase!");
            //if (i.OnGoing == false) return BadRequest("Deze ideation is nog niet publiek geplaatst!");
            return Ok(i);
        }


        //Method to get all ideations for a project
        [
            HttpGet]
        [Route("GetIdeations")]
        public IActionResult GetIdeations(int projectId)
        {
            List<Ideation> i = _modMgr.GetIdeations(projectId) as List<Ideation>;
            if (i == null) return NotFound("Geen ideations gevonden!");

            return Ok(i);
        }

        // GET api/<controller>/GetModuleForPhase?phaseId=1

            
        //This method gets a module for a specific phase
        //It looks what type the module is and loads the according info with it
        [HttpGet]
        [Route("GetModuleForPhase")]
        public IActionResult GetModuleForPhase(int phaseId)
        {

            //boolean to determine the module type
            bool isQuestionnaire = false;


            //Gets a phase by its ID
            Phase phase = _projMgr.GetPhase(phaseId, false);

            //This is the module that's supposed to be returned, after the type is determined
            Module toReturn = new Module();

            //Gets all modules for the project that belongs to the phase 
            List<Module> modules = _modMgr.GetAllModules(phase.Project.Id).ToList();

            //Checks if the module type is a questionnaire or ideation
            foreach (Module mod in modules)
            {
                if (mod.ParentPhase.Id == phase.Id)
                {
                    if (mod.ModuleType == ModuleType.Questionnaire)
                    {
                        isQuestionnaire = true;
                        //toReturnQuestionnaire = (Questionnaire)mod;
                    }
                    else
                    {
                        isQuestionnaire = false;
                        //toReturnIdeation = (Ideation)mod;
                    }

                    toReturn = mod;
                }
            }


            //For all the questionnaires -> it will get all child attributes that are not values(questions, answers, options etc)
            if (isQuestionnaire)
            {
                Questionnaire toReturnQuestionnaire = (Questionnaire) toReturn;


                List<QuestionnaireQuestion> qQuestions = _qqMgr.GetAllByModuleId(toReturnQuestionnaire.Id);

                foreach (QuestionnaireQuestion qQ in qQuestions)
                {
                    if (qQ.QuestionType == QuestionType.Drop || qQ.QuestionType == QuestionType.Multi ||
                        qQ.QuestionType == QuestionType.Single)
                    {
                        //TODO optionlogica#1#
                    }
                }

                toReturnQuestionnaire.Questions = qQuestions;
                return Ok(toReturnQuestionnaire);
            }

            //For every Ideation it gets all the ideas, answers to ideas etc

            else if (!isQuestionnaire)
            {
                Ideation toReturnIdeation = (Ideation) toReturn;

                List<IdeationQuestion> ideationQuestions = _iqMgr.GetAllByModuleId(toReturnIdeation.Id).ToList();

                foreach (IdeationQuestion ideationQ in ideationQuestions)
                {
                    List<Idea> ideas = _iqMgr.GetIdeas(ideationQ.Id);
                }

                toReturnIdeation.CentralQuestions = ideationQuestions;

                return Ok(toReturnIdeation);
            }


            throw new Exception("Skipping the IF statement for some reason...");
        }
        

        //Post methods that are supposed to let us fill in questionnaires on the android device

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            var id = _modMgr.GetIdeations(1);
            return null;
        }

        // PUT api/<controller>/5
        [HttpPut]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return null;
        }

        /*
        [HttpPost]
        public IActionResult VoteIdea(int IdeaId)
        {
            var idea = iqMgr.GetIdea(IdeaId, false);

            return
                Ok(idea);
        }


        //moduleId
        /*[HttpGet("{id}")]
        public async Task<ActionResult<Idea>> GetIdea(int id)
        {

            var idea = _idQuesMan.GetIdea(id);
            if (idea == null)

        else if (!isQuestionnaire)
        {
            Ideation toReturnIdeation = (Ideation) toReturn;

            List<IdeationQuestion> ideationQuestions = iqMgr.GetAllByModuleId(toReturnIdeation.Id).ToList();

            foreach (IdeationQuestion ideationQ in ideationQuestions)
            {
                return NotFound();
            }

            return idea;
        }

        [HttpPost]
        public async Task<ActionResult<Idea>> PostIdea(Idea idea)

        throw new Exception("Skipping the IF statement for some reason...");
    }

    // POST api/<controller>
    [HttpPost]
    public IActionResult Post([FromBody] string value)
    {
        return null;
    }

    // PUT api/<controller>/5
    [HttpPut]
    public IActionResult Put(int id, [FromBody] string value)
    {
        return null;
    }

    */
    
    }
}