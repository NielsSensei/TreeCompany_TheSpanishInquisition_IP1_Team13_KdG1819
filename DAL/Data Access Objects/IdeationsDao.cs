namespace DAL.Data_Access_Objects
{
    /*
     * @authors Sacha Buelens, Niels Van Zandbergen & Xander Veldeman
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
    }
}
