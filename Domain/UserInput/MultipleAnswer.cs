using System.Collections.Generic;

namespace Domain.UserInput
{
    public class MultipleAnswer : Answer
    {
        public string CustomOption { get; set; }
        public bool DropdownList { get; set; }
        public List<string> Choices { get; set; }
        public List<string> Options { get; set; }
    }
}
