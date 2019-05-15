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
        [Authorize(Roles = "Admin, SuperAdmin")]
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
        [Authorize(Roles = "Admin, SuperAdmin")]
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
                VoteLevel = Domain.Users.Role.Anonymous,
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

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult AddQuestionnaireQuestion(int questionnaireId, CreateQuestionnaireQuestionModel cqqm)
        {
            Questionnaire toAdd = ModMgr.GetQuestionnaire(questionnaireId, false);
            QuestionnaireQuestion newQuestion = new QuestionnaireQuestion
            {

                QuestionText = cqqm.QuestionText,
                QuestionType = cqqm.QuestionType,
                Module = toAdd,
                Questionnaire = toAdd,
                Optional = cqqm.Optional,
                Answers = new List<Answer>(),
                Options = new List<string>()
            };
            
            if (cqqm.QuestionType.Equals(QuestionType.Drop) || cqqm.QuestionType.Equals(QuestionType.Multi) || cqqm.QuestionType.Equals(QuestionType.Single) )
            {
                foreach (string option in cqqm.Options)
                {
                    newQuestion.Options.Add(option);
                }
            }
            
            QqMgr.MakeQuestion(newQuestion, toAdd.Id);
            ModMgr.EditQuestionnaire(toAdd);

            return RedirectToAction("EditQuestionnaire", new { questionnaireId = toAdd.Id});
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
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
            ViewData["Cqqm"] = new CreateQuestionnaireModel();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
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

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult DeleteQuestionnaire(int questionnaireid)
        {
            Questionnaire questionnaire = ModMgr.GetQuestionnaire(questionnaireid, false);
            
            Phase parentPhase = ProjMgr.GetPhase(questionnaire.ParentPhase.Id);
            parentPhase.Module = null;
            ProjMgr.EditPhase(parentPhase);
            
            List<QuestionnaireQuestion> questions = QqMgr.GetAllByModuleId(questionnaireid);

            foreach (QuestionnaireQuestion q in questions)
            {
                if(q.QuestionType == QuestionType.Drop || q.QuestionType == QuestionType.Multi || q.QuestionType == QuestionType.Single)
                {
                    List<string> options = QqMgr.GetAllOptionsForQuestion(q.Id).ToList();

                    foreach (string option in options)
                    {
                        int optionId = QqMgr.GetOptionId(option, q.Id);
                        QqMgr.DestroyOption(optionId);
                    }
                }

                QqMgr.RemoveQuestion(q.Id);
            }

            ModMgr.RemoveModule(questionnaireid, true);
            
            return RedirectToAction("CollectProject", "Platform", new { id = questionnaire.Project.Id});
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult DeleteQuestionnaireQuestion(int questionid)
        {
            QuestionnaireQuestion toDelete = QqMgr.GetQuestion(questionid, true);

            if(toDelete.QuestionType == QuestionType.Drop || toDelete.QuestionType == QuestionType.Multi || toDelete.QuestionType == QuestionType.Single)
            {
                List<string> options = QqMgr.GetAllOptionsForQuestion(toDelete.Id).ToList();

                if(options.Count != 0)
                {
                    foreach (string option in options)
                    {
                        QqMgr.DestroyOption(QqMgr.GetOptionId(option, questionid));
                    }
                }
            }
            QqMgr.RemoveQuestion(questionid);
            return RedirectToAction("EditQuestionnaire", new { questionnaireid = toDelete.Module.Id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult EditQuestionnaireQuestion(int questionid)
        {
            throw new NotImplementedException("Work In Progress");
        }
        
        [HttpGet]
        public IActionResult AnswerQuestionnaire(int questionnaireid)
        {
            Questionnaire questionnaire = ModMgr.GetQuestionnaire(questionnaireid, false);

            List<QuestionnaireQuestion> questions = QqMgr.GetAllByModuleId(questionnaireid);

            foreach (QuestionnaireQuestion qQ in questions)
            {
                if (qQ.QuestionType == QuestionType.Drop || qQ.QuestionType == QuestionType.Multi || qQ.QuestionType == QuestionType.Single)
                {
                    qQ.Options = QqMgr.GetAllOptionsForQuestion(qQ.Id).ToList();
                }
                else qQ.Options = new List<string>();

                qQ.Answers = new List<Answer>();
            }

            questionnaire.Questions = questions;
            
            return View(questionnaire);
        }

        [HttpPost]
        public IActionResult AnswerQuestionnaire(List<QuestionnaireQuestion> list)
        {
            return null;
        }
    }
}
