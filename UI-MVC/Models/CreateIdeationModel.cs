using System.ComponentModel.DataAnnotations;
using Domain.Projects;

namespace UIMVC.Models
{
    public class CreateIdeationModel
    {
        public Phase Parent { get; set; }
        public string Title { get; set; }
        public string ModuleType { get; set; }
        public string ExtraInfo { get; set; }
        [RegularExpression(@"youtube\.com\/embed\/.*", ErrorMessage = "De link moet embed zijn," + 
        "bijvoorbeeld: youtube.com/embed/abc123def")]
        public string MediaLink { get; set; }
    }
}