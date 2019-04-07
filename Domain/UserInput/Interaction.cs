using Domain.Users;

namespace Domain.UserInput
{
    public class Interaction
    {
        // Added by NG
        public User User { get; set; }
        public IOT_Device DeviceId { get; set; }
    }
}