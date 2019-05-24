namespace Domain.UserInput
{
    /*
     * @authors Nathan Gijselings, David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
     */
    public class Field
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int TextLength { get; set; }
        public Idea Idea{ get; set; }
    }
}
