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
        private ModuleManager mMgr { get; set; }
        private ProjectManager pMgr { get; set; }
        private QuestionnaireQuestionManager qqMgr { get; set; }
        private UserManager uMgr { get; set; }

         

        //Listing some basic methods that map to the functionalities described in YouTrack
        [HttpGet]
        public IActionResult CreateQuestionnaire()
        {
            ViewData["projects"] = pMgr.GetAllProjectsForPlatform(1).AsEnumerable();

            return View();
        }

        [HttpPost]
        public IActionResult CreateQuestionnaire(Module questionnaire, Project project, List<QuestionnaireQuestion> qQuestions)
        {
            return RedirectToAction("");
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
