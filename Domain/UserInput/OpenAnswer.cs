namespace Domain.UserInput
{
    /*
     * @authors Nathan Gijselings, David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
     */
    public class OpenAnswer : Answer
    {
        public bool IsUserEmail { get; set; }
        public string AnswerText { get; set; }
    }
}
