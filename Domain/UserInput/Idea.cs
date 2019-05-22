using Domain.Identity;

namespace Domain.UserInput
{
    /*
     * @authors Nathan Gijselings, David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
     */
    public class Idea
    {
        public int Id { get; set; }
        public IdeationQuestion IdeaQuestion { get; set; }
        public UimvcUser User { get; set; }
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
        public IotDevice Device { get; set; }
        public bool IsDeleted { get; set; }
        public Field Field { get; set; }
        public ClosedField Cfield { get; set; }
        public ImageField Ifield { get; set; }
        public VideoField Vfield { get; set; }
        public MapField Mfield { get; set; }
    }
}
