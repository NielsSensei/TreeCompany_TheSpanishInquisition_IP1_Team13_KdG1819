using Domain.UserInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMVC.Models
{
    public class CreateQuestionnaireQuestionModel
    {
        public QuestionType QuestionType { get; set; }
        public bool Optional { get; set; }
        public string QuestionText { get; set; }
    }
}
