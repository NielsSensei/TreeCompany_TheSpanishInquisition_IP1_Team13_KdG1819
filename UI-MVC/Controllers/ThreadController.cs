using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers
{
    public class ThreadController : Controller
    {
        [Authorize]
        public IActionResult AddIdea(int ideationQuestion, string user)
        {

            return RedirectToAction("CollectIdeationThread", "Platform", new { Id = ideationQuestion });
        }
    }
}