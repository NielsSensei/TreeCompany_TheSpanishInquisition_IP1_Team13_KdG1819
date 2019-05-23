namespace DAL.Data_Access_Objects
{
    /*
     * @author Niels Van Zandbergen
     * @documentation Niels Van Zandbergen
     *
     * ReportApproved is hier de persistentie van de enum ReportStatus, het getal komt overeen met de byte value van de enum.
     *
     * @see Domain.UserInput.Report
     */
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