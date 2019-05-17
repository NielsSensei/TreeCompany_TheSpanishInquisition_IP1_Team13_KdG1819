using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Domain.Projects;
using Domain.UserInput;
using Domain.Users;

namespace UIMVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private ModuleManager _modMan;
        private IdeationQuestionManager _idQuesMan;

        public HelloController()
        {
            _modMan = new ModuleManager();
            _idQuesMan = new IdeationQuestionManager();
        }

        //api/hello/GetIdeationQuestions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdeationQuestion>>> GetIdeationQuestions()
        {
            var questions = _idQuesMan.GetAll();

            if (questions == null)
            {
                return NotFound();
            }

            return questions;
        }


        /*
        // GET: api/hello
        //projectID
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Idea>>> GetIdeas()
        {
            return _idQuesMan.GetIdeas(1);
        }
*/


        //moduleId
        [HttpGet("{id}")]
        public async Task<ActionResult<Idea>> GetIdea(int id)
        {

            var idea = _idQuesMan.GetIdea(id);
            if (idea == null)
            {
                return NotFound();
            }

            return idea;
        }

        [HttpPost]
        public async Task<ActionResult<Idea>> PostIdea(Idea idea)
        {
            _idQuesMan.MakeIdea(idea);

            return CreatedAtAction(nameof(GetIdea), new { id = idea.Id }, idea);
        }
    }
}