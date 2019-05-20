using System.Collections.Generic;
using Domain.Projects;

namespace Domain.UserInput
{
    public class QuestionnaireQuestion : Question
    {
        public QuestionType QuestionType { get; set; }
        public bool Optional { get; set; }
        public Questionnaire Questionnaire { get; set; }
        public List<Answer> Answers { get; set; }
        public List<string> Options { get; set; }
    }
}
