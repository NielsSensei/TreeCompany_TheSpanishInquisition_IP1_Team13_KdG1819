namespace DAL.Data_Access_Objects
{
    public class IdeationsDao
    {
        public int ModuleId { get; set; }
        public string UserId { get; set; }
        public string ExtraInfo { get; set; }
        public bool Organisation { get; set; }
        public int EventId { get; set; }
        public bool UserIdea { get; set; }
        public byte[] MediaFile { get; set; }
        public byte RequiredFields { get; set; }
    }
}
