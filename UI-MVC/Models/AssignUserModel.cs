namespace UIMVC.Models
{
    /**
     * @Author Xander Veldeman
     */
    public class AssignUserModel
    {
        public string UserMail { get; set; }
        public int PlatformId { get; set; }
        public AssignUserRole Role { get; set; }
    }

    public enum AssignUserRole
    {
        ADMIN = 1,
        MODERATOR
    }
}