using System.Collections.Generic;
using Domain.Users;

namespace Domain.Projects
{
    public class Module
    {
        // Added by NG
        // Modified by EKT & NVZ & DM
        public int Id { get; set; }
        public Project Project{ get; set; }       
        public Phase ParentPhase { get; set; }
        public string Title { get; set; }
        public bool OnGoing { get; set; }
        public int LikeCount { get; set; }
        public int FbLikeCount{ get; set; }
        public int TwitterLikeCount { get; set; }
        public int ShareCount { get; set; }
        public int RetweetCount { get; set; }      
        public Role VoteLevel { get; set; }
        public ModuleType type { get; set; }

        public List<Phase> Phases { get; set; }
        public List<string> Tags { get; set; }

        // Added by EKT
        // Modified by NVZ
        // Methods

        #region

        // NOTE ABOUT SHARING, RETWEETING & VOTING: These are pure UI requirements, in the domain it should be
        // persisted as an simple integer that increments whenever someone performs the action. - NVZ
        public void ShareModule()
        {
            ShareCount++;
        }

        public void RetweetModule()
        {
            RetweetCount++;
        }

        public void VoteOnModule()
        {
            LikeCount++;
        }

        public void ChangeStatus(bool status)
        {
            OnGoing = status;
        }

        public void AddTag(string tag)
        {
            Tags.Add(tag);
        }

        #endregion
    }
}