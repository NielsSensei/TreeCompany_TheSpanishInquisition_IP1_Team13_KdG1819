﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    class PhasesDTO
    {
        public int PhaseID { get; set; }
        public int ProjectID { get; set; }
        public string Description { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}