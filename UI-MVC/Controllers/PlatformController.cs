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

        [Route("Platform/{id}")]
        public IActionResult Index(int id)
        {
            Domain.Users.Platform platform = _platformMgr.GetPlatform(id);
            if (platform == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);
            }
            return View(platform);
        }

        #region Create

        public IActionResult EditPlatform(int id)
        {
            Domain.Users.Platform platform = _platformMgr.GetPlatform(id);
            if (platform == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);
            }
            return View(platform);
        }

        [HttpPost]
        public IActionResult EditPlatform(Domain.Users.Platform platform)
        {
            _platformMgr.EditPlatform(platform);
            // TODO: make the redirect work
            return RedirectToAction("Index", new {id = platform.Id});
        }

        #endregion
    }
}