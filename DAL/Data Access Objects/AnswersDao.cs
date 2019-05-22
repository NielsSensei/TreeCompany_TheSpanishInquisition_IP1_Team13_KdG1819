namespace DAL.Data_Access_Objects
{
    /*
     * @authors Sacha Buelens, Niels Van Zandbergen & Xander Veldeman
     * @documentation Niels Van Zandbergen
     *
     * Persistentie van een Answer. AnswerText wordt gevuld als het type QuestionnaireQuestion OPEN of MAIL is anders
     * wordt het gepersisteerd binnen Options & Choices.
     *
     * @see Domain.UserInput.Answer
     * @see DAL.Data_Access_Objects.ChoicesDao
     * @see DAL.Data_Access_Objects.OptionsDao
     * @see Domain.UserInput.MultipleAnswer
     * @see Domain.UserInput.OpenAnswer
     * @see Domain.UserInput.QuestionnaireQuestion
     * 
     */
    public class AnswersDao
    {
        public int AnswerId { get; set; }
        public int QQuestionId { get; set; }
        public string UserId { get; set; }
        public string AnswerText { get; set; }
    }
}
