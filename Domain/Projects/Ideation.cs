using System.Collections.Generic;
using Domain.Common;
using Domain.UserInput;

namespace Domain.Projects
{
    public class Ideation : Module
    {
        // Added by NG
        // Modified by NVZ


        public int ModuleId { get; set; }


        public string UserName { get; set; }
        
        
        
        public bool UserIdea { get; set; }

        
        public List<IdeationQuestion> CentralQuestions { get; set; }


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
                UserName = this.UserName,
                CentralQuestions = this.CentralQuestions,
                Media = this.Media,
                OnGoing = this.OnGoing,
                NumberOfVotes = this.NumberOfVotes,
                NumberOfShares = this.NumberOfShares,
                NumberOfRetweets = this.NumberOfRetweets
            };
            return info;
        }

        #endregion
    }
}