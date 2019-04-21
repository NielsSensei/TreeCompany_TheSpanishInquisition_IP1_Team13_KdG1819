using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers
{
    public class ModerationController : Controller
    {
        //TODO: Voeg hier een ROLE toe zodat je niet via de link hier geraakt!
        public IActionResult CollectIdeas()
        {
            return View();
        } 
    }
}