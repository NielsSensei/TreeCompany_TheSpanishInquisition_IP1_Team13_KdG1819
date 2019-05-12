using BL;
using Domain.Projects;
using Domain.UserInput;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using UIMVC.Models;

namespace UIMVC.Controllers
{
    public class AdminController : Controller
    {
        private ModuleManager ModMgr { get; }
        private ProjectManager ProjMgr { get; }
        private QuestionnaireQuestionManager QqMgr { get; }

        public AdminController()
        {
            ModMgr = new ModuleManager();
            ProjMgr = new ProjectManager();
            QqMgr = new QuestionnaireQuestionManager();
        }
        
        [HttpGet]
        [Authorize(Roles = "ADMIN, SUPERADMIN")]
        public IActionResult AddQuestionnaire(int projectId)
        {
            Project toAddQuestionnaireTo = ProjMgr.GetProject(projectId, true);

            List<Phase> availablePhases = new List<Phase>();

            foreach (Phase phase in ProjMgr.GetAllPhases(projectId).ToList())
            {
                if(ModMgr.GetQuestionnaire(phase.Id, projectId) == null)
                {
                    availablePhases.Add(phase);
                }
            }

            toAddQuestionnaireTo.Phases = availablePhases.ToList();

            ViewData["project"] = toAddQuestionnaireTo;


            return View();
        }


        [HttpPost]
        [Authorize(Roles = "ADMIN, SUPERADMIN")]
        public IActionResult AddQuestionnaire(CreateQuestionnaireModel cqm, int projectId)
        {
            if(cqm == null)
            {
                return BadRequest("Questionnaire cannot be NULL!");
            }

            Project questionnaireProject = ProjMgr.GetProject(projectId, false);
            Phase parentPhase = ProjMgr.GetPhase(Int32.Parse(Request.Form["ParentPhase"].ToString()));
            
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
                ModuleType = ModuleType.Questionnaire,
                Phases = new List<Phase>(),
                Tags = new List<string>(),
                UserCount = 0,
                Questions = new List<QuestionnaireQuestion>()
            };

            newQuestionnaire.Phases.Add(parentPhase);
            ModMgr.MakeQuestionnaire(newQuestionnaire);

            return RedirectToAction("EditQuestionnaire",new { questionnaireId = newQuestionnaire.Id});
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, SUPERADMIN")]
        public IActionResult AddQuestionnaireQuestion(int questionnaireid)
        {
            ViewData["Questionnaire"] = ModMgr.GetQuestionnaire(questionnaireid, false);
            return View(new QuestionnaireQuestion());
        }
        
        [HttpPost]
        [Authorize(Roles = "ADMIN, SUPERADMIN")]
        public IActionResult AddQuestionnaireQuestion(int questionnaireId, QuestionnaireQuestion qQ)
        {
            Questionnaire toAdd = ModMgr.GetQuestionnaire(questionnaireId, false);
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
            QqMgr.MakeQuestion(newQuestion, toAdd.Id);
            ModMgr.EditQuestionnaire(toAdd);

            return RedirectToAction("AddQuestionnaire", toAdd.Id);
        }

        [HttpGet]
        public IActionResult PublishQuestionnaire(int questionnaireId)
        {
            return null;
            //return View(modMgr.GetQuestionnaire(questionnaireId, false));
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, SUPERADMIN")]
        public IActionResult EditQuestionnaire(int questionnaireId)
        {
            Questionnaire q = ModMgr.GetQuestionnaire(questionnaireId, false);

            List<Phase> availablePhases = new List<Phase>();
            Phase parentPhase = ProjMgr.GetPhase(q.ParentPhase.Id);
            List<QuestionnaireQuestion> questions = QqMgr.GetAllByModuleId(questionnaireId).ToList();
            foreach (QuestionnaireQuestion question in questions)
            {
                question.Answers = QqMgr.GetAnswers(question.Id);
            }

            foreach (Phase phase in ProjMgr.GetAllPhases(q.Project.Id).ToList())
            {
                if (ModMgr.GetQuestionnaire(phase.Id, q.Project.Id) == null)
                {
                    availablePhases.Add(phase);
                }
            }

            q.Project.Phases = availablePhases.ToList();
            q.ParentPhase = parentPhase;
            q.Questions = questions;

            ViewData["Project"] = q.Project;
            ViewData["Questionnaire"] = q;
            return View();
        }

        [HttpPost]
        public IActionResult EditQuestionnaire(EditQuestionnaireModel eqm, int questionnaireid)
        {
            Questionnaire toBeUpdated = ModMgr.GetQuestionnaire(questionnaireid, false);

            Phase parentPhase = new Phase();
            String parentPhaseContent = Request.Form["ParentPhase"];

            if (!parentPhaseContent.Equals(""))
            {
                parentPhase = ProjMgr.GetPhase(Int32.Parse(Request.Form["ParentPhase"].ToString()));
                parentPhase.Module = toBeUpdated;

                Phase previousParent = ProjMgr.GetPhase(toBeUpdated.ParentPhase.Id);
                previousParent.Module = null;

                
                toBeUpdated.ParentPhase = parentPhase;
                ProjMgr.EditPhase(previousParent);

            }
            else
            {
                parentPhase = toBeUpdated.ParentPhase;
            }

            if(eqm.VoteLevel != null)
            {
                toBeUpdated.VoteLevel = eqm.VoteLevel;
            }
                             
            toBeUpdated.OnGoing = eqm.OnGoing;
            toBeUpdated.Title = eqm.Title;
            
            
            ModMgr.EditQuestionnaire(toBeUpdated);

            return RedirectToAction("EditQuestionnaire", new { questionnaireId = questionnaireid});
        }
    }
}
