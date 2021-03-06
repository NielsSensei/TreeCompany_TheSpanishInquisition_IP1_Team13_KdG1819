﻿using BL;
using Domain.Projects;
using Domain.UserInput;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using UIMVC.Models;
using UIMVC.Services;

namespace UIMVC.Controllers
{
    /*
     * @authors Sacha Buelens & Xander Veldeman
     */
    public class QuestionnaireController : Controller
    {
        private ModuleManager ModMgr { get; }
        private ProjectManager ProjMgr { get; }
        private QuestionnaireQuestionManager QqMgr { get; }
        private UserManager<UimvcUser> _userManager;
        private UserService _userService;

        public QuestionnaireController(UserManager<UimvcUser> userManager, UserService userService)
        {
            ModMgr = new ModuleManager();
            ProjMgr = new ProjectManager();
            QqMgr = new QuestionnaireQuestionManager();
            _userManager = userManager;
            _userService = userService;
        }

        /**
         * @author Sacha Beulens
         */
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult AddQuestionnaire(int projectId)
        {
            Project toAddQuestionnaireTo = ProjMgr.GetProject(projectId, true);

            List<Phase> availablePhases = new List<Phase>();

            foreach (Phase phase in ProjMgr.GetAllPhases(projectId).ToList())
            {
                if (ModMgr.GetQuestionnaire(phase.Id, projectId) == null)
                {
                    availablePhases.Add(phase);
                }
            }

            toAddQuestionnaireTo.Phases = availablePhases.ToList();

            ViewData["project"] = toAddQuestionnaireTo;

            return View();
        }

        /**
         * @author Sacha Beulens
         */
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult AddQuestionnaire(AddQuestionnaireModel cqm, int projectId)
        {
            if (cqm == null)
            {
                return BadRequest("Questionnaire cannot be NULL!");
            }

            Project questionnaireProject = ProjMgr.GetProject(projectId, true);
            Phase parentPhase = ProjMgr.GetPhase(Int32.Parse(Request.Form["ParentPhase"].ToString()), false);

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

            return RedirectToAction("ChangeQuestionnaire", new {questionnaireId = newQuestionnaire.Id});
        }

        /**
         * @author Sacha Beulens
         */
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult AddQuestionnaireQuestion(int questionnaireId, AddQuestionnaireQuestionModel cqqm)
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

            if (cqqm.QuestionType.Equals(QuestionType.Drop) || cqqm.QuestionType.Equals(QuestionType.Multi) ||
                cqqm.QuestionType.Equals(QuestionType.Single))
            {
                foreach (string option in cqqm.Options)
                {
                    newQuestion.Options.Add(option);
                }
            }

            QqMgr.MakeQuestion(newQuestion, toAdd.Id);
            ModMgr.EditQuestionnaire(toAdd);

            return RedirectToAction("ChangeQuestionnaire", new {questionnaireId = toAdd.Id});
        }
        

        /**
         * @author Sacha Beulens
         */
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult ChangeQuestionnaire(int questionnaireId)
        {
            Questionnaire q = ModMgr.GetQuestionnaire(questionnaireId, false);

            List<Phase> availablePhases = new List<Phase>();
            Phase parentPhase = ProjMgr.GetPhase(q.ParentPhase.Id, true);
            List<QuestionnaireQuestion> questions = QqMgr.GetAllByModuleId(questionnaireId).ToList();
            foreach (QuestionnaireQuestion question in questions)
            {
                question.Answers = QqMgr.GetAnswers(question.Id);

                if (question.QuestionType == QuestionType.Drop || question.QuestionType == QuestionType.Multi ||
                    question.QuestionType == QuestionType.Single)
                {
                    question.Options = QqMgr.GetAllOptionsForQuestion(question.Id).ToList();
                }
                else question.Options = new List<string>();
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
            ViewData["Cqqm"] = new AddQuestionnaireQuestionModel();
            return View();
        }

        /**
         * @author Sacha Beulens, Xander Veldeman
         */
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult ChangeQuestionnaire(ChangeQuestionnaireModel eqm, int questionnaireid)
        {
            Questionnaire toBeUpdated = ModMgr.GetQuestionnaire(questionnaireid, false);

            Phase parentPhase = new Phase();
            String parentPhaseContent = Request.Form["ParentPhase"];

            if (!parentPhaseContent.Equals(""))
            {
                parentPhase = ProjMgr.GetPhase(Int32.Parse(Request.Form["ParentPhase"].ToString()), false);
                parentPhase.Module = toBeUpdated;

                Phase previousParent = ProjMgr.GetPhase(toBeUpdated.ParentPhase.Id, false);
                previousParent.Module = null;


                toBeUpdated.ParentPhase = parentPhase;
                ProjMgr.EditPhase(previousParent);
            }
            else
            {
                parentPhase = toBeUpdated.ParentPhase;
            }

            if (eqm.VoteLevel != null)
            {
                toBeUpdated.VoteLevel = eqm.VoteLevel;
            }

            toBeUpdated.OnGoing = eqm.OnGoing;
            toBeUpdated.Title = eqm.Title;

            ModMgr.EditQuestionnaire(toBeUpdated);

            return RedirectToAction("ChangeQuestionnaire", new {questionnaireId = questionnaireid});
        }

        /**
         * @author Sacha Beulens
         */
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult DeleteQuestionnaire(int questionnaireid)
        {
            Questionnaire questionnaire = ModMgr.GetQuestionnaire(questionnaireid, false);

            Phase parentPhase = ProjMgr.GetPhase(questionnaire.ParentPhase.Id, false);
            parentPhase.Module = null;
            ProjMgr.EditPhase(parentPhase);

            List<QuestionnaireQuestion> questions = QqMgr.GetAllByModuleId(questionnaireid);

            foreach (QuestionnaireQuestion q in questions)
            {
                if (q.QuestionType == QuestionType.Drop || q.QuestionType == QuestionType.Multi ||
                    q.QuestionType == QuestionType.Single)
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

            return RedirectToAction("CollectProject", "Project", new {id = questionnaire.Project.Id});
        }

        /**
         * @author Sacha Beulens
         */
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult DeleteQuestionnaireQuestion(int questionid)
        {
            QuestionnaireQuestion toDelete = QqMgr.GetQuestion(questionid, false);

            if (toDelete.QuestionType == QuestionType.Drop || toDelete.QuestionType == QuestionType.Multi ||
                toDelete.QuestionType == QuestionType.Single)
            {
                List<string> options = QqMgr.GetAllOptionsForQuestion(toDelete.Id).ToList();

                if (options.Count != 0)
                {
                    foreach (string option in options)
                    {
                        QqMgr.DestroyOption(QqMgr.GetOptionId(option, questionid));
                    }
                }
            }

            QqMgr.RemoveQuestion(questionid);
            return RedirectToAction("ChangeQuestionnaire", new {questionnaireid = toDelete.Module.Id});
        }
        
        #region AnswerQuestionnaire

        /**
         * @author Xander Veldeman
         */
        [HttpGet]
        public IActionResult AnswerQuestionnaire(int questionnaireid)
        {
            Questionnaire questionnaire = ModMgr.GetQuestionnaire(questionnaireid, false);

            List<QuestionnaireQuestion> questions = QqMgr.GetAllByModuleId(questionnaireid);

            foreach (QuestionnaireQuestion qQ in questions)
            {
                if (qQ.QuestionType == QuestionType.Drop || qQ.QuestionType == QuestionType.Multi ||
                    qQ.QuestionType == QuestionType.Single)
                {
                    qQ.Options = QqMgr.GetAllOptionsForQuestion(qQ.Id).ToList();
                }
                else qQ.Options = new List<string>();

                qQ.Answers = new List<Answer>();
            }

            questionnaire.Questions = questions;

            ViewData["Question"] = questionnaire.Questions[0];
            ViewData["Questionnaire"] = questionnaire;

            return View(new AddAnswerModel());
        }

        /**
         * @author Xander Veldeman
         */
        [HttpGet]
        public IActionResult NextQuestionnaire(int questionid, int questionnaireid, bool invalid = false)
        {
            Questionnaire questionnaire = ModMgr.GetQuestionnaire(questionnaireid, false);

            List<QuestionnaireQuestion> questions = QqMgr.GetAllByModuleId(questionnaireid);

            questionnaire.Questions = questions;

            foreach (QuestionnaireQuestion qQ in questions)
            {
                if (qQ.QuestionType == QuestionType.Drop || qQ.QuestionType == QuestionType.Multi ||
                    qQ.QuestionType == QuestionType.Single)
                {
                    qQ.Options = QqMgr.GetAllOptionsForQuestion(qQ.Id).ToList();
                }
                else qQ.Options = new List<string>();

                qQ.Answers = new List<Answer>();
            }

            var question = questions.FirstOrDefault(questionA => questionA.Id == questionid);

            ViewData["InValid"] = invalid;

            ViewData["Question"] = question;
            ViewData["Questionnaire"] = questionnaire;
            return View(new AddAnswerModel());
        }

        /**
         * @author Xander Veldeman
         * @documentation Xander Veldeman
         *
         * @params AddAnswerModel: voorziet een antwoord op een questionnaire met een OpenAnswer, MultipleAnswer of een
         * lijst van Checkbox antwoorden. Het model heeft zoizo een van deze 3 type properties. Indien het niet zo is,
         * worden de antwoorden invalide verklaart.
         *
         * @see UIMVC.Models.AddAnswerModel
         * @see Domain.UserInput.MultipleAnswer
         * @see Domain.UserInput.OpenAnswer
         * 
         */
        [HttpPost]
        public async Task<IActionResult> AnswerQuestionnaire(AddAnswerModel addAnswerModel, int questionId, int questionnaireId)
        {
            QuestionnaireQuestion question = QqMgr.GetQuestion(questionId, true);

            UimvcUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.GetUserAsync(User);
            }
            else
            {
                user = _userService.CollectAnonymousUser();
            }

            if (addAnswerModel.MultipleAnswer?.Choices != null && addAnswerModel.MultipleAnswer.Choices[0] != null)
            {
                if (!question.Optional)
                {
                    if (addAnswerModel.MultipleAnswer.Choices == null || !addAnswerModel.MultipleAnswer.Choices.Any())
                    {
                        return RedirectToAction("NextQuestionnaire",
                            new {questionId = questionId, questionnaireId = questionnaireId, invalid = true});
                    }
                }

                MultipleAnswer answer = new MultipleAnswer()
                {
                    Choices = addAnswerModel.MultipleAnswer.Choices,
                    DropdownList = addAnswerModel.MultipleAnswer.DropdownList,
                    Question = question,
                    User = user,
                    CustomOption = null
                };
                QqMgr.MakeAnswer(answer);
            }
            else if (addAnswerModel.OpenAnswer != null)
            {
                if (!question.Optional)
                {
                    if (addAnswerModel.OpenAnswer.AnswerText == null || !addAnswerModel.OpenAnswer.AnswerText.Any())
                    {
                        return RedirectToAction("NextQuestionnaire",
                            new {questionId = questionId, questionnaireId = questionnaireId, invalid = true});
                    }
                }

                OpenAnswer answer;
                if (addAnswerModel.OpenAnswer.AnswerText == null)
                {
                    answer = new OpenAnswer()
                    {
                        AnswerText = "EMPTY",
                        IsUserEmail = addAnswerModel.OpenAnswer.IsUserEmail,
                        Question = question,
                        User = user
                    };
                }
                else
                {
                    answer = new OpenAnswer()
                    {
                        AnswerText = addAnswerModel.OpenAnswer.AnswerText,
                        IsUserEmail = addAnswerModel.OpenAnswer.IsUserEmail,
                        Question = question,
                        User = user
                    };
                }

                QqMgr.MakeAnswer(answer);
            }
            else if (addAnswerModel.CheckboxAnswers != null)
            {
                if (!question.Optional)
                {
                    if ((addAnswerModel.CheckboxAnswers == null ||
                        addAnswerModel.CheckboxAnswers.All(checkboxAnswer => checkboxAnswer.Checked == false)) &&
                        (addAnswerModel.CustomAnswer == null ||
                         addAnswerModel.CustomAnswer.Checked && addAnswerModel.CustomAnswer.Value == null ||
                         addAnswerModel.CustomAnswer.Checked == false))
                    {
                        return RedirectToAction("NextQuestionnaire",
                            new {questionId = questionId, questionnaireId = questionnaireId, invalid = true});
                    }
                }

                MultipleAnswer answer = new MultipleAnswer()
                {
                    Choices = new List<string>(addAnswerModel.CheckboxAnswers.ToList()
                        .Where(checkboxAnswer => checkboxAnswer.Checked)
                        .Select(checkboxAnswer => checkboxAnswer.Value)),
                    DropdownList = false,
                    Question = question,
                    User = user
                };
                if (addAnswerModel.CustomAnswer.Checked)
                {
                    answer.CustomOption = addAnswerModel.CustomAnswer.Value;
                }
                QqMgr.MakeAnswer(answer);
            }
            else if (!question.Optional)
            {
                return RedirectToAction("NextQuestionnaire",
                    new {questionId = questionId, questionnaireId = questionnaireId, invalid = true});
            }

            var nextQuestion = GetNextQuestion(questionnaireId, questionId);
            if (nextQuestion == null)
                return RedirectToAction("FinishedQuestionnaire", new {questionnaireId = questionnaireId});
            return RedirectToAction("NextQuestionnaire",
                new {questionid = nextQuestion.Id, questionnaireid = questionnaireId});
        }


        /**
         * @author Xander Veldeman
         */
        private QuestionnaireQuestion GetNextQuestion(int questionnaireId, int questionId)
        {
            var questions = QqMgr.GetAllByModuleId(questionnaireId);
            var prevQuestion = QqMgr.GetQuestion(questionId, true);
            QuestionnaireQuestion question = null;
            try
            {
                question = questions[questions.FindIndex(questionA => questionA.Id == prevQuestion.Id) + 1];
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Console.WriteLine(exception);
            }

            return question;
        }

        /**
         * @author Xander Veldeman
         */
        public IActionResult FinishedQuestionnaire(int questionnaireId)
        {
            var questionnaire = ModMgr.GetQuestionnaire(questionnaireId, true);
            return View(questionnaire);
        }

        #endregion
    }
}