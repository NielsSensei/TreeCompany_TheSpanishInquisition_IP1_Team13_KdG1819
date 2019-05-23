namespace Domain.Users
{
    /*
     * @authors David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
     */
    public enum Role
    {
        Anonymous     = 1,
        LoggedIn      = 2,
        LoggedInVerified = 3,
        LoggedInOrg   = 4,
        Moderator     = 5,
        Admin         = 6,
        SuperAdmin    = 7
    }
}
