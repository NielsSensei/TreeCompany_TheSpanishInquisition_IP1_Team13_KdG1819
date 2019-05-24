using Domain.Projects;

namespace UIMVC.Models
{
    public class ChangeIdeationModel
    {
        public string Title { get; set; }
        public string ExtraInfo { get; set; }
        public string MediaFile { get; set; }
        public bool UserVote { get; set; }
        public bool Field { get; set; }
        public bool ClosedField { get; set; }
        public bool MapField { get; set; }
        public bool VideoField { get; set; }
        public bool ImageField { get; set; }
    }
}