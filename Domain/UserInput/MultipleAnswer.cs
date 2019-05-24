using System.Collections.Generic;

namespace Domain.UserInput
{
    /*
     * @authors Nathan Gijselings, David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
     */
    public class MultipleAnswer : Answer
    {
        public string CustomOption { get; set; }
        public bool DropdownList { get; set; }
        public List<string> Choices { get; set; }
    }
}
