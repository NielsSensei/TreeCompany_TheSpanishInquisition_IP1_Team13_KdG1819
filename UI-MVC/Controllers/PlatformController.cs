using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace UIMVC.Controllers
{
    
    public class PlatformController : Controller
    {
        private PlatformManager _platformMgr;

        public PlatformController()
        {
            _platformMgr = new PlatformManager();
        }

        [Route("platform/{id}")]
        public IActionResult Index(int id)
        {
            Domain.Users.Platform platform = _platformMgr.GetPlatform(id);
            if (platform == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);
            }
            return View(platform);
        }
    }
}