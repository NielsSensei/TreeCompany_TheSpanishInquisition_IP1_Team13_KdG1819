namespace DAL.Data_Access_Objects
{
    public class ReportsDao
    {
        public int ReportId{ get; set; }
        public int IdeaId { get; set; }
        public string FlaggerId { get; set; }
        public string ReporteeId { get; set; }
        public string Reason { get; set; }
        public int ReportApproved { get; set; }
    }
}