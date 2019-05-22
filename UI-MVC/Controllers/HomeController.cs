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
         * @Author Xander Veldeman
         */
        public IActionResult Index()
        {
            var platforms = _platformMgr.ReadAllPlatforms();

            return View(platforms);
        }

        /**
         * @Author Xander Veldeman
         */
        public IActionResult About()
        {
            return View();
        }

        /**
         * @Author Xander Veldeman
         */
        public IActionResult Privacy()
        {
            return View();
        }

        /**
         * @Author Niels Van Zandbergen
         */
        public IActionResult FAQ()
        {
            return View();
        }
    }
}
