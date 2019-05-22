namespace DAL.Data_Access_Objects
{
    /*
     * @author Nathan Gijselings
     * @documentation Niels Van Zandbergen
     *
     * Bij de aanmaak van een QuestionnaireQuestion met een type SINGLE, DROP of MULTI duiden op een vraag met
     * Opties. Deze opties worden hier gepersisteerd los van de QuestionnaireQuestion omdat ze zowel van de
     * gebruiker zelf kunnen zijn als de maker van de Questionnaire. Dit is om te voorkomen dat we hele tijd
     * QuestionnaireQuestion een optie te laten toevoegen wanneer een gebruiker een bijvoegt. Zo is het ook
     * makkelijker om Opties van gebruikers te onderscheiden want deze worden natuurlijk niet meegetoond bij elke
     * Multiplechoice vraag.
     *
     * @see Domain.UserInput.Answer
     * @see Domain.UserInput.MultipleAnswer
     * @see Domain.UserInput.QuestionnaireQuestion
     * 
     */
    public class OptionsDao
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public int QquestionId { get; set; }
    }
}
