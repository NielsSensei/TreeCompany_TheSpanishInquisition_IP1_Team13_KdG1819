using System.Collections.Generic;
using Domain.Projects;

namespace Domain.UserInput
{
    public class QuestionnaireQuestion : Question
    {
        // Added by NG
        // Modified by XV & NVZ & NG
        public QuestionType QuestionType { get; set; }
        public bool Optional { get; set; }
        public Questionnaire Questionnaire { get; set; }
        public List<Answer> Answers { get; set; }
        public List<string> Options { get; set; }

        // Added by EKT
        // Modified by NVZ & NG
        // Methods

        #region

        public void SetAnswer(Answer answer)
        {
            Answers.Add(answer);
        }

        public void MakeOptional()
        {
            Optional = true;
        }

        #endregion
    }
}