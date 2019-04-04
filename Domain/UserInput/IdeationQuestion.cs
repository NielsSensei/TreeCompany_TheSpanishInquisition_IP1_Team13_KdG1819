using Domain.Projects;
using System.Collections.Generic;

namespace Domain.UserInput
{
    public class IdeationQuestion : Question
    {
        // Added by NG
        // Modified by XV & NVZ & EKT & DM
        public string Description { get; set; }
        public string SiteURL { get; set; }
        public string QuestionTitle { get; set; }
        public IOT_Device Device { get; set; }
        public Ideation Ideation { get; set; }

        public List<Idea> Ideas { get; set; }
        public List<string> AcceptedAnswerTypes { get; set; }

        // Added by EKT
        // Modified by NVZ
        // Methods

        #region

        public IdeationQuestion GetIdeationQuestionInfo()
        {
            IdeationQuestion info = new IdeationQuestion
            {
                Description = this.Description,
                SiteURL = this.SiteURL,
                Ideas = this.Ideas,
            };
            return info;
        }

        public void AddIdea(Idea idea)
        {
            Ideas.Add(idea);
        }

        #endregion
    }
}