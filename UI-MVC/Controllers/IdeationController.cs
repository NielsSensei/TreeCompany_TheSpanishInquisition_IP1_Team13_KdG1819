using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Domain.Identity;
using Domain.Projects;
using Domain.UserInput;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using UIMVC.Models;
using UIMVC.Services;

namespace UIMVC.Controllers
{
    /*
     * @authors Niels Van Zandbergen & Xander Veldeman
     */
    public class IdeationController : Controller
    {
        private readonly IdeationQuestionManager _ideaMgr;
        private readonly ModuleManager _moduleMgr;
        private readonly ProjectManager _projMgr;
        private readonly UserManager<UimvcUser> _userManager;
        private readonly RoleService _roleService;

        public IdeationController(UserManager<UimvcUser> userManager, RoleService roleService)
        {
            _ideaMgr = new IdeationQuestionManager();
            _moduleMgr = new ModuleManager();
            _projMgr = new ProjectManager();
            _userManager = userManager;
            _roleService = roleService;
        }
        
        #region Ideation
        #region Add
        /*
         * @author Niels Van Zandbergen
         */
        [Authorize(Roles = "SuperAdmin, Moderator, Admin")]
        [HttpGet]
        public IActionResult AddIdeation(int project)
        {
            List<Phase> allPhases = (List<Phase>) _projMgr.GetAllPhases(project);
            List<Phase> availablePhases = new List<Phase>();

            foreach (Phase phase in allPhases)
            {
                if (_moduleMgr.GetIdeation(phase.Id, project) == null)
                {
                    availablePhases.Add(phase);
                }
            }

            if (availablePhases.Count == 0)
            {
                return BadRequest("No available phases");
            }

            ViewData["Phases"] = availablePhases;
            ViewData["Project"] = project;

            return View();
        }
        
        /*
         * @author Niels Van Zandbergen
         */
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        public IActionResult AddIdeation(AddIdeationModel aim, int project, string user)
        {
            if (aim == null)
            {
                return BadRequest("Ideation can't be null");
            }

            Ideation i = new Ideation()
            {
                Project = new Project() {Id = project},
                ParentPhase = new Phase() {Id = Int32.Parse(Request.Form["Parent"].ToString())},
                User = new UimvcUser(){Id = user},
                ModuleType = ModuleType.Ideation,
                UserVote = aim.UserVote,
                Title = aim.Title,
                OnGoing = true
            };
            
            IdeationSettings settings = new IdeationSettings()
            {
                Field = aim.Field,
                ClosedField = aim.ClosedField,
                MapField = aim.MapField,
                VideoField = aim.VideoField,
                ImageField = aim.ImageField
            };

            i.Settings = settings;
            i.Settings.Ideation = i;

            if (aim.ExtraInfo != null)
            {
                i.ExtraInfo = aim.ExtraInfo;
            }

            if (aim.MediaLink != null && aim.MediaLink.Contains("youtube.com/watch?v="))
            {
                i.MediaLink = "https://youtube.com/embed/" + aim.MediaLink.Split("=")[1].Split("&")[0];
            }

            _moduleMgr.MakeIdeation(i);

            return RedirectToAction("CollectProject", "Project", new {Id = project});
        }
        #endregion
        
        #region Change
        /*
         * @author Niels Van Zandbergen
         */
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public IActionResult ChangeIdeation(int id)
        {
            Ideation i = _moduleMgr.GetIdeation(id, true);

            ViewData["Project"] = i.Project.Id;

            List<Phase> allPhases = (List<Phase>) _projMgr.GetAllPhases(i.Project.Id);
            List<Phase> availablePhases = new List<Phase>();

            foreach (Phase phase in allPhases)
            {
                if (_moduleMgr.GetIdeation(phase.Id, i.Project.Id) == null)
                {
                    availablePhases.Add(phase);
                }
            }

            ViewData["Phases"] = availablePhases;
            ViewData["PhaseCount"] = availablePhases.Count;
            ViewData["Title"] = i.Title;
            ViewData["Parent"] = _projMgr.GetPhase(i.ParentPhase.Id, true);
            ViewData["Ideation"] = id;
            ViewData["ExtraInfo"] = i.ExtraInfo;
            ViewData["UserVote"] = i.UserVote;

            return View();
        }
        
        /*
         * @author Niels Van Zandbergen
         */
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        public IActionResult ConfirmChangeIdeation(int ideation, ChangeIdeationModel cim)
        {
            Ideation i = new Ideation()
            {
                Id = ideation,
                Title = cim.Title,
                ExtraInfo = cim.ExtraInfo,
                UserVote = cim.UserVote
            };
            
            IdeationSettings settings = new IdeationSettings()
            {
                Field = cim.Field,
                ClosedField = cim.ClosedField,
                MapField = cim.MapField,
                VideoField = cim.VideoField,
                ImageField = cim.ImageField
            };

            i.Settings = settings;
            i.Settings.Ideation = i;
            
            if(cim.MediaFile != null && cim.MediaFile.Contains("youtube.com/watch?v="))
            {
                i.MediaLink = "https://youtube.com/embed/" + cim.MediaFile.Split("=")[1].Split("&")[0];
            }

            try
            {
                if (Int32.Parse(Request.Form["ParentPhase"].ToString()) != 0)
                {
                    i.ParentPhase = _projMgr.GetPhase(Int32.Parse(Request.Form["ParentPhase"].ToString()), false);
                    _moduleMgr.EditIdeation(i);
                }

            }catch(FormatException e)
            {
                Ideation previous = _moduleMgr.GetIdeation(i.Id, false);
                i.ParentPhase = _projMgr.GetPhase(previous.ParentPhase.Id, true);
                _moduleMgr.EditIdeation(i);
            }

            return RedirectToAction("CollectIdeation", "Ideation", new {Id = ideation});
        }
        #endregion
        
        /*
         * @authors Niels Van Zandbergen & Xander Veldeman
         */
        public IActionResult CollectIdeation(int id)
        {
            Ideation ideation = _projMgr.ModuleMan.GetIdeation(id, true);

            return View(ideation);
        }
        
        /*
         * @author Niels Van Zandbergen
         */
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult DestroyIdeation(int id)
        {
            Ideation i = _moduleMgr.GetIdeation(id, false);
                     
            _moduleMgr.RemoveModule(id, false);
         
            return RedirectToAction("CollectProject", "Project", new { Id = i.Project.Id });
        }
        #endregion
        
        #region CentralQuestion
        /*
         * @author Niels Van Zandbergen
         */
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public IActionResult AddCentralQuestion(int ideation)
        {
            ViewData["Ideation"] = ideation;
            return View();
        }
        
        /*
         * @author Niels Van Zandbergen
         */
        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        public IActionResult AddCentralQuestion(AddIdeationQuestionModel ciqm, int ideation)
        {
            if (ciqm == null)
            {
                return BadRequest("IdeationQuestion can't be null");
            }

            IdeationQuestion iq = new IdeationQuestion()
            {
                Description = ciqm.Description,
                SiteUrl = ciqm.SiteUrl,
                QuestionTitle = ciqm.QuestionTitle,
                Ideation = new Ideation(){ Id = ideation }
            };

            _ideaMgr.MakeQuestion(iq, ideation);

            return RedirectToAction("CollectIdeation", "Ideation", new {Id = ideation});
        }
        
        /*
         * @authors Niels Van Zandbergen & Xander Veldeman
         */
        public IActionResult CollectIdeationThread(int id, string message)
        {
            IdeationQuestion iq = _ideaMgr.GetQuestion(id, true);

            ViewData["Message"] = message;
            ViewData["IdeationQuestion"] = iq;

            return View(iq);
        }
        #endregion
        
        #region Idea
        /*
         * @author Niels Van Zandbergen
         */
        [HttpGet]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public IActionResult CollectAllIdeas(string filter = "all")
        {
            List<Idea> ideas = new List<Idea>();

            switch (filter)
            {
                case "all": ideas = _ideaMgr.GetIdeas(); break;
                case "admin": ideas = _ideaMgr.GetIdeas().FindAll(i => i.ReviewByAdmin); break;
                case "report": ideas = _ideaMgr.GetIdeas().FindAll(i => !i.ReviewByAdmin && i.Reported); break;
            }

            return View(ideas);
        }
        
        /*
         * @author Niels Van Zandbergen
         */
        [HttpGet]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public IActionResult CollectIdeaDetails(int id)
        {
            Idea idea = _ideaMgr.GetIdea(id, true);
            idea.User = _userManager.Users.FirstOrDefault(u => u.Id.Equals(idea.User.Id));
            if (idea.Visible)
            {
                ViewData["Reports"] = _ideaMgr.GetAllReportsByIdea(id);

                return View(idea);
            }

            return RedirectToAction("HandleErrorCode", "Errors", new {statuscode = 404,
                path="/Moderation/CollectIdea/" + id });
        }
        
        /*
         * @author Niels Van Zandbergen
         */
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public IActionResult DestroyIdea(int idea, string from, int thread)
        {
            Idea toDelete = _ideaMgr.GetIdea(idea, false);
            toDelete.IsDeleted = true;

            _ideaMgr.EditIdea(toDelete);

            if (from.Equals("ModerationPanel"))
            {
                return RedirectToAction(controllerName: "Ideation", actionName: "CollectAllIdeas");
            }


            if (from.Equals("IdeationThread") && thread > 0)
            {
                return RedirectToAction("CollectIdeationThread", "Ideation",
                    new {Id = thread});
            }

            return RedirectToAction("HandleErrorCode", "Errors", 
                new { statuscode = 404, path="/Moderation/DestroyIdea/" + idea });
        }
        
        #region AddIdea
        /*
         * @authors Niels Van Zandbergen & Xander Veldeman
         */
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddIdea(string user, IFormCollection form, int ideation, int parent, List<string> fieldStrings)
        {
            Idea idea = new Idea()
            {
                IsDeleted = false,
                IdeaQuestion = _ideaMgr.GetQuestion(ideation, true),
                User = new UimvcUser(){ Id = user},
                Reported = false,
                ReviewByAdmin = false,
                Visible = true,
                Status = "NIET GESELECTEERD",
                Device = new IotDevice(){ Id = 0 },
                ParentIdea =  new Idea() { Id = 0 }
            };

            if (parent != 0)
            {
                idea.ParentIdea = new Idea(){ Id = parent };
            }

            idea.VerifiedUser = await _roleService.IsVerified(User);

            if (!Request.Form["Title"].ToString().Equals(""))
            {
                idea.Title = Request.Form["Title"].ToString();
            }
            else
            {
                return RedirectToAction("CollectIdeationThread", "Ideation",
                    new
                    {
                        Id = ideation, message = "Helaas hebben we een titel nodig voor een goed idee, anders is de kans" +
                                                 " groter dat het niet gelezen wordt!"
                    });
            }

            List<string> error = new List<string>();
            IdeationSettings settings = _moduleMgr.GetIdeation(idea.IdeaQuestion.Ideation.Id, true).Settings;
            
            if (!Request.Form["FieldText"].ToString().Equals(""))
            {
                Field field = new Field()
                {
                    Idea = idea
                };

                field.Text = Request.Form["FieldText"].ToString();
                field.TextLength = field.Text.Length;

                idea.Field = field;
            }
            else
            {
                if (settings.Field)
                {
                    error.Add("Tekst");
                }
            }

            if (!Request.Form["newIdeaMapX"].ToString().Equals("Eerste coordinaat") &&
                !Request.Form["newIdeaMapY"].ToString().Equals("Tweede coordinaat") &&  
                !Request.Form["newIdeaMapX"].ToString().Equals("") &&
                !Request.Form["newIdeaMapY"].ToString().Equals(""))
            {
                MapField field = new MapField()
                {    
                    Idea = idea
                };
                
                field.LocationX = Double.Parse(Request.Form["newIdeaMapX"].ToString().Replace(".", ","));
                field.LocationY = Double.Parse(Request.Form["newIdeaMapY"].ToString().Replace(".", ","));

                idea.Mfield = field;
            }
            else
            {
                if (settings.MapField)
                {
                    error.Add("Map");
                }
            }

            if (!Request.Form["FieldVideo"].ToString().Equals(""))
            {
                VideoField field = new VideoField()
                {
                    Idea = idea
                };

                field.VideoLink = "https://www.youtube.com/embed/" +
                                  Request.Form["FieldVideo"].ToString().Split("=")[1].Split("&")[0];
                idea.Vfield = field;
            }
            else
            {
                if (settings.VideoField)
                {
                    error.Add("Filmpje");
                }
            }

            if (form.Files.Any())
            {
                ImageField field = new ImageField()
                {
                    Idea = idea
                };
                
                using (var memoryStream = new MemoryStream())
                {
                    await form.Files[0].CopyToAsync(memoryStream);
                    field.UploadedImage = memoryStream.ToArray();
                }

                idea.Ifield = field;
            }
            else
            {
                if (settings.ImageField)
                {
                    error.Add("Foto");
                }
            }

            if (fieldStrings.Any())
            {
                ClosedField field = new ClosedField()
                {
                    Idea = idea,
                    Options = new List<string>()
                };
                
                foreach (string item in fieldStrings)
                {
                    field.Options.Add(item);
                }

                idea.Cfield = field;
            }
            else
            {
                if (settings.ClosedField)
                {
                    error.Add("Opsomming");
                }
            }

            if (!error.Any())
            {
                try
                {
                    _ideaMgr.MakeIdea(idea);
                }
                catch (DuplicateNameException e)
                {
                    return RedirectToAction("CollectIdeationThread", "Ideation",
                        new
                        {
                            Id = ideation, message = "Dit idee heeft iemand anders al eens " +
                                                     "bedacht, je kan er wel op reageren."
                        });
                }   
            }
            else
            {
                String myString = "";

                if (error.Any())
                {
                    for(int i = 0; i < error.Count; i++)
                    {
                        myString += error[i];
                        if (i != error.Count - 1) myString += ", ";
                    }
                }
                
                return RedirectToAction("CollectIdeationThread", "Ideation", 
                    new { Id = ideation, message = "Volgende items ontbreken: " + myString }); 
            }
            
            return RedirectToAction("CollectIdeationThread", "Ideation", new { Id = ideation});
        }
        #endregion
        
        #region ChangeIdea
        /*
         * @authors Niels Van Zandbergen & Xander Veldeman
         */
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeIdea(IFormCollection form, int ideation, int idea, List<string> fieldStrings)
        {
            Idea toEdit = _ideaMgr.GetIdea(idea, false);
            
            if (!Request.Form["FieldText"].ToString().Equals(""))
            {
                if (toEdit.Field != null && !toEdit.Field.Text.Equals(Request.Form["FieldText"].ToString()) || toEdit.Field == null)
                {
                    Field field = new Field() { Idea = toEdit };

                    field.Text = Request.Form["FieldText"].ToString();
                    field.TextLength = field.Text.Length;

                    toEdit.Field = field;
                }
            }

            if (!Request.Form["newIdeaMapX"].ToString().Equals("Eerste coordinaat") &&
                !Request.Form["newIdeaMapY"].ToString().Equals("Tweede coordinaat") &&  
                !Request.Form["newIdeaMapX"].ToString().Equals("") &&
                !Request.Form["newIdeaMapY"].ToString().Equals(""))
            {
                if(toEdit.Mfield != null && !toEdit.Mfield.LocationX.ToString().Equals(Request.Form["newIdeaMapX"].ToString()) && 
                   !toEdit.Mfield.LocationY.ToString().Equals(Request.Form["newIdeaMapY"].ToString()) || toEdit.Mfield == null)
                {
                    MapField field = new MapField() { Idea = toEdit };
                
                    field.LocationX = Double.Parse(Request.Form["newIdeaMapX"].ToString().Replace(".", ","));
                    field.LocationY = Double.Parse(Request.Form["newIdeaMapY"].ToString().Replace(".", ","));

                    toEdit.Mfield = field;
                }
            }

            if (!Request.Form["FieldVideo"].ToString().Equals("") && 
                Request.Form["FieldVideo"].ToString().Contains("youtube.com/watch?v="))
            {
                if (toEdit.Vfield != null && !toEdit.Vfield.VideoLink.Equals(Request.Form["FieldVideo"].ToString()) ||
                    toEdit.Vfield == null)
                {
                    VideoField field = new VideoField() { Idea = toEdit };

                    field.VideoLink = "https://www.youtube.com/embed/" + 
                                      Request.Form["FieldVideo"].ToString().Split("=")[1].Split("&")[0];
                    toEdit.Vfield = field;
                }
            }

            if (form.Files.Any())
            {
                ImageField field = new ImageField() { Idea = toEdit };
                
                using (var memoryStream = new MemoryStream())
                {
                    await form.Files[0].CopyToAsync(memoryStream);
                    field.UploadedImage = memoryStream.ToArray();
                }

                if (toEdit.Ifield != null && toEdit.Ifield.UploadedImage != field.UploadedImage || toEdit.Ifield == null)
                {
                    toEdit.Ifield = field; 
                }
            }

            if (fieldStrings.Any())
            {
                ClosedField field = new ClosedField() { Idea = toEdit, Options = new List<string>() };
                
                foreach (string item in fieldStrings)
                {
                    field.Options.Add(item);
                }

                toEdit.Cfield = field;
            }
            
            if (toEdit.Field != null || toEdit.Cfield != null || toEdit.Mfield != null || toEdit.Vfield != null
                || toEdit.Ifield != null)
            {
                _ideaMgr.EditIdea(toEdit);
            }
            else
            {
                return RedirectToAction("CollectIdeationThread", "Ideation", new
                {
                    Id = ideation, message = "Er is iets misgelopen waardoor je veranderingen niet zijn opgeslagen."
                });
            }

            return RedirectToAction("CollectIdeationThread", "Ideation", new { Id = ideation });
        }
        #endregion
        
        /*
         * @author Niels Van Zandbergen
         */
        [Authorize]
        public IActionResult AddVote(int idea, string user, int thread)
        {
            if (_ideaMgr.MakeVote(idea, user))
            {
                return RedirectToAction("CollectIdeationThread", "Ideation", routeValues: new
                    { id = thread, message = "Stem gelukt, dankjewel!" });
            }

            return RedirectToAction("CollectIdeationThread", "Ideation", routeValues: new
                {id = thread, message = "Al gestemd op dit idee!"});
        }
        #endregion
        
        #region Report
        /*
         * @author Niels Van Zandbergen
         */
        [HttpPost]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public IActionResult ReviewByAdmin(int idea, int  report)
        {
            Idea foundIdea = _ideaMgr.GetIdea(idea, false);
            Report foundReport = _ideaMgr.GetReport(report, false);

            foundIdea.ReviewByAdmin = true;
            foundReport.Status = ReportStatus.StatusNeedAdmin;

            _ideaMgr.EditIdea(foundIdea);
            _ideaMgr.EditReport(foundReport);

            return RedirectToAction(controllerName: "Ideation", actionName: "CollectAllIdeas", routeValues: "admin");
        }
        
        /*
         * @author Niels Van Zandbergen
         */
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult ApproveReport(int report)
        {
            Report foundReport = _ideaMgr.GetReport(report, false);

            foundReport.Status = ReportStatus.StatusApproved;

            _ideaMgr.EditReport(foundReport);

            return RedirectToAction(controllerName: "Ideation", actionName: "CollectAllIdeas", routeValues: "report");
        }

        /*
         * @author Niels Van Zandbergen
         */
        [HttpPost]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public IActionResult DenyReport(int report, int idea)
        {
            Report foundReport = _ideaMgr.GetReport(report, false);
            foundReport.Status = ReportStatus.StatusDenied;
            _ideaMgr.EditReport(foundReport);

            HandleRemainingReports(idea);

            return RedirectToAction(controllerName: "Ideation", actionName: "CollectAllIdeas", routeValues: "report");
        }

        /*
         * @author Niels Van Zandbergen
         */
        [HttpPost]
        [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
        public IActionResult DestroyReport(int report, int idea)
        {
            _ideaMgr.RemoveReport(report);

            HandleRemainingReports(idea);

            return RedirectToAction(controllerName: "Ideation", actionName: "CollectAllIdeas" , routeValues: "report");
        }
        
        /*
         * @author Niels Van Zandbergen
         */
        private void HandleRemainingReports(int idea)
        {
            IEnumerable<Report> remainingReports = _ideaMgr.GetAllReportsByIdea(idea);
            if (!remainingReports.Any())
            {
                Idea foundIdea = _ideaMgr.GetIdea(idea, false);
                foundIdea.ReviewByAdmin = false;
                foundIdea.Reported = false;

                _ideaMgr.EditIdea(foundIdea);
            }
        }
        
        /*
         * @author Niels Van Zandbergen
         */
        [Authorize]
        public IActionResult AddReport(int idea, string flagger, int thread)
        {
            Idea ToReport = _ideaMgr.GetIdea(idea, false);

            Report report = new Report()
            {
                Idea = ToReport,
                Flagger = new UimvcUser() {Id = flagger},
                Reportee = new UimvcUser() {Id = ToReport.User.Id},
                Status = ReportStatus.StatusNotViewed
            };

            if (!Request.Form["Reason"].ToString().Equals(""))
            {
                Report alreadyReport = _ideaMgr.GetAllReportsByIdea(idea).FirstOrDefault(
                    r => r.Flagger.Id.Equals(flagger));

                if (alreadyReport == null)
                {
                    report.Reason = Request.Form["Reason"].ToString();
                    _ideaMgr.MakeReport(report);

                    ToReport.Reported = true;
                    _ideaMgr.EditIdea(ToReport);

                    return RedirectToAction("CollectIdeationThread", "Ideation", routeValues: new
                        {id = thread, message = "Idee geraporteerd!"});
                }

                return RedirectToAction("CollectIdeationThread", "Ideation", routeValues: new
                    {id = thread, message = "Je oude rapport is nog in de behandeling"});
            }

            return RedirectToAction("CollectIdeationThread", "Ideation", routeValues: new
                {id = thread, message = "Je hebt geen reden opgegeven voor je rapport!"});
        }
        #endregion
    }
}