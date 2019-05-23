namespace DAL.Data_Access_Objects
{
    /*
     * @authors Sacha Buelens & Niels Van Zandbergen
     */
    public class UserActivitiesDao
    {
        public int ActivityId { get; set; }
        public string UserId { get; set; }
        public int PlatformId { get; set; }
        public int? ProjectId { get; set; }
        public int? VoteId { get; set; }
        public int? ModuleId { get; set; }
        public int? IquestionId { get; set; }
        public int? IdeaId { get; set; }
        public string ActionDescription { get; set; }
    }
}
