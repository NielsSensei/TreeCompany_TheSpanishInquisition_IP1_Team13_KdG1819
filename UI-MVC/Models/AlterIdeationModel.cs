using Domain.Projects;

namespace UIMVC.Models
{
    public class AlterIdeationModel
    {
        public string Title { get; set; }
        public string ExtraInfo { get; set; }
        public Phase ParentPhase { get; set; }
    }
}