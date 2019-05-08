using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Projects;
using Domain.Users;

namespace UIMVC.Models
{
    public class ProjectViewModel
    {
        [Required]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string Goal { get; set; }
        
        public string Status { get; set; }
        
        public Phase CurrentPhase { get; set; }

    }
}