using Domain.Projects;

namespace Domain.UserInput
{
    /*
     * @authors Nathan Gijselings, David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
     */
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public Module Module { get; set; }
    }
}
