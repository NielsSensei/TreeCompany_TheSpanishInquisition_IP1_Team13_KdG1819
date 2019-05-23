namespace DAL.Data_Access_Objects
{
    /*
     * @authors Sacha Buelens & Niels Van Zandbergen
     * @documenation Niels Van Zandbergen
     *
     * Omdat buiten deze properties Questionnaire geen extra properties heeft zoals Ideation wordt er geen extra
     * tabel voorzien voor Questionnaires terwijl dit voor Ideations wel gedaan is. Merk ook op dat Tags
     * geconcateneerd wordt net zoals FieldStrings in IdeaFieldsDao. Ze maken beiden gebruik van hetzelfde formaat.
     * ModuleType wordt van de enum geconverteerd naar zijn superklasse byte en zo ook opgeslagen.
     *
     * @see DAL.Data_Access_Objects.IdeaFieldsDao
     * @see DAL.Data_Access_Objects.IdeationsDao
     * @see Domain.Projects.Ideation
     * @see Domain.Projects.Module
     * @see Domain.Projects.ModuleType
     * @see Domain.Projects.Questionnaire
     * 
     */
    public class ModulesDao
    {
        public int ModuleId { get; set; }
        public int ProjectId { get; set; }
        public int PhaseId { get; set; }
        public bool OnGoing { get; set; }
        public string Title { get; set; }
        public int LikeCount { get; set; }
        public int FbLikeCount { get; set; }
        public int TwitterLikeCount { get; set; }
        public int ShareCount { get; set; }
        public int RetweetCount { get; set; }
        public string Tags { get; set; }
        public byte ModuleType { get; set; }
    }
}
