using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.UserInput
{
    public class OpenAnswer : Answer
    {
        // Added by NG
        // Modified by EKT & DM & NVZ
        /*
        OPEN = 0 --> DIT TYPE
        SINGLE = 1,
        MULTI = 2,
        DROP = 3,
        MAIL = 4  --> DIT TYPE
        -NVZ
        */
        public bool IsUserEmail { get; set; }
        public string AnswerText { get; set; }
    }
}
