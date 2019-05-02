using BL;
using Domain.Projects;
using Domain.UserInput;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMVC.Controllers
{
    //Created by SB on 26/04/2019
    public class AdminController : Controller
    {
        //Managers (possibly) needed
        private ModuleManager modMgr { get; set; }
        private ProjectManager projMgr { get; set; }
        private QuestionnaireQuestionManager qqMgr { get; set; }
        

         

        //Listing some basic methods that map to the functionalities described in YouTrack
        [HttpGet]
        public IActionResult CreateQuestionnaire(int projectId)
        {
            ViewData["project"] = projMgr.GetProject(projectId, true);
            ViewData["questionnaire"] = new Questionnaire();


            return View();
        }

        [HttpGet]
        public IActionResult CreateQuestionnaire(int projectId, int questionnaireId)
        {
            ViewData["project"] = projMgr.GetProject(projectId, true);
            ViewData["questionnaire"] = modMgr.GetModule(questionnaireId, true, true);


            return View();
        }


        [HttpPost]
        public IActionResult CreateQuestionnaire(Module questionnaire, int projectId, int phaseId)
        {

            Questionnaire newQuestionnaire = new Questionnaire
            {
                Project = projMgr.GetProject(projectId, false),
                
                UserCount = 0,
                Questions = new List<QuestionnaireQuestion>()
            };
            return RedirectToAction("AddQuestionnaireQuestion",newQuestionnaire.Id);
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





        public IActionResult CreateNewProject()
        {
            return null;
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
