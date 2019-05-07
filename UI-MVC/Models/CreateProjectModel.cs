using System.Collections.Generic;
using Domain.Projects;
using Domain.Users;

namespace UIMVC.Models
{
    public class CreateProjectModel
    {
        public Project Project { get; set; }
 
        public List<Phase> Phases { get; set; }

     }
}