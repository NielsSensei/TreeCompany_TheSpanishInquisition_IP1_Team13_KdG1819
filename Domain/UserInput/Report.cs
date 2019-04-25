using Domain.Users;

namespace Domain.UserInput
{
    public class Report
    {
        // Added by NVZ
        public int Id { get; set; }
        public Idea Idea { get; set; }
        public User Flagger { get; set; }
        public User Reportee { get; set; }
        public string Reason { get; set; }
        public ReportStatus Status { get; set; }
    }
}