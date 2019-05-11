using Domain.Projects;

namespace Domain.UserInput
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public Module Module { get; set; }
    }
}