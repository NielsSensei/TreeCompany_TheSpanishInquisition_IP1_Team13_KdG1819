using Domain.Projects;

namespace Domain.UserInput
{
    public class Question
    {
        // Added by NG
        // Modified by EKT & NVZ & DM
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public Module Module { get; set; }
    }
}