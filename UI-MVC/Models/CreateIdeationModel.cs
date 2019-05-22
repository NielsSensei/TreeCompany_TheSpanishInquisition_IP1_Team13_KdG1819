using Domain.Projects;

namespace UIMVC.Models
{
    public class CreateIdeationModel
    {
        public Phase Parent { get; set; }
        public string Title { get; set; }
        public string ModuleType { get; set; }
        public string ExtraInfo { get; set; }
        public string MediaLink { get; set; }
        public bool UserVote { get; set; }
    }
}