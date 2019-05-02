using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    public class UserActivitiesDTO
    {
        public int ActivityID { get; set; }
        public int UserID { get; set; }
        public int PlatformID { get; set; }
        public int? ProjectID { get; set; }
        public int? VoteID { get; set; }
        public int? ModuleID { get; set; }
        public int? IQuestionID { get; set; }
        public int? IdeaID { get; set; }
        public string ActionDescription { get; set; }
    }
}
