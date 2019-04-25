using System.Collections.Generic;
using BL;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;
using UIMVC.Models;

namespace UIMVC.Controllers
{
    public class ModerationController : Controller
    {
        private IdeationQuestionManager _ideaMgr;
        private PlatformManager _platformMgr;

        public ModerationController()
        {
            _ideaMgr = new IdeationQuestionManager();
            _platformMgr = new PlatformManager();
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
            ViewData["platforms"] = _platformMgr.ReadAllPlatforms();
            return View();
        }

        [HttpPost]
        public IActionResult CreatePlatform(CreatePlatformModel cpm)
        {
            if (cpm == null)
            {
                return BadRequest("Platform cannot be null");
            }
            Platform platform = new Platform()
            {
                Id = _platformMgr.GetNextAvailableId(),
                Name = cpm.Name,
                Url = cpm.Url,
                Owners = new List<User>(),
                Users = new List<User>()
            };
            
            _platformMgr.MakePlatform(platform);
            
            return RedirectToAction("Index", "Platform", new {Id = platform.Id} );
        }
    }
}