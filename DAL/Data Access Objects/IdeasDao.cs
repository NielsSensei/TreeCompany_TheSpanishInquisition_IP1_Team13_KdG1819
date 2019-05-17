namespace DAL.Data_Access_Objects
{
    public class IdeasDao
    {
        public int IdeaId { get; set; }
        public int IquestionId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public bool Reported { get; set; }
        public bool ReviewByAdmin { get; set; }
        public bool Visible { get; set; }
        public int VoteCount { get; set; }
        public int RetweetCount { get; set; }
        public int ShareCount { get; set; }
        public string Status { get; set; }
        public bool VerifiedUser { get; set; }
        public bool IsDeleted { get; set; }
        public int ParentId { get; set; }
        public int DeviceId { get; set; }
    }
}
