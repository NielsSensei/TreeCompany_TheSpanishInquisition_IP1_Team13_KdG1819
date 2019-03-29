using Domain.Common;
using Domain.UserInput;

namespace Domain.Projects
{
    public class Ideation : Module
    {
        // Added by NG
        // Modified by NVZ
        public bool UserIdea { get; set; }
        public IdeationQuestion CentralQuestion { get; set; }
        public Media Media { get; set; }
        public string UserName { get; set; }
        
        // Added by EKT
        // Modified by NVZ
        // Methods
        #region
        public Ideation GetIdeationInfo()
        {
            Ideation info = new Ideation()
            {
                UserName =  this.UserName,
                CentralQuestion = this.CentralQuestion,
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