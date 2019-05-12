using BL;
using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly PlatformManager _platformMgr;

        public HomeController()
        {
            _platformMgr = new PlatformManager();
        }

        public IActionResult Index()
        {
            var platforms = _platformMgr.ReadAllPlatforms();
            return View(platforms);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
