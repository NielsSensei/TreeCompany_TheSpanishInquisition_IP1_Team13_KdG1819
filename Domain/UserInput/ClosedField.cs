using System.Collections.Generic;

namespace Domain.UserInput
{
    public class ClosedField : Field
    {
        // Added by NG
        public bool MultipleChoice { get; set; }
        public ICollection<string> Options { get; set; }
        public ICollection<string> Answer { get; set; }
        
        // Added by EKT
        // Modified by NVZ
        // Methods
        #region

        public void AddOption(string option)
        {
            Options.Add(option);
        }

        public void AddAnswer(string answer)
        {
            Answer.Add(answer);
        }
        
        #endregion
    }
}