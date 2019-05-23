namespace UIMVC.Services
{
    /**
     * @author Xander Veldeman
     *
     * Used for Sendgrid email verification
     */
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}
