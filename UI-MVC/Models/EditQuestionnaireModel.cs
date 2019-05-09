﻿using Domain.Projects;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMVC.Models
{
    public class EditQuestionnaireModel
    {
        public string Title { get; set; }
        public Phase ParentPhase { get; set; }
        public bool OnGoing { get; set; }
        public Role VoteLevel { get; set; }

    }
}
