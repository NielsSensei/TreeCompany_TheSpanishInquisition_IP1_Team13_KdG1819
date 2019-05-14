namespace DAL.Data_Transfer_Objects
{
    public class ReportsDTO
    {
        public int ReportID{ get; set; }
        public int IdeaID { get; set; }
        public string FlaggerID { get; set; }
        public string ReporteeID { get; set; }
        public string Reason { get; set; }
        public int ReportApproved { get; set; }
    }
}