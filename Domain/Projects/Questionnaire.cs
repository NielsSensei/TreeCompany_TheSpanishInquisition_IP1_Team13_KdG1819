using System.Collections.Generic;
using Domain.UserInput;

namespace Domain.Projects
{
    public class Questionnaire : Module
    {
        // Added by NG
        public int UserCount { get; set; }
        public List<QuestionnaireQuestion> Questions { get; set; }
        
        // Added by EKT
        // Modified by NVZ
        // Methods
        #region
        public Questionnaire GetQuestionnaireInfo()
        {
            Questionnaire info = new Questionnaire()
            {
                UserCount = this.UserCount,
                OnGoing = this.OnGoing,
                LikeCount = this.LikeCount,
                ShareCount = this.ShareCount,
                RetweetCount = this.RetweetCount
            };
            return info;
        }

        public void AddQuestion(QuestionnaireQuestion question)
        {
            Questions.Add(question);
        }
        #endregion
    }
}