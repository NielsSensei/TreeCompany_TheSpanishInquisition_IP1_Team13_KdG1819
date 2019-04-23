using System.Linq;
using BL;
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
        public IActionResult CollectAllIdeas(string filter)
        {
            if(filter.Contains("admin"))
            {
                return View(_ideaMgr.GetIdeas().Where(i => i.ReviewByAdmin));
            }else if (filter.Contains("report"))
            {
                return View(_ideaMgr.GetIdeas().Where(i => i.Reported && !i.ReviewByAdmin));
            }
            return View(_ideaMgr.GetIdeas());
        } 
    }
}