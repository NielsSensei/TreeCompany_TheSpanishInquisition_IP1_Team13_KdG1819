using Domain.Projects;
using System.Collections.Generic;

namespace Domain.UserInput
{
    public class IdeationQuestion : Question
    {
        public string Description { get; set; }
        public string SiteUrl { get; set; }
        public string QuestionTitle { get; set; }
        public Ideation Ideation { get; set; }
        public List<Idea> Ideas { get; set; }
        public List<string> AcceptedAnswerTypes { get; set; }
    }
}