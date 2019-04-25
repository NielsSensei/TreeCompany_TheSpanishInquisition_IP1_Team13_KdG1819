using BL;
using Domain.UserInput;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers
{
    public class ModerationController : Controller
    {
        private readonly IdeationQuestionManager _ideaMgr;
        private readonly UserManager _usrMgr;

        public ModerationController()
        {
            _ideaMgr = new IdeationQuestionManager();
            _usrMgr = new UserManager();
        }
        
        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpGet]
        [Authorize]
        public IActionResult CollectAllIdeas()
        {
            return View(_ideaMgr.GetIdeas());
        }

        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpGet]
        [Authorize]
        public IActionResult CollectIdea(int id)
        {
            Idea idea = _ideaMgr.GetIdea(id);
            if (idea.Visible)
            {
                idea.User  = _usrMgr.GetUser(idea.User.Id, false);
                ViewData["Reports"] = _ideaMgr.GetAllReportsByIdea(id);
            
                return View(idea);  
            }

            return RedirectToAction(controllerName: "Errors", actionName: "HandleErrorCode", routeValues: id);
        }

    }
}