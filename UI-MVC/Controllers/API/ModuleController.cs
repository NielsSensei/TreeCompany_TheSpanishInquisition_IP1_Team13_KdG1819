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
    [Route("api/[controller]")]
    public class ModuleController : Controller
    {
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


        [HttpGet]
        [Route("GetQuestionnaires")]
        public IActionResult GetQuestionnaires(int projectId)
        {
            List<Questionnaire> i = _modMgr.GetQuestionnaires(projectId) as List<Questionnaire>;
            if (i == null) return NotFound("Geen vragenlijst gevonden!");
            return Ok(i);
        }

        // GET api/<controller>/GetIdeation?projectId=1&phaseId=1
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


        [HttpGet]
        [Route("GetIdeationQuestions")]
        public IActionResult GetIdeationQuestions(int moduleId)
        {
            List<IdeationQuestion> questions = _iqMgr.GetAllByModuleId(moduleId);
            if (questions == null) return NotFound("Geen Centrale vragen");

            return Ok(questions);
        }


        [HttpGet]
        [Route("GetIdea")]
        public IActionResult GetIdea(int ideaId)
        {
            Idea i = _iqMgr.GetIdea(ideaId, false);
            if (i == null) return NotFound("Geen vragenlijst gevonden voor deze fase!");

            return Ok(i);
        }

        [HttpGet]
        [Route("GetIdeation")]
        public IActionResult GetIdeation(int moduleId)
        {
            Ideation i = _modMgr.GetIdeation(moduleId, false);
            if (i == null) return NotFound("Geen vragenlijst gevonden voor deze fase!");

            return Ok(i);
        }

        // f
        [HttpGet]
        [Route("GetIdeation")]
        public IActionResult GetIdeationForPhase(int projectId, int phaseId)
        {
            Ideation i = _modMgr.GetIdeation(phaseId, projectId);
            if (i == null) return NotFound("Geen vragenlijst gevonden voor deze fase!");
            //if (i.OnGoing == false) return BadRequest("Deze ideation is nog niet publiek geplaatst!");
            return Ok(i);
        }

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

        /*
        [HttpGet]
        [Route("GetModuleForPhase")]
        public IActionResult GetModuleForPhase(int phaseId)
        {
            bool isQuestionnaire = false;

            Phase phase = projMgr.GetPhase(phaseId, false);

            Module toReturn = new Module();

            List<Module> modules = modMgr.GetAllModules(phase.Project.Id).ToList();

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

            if (isQuestionnaire)
            {
                Questionnaire toReturnQuestionnaire = (Questionnaire) toReturn;


                List<QuestionnaireQuestion> qQuestions = qqMgr.GetAllByModuleId(toReturnQuestionnaire.Id);

                foreach (QuestionnaireQuestion qQ in qQuestions)
                {
                    if (qQ.QuestionType == QuestionType.Drop || qQ.QuestionType == QuestionType.Multi ||
                        qQ.QuestionType == QuestionType.Single)
                    {
                        /*TODO optionlogica#1#
                    }
                }

                toReturnQuestionnaire.Questions = qQuestions;
                return Ok(toReturnQuestionnaire);
            }

            else if (!isQuestionnaire)
            {
                Ideation toReturnIdeation = (Ideation) toReturn;

                List<IdeationQuestion> ideationQuestions = iqMgr.GetAllByModuleId(toReturnIdeation.Id).ToList();

                foreach (IdeationQuestion ideationQ in ideationQuestions)
                {
                    List<Idea> ideas = iqMgr.GetIdeas(ideationQ.Id);
                }

                toReturnIdeation.CentralQuestions = ideationQuestions;

                return Ok(toReturnIdeation);
            }


            throw new Exception("Skipping the IF statement for some reason...");
        }
        */


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
*/

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

    //DAVIDSHIZZLE TO REFACTOR
    //moduleId
    /*[HttpGet("{id}")]
    public async Task<ActionResult<Idea>> GetIdea(int id)
    {

        var idea = _idQuesMan.GetIdea(id);
        if (idea == null)
        {
            _idQuesMan.MakeIdea(idea);

            return CreatedAtAction(nameof(GetIdea), new { id = idea.Id }, idea);
        }*/


        /*HttpPost]

        public async Task<ActionResult<Idea>> PostIdea(Idea idea)
        {
            _idQuesMan.MakeIdea(idea);
            return CreatedAtAction(nameof(GetIdea), new
            {
                id = idea.Id
            }, idea);
        }

        #1#*/
    }
}