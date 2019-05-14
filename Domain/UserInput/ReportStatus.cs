namespace Domain.UserInput
{
    public enum ReportStatus : byte
    {
        StatusNotViewed = 0,
        StatusNeedAdmin = 1,
        StatusApproved = 2,
        StatusDenied = 3
    }
}
