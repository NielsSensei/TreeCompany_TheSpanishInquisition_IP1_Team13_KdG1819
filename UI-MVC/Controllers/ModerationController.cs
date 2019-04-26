using BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers
{
    public class ModerationController : Controller
    {
        private IdeationQuestionManager _ideaMgr;
        private UserManager _userMgr;

        public ModerationController()
        {
            _ideaMgr = new IdeationQuestionManager();
            _userMgr = new UserManager();
        }
        
        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpGet]
        public IActionResult CollectAllIdeas()
        {
            return View(_ideaMgr.GetIdeas());
        }

        [HttpGet]
        [Authorize]
        public IActionResult CollectAllUsers()
        {
            return View(_userMgr.GetUsers());
        }
    }
}