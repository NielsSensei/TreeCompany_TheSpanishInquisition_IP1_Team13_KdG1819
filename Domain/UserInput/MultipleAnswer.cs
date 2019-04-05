using System.Collections.Generic;

namespace Domain.UserInput
{
    public class MultipleAnswer : Answer
    {
        // Added by NG
        public List<string> Options { get; set; }
        public string CustomOption { get; set; }
        public List<string> Choices { get; set; }
        public bool DropdownList { get; set; }

        // Added by EKT
        // Modified by NVZ
        // Methods

        #region

        public void AddOption(string option)
        {
            Options.Add(option);
        }

        public void SetCustomOption(string option)
        {
            CustomOption = option;
        }
        #endregion
    }
}