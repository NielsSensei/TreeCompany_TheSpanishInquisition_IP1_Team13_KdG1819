using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Domain.Identity;
using Domain.Projects;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UIMVC.Models;

namespace UIMVC.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectManager _projManager;
        private ModuleManager _modManager;
        private readonly UserManager<UimvcUser> _userManager;


         public ProjectController(UserManager<UimvcUser> userManager)
        {
            _modManager = new ModuleManager();
            _projManager = new ProjectManager();
            _userManager = userManager;
        }

         #region Project

         #region Add

        [Authorize(Roles ="Admin, SuperAdmin")]
        [HttpGet]
        public IActionResult AddProject(int platform)
        {
            ViewData["platform"] = platform;
            return View();
        }

        [Authorize(Roles ="Admin, SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> AddProject(CreateProjectModel pvm, int platform, string user)
        {
            if (pvm == null)
            {
                return BadRequest("Project cannot be null");
            }

            Project pr = new Project()
            {
                User = new UimvcUser(){ Id = user },
                CurrentPhase = pvm.CurrentPhase,
                Title = pvm.Title,
                Platform = new Platform() {Id = platform},
                Status = pvm.Status.ToUpper(),
                LikeVisibility = 1,
                Goal = pvm.Goal,
                Visible = pvm.Visible
            };

             Project newProj = _projManager.MakeProject(pr);

             foreach (IFormFile file in pvm.InitialProjectImages)
             {
                 using (var memoryStream = new MemoryStream())
                 {
                     await file.CopyToAsync(memoryStream);
                     _projManager.MakeProjectImage(memoryStream.ToArray(), newProj.Id);
                 }
             }

             return RedirectToAction("Index", "Platform", new {id = platform });
        }

        [Authorize(Roles ="Admin, SuperAdmin")]
        public async Task<IActionResult> AddImage(IFormFile file, int projectId)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                _projManager.MakeProjectImage(memoryStream.ToArray(), projectId);
            }

            return RedirectToAction("CollectProject", "Platform",
                new {id = projectId});
        }
         #endregion


         #region ChangeProject

        [Authorize(Roles ="Admin, SuperAdmin")]
        [HttpGet]
        public IActionResult ChangeProject(int id)
        {
            Project project = _projManager.GetProject(id, false);

             if (project == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);
            }

            ViewData["Project"] = project;
            return View();
        }

        [Authorize(Roles ="Admin, SuperAdmin")]
        [HttpPost]
        public ActionResult ChangeProject(EditProjectModel epm, int id)
        {
            Project updateProj = _projManager.GetProject(id, false);

            updateProj.Title = epm.Title;
            updateProj.Goal = epm.Goal;
            updateProj.Visible = epm.Visible;
            updateProj.Status = epm.Status.ToUpper();


             _projManager.EditProject(updateProj);
            return RedirectToAction("CollectProject", "Platform", new {id = updateProj.Id});
        }

         #endregion


         #region DestroyProject

        [Authorize(Roles ="Admin, SuperAdmin")]
        [HttpGet]
        public IActionResult DestroyProject(int id)
        {
            Project project = _projManager.GetProject(id, false);
            int platformId = project.Platform.Id;
            project.Phases = (List<Phase>) _projManager.GetAllPhases(project.Id);

            List<Ideation> ideations = (List<Ideation>) _modManager.GetIdeations(project.Id);
            List<Questionnaire> questionnaires = (List<Questionnaire>) _modManager.GetQuestionnaires(project.Id);

            if (ideations.Count != 0)
            {
                foreach (Ideation ideation in ideations)
                {
                    _modManager.RemoveModule(ideation.Id, false);
                }
            }

            if (questionnaires.Count != 0)
            {
                foreach (Questionnaire questionnaire in questionnaires)
                {
                    _modManager.RemoveModule(questionnaire.Id, true);
                }
            }

            if (project.Phases.Count != 0)
            {
                foreach (var phase in project.Phases)
                {
                    _projManager.RemovePhase(phase.Id);
                }
            }

            _projManager.RemoveImagesForProject(project.Id);

            _projManager.RemoveProject(id);


             return RedirectToAction("Index", "Platform", new {id = platformId});
        }

         #endregion

         #endregion

         #region phase

         #region AddPhase

         [Authorize(Roles ="Admin, SuperAdmin")]

         [HttpGet]
        public IActionResult AddPhase(int projectId)
        {
            ViewData["project"] = projectId;

             return View();
        }


         [Authorize(Roles ="Admin, SuperAdmin")]
        [HttpPost]
        public IActionResult AddPhase(PhaseModel pm, int projectId)
        {
            if (pm == null)
            {
                return BadRequest("Phase cannot be null");
            }

            Phase p = new Phase()
            {
                Project = _projManager.GetProject(projectId, false),
                Description = pm.Description,
                StartDate = pm.StartDate,
                EndDate = pm.EndDate
            };

             _projManager.MakePhase(p);

             return RedirectToAction("CollectProject", "Platform", new {id = projectId});
        }

         #endregion


         #region ChangePhase

         [Authorize(Roles ="Admin, SuperAdmin")]
        [HttpGet]
        public IActionResult ChangePhase(int phaseId)
        {
            Phase phase = _projManager.GetPhase(phaseId);


             if (phase == null)
            {
                return RedirectToAction("HandleErrorCode", "Errors", 404);
            }

            ViewData["Phase"] = phase;
            return View();
        }

         [Authorize(Roles ="Admin, SuperAdmin")]
        [HttpPost]
        public IActionResult ChangePhase(PhaseModel pm, int phaseId)
        {
            Phase updatePhase = _projManager.GetPhase(phaseId);

             updatePhase.Description = pm.Description;
            updatePhase.StartDate = pm.StartDate;
            updatePhase.EndDate = pm.EndDate;

             _projManager.EditPhase(updatePhase);
            return RedirectToAction("CollectProject", "Platform", new {id = updatePhase.Project.Id});
        }

         #endregion



         [Authorize(Roles ="Admin, SuperAdmin")]
        [HttpGet]
        public IActionResult SetCurrentPhase(int projectId, int phaseId)
        {
            Project p = _projManager.GetProject(projectId, true);

             Phase ph = _projManager.GetPhase(phaseId);

             p.CurrentPhase = ph;

             _projManager.EditProject(p);

             return RedirectToAction("CollectProject", "Platform", new {id = projectId});
        }

         #region DestroyPhase


        [Authorize(Roles ="Admin, SuperAdmin")]
        [HttpGet]
        public IActionResult DestroyPhase(int phaseId, int projectId)
        {
            _projManager.RemovePhase(phaseId);

             return RedirectToAction("CollectProject", "Platform", new {id = projectId});
        }

         #endregion

         #endregion
    }
}
