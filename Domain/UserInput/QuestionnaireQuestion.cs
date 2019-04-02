using System.Collections.Generic;
using Domain.Projects;

namespace Domain.UserInput
{
    public class QuestionnaireQuestion : Question
    {
        // Added by NG
        // Modified by XV & NVZ
        // Modified by NG
        public List<Answer> Answers { get; set; }
        public QuestionType QuestionType { get; set; }
        public bool Optional { get; set; }

        public Questionnaire Questionnaire { get; set; }
        // Please refer to IdeationQuestion.cs because my question is similiar to that one. - NVZ
        // public Questionnaire Questionnaire { get; set; }

        // Added by EKT
        // Modified by NVZ
        // Modified by NG
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