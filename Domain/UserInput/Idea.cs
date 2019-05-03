using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Domain.Identity;
using Domain.Users;

namespace Domain.UserInput
{
    public class Idea
    {
        // Added by NG
        // Modified by XV, EKT & NVZ
        public int Id { get; set; }
        public IdeationQuestion IdeaQuestion { get; set; }
        public UIMVCUser User { get; set; }
        public bool Reported { get; set; }
        public bool ReviewByAdmin { get; set; }
        public string Title { get; set; }
        public int RetweetCount { get; set; }
        public int ShareCount { get; set; }
        public bool Visible { get; set; }
        public int VoteCount { get; set; }
        public string Status { set; get; }
        public Idea ParentIdea { get; set; }
        public bool VerifiedUser { get; set; }
        public IOT_Device Device { get; set; }

        // NOTES about boolean properties Reported & Visible.
        // 1. When an idea is reported it is still visible while the Moderation team takes its time to
        // verify this report. At that point Reported will be true and Visible also will be true
        // 2. If this report is unjustified according to the Moderation team Reported will become
        // false again and Visible will remain true.
        // 3. If the report is right and the idea is wrong, not in a political or opinionated viewpoint,
        // because it mocks a certain politician, for example, Reported will be true and Visible will
        // be false and it will not be shown again on the Ideation until a Moderator reverts this.
        // At this time the User may be banned as well but that again is one of the responsibilities of
        // the Moderator and not of the system. Automatic bans will not be done.
        // 4. An moderator is also allow to pass the review to an admin which in that case this 
        // will become true as well but reported will become falls so another moderator does
        // not accidentally fix this.
        // I wrote this so there will not be any confusion like I had before writing this - NVZ

        public Field Field { get; set; }
        public ClosedField Cfield { get; set; }
        public ImageField Ifield { get; set; }
        public VideoField Vfield { get; set; }
        public MapField Mfield { get; set; }

        // Added by EKT
        // Modified by NVZ
        // Methods

        #region
        /**
         * Gebruik Identity
         */
//        public bool IsVeriefiedUser()
//        {
//            if (User.Role == Role.LOGGEDINVERIFIED)
//            {
//                return true;
//            }
//
//            return false;
//        }

        public Idea GetIdeaInfo()
        {
            Idea info = new Idea()
            {
                Title = this.Title,
                VoteCount = this.VoteCount,
                User = this.User,
                ParentIdea = this.ParentIdea
            };

            return info;
        }

        public void AddVote()
        {
            VoteCount++;
        }
        #endregion
    }
}