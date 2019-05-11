using System.Collections.Generic;
using Domain.Users;

namespace Domain.Projects
{
    public class Module
    {
        public int Id { get; set; }
        public Project Project { get; set; }       
        public Phase ParentPhase { get; set; }
        public string Title { get; set; }
        public bool OnGoing { get; set; }
        public int LikeCount { get; set; }
        public int FbLikeCount{ get; set; }
        public int TwitterLikeCount { get; set; }
        public int ShareCount { get; set; }
        public int RetweetCount { get; set; }      
        public Role VoteLevel { get; set; }
        public ModuleType ModuleType { get; set; }
        public List<Phase> Phases { get; set; }
        public List<string> Tags { get; set; }
    }
}