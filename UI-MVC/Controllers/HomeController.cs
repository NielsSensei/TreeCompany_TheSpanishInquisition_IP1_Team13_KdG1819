using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIMVC.Models;

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
