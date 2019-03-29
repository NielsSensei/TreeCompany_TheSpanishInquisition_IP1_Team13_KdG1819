using System.Collections.Generic;

namespace Domain.UserInput
{
    public class SingleAnswer : Answer
    {
        // Added by NG
        public List<string> Options { get; set; }
        public string Choice { get; set; }
        public bool DropDownList { get; set; }
        
        // Added by EKT
        // Methods
        #region

        public void AddOption(string option)
        {
            Options.Add(option);
        }
        #endregion
    }
}