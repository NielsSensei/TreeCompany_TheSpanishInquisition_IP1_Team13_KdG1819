namespace Domain.UserInput
{
    /*
     * @author Niels Van Zandbergen
     */
    public enum ReportStatus : byte
    {
        StatusNotViewed = 0,
        StatusNeedAdmin = 1,
        StatusApproved = 2,
        StatusDenied = 3
    }
}
