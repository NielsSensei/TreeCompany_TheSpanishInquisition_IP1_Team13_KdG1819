using BL;
using Domain.Identity;
using Domain.UserInput;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace UIMVC.Controllers
{
    public class ThreadController : Controller
    {
        private readonly IdeationQuestionManager _iqMgr;

        public ThreadController()
        {
            _iqMgr = new IdeationQuestionManager();
        }
        
        [Authorize]
        public IActionResult AddIdea(int ideationQuestion, string user, int parent)
        {
            Idea idea = new Idea()
            {
                IsDeleted = false,
                IdeaQuestion = _iqMgr.GetQuestion(ideationQuestion, false),
                User = new UIMVCUser(){ Id = user},
                Reported = false,
                ReviewByAdmin = false,
                Visible = true,
                Status = "NIET GESELECTEERD"
            };

            if (parent != 0)
            {
                idea.ParentIdea = new Idea(){ Id = parent};
            }
            
            //TODO als verified dan voegt ge dit toe aan de idea.
            
            if (!Request.Form["newIdeaTitle"].ToString().Equals(null))
            {
                idea.Title = Request.Form["newIdeaTitle"].ToString();
            }

            if (!Request.Form["newIdeaField"].ToString().Equals(null))
            {
                Field field = new Field()
                {
                    Idea = idea    
                };  
                
                field.Text = Request.Form["newIdeaField"].ToString();
                field.TextLength = field.Text.Length;

                idea.Field = field;
            }

            //TODO dit met iq settings.
            if (idea.Field != null || idea.Cfield != null || idea.Mfield != null || idea.Vfield != null
                || idea.Ifield != null) 
            {
                _iqMgr.MakeIdea(idea);     
            }
            
            return RedirectToAction("CollectIdeationThread", "Platform", new { Id = ideationQuestion });
        }
    }
}