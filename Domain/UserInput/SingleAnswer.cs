using System.Collections.Generic;

namespace Domain.UserInput
{
    public class SingleAnswer : Answer
    {
        // Added by NG
        public bool IsUserEmail { get; set; }
        public string OpenAnswer { get; set; }
    }
}