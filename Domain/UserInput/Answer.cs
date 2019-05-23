using Domain.Identity;

namespace Domain.UserInput
{
    /*
     * @authors Nathan Gijselings, David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
     */
    public class Answer
    {
        public int Id { get; set; }
        public UimvcUser User { get; set; }
        public QuestionnaireQuestion Question { get; set; }
    }
}
