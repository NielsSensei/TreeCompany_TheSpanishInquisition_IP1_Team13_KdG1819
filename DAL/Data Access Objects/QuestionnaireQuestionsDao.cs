namespace DAL.Data_Access_Objects
{
    public class QuestionnaireQuestionsDao
    {
        public int QquestionId { get; set; }
        public int ModuleId { get; set; }
        public string QuestionText { get; set; }
        public byte QType { get; set; }
        public bool Required { get; set; }
    }
}
