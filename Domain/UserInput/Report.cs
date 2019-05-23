using Domain.Identity;

namespace Domain.UserInput
{
    /*
     * @author Niels Van Zandbergen
     */
    public class Report
    {
        public int Id { get; set; }
        public Idea Idea { get; set; }
        public UimvcUser Flagger { get; set; }
        public UimvcUser Reportee { get; set; }
        public string Reason { get; set; }
        public ReportStatus Status { get; set; }
    }
}
