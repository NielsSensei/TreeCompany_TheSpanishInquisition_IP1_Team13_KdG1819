using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain.Identity;
using Domain.UserInput;

namespace BL
{
    public class VoteManager
    {
        // Added by NVZ
        private IdeationVoteRepository VoteRepo { get; set; }

        // Added by NVZ
        public VoteManager()
        {
            VoteRepo = new IdeationVoteRepository();
        }
        
        // Added By NVZ
        // Edited by NG
        // Methods
        #region
        /*
         * This is needed for the voting feature. - NVZ
         */
        public bool VerifyVotingOnFeedback(int feedbackId, string userId, int? deviceId, double? x, double? y)
        {
            if (deviceId.HasValue)
            {
                if (!LocationCheck((int) deviceId, x, y)) { return false; }

                return false;
            }
            else
            {
                Vote checkIfVoted = VoteRepo.ReadAll().FirstOrDefault(v => v.User.Id == userId && v.Idea.Id ==
                                                                           feedbackId);
                if (checkIfVoted != null)
                {
                    return false;
                }
                
                return true;
            }
        }

        public void MakeVote(int feedbackId, string userId, int? deviceId, double? x, double? y, bool site)
        {
            if (site)
            {
                Vote v = new Vote()
                {
                    User = new UIMVCUser(){ Id = userId},
                    Idea = new Idea(){ Id = feedbackId },
                    Positive = true,
                };

                VoteRepo.Create(v);
            }
        }

        
        public bool LocationCheck(int deviceId, double? x, double? y)
        {
//            IOT_Device device = voteRepo.ReadDevice(deviceId);
//            // deltas squared to make positive
//            double? deltaX = device.LocationX - x;
//            deltaX *= deltaX;
//            double? deltaY = device.LocationY - y;
//            deltaY *= deltaY;
//
//            // arbitrary numbers
//            if (deltaX < 10 && deltaY < 10) { return true; }
//            return false;
            //throw new NotImplementedException("Sorry, not implemented yet!");
            return false;
        } 
        #endregion
    }
}