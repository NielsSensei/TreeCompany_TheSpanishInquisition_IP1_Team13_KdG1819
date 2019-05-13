namespace Domain.UserInput
{
    public enum ReportStatus : byte
    {
        STATUS_NOTVIEWED = 0,
        STATUS_NEEDADMIN = 1,
        STATUS_APPROVED = 2,
        STATUS_DENIED = 3
    }
}