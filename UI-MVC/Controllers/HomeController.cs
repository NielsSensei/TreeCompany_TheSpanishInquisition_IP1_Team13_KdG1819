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

        /**
         * @author Xander Veldeman
         */
        public IActionResult Index()
        {
            var platforms = _platformMgr.GetAllPlatforms();

            return View(platforms);
        }

        /**
         * @author Xander Veldeman
         */
        public IActionResult About()
        {
            return View();
        }

        /**
         * @author Xander Veldeman
         */
        public IActionResult Privacy()
        {
            return View();
        }

        /**
         * @author Niels Van Zandbergen
         */
        public IActionResult FAQ()
        {
            return View();
        }
    }
}
