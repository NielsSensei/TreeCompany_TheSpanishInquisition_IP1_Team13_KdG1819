﻿using System;
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
        ProjectManager projMgr;
        ModuleManager modMgr;
        QuestionnaireQuestionManager qqMgr;
        IdeationQuestionManager iqMgr;


        public ModuleController()
        {
            projMgr = new ProjectManager();
            modMgr = new ModuleManager();
            qqMgr = new QuestionnaireQuestionManager();
            iqMgr = new IdeationQuestionManager();
        }

        // GET: api/<controller>/GetModules?projectId=1
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

        // GET api/<controller>/GetQuestionnaire?projectId=1&phaseId=1
        [HttpGet]
        [Route("GetQuestionnaire")]
        public IActionResult GetQuestionnaire(int projectId, int phaseId)
        {
            Questionnaire q = modMgr.GetQuestionnaire(phaseId, projectId);
            if (q == null)
            {
                return NotFound("Geen Questionnaire gevonden voor deze phase!");
            }

            if (q.OnGoing == false) return BadRequest("Deze vragenlijst is nog niet publiek geplaatst!");
            return Ok(q);
        }

        // GET api/<controller>/GetIdeation?projectId=1&phaseId=1
        [HttpGet]
        [Route("GetIdeation")]
        public IActionResult GetIdeation(int projectId, int phaseId)
        {
            Ideation i = modMgr.GetIdeation(phaseId, projectId);

            if (i == null) return NotFound("Geen vragenlijst gevonden voor deze fase!");

            //if (i.OnGoing == false) return BadRequest("Deze ideation is nog niet publiek geplaatst!");

            return Ok(i);
        }

        // GET api/<controller>/GetModuleForPhase?phaseId=1
        [HttpGet]
        [Route("GetModuleForPhase")]
        public IActionResult GetModuleForPhase(int phaseId)
        {
            bool isQuestionnaire = false;

            Phase phase = projMgr.GetPhase(phaseId);

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
                        /*TODO optionlogica*/
                    }
                }

                toReturnQuestionnaire.Questions = qQuestions;
                return Ok(toReturnQuestionnaire);
            }

            else
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


        //api/module/GetIdeas?id=1
        [HttpGet("GetIdeas")]
        public IActionResult GetIdeas(int id)
        {
            var ideas = iqMgr.GetIdeas(id);
            if (ideas == null)
            {
                return NotFound();
            }

            return Ok(ideas);
        }

        /*[HttpPost]
        public async Task<ActionResult<Idea>> PostIdea(Idea idea)
        {
            _idQuesMan.MakeIdea(idea);

            return CreatedAtAction(nameof(GetIdea), new {id = idea.Id}, idea);
        }#1#*/
    }
}