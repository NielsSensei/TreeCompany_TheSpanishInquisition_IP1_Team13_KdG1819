using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Projects;
using Microsoft.AspNetCore.Http;

namespace UIMVC.Models
{
    public class AddProjectModel
    {
        [Required(ErrorMessage = "Titel moet ingevuld worden")]
        [StringLength(30)]
        public string Title { get; set; }
        
        public string Goal { get; set; }

        public string Status { get; set; }

        public bool Visible { get; set; }

        [Required(ErrorMessage = "Vul de eerste fase in")]
        public Phase CurrentPhase { get; set; }
        public List<IFormFile> InitialProjectImages { get; set; }
    }
}
