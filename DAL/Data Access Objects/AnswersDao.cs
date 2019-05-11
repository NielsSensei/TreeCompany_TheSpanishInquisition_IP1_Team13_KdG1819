namespace DAL.Data_Access_Objects
{
    public class AnswersDao
    {
        public int AnswerId { get; set; }
        public int QQuestionId { get; set; }
        public string UserId { get; set; }
        public string AnswerText { get; set; }
    }
}
