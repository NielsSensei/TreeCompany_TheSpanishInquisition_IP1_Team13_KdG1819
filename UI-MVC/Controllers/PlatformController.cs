using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Domain.Identity;
using Domain.Projects;
using Domain.UserInput;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UIMVC.Models;
using UIMVC.Services;

namespace UIMVC.Controllers
{
    public class PlatformController : Controller
    {
        private readonly PlatformManager _platformMgr;
        private readonly ProjectManager _projectMgr;
        private readonly IdeationQuestionManager _iqMgr;
        private readonly UserManager<UimvcUser> _userManager;
        private readonly RoleService _roleService;

        public PlatformController(UserManager<UimvcUser> userManager, RoleService service)
        {
            _platformMgr = new PlatformManager();
            _projectMgr = new ProjectManager();
            _iqMgr = new IdeationQuestionManager();
            _userManager = userManager;
            _roleService = service;
        }

        [Route("Platform/{id}")]
        public IActionResult Index(int id, string message)
        {
            Platform platform = _platformMgr.GetPlatform(id, true);
            if (platform == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 
                    new { statuscode = 404, path="/Platform/" + id  });
            }

            if (message != null)
            {
                ViewData["StatusMessage"] = message;  
            }
            
            return View(platform);
        }

        #region Platform

        public IActionResult Search(string search)
        {
            ViewData["search"] = search;
            var platforms = _platformMgr.SearchPlatforms(search);
            return View(platforms);
        }

        #endregion

        #region Change

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> ChangePlatform(int id)
        {
            if (User.IsInRole(Role.Admin.ToString("G")) &&
                (await _userManager.GetUserAsync(User)).PlatformDetails != id)
                return BadRequest("You are no admin of this platform");

            Platform platform = _platformMgr.GetPlatform(id, false);
            if (platform == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 
                    new { statuscode = 404, path="/Platform/ChangePlatform/" + id });
            }

            ViewData["platform"] = platform;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> ChangePlatform(AddPlatformModel platformEdit, int platformId)
        {
            Platform platform = new Platform()
            {
                Id = platformId,
                Name = platformEdit.Name,
                Url = platformEdit.Url
            };

            if (platformEdit.IconImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await platformEdit.IconImage.CopyToAsync(memoryStream);
                    platform.IconImage = memoryStream.ToArray();
                }
            }

            if (platformEdit.CarouselImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await platformEdit.CarouselImage.CopyToAsync(memoryStream);
                    platform.CarouselImage = memoryStream.ToArray();
                }
            }

            if (platformEdit.FrontPageImage != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await platformEdit.FrontPageImage.CopyToAsync(memoryStream);
                    platform.FrontPageImage = memoryStream.ToArray();
                }
            }

            _platformMgr.EditPlatform(platform);
            return RedirectToAction("ChangePlatform", new {id = platform.Id});
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> AssignAdmin(string usermail, int platformId)
        {
            UimvcUser user = await _userManager.FindByEmailAsync(usermail);
            Platform platform = _platformMgr.GetPlatform(platformId, true);
            if (user == null) return BadRequest("Can't find user");
            if (platform == null) return BadRequest("Can't find platform");

            user.PlatformDetails = platformId;
            await _userManager.UpdateAsync(user);

            return Ok(user);
        }

        #endregion

        #region Project

        [HttpGet]
        public IActionResult CollectProject(int id)
        {
            Project project = _projectMgr.GetProject(id, true);

            if (project.Visible && project.Id != 0 || _roleService.IsAdmin(HttpContext.User).Result)
            {
                List<Phase> phases = (List<Phase>) _projectMgr.GetAllPhases(id);

                foreach (Phase phase in phases)
                {
                    if (phase.Id == project.CurrentPhase.Id)
                    {
                        project.CurrentPhase = phase;
                    }
                }

                phases.Remove(project.CurrentPhase);
                ViewData["Phases"] = phases;

                return View(project);
            }

            if (!project.Visible)
            {
                return RedirectToAction("Index", "Platform", new
                {
                    id = project.Platform.Id, message = "Project is niet ingesteld voor het algemene publiek!"
                });
            }

            
            return RedirectToAction("HandleErrorCode", "Errors", 
                new { statuscode = 404, path="/Platform/CollectProject/" + id  });
        }

        #endregion

        #region Ideation

        public IActionResult CollectIdeation(int id)
        {
            Ideation ideation = _projectMgr.ModuleMan.GetIdeation(id, true);

            return View(ideation);
        }

        public IActionResult CollectIdeationThread(int id, string message)
        {
            IdeationQuestion iq = _iqMgr.GetQuestion(id, true);

            ViewData["Message"] = message;
            ViewData["IdeationQuestion"] = iq;

            return View(iq);
        }
        
        #endregion

        #region Idea

        [Authorize]
        public IActionResult AddVote(int idea, string user, int thread)
        {
            if (_iqMgr.MakeVote(idea, user))
            {
                return RedirectToAction("CollectIdeationThread", "Platform", routeValues: new
                    { id = thread, message = "Stem gelukt, dankjewel!" });
            }

            return RedirectToAction("CollectIdeationThread", "Platform", routeValues: new
                {id = thread, message = "Al gestemd op dit idee!"});
        }

        [Authorize]
        public IActionResult AddReport(int idea, string flagger, int thread)
        {
            Idea ToReport = _iqMgr.GetIdea(idea, false);

            Report report = new Report()
            {
                Idea = ToReport,
                Flagger = new UimvcUser() {Id = flagger},
                Reportee = new UimvcUser() {Id = ToReport.User.Id},
                Status = ReportStatus.StatusNotViewed
            };

            if (!Request.Form["Reason"].ToString().Equals(""))
            {
                Report alreadyReport = _iqMgr.GetAllReportsByIdea(idea).FirstOrDefault(
                    r => r.Flagger.Id.Equals(flagger));

                if (alreadyReport == null)
                {
                    report.Reason = Request.Form["Reason"].ToString();
                    _iqMgr.MakeReport(report);

                    ToReport.Reported = true;
                    _iqMgr.EditIdea(ToReport);

                    return RedirectToAction("CollectIdeationThread", "Platform", routeValues: new
                        {id = thread, message = "Idee geraporteerd!"});
                }

                return RedirectToAction("CollectIdeationThread", "Platform", routeValues: new
                    {id = thread, message = "Je oude rapport is nog in de behandeling"});
            }

            return RedirectToAction("CollectIdeationThread", "Platform", routeValues: new
                {id = thread, message = "Je hebt geen reden opgegeven voor je rapport!"});
        }
        
        #endregion


    }
}
