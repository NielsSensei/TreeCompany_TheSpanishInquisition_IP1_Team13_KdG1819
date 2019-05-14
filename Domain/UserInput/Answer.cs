using Domain.Identity;

namespace Domain.UserInput
{
    public class Answer
    {
        public int Id { get; set; }
        public UimvcUser User { get; set; }
        public QuestionnaireQuestion Question { get; set; }
    }
}
