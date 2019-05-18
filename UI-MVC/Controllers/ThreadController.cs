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
using UIMVC.Models;
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

            if (fieldStrings != null)
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
    }
}