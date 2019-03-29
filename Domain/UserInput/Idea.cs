using System.Collections.Generic;
using Domain.Users;

namespace Domain.UserInput
{
    public class Idea
    {
        // Added by NG
        // Modified by XV, EKT & NVZ
        public int Id { get; set; }
        public string Title { get; set; }

        public User User { get; set; }


        public List<Field> Fields { get; set; }

        // NOTES about boolean properties Reported & Visible.
        // 1. When an idea is reported it is still visible while the Moderation team takes its time to
        // verify this report. At that point Reported will be true and Visible also will be true.
        //
        // 2. If this report is unjustified according to the Moderation team Reported will become
        // false again and Visible will remain true.
        //
        // 3. If the report is right and the idea is wrong, not in a political or opinionated viewpoint,
        // because it mocks a certain politician, for example, Reported will be true and Visible will
        // be false and it will not be shown again on the Ideation until a Moderator reverts this.
        // At this time the User may be banned as well but that again is one of the responsibilities of
        // the Moderator and not of the system. Automatic bans will not be done.
        //
        // I wrote this so there will not be any confusion like I had before writing this - NVZ
        public bool Reported { get; set; }
        public bool ReviewByAdmin { get; set; }

        public int RetweetCount { get; set; }
        public int ShareCount { get; set; }

        public bool Visible { get; set; }
        public int VoteCount { get; set; }

        public string Status { set; get; }

        public Idea ParentIdea { get; set; }

        // NOTE about votes: As I noted in the User.cs file, Tree Company strives to keep voting as
        // anonymous as possible. We just need the count for the project and thus we will not keep
        // this property. I would love some feedback on this, thanks! - NVZ
        // public ICollection<Vote> Votes { get; set; }
        public Question Question { get; set; }

        public IOT_Device Device { get; set; }

        // Added by EKT
        // Modified by NVZ
        // Methods

        #region

        public bool IsVeriefiedUser()
        {
            if (User.Role == Role.LOGGEDINVERIFIED)
            {
                return true;
            }

            return false;
        }

        public Idea GetIdeaInfo()
        {
            Idea info = new Idea()
            {
                Title = this.Title,
                VoteCount = this.VoteCount,
                User = this.User,
                ParentIdea = this.ParentIdea,
                Fields = this.Fields
            };

            return info;
        }

        public void AddVote()
        {
            VoteCount++;
        }

        public void AddField(Field field)
        {
            Fields.Add(field);
        }

        #endregion
    }
}