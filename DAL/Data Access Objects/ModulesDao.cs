namespace DAL.Data_Access_Objects
{
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
        public bool IsQuestionnaire { get; set; }
    }
}
