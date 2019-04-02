using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    class IdeasDTO
    {
        public int IdeaID { get; set; }
        public int iQuestionID { get; set; }
        public int UserID { get; set; }
        public bool Reported { get; set; }
        public bool ReviewByAdmin { get; set; }
        public bool Visible { get; set; }
        public int VoteCount { get; set; }
        public int RetweetCount { get; set; }
        public int ShareCount { get; set; }
        public string Status { get; set; }
        public bool VerifiedUser { get; set; }
        public int ParentID { get; set; }
    }
}
