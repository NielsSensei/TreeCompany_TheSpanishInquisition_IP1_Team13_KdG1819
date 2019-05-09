using Domain.Users;

namespace UIMVC.Models
{
    public class AssignRoleModel
    {
        public string UserId { get; set; }
        public Role Role { get; set; }
    }
}