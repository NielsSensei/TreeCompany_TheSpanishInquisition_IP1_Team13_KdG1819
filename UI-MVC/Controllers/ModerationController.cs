using BL;
using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers
{
    public class ModerationController : Controller
    {
        private IdeationQuestionManager _ideaMgr;
        private PlatformManager _platformMgr;

        public ModerationController()
        {
            _ideaMgr = new IdeationQuestionManager();
            //_platformMgr = new PlatformManager();
        }
        
        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        [HttpGet]
        public IActionResult CollectAllIdeas()
        {
            return View(_ideaMgr.GetIdeas());
        } 

        [HttpGet]
        public IActionResult CreatePlatform()
        {
            return View();
        }
    }
}