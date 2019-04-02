using System.Collections.Generic;

namespace Domain.UserInput
{
    public class IdeationQuestion : Question
    {
        // Added by NG
        public int IdeationId { get; set; }

        // Modified by XV & NVZ
        public string Description { get; set; }
        public string SiteURL { get; set; }
        public List<Idea> Ideas { get; set; }

        public List<string> AcceptedAnswerTypes { get; set; }

        public string QuestionTitle { get; set; }
        public IOT_Device Device { get; set; }

        // Question about this property, how ironic: Is this property necessary at all because we can
        // get this class by accessing the centralQuestion property within Ideation ? - NVZ
        //public Ideation Ideation { get; set; }

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