namespace DAL.Data_Transfer_Objects
{
    public class ReportsDTO
    {
        public int ReportID{ get; set; }
        public int IdeaID { get; set; }
        public int FlaggerID { get; set; }
        public int ReporteeID { get; set; }
        public string Reason { get; set; }
        public int ReportApproved { get; set; }
    }
}