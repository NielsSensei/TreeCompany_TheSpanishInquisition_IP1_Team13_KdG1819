using System;
using System.Data;
using System.Threading.Tasks;
using BL;
using Domain.Identity;
using Domain.UserInput;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> AddIdea(int ideationQuestion, string user, int parent)
        {
            Idea idea = new Idea()
            {
                IsDeleted = false,
                IdeaQuestion = _iqMgr.GetQuestion(ideationQuestion, false),
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
                idea.ParentIdea = new Idea(){ Id = parent};
            }
            
            idea.VerifiedUser = await _roleService.IsVerified(User);

            if (!Request.Form["newIdeaTitle"].ToString().Equals(""))
            {
                idea.Title = Request.Form["newIdeaTitle"].ToString();
            }

            if (!Request.Form["newIdeaField"].ToString().Equals(""))
            {
                Field field = new Field()
                {
                    Idea = idea
                };

                field.Text = Request.Form["newIdeaField"].ToString();
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
                            Id = ideationQuestion, message = "Dit idee heeft iemand anders al eens " +
                                                             "bedacht, je kan er wel op reageren."
                        });
                }
            }

            return RedirectToAction("CollectIdeationThread", "Platform", new { Id = ideationQuestion });
        }
    }
}