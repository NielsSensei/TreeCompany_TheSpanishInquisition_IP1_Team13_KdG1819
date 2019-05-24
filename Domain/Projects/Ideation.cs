using System.Collections.Generic;
using Domain.Identity;
using Domain.UserInput;
using Domain.Users;

namespace Domain.Projects
{
    /*
     * @authors Nathan Gijselings, David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Niels Van Zandbergen
     */
    public class Ideation : Module
    {
        public UimvcUser User { get; set; }
        public bool UserVote { get; set; }
        public Event Event{ get; set; }
        public string MediaLink { get; set; }
        public string ExtraInfo { get; set; }
        public IdeationSettings Settings { get; set; }
        public List<IdeationQuestion> CentralQuestions { get; set; }
        
    }
}
