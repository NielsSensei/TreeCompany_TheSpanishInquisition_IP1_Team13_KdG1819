using System.ComponentModel.DataAnnotations;
using Domain.Projects;

namespace UIMVC.Models
{
    public class AlterIdeationModel
    {
        public string Title { get; set; }
        public string ExtraInfo { get; set; }
        public Phase ParentPhase { get; set; }
        [RegularExpression(@"youtube\.com\/embed\/.*", ErrorMessage = "De link moet embed zijn," + 
        "bijvoorbeeld: youtube.com/embed/abc123def")]
        public string MediaFile { get; set; }
    }
}