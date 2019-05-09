using Domain.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMVC.Models
{
    public class CreateQuestionnaireModel
    {
        public String Title { get; set; }
        public Phase ParentPhase { get; set; }
    }
}
