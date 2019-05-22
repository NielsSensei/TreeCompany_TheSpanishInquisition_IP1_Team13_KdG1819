namespace DAL.Data_Access_Objects
{
    /*
     * @author Nathan Gijselings
     * @documentation Niels Van Zandbergen
     *
     * ChoicesDao is een representatie van een de table Choices en een tussentabel tussen Options en Answers. Dit is
     * om redundantie te voorkomen dat voor elke keuze de tekst ervan wordt bijgehouden in de Choices Tabel.
     *
     * @see DAL.Data_Access_Objects.AnswersDao
     * @see DAL.Data_Access_Objects.OptionsDao
     */
    public class ChoicesDao
    {
        public int ChoiceId { get; set; }
        public int AnswerId { get; set; }
        public int OptionId { get; set; }
    }
}
