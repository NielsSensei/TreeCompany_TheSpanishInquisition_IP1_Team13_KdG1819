using System.Collections.Generic;

namespace Domain.UserInput
{
    public class ClosedField : Field
    {
        // Added by NG
        public bool MultipleChoice { get; set; }
        public List<string> Options { get; set; }
        
        // Added by EKT
        // Modified by NVZ
        // Methods
        #region

        public void AddOption(string option)
        {
            Options.Add(option);
        }
       
        #endregion
    }
}