using System.Collections.Generic;
using Domain.Identity;
using Domain.UserInput;
using Domain.Users;

namespace Domain.Projects
{
    public class Ideation : Module
    {
        public UimvcUser User { get; set; }
        public bool UserIdea { get; set; }
        public Event Event{ get; set; }
        //public Media Media { get; set; }
        public string ExtraInfo { get; set; }
        public int RequiredFields { get; set; }
        public List<IdeationQuestion> CentralQuestions { get; set; }
    }
}
