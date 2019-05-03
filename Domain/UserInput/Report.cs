using System;
using Domain.Identity;
using Domain.Users;

namespace Domain.UserInput
{
    public class Report
    {
        // Added by NVZ
        public int Id { get; set; }
        public Idea Idea { get; set; }
        public UIMVCUser Flagger { get; set; }
        public UIMVCUser Reportee { get; set; }
        public string Reason { get; set; }
        public ReportStatus Status { get; set; }
    }
}