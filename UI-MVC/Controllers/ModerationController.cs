using System.Collections.Generic;
using BL;
using Domain.UserInput;
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
        public IActionResult CollectAllIdeas()
        {
            return View(_ideaMgr.GetIdeas());
        } 
    }
}