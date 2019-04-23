using System.Collections.Generic;
using System.Linq;
using BL;
using Domain.UserInput;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers
{
    public class ModerationController : Controller
    {
        private IdeationQuestionManager _ideaMgr;

        public ModerationController()
        {
            _ideaMgr = new IdeationQuestionManager();
        }
        
        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpGet]
        [Authorize]
        public IActionResult CollectAllIdeas()
        {
            return View();
        }

        public JsonResult CollectIdeas(string filter)
        {
            List<Idea> ideas = new List<Idea>();
            
            switch (filter)
            {
                case null: ideas = _ideaMgr.GetIdeas(); break; 
                case "admin": ideas = _ideaMgr.GetIdeas().FindAll(i => i.ReviewByAdmin); break;
                case "report": ideas = _ideaMgr.GetIdeas().FindAll(i => !i.ReviewByAdmin && i.Reported); break;
            }

            return Json(ideas);
        }
    }
}