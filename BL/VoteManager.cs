using System.Linq;
using DAL.repos;
using Domain.Identity;
using Domain.UserInput;

namespace BL
{
    public class VoteManager
    {
        private IdeationVoteRepository VoteRepo { get; }
        
        public VoteManager()
        {
            VoteRepo = new IdeationVoteRepository();
        }
        
        #region Voting
        public bool VerifyVotingOnFeedback(int feedbackId, string userId, int? deviceId, double? x, double? y)
        {
            if (deviceId.HasValue)
            {
                if (!LocationCheck((int) deviceId, x, y))
                {
                    return false;
                }

                return true;
            }

            Vote checkIfVoted = VoteRepo.ReadAll().FirstOrDefault(v => v.User.Id.Equals(userId) && v.Idea.Id == feedbackId);
            if (checkIfVoted != null)
            {
                return false;
            }

            return true;
        }

        public bool LocationCheck(int deviceId, double? x, double? y)
        {
            return false;
        }

        public void RemoveVotes(int ideaId)
        {
            VoteRepo.DeleteVotes(ideaId);
        }

        public void MakeVote(int feedbackId, string userId, int? deviceId, float? x, float? y, bool site)
        {
            if (site)
            {
                Vote v = new Vote()
                {
                    User = new UimvcUser(){ Id = userId },
                    Idea = new Idea(){ Id = feedbackId },
                    Positive = true
                };

                VoteRepo.Create(v);
            }
        }
        #endregion
    }
}
