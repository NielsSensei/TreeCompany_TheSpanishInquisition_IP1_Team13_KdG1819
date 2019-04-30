using System.Collections.Generic;
using Domain.Projects;
using Domain.Users;

namespace UIMVC.Models
{
    public class ProjectViewModel
    {
        public Project Project { get; set; }
        public Phase Phase { get; set; }

        public List<Phase> Phases { get; set; }

        public Platform Platform { get; set; }
    }
}