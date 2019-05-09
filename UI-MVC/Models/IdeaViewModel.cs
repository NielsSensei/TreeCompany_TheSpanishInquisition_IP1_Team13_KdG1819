using Domain.UserInput;

namespace UIMVC.Models
{
    public class IdeaViewModel
    {
        public Idea ToView { get; set; }
        public byte IdeaLevel { get; set; }
    }
}