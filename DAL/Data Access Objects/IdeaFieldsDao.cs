namespace DAL.Data_Access_Objects
{
    /*
     * @authors Sacha Buelens & Niels Van Zandbergen
     * @documentation Niels Van Zandbergen
     *
     * Voor Idee zijn er 5 verschillende Fields die elk een of meerdere kolommen bevatten binnen deze tabel hier een
     * korte opsomming voor welke Field overeenkomt met welke kolommen binnen de IdeaFields tabel en wat we hier
     * verwachten:
     * 
     * - De text van Field wordt gepersisteerd als FieldText.
     * - De opties van ClosedField wordt geconcateneerd met een bepaald formaat en opgeslagen onder FieldStrings.       
     * - De coordinaten van Mapfield worden gepersisteerd als LocationX en LocationY.
     * - ImageField wordt gepersisteerd als een bytearray die de image zelf bevat.
     * - VideoField verwacht een youtubelink die hij kan tonen in een embed youtube iframe.
     *
     * @see Domain.UserInput.ClosedField
     * @see Domain.UserInput.Field
     * @see Domain.UserInput.ImageField
     * @see Domain.UserInput.MapField
     * @see Domain.UserInput.VideoField
     */
    public class IdeaFieldsDao
    {
        public int FieldId { get; set; }
        public int IdeaId { get; set; }
        public string FieldText { get; set; }
        public string FieldStrings { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        public byte[] UploadedImage { get; set; }
        public string MediaLink { get; set; }
    }
}
