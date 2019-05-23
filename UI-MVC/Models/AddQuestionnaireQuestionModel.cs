using System.Collections.Generic;
using Domain.UserInput;

namespace UIMVC.Models
{
    public class AddQuestionnaireQuestionModel
    {
        public QuestionType QuestionType { get; set; }
        public bool Optional { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
    }
}