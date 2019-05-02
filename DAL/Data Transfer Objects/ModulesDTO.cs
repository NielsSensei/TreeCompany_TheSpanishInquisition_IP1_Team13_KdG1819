using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    public class ModulesDTO
    {
        public int ModuleID { get; set; }
        public int ProjectID { get; set; }
        public int PhaseID { get; set; }
        public bool OnGoing { get; set; }
        public string Title { get; set; }
        public int LikeCount { get; set; }
        public int FbLikeCount { get; set; }
        public int TwitterLikeCount { get; set; }
        public int ShareCount { get; set; }
        public int RetweetCount { get; set; }
        public string Tags { get; set; }
        public bool IsQuestionnaire { get; set; }
    }
}
