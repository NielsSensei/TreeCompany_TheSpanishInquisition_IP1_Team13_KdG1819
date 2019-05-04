using BL;
using Domain.Projects;
using Domain.UserInput;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMVC.Models;

namespace UIMVC.Controllers
{
    //Created by SB on 26/04/2019
    public class AdminController : Controller
    {
        //Managers (possibly) needed
        private ModuleManager modMgr { get; set; }
        private ProjectManager projMgr { get; set; }
        private QuestionnaireQuestionManager qqMgr { get; set; }

        public AdminController()
        {
            modMgr = new ModuleManager();
            projMgr = new ProjectManager();
            qqMgr = new QuestionnaireQuestionManager();
        }




        //Listing some basic methods that map to the functionalities described in YouTrack
        [HttpGet]
        public IActionResult AddQuestionnaire(int projectId)
        {
            Project toAddQuestionnaireTo = projMgr.GetProject(projectId, true);

            List<Phase> availablePhases = new List<Phase>();

            foreach (Phase phase in projMgr.GetAllPhases(projectId).ToList())
            {
                if(modMgr.GetModule(phase.Id, projectId) == null)
                {
                    availablePhases.Add(phase);
                }
            }

            toAddQuestionnaireTo.Phases = availablePhases.ToList();

            ViewData["project"] = toAddQuestionnaireTo;


            return View();
        }


        [HttpPost]
        public IActionResult AddQuestionnaire(CreateQuestionnaireModel cqm, int projectId)
        {
            if(cqm == null)
            {
                return BadRequest("Questionnaire cannot be NULL!");
            }

            Project questionnaireProject = projMgr.GetProject(projectId, false);
            Phase parentPhase = projMgr.GetPhase(Int32.Parse(Request.Form["ParentPhase"].ToString()));



            Questionnaire newQuestionnaire = new Questionnaire
            {
                Project = questionnaireProject,
                ParentPhase = parentPhase,
                OnGoing = false,
                Title = cqm.Title,
                LikeCount = 0,
                FbLikeCount = 0,
                TwitterLikeCount = 0,
                ShareCount = 0,
                VoteLevel = Domain.Users.Role.ANONYMOUS,
                type = ModuleType.Questionnaire,
                Phases = new List<Phase>(),
                Tags = new List<string>(),
                UserCount = 0,
                Questions = new List<QuestionnaireQuestion>()
            };

            newQuestionnaire.Phases.Add(parentPhase);
            modMgr.MakeQuestionnaire(newQuestionnaire);

            return RedirectToAction("EditQuestionnaire",new { questionnaireId = newQuestionnaire.Id});
        }

        [HttpGet]
        public IActionResult AddQuestionnaireQuestion(int questionnaireid)
        {
            ViewData["Questionnaire"] = modMgr.GetModule(questionnaireid, false, true);
            return View(new QuestionnaireQuestion());
        }




        [HttpPost]
        public IActionResult AddQuestionnaireQuestion(int questionnaireId, QuestionnaireQuestion qQ)
        {
            Questionnaire toAdd = (Questionnaire) modMgr.GetModule(questionnaireId, false, true);
            QuestionnaireQuestion newQuestion = new QuestionnaireQuestion
            {

                QuestionText = qQ.QuestionText,
                QuestionType = qQ.QuestionType,
                Module = toAdd,
                Questionnaire = toAdd,
                Optional = qQ.Optional,
                Answers = new List<Answer>()



            };

            toAdd.Questions.Add(qQ);
            qqMgr.MakeQuestion(newQuestion, toAdd.Id);
            modMgr.EditModule(toAdd);

            return RedirectToAction("CreateQuestionnaire", toAdd.Id);

            

            
        }

        [HttpGet]
        public IActionResult PublishQuestionnaire(int questionnaireId)
        {

            return View(modMgr.GetModule(questionnaireId, false, true));
        }

        [HttpGet]
        public IActionResult EditQuestionnaire(int questionnaireId)
        {
            Questionnaire q = (Questionnaire) modMgr.GetModule(questionnaireId, false, true);
            return View(q);
        }






        public IActionResult UpdateProject()
        {
            return null;
        }

        public IActionResult RemoveProject()
        {
            return null;
        }


        //Dont know if neccessary 
        public IActionResult HideProject()
        {
            return null;
        }



    }
}
