using Domain.Users;

namespace UIMVC.Models
{
    public class ChangeQuestionnaireModel
    {
        public string Title { get; set; }
        public bool OnGoing { get; set; }
        public Role VoteLevel { get; set; }
    }
}
