using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    public class ProjectsDTO
    {
        public int ProjectID { get; set; }
        public int CurrentPhaseID { get; set; }
        public int UserID { get; set; }
        public int PlatformID { get; set; }
        public string Title { get; set; }
        public string Goal { get; set; }
        public string Status { get; set; }
        public bool Visible { get; set; }
        public int ReactionCount { get; set; }
        public int LikeCount { get; set; }
        public int FbLikeCount { get; set; }
        public int TwitterLikeCount { get; set; }
        public int LikeVisibility { get; set; }
    }
}
