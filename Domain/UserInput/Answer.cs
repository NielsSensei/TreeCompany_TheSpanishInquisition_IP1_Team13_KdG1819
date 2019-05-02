using Domain.Identity;
using Domain.Users;

namespace Domain.UserInput
{
    public class Answer
    {
        // Added by NG
        // Modified by XV, EKT & NVZ
        public int Id { get; set; }
        public UIMVCUser User { get; set; }      
        public QuestionnaireQuestion Question { get; set; }
    }
}