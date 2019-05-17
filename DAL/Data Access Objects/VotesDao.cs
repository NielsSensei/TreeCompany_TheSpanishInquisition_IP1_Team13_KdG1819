namespace DAL.Data_Access_Objects
{
    public class VotesDao
    {
        public int VoteId { get; set; }
        public int DeviceId { get; set; }
        public int InputId { get; set; }
        public byte InputType { get; set; }
        public string UserId { get; set; }
        public string UserMail { get; set; }
        public float? LocationX { get; set; }
        public float? LocationY { get; set; }
        public string Choices { get; set; }
    }
}
