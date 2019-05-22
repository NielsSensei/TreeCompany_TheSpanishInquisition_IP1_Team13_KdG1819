namespace DAL.Data_Access_Objects
{
    /*
     * @authors Nathan Gijselings & Niels Van Zandbergen
     */
    public class IdeationsDao
    {
        public int ModuleId { get; set; }
        public string UserId { get; set; }
        public string ExtraInfo { get; set; }
        public bool Organisation { get; set; }
        public int EventId { get; set; }
        public bool UserVote { get; set; }
        public string MediaFile { get; set; }
        public byte RequiredFields { get; set; }
    }
}
