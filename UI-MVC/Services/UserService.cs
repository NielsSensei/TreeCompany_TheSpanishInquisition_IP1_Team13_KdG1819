using BL;

namespace UIMVC.Services
{
    public class UserService
    {
        private readonly UserManager _usrMgr;

        public UserService()
        {
            _usrMgr = new UserManager();
        }

        public string CollectUserName(int id)
        {
            return _usrMgr.GetUser(id, false).Name;
        }
    }
}