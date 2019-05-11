using Domain.Projects;

namespace UIMVC.Models
{
    public class CreateQuestionnaireModel
    {
        public string Title { get; set; }
        public Phase ParentPhase { get; set;}
    }
}