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




        [HttpPost]
        public IActionResult AddQuestionnaireQuestion(int questionnaireid, CreateQuestionnaireQuestionModel cqqm)
        {
           

            Questionnaire toAdd = (Questionnaire) modMgr.GetModule(questionnaireid, false, true);

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



            if (cqqm.QuestionType.Equals(QuestionType.DROP) || cqqm.QuestionType.Equals(QuestionType.MULTI) || cqqm.QuestionType.Equals(QuestionType.SINGLE) )
            {
                foreach (string option in cqqm.Options)
                {
                    newQuestion.Options.Add(option);
                }
            }

           

           
            qqMgr.MakeQuestion(newQuestion, toAdd.Id);
            modMgr.EditModule(toAdd);

            return RedirectToAction("EditQuestionnaire", new { questionnaireId = toAdd.Id});

            

            
        }

        [HttpGet]
        public IActionResult EditQuestionnaire(int questionnaireId)
        {
            Questionnaire q = (Questionnaire) modMgr.GetModule(questionnaireId, false, true);

            List<Phase> availablePhases = new List<Phase>();
            Phase parentPhase = projMgr.GetPhase(q.ParentPhase.Id);
            List<QuestionnaireQuestion> questions = qqMgr.GetAllByModuleId(questionnaireId).ToList();
            foreach (QuestionnaireQuestion question in questions)
            {
                question.Answers = qqMgr.GetAnswers(question.Id);
            }



            foreach (Phase phase in projMgr.GetAllPhases(q.Project.Id).ToList())
            {
                if (modMgr.GetModule(phase.Id, q.Project.Id) == null)
                {
                    availablePhases.Add(phase);
                }
            }

            q.Project.Phases = availablePhases.ToList();
            q.ParentPhase = parentPhase;
            q.Questions = questions;

            ViewData["Project"] = q.Project;
            ViewData["Questionnaire"] = q;
            ViewData["Cqqm"] = new CreateQuestionnaireQuestionModel();
            return View();
        }

        [HttpPost]
        public IActionResult EditQuestionnaire(EditQuestionnaireModel eqm, int questionnaireid)
        {
            Questionnaire toBeUpdated = (Questionnaire)modMgr.GetModule(questionnaireid, false, true);

            Phase parentPhase = new Phase();
            String parentPhaseContent = Request.Form["ParentPhase"];

            if (!parentPhaseContent.Equals(""))
            {
                parentPhase = projMgr.GetPhase(Int32.Parse(Request.Form["ParentPhase"].ToString()));
                parentPhase.Module = toBeUpdated;

                Phase previousParent = projMgr.GetPhase(toBeUpdated.ParentPhase.Id);
                previousParent.Module = null;
                                
                toBeUpdated.ParentPhase = parentPhase;
                projMgr.EditPhase(previousParent);

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
            
            
            modMgr.EditModule(toBeUpdated);

            return RedirectToAction("EditQuestionnaire", new { questionnaireId = questionnaireid});
        }

        [HttpPost]
        public IActionResult DeleteQuestionnaire(int questionnaireid)
        {
            Questionnaire questionnaire = (Questionnaire)modMgr.GetModule(questionnaireid, false, true);

            

            Phase parentPhase = projMgr.GetPhase(questionnaire.ParentPhase.Id);
            parentPhase.Module = null;
            projMgr.EditPhase(parentPhase);

            List<QuestionnaireQuestion> questions = qqMgr.GetAllByModuleId(questionnaireid);

            foreach (QuestionnaireQuestion q in questions)
            {
                if(q.QuestionType == QuestionType.DROP || q.QuestionType == QuestionType.MULTI || q.QuestionType == QuestionType.SINGLE)
                {
                    List<string> options = qqMgr.GetAllOptionsForQuestion(q.Id).ToList();

                    foreach (string option in options)
                    {
                        int optionId = qqMgr.GetOptionId(option, q.Id);
                        qqMgr.DestroyOption(optionId);
                    }
                }

                qqMgr.RemoveQuestion(q.Id);
            }

            modMgr.RemoveModule(questionnaireid, questionnaire.Project.Id, true);





            return RedirectToAction("CollectProject", "Platform", new { id = questionnaire.Project.Id});
        }

        [HttpGet]
        public IActionResult DeleteQuestionnaireQuestion(int questionid)
        {
            QuestionnaireQuestion toDelete = qqMgr.GetQuestion(questionid, true);

            if(toDelete.QuestionType == QuestionType.DROP || toDelete.QuestionType == QuestionType.MULTI || toDelete.QuestionType == QuestionType.SINGLE)
            {
                List<string> options = qqMgr.GetAllOptionsForQuestion(toDelete.Id).ToList();

                if(options.Count != 0)
                {
                    foreach (string option in options)
                    {
                        qqMgr.DestroyOption(qqMgr.GetOptionId(option, questionid));
                    }
                }

                



            }
            qqMgr.RemoveQuestion(questionid);
            return RedirectToAction("EditQuestionnaire", new { questionnaireid = toDelete.Module.Id });
        }

        [HttpPost]
        public IActionResult EditQuestionnaireQuestion(int questionid)
        {
            throw new NotImplementedException("Work In Progress");
        }

        [HttpGet]
        public IActionResult AnswerQuestionnaire(int questionnaireid)
        {
            Questionnaire questionnaire = (Questionnaire) modMgr.GetModule(questionnaireid, true, true);

            List<QuestionnaireQuestion> questions = qqMgr.GetAllByModuleId(questionnaireid);

            foreach (QuestionnaireQuestion qQ in questions)
            {
                if (qQ.QuestionType == QuestionType.DROP || qQ.QuestionType == QuestionType.MULTI || qQ.QuestionType == QuestionType.SINGLE)
                {
                    qQ.Options = qqMgr.GetAllOptionsForQuestion(qQ.Id).ToList();
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
