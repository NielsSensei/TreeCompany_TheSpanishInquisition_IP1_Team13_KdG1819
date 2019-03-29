using System.Collections.Generic;
using Domain.UserInput;

namespace Domain
{
    public class Questionnaire : Module
    {
        // Added by NG
        public int UserCount { get; set; }
        public ICollection<QuestionnaireQuestion> Questions { get; set; }
        
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
                NumberOfVotes = this.NumberOfVotes,
                NumberOfShares = this.NumberOfShares,
                NumberOfRetweets = this.NumberOfRetweets
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