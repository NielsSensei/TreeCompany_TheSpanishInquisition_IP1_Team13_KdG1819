using System.Collections.Generic;

namespace Domain.UserInput
{
    public class MultipleAnswer : Answer
    {
        // Added by NG
        // Modified by EKT & DM
        /*
        OPEN = 0 
        SINGLE = 1, --> DIT TYPE
        MULTI = 2, --> DIT TYPE
        DROP = 3, --> DIT TYPE
        MAIL = 4  
        -NVZ
        */
        public string CustomOption { get; set; }
        public bool DropdownList { get; set; }
        public List<string> Choices { get; set; }
        

        // Added by EKT
        // Modified by NVZ
        // Methods

        #region

        
        public void SetCustomOption(string option)
        {
            CustomOption = option;
        }
        #endregion
    }
}