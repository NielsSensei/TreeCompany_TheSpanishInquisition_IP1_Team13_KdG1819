using System.Collections.Generic;
using Domain.UserInput;

namespace Domain.Projects
{
    /*
     * @authors Nathan Gijselings, David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
     */
    public class Questionnaire : Module
    {
        public int UserCount { get; set; }
        public List<QuestionnaireQuestion> Questions { get; set; }
    }
}
