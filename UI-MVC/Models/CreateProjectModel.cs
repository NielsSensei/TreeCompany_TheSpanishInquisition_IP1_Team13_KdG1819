using System;
using System.ComponentModel.DataAnnotations;
using Domain.Projects;

namespace UIMVC.Models
{
    public class CreateProjectModel
    {
        [Required(ErrorMessage = "Titel moet ingevuld worden")]
        [StringLength(30)]
        public string Title { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Vul een juiste datum in")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Vul een juiste datum in")]
        public DateTime EndDate { get; set; }

        public string Goal { get; set; }

        public string Status { get; set; }

        public bool Visible { get; set; }

        [Required(ErrorMessage = "Vul de eerste fase in")]
        public Phase CurrentPhase { get; set; }
    }
}