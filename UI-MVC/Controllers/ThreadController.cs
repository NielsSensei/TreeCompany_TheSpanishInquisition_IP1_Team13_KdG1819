using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Domain.Identity;
using Domain.UserInput;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UIMVC.Services;

namespace UIMVC.Controllers
{
    public class ThreadController : Controller
    {
        private readonly IdeationQuestionManager _iqMgr;
        private readonly RoleService _roleService;

        public ThreadController(RoleService roleService)
        {
            _iqMgr = new IdeationQuestionManager();
            _roleService = roleService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddIdea(string user, IFormCollection form, int ideation, int parent, List<string> fieldStrings)
        {
            Idea idea = new Idea()
            {
                IsDeleted = false,
                IdeaQuestion = _iqMgr.GetQuestion(ideation, false),
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
                return RedirectToAction("CollectIdeationThread", "Platform",
                    new
                    {
                        Id = ideation, message = "Helaas hebben we een titel nodig voor een goed idee, anders is de kans" +
                                                 " groter dat het niet gelezen wordt!"
                    });
            }

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
            
            if (idea.Field != null || idea.Cfield != null || idea.Mfield != null || idea.Vfield != null
                || idea.Ifield != null)
            {
                try
                {
                    _iqMgr.MakeIdea(idea);
                }
                catch (DuplicateNameException e)
                {
                    return RedirectToAction("CollectIdeationThread", "Platform",
                        new
                        {
                            Id = ideation, message = "Dit idee heeft iemand anders al eens " +
                                                             "bedacht, je kan er wel op reageren."
                        });
                }
            }

            return RedirectToAction("CollectIdeationThread", "Platform", new { Id = ideation});
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeIdea(IFormCollection form, int ideation, int idea, List<string> fieldStrings)
        {
            Idea toEdit = _iqMgr.GetIdea(idea);
            
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
                _iqMgr.EditIdea(toEdit);
            }
            else
            {
                return RedirectToAction("CollectIdeationThread", "Platform", new
                {
                    Id = ideation, message = "Er is iets misgelopen waardoor je veranderingen niet zijn opgeslagen."
                });
            }

            return RedirectToAction("CollectIdeationThread", "Platform", new { Id = ideation });
        }
    }
}