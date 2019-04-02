
namespace Domain.UserInput
{
    public class Answer
    {
        // Added by NG
        // Modified by XV, EKT & NVZ
        public int Id { get; set; }
        public string OpenAnswer { get; set; }
        
        
        
        public bool IsUserEmail { get; set; }
        public int QuestionnaireQuestionID { get; set; }
        public bool Completed { get; set; }
    }
}