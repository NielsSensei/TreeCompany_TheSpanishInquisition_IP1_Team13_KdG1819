using System;
using Domain.Projects;

namespace UIMVC.Models
{
    public class CreatePhaseModel
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}