using System.Collections.Generic;
using Domain.Common;
using Domain.UserInput;
using Domain.Users;

namespace Domain.Projects
{
    public class Ideation : Module
    {
        // Added by NG
        // Modified by NVZ
        
        public User User { get; set; } //organisation erft van over
        public bool UserIdea { get; set; }  
        public List<IdeationQuestion> CentralQuestions { get; set; }

        public Event Event{ get; set; }
        public Media Media { get; set; }
        public string ExtraInfo { get; set; }
        public string RequiredFields { get; set; }

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