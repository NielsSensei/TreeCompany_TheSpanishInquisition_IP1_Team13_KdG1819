using System.Collections.Generic;
using Domain.Users;

namespace Domain
{
    public class Module
    {
        // Added by NG
        // Modified by EKT & NVZ
        public int Id { get; set; }
        public ICollection<Phase> Phases { get; set; }
        public Phase CreatedPhase { get; set; }
        public bool OnGoing { get; set; }
        public int NumberOfVotes { get; set; }
        public int NumberOfShares { get; set; }
        public int NumberOfRetweets { get; set; }
        public ICollection<string> Tags { get; set; }
        public Role VoteLevel { get; set; }

        // Added by EKT
        // Modified by NVZ
        // Methods
        #region
        // NOTE ABOUT SHARING, RETWEETING & VOTING: These are pure UI requirements, in the domain it should be
        // persisted as an simple integer that increments whenever someone performs the action. - NVZ
        public void ShareModule()
        {
            NumberOfShares++;
        }

        public void RetweetModule()
        {
            NumberOfRetweets++;
        }

        public void VoteOnModule()
        {
            NumberOfVotes++;
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