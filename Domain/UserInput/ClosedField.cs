using System.Collections.Generic;

namespace Domain.UserInput
{
    public class ClosedField : Field
    {
        public bool MultipleChoice { get; set; }
        public List<string> Options { get; set; }
    }
}