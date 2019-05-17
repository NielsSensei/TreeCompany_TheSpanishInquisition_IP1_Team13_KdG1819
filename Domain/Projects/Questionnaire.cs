using System.Collections.Generic;
using Domain.UserInput;

namespace Domain.Projects
{
    public class Questionnaire : Module
    {
        public int UserCount { get; set; }
        public List<QuestionnaireQuestion> Questions { get; set; }
    }
}
