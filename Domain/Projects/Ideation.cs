using System.Collections.Generic;
using Domain.Common;
using Domain.Identity;
using Domain.UserInput;
using Domain.Users;

namespace Domain.Projects
{
    public class Ideation : Module
    {
        // Added by NG
        // Modified by NVZ & EKT & DM
        public UIMVCUser User { get; set; } 
        public bool UserIdea { get; set; }  
        public Event Event{ get; set; }
        public Media Media { get; set; }
        public string ExtraInfo { get; set; }
        //Note: This is a code to dertemine what fields are needed. - NVZ
        public int RequiredFields { get; set; }

        public List<IdeationQuestion> CentralQuestions { get; set; }

        // Added by EKT
        // Modified by NVZ
        // Methods

        #region

        public Ideation GetIdeationInfo()
        {
            Ideation info = new Ideation()
            {
                User = this.User,
                CentralQuestions = this.CentralQuestions,
                Media = this.Media,
                OnGoing = this.OnGoing,
                LikeCount = this.LikeCount,
                ShareCount = this.ShareCount,
                RetweetCount = this.RetweetCount
            };
            return info;
        }

        #endregion
    }
}