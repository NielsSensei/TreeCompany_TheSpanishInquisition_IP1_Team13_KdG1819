namespace DAL.Data_Access_Objects
{
    /*
     * @authors Sacha Buelens, Niels Van Zandbergen & Xander Veldeman
     */
    public class ProjectsDao
    {
        public int ProjectId { get; set; }
        public int CurrentPhaseId { get; set; }
        public string UserId { get; set; }
        public int PlatformId { get; set; }
        public string Title { get; set; }
        public string Goal { get; set; }
        public string Status { get; set; }
        public bool Visible { get; set; }
        public int ReactionCount { get; set; }
        public int LikeCount { get; set; }
        public int FbLikeCount { get; set; }
        public int TwitterLikeCount { get; set; }
        public int LikeVisibility { get; set; }
    }
}
