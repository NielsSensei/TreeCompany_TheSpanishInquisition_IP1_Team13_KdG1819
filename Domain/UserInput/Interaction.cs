using Domain.Users;

namespace Domain.UserInput
{
    public class Interaction
    {
        // Added by NG
        //TODO bespreek issue #22
        public User User { get; set; }
        public IOT_Device DeviceId { get; set; }
    }
}